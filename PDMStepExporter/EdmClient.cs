using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using EPDM.Interop.epdm;

namespace PDMStepExporter
{
    /// <summary>
    /// EPDM（Enterprise PDM）客户端封装：列出库、登录、按变量搜索、取本地副本。
    /// 接口基于官方文档：EPDM.Interop.epdm（主互操作程序集）。
    /// </summary>
    public class EdmClient
    {
        private IEdmVault5 _vault;
        private IEdmVault7 _vault7;
        private long _rootFolderId;

        public bool IsLoggedIn
        {
            get { return _vault != null && _vault.IsLoggedIn; }
        }

        public string VaultName { get; private set; }

        /// <summary>库视图根文件夹 ID（用于拼接本地路径）。</summary>
        public long RootFolderId
        {
            get { return _rootFolderId; }
        }

        /// <summary>
        /// 列出本机已配置的 PDM 库视图名称。
        /// </summary>
        public static string[] GetVaultList()
        {
            List<string> names = new List<string>();
            IEdmVault5 v = new EdmVault5();
            try
            {
                IEdmVault8 v8 = (IEdmVault8)v;
                EdmViewInfo[] views = null;
                v8.GetVaultViews(out views, false);
                if (views != null)
                {
                    foreach (EdmViewInfo vi in views)
                    {
                        // EdmViewInfo 是值类型（struct），不能与 null 比较
                        if (!string.IsNullOrEmpty(vi.mbsVaultName))
                        {
                            names.Add(vi.mbsVaultName);
                        }
                    }
                }
            }
            finally
            {
                Marshal.ReleaseComObject(v);
            }
            return names.ToArray();
        }

        /// <summary>
        /// 登录 PDM 库。userName 为空时使用当前 Windows 身份（LoginAuto）。
        /// </summary>
        public void Login(string vaultName, string userName, string password, IntPtr windowHandle)
        {
            Logout();

            IEdmVault5 v = new EdmVault5();
            try
            {
                if (string.IsNullOrEmpty(userName))
                {
                    // Windows 身份登录（官方示例推荐方式）
                    v.LoginAuto(vaultName, windowHandle.ToInt32());
                    if (!v.IsLoggedIn)
                    {
                        throw new Exception("登录失败（Windows 身份）。请确认当前 Windows 用户拥有该库的访问权限，或填写用户名/密码后重试。");
                    }
                }
                else
                {
                    // 显式用户名/密码登录（部分库配置需要；使用动态绑定以兼容不同版本签名）
                    dynamic dv = v;
                    object logged = null;
                    try
                    {
                        logged = dv.Login(vaultName, "", userName, password, true);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("登录失败：" + ex.Message);
                    }
                    IEdmVault5 lv = logged as IEdmVault5;
                    if (lv == null || !lv.IsLoggedIn)
                    {
                        throw new Exception("登录失败：用户名或密码错误，或该库不允许此方式登录。");
                    }
                    v = lv;
                }

                _vault = v;
                _vault7 = (IEdmVault7)_vault;
                IEdmFolder5 root = _vault7.RootFolder;
                _rootFolderId = root.ID;
                VaultName = vaultName;
            }
            catch
            {
                if (v != null && v != _vault)
                {
                    Marshal.ReleaseComObject(v);
                }
                throw;
            }
        }

        public void Logout()
        {
            if (_vault != null)
            {
                try
                {
                    // Logout 在部分 EPDM 版本中不在 IEdmVault5 上，用动态绑定兼容
                    if (_vault.IsLoggedIn) ((dynamic)_vault).Logout();
                }
                catch
                {
                    // 忽略登出异常
                }
                Marshal.ReleaseComObject(_vault);
                _vault = null;
                _vault7 = null;
            }
        }

        /// <summary>
        /// 按变量（物料编码 / 规格型号）搜索库内文件（仅最新版本）。
        /// fuzzy=true 时使用“包含”匹配（EdmVarOp_StringContains），否则精确匹配（EdmVarOp_StringEqualTo）。
        /// </summary>
        public List<EdmSearchItem> Search(string variableName, string term, bool fuzzy, bool recursive)
        {
            List<EdmSearchItem> items = new List<EdmSearchItem>();
            if (!IsLoggedIn)
            {
                throw new InvalidOperationException("尚未登录 PDM 库。");
            }

            // 搜索条件组 API（BeginAND/AddVariable2/EndAND）在不同 EPDM 版本中位于不同接口，
            // 统一用 dynamic 动态绑定，避免编译期接口版本不匹配
            dynamic search = _vault.CreateSearch();
            try
            {
                search.SetToken(EdmSearchToken.Edmstok_FindFiles, true);
                search.SetToken(EdmSearchToken.Edmstok_FindFolders, false);
                search.SetToken(EdmSearchToken.Edmstok_Recursive, recursive);

                search.BeginAND();
                object varName = variableName;
                object varValue = term;
                int op = fuzzy
                    ? (int)EdmVarOp.EdmVarOp_StringContains
                    : (int)EdmVarOp.EdmVarOp_StringEqualTo;
                search.AddVariable2(ref varName, ref varValue, op);
                search.EndAND();

                IEdmSearchResult5 r = search.GetFirstResult();
                while (r != null)
                {
                    IEdmFile5 file = null;
                    try
                    {
                        file = (IEdmFile5)r;
                    }
                    catch (InvalidCastException)
                    {
                        // 搜索结果含非文件对象时跳过
                    }

                    if (file != null)
                    {
                        EdmSearchItem it = new EdmSearchItem();
                        it.InputTerm = term;
                        it.MatchedField = variableName;
                        it.FileName = file.Name;
                        it.Version = r.Version;
                        // 优先用搜索结果的 ParentFolderID；若为 0 则通过 file 对象遍历父文件夹
                        it.ParentFolderId = GetParentFolderId(file, r.ParentFolderID);
                        it.IsExportable = IsModelFile(file.Name);

                        // 通过搜索结果自带的父文件夹 ID 取准确的本地路径（支持子文件夹）
                        string local = ResolveLocalPath(file, r.ParentFolderID);
                        it.LocalPath = local;
                        it.HasLocalCopy = !string.IsNullOrEmpty(local) && File.Exists(local);
                        it.File = file;
                        items.Add(it);
                    }

                    r = search.GetNextResult();
                }
            }
            finally
            {
                Marshal.ReleaseComObject(search);
            }
            return items;
        }

        /// <summary>
        /// 获取文件的父文件夹 ID。优先用搜索结果自带的 ParentFolderID，
        /// 若为 0（部分 EPDM 版本搜索结果不返回此字段），则通过 file 对象遍历第一个父文件夹。
        /// </summary>
        private long GetParentFolderId(IEdmFile5 file, long fallbackId)
        {
            if (fallbackId > 0) return fallbackId;
            try
            {
                dynamic f = file;
                dynamic pos = f.GetFirstFolderPosition();
                if (pos != null)
                {
                    dynamic folder = f.GetNextFolder(pos);
                    if (folder != null) return folder.ID;
                }
            }
            catch (Exception)
            {
                // 遍历失败则退回 fallback
            }
            return fallbackId;
        }

        /// <summary>
        /// 计算文件在本地视图中的完整路径。
        /// IEdmFile5.GetLocalPath(lParentFolderID) 官方语义：传入文件的父文件夹 ID，
        /// 返回该文件夹本地视图路径 + 文件名。之前误传根文件夹 ID 导致子文件夹中文件路径错误。
        /// </summary>
        private string ResolveLocalPath(IEdmFile5 file, long parentFolderId)
        {
            try
            {
                // 优先：用文件实际父文件夹 ID 取本地路径（正确处理子文件夹）
                return file.GetLocalPath((int)parentFolderId);
            }
            catch (Exception)
            {
                // 兜底：部分版本父文件夹 ID 可能不可用，退回根文件夹拼接（仅根目录文件正确）
                try
                {
                    return file.GetLocalPath((int)_rootFolderId);
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 确保文件在本地视图中有副本（自动 Get 最新版）。
        /// 尝试多种 EPDM API 方式：IEdmBatchGet（官方推荐）、文件对象的 Get 方法。
        /// </summary>
        public bool EnsureLocalCopy(EdmSearchItem item, bool forceLatest, out string error)
        {
            error = "";
            if (item == null || item.File == null) { error = "文件对象为空"; return false; }

            // 先确保本地路径正确
            string local = ResolveLocalPath(item.File, item.ParentFolderId);
            if (!string.IsNullOrEmpty(local)) item.LocalPath = local;

            // 不强制最新版且本地已有副本 → 直接复用
            if (!forceLatest && File.Exists(item.LocalPath))
            {
                item.HasLocalCopy = true;
                return true;
            }

            // 尝试自动 Get（多种方式）
            System.Text.StringBuilder diag = new System.Text.StringBuilder();
            bool got = TryAutoGet(item, diag);

            if (got || File.Exists(item.LocalPath))
            {
                item.HasLocalCopy = true;
                return true;
            }

            error = "自动Get未成功。" + diag.ToString() + "路径：" + item.LocalPath;
            item.HasLocalCopy = false;
            return false;
        }

        /// <summary>
        /// 尝试多种方式自动获取文件最新版到本地视图。
        /// </summary>
        private bool TryAutoGet(EdmSearchItem item, System.Text.StringBuilder diag)
        {
            int fileId = 0;
            try { fileId = ((dynamic)item.File).ID; } catch { }
            if (fileId <= 0) { diag.Append("无法获取文件ID。"); return false; }

            // 用强类型调用 IEdmBatchGet（EPDM 标准接口）
            try
            {
                IEdmVault7 vault7 = _vault7;
                if (vault7 == null) vault7 = (IEdmVault7)_vault;
                IEdmBatchGet batchGet = (IEdmBatchGet)vault7.CreateUtility(EdmUtility.EdmUtil_BatchGet);
                if (batchGet == null)
                {
                    diag.Append("CreateUtility返回null。");
                }
                else
                {
                    // 构造 EdmSelItem 数组（必须用 ref 传递！）
                    EdmSelItem[] selItems = new EdmSelItem[1];
                    selItems[0].mlDocID = fileId;
                    selItems[0].mlProjID = (int)item.ParentFolderId;

                    // 1. AddSelection(vault, ref EdmSelItem[])
                    batchGet.AddSelection((EdmVault5)_vault, ref selItems);

                    // 2. CreateTree(hwnd, flags)
                    // Egcf_IncludeAutoCacheFiles=2048：自动缓存引用文件（装配体的子零件也会被Get）
                    int hwnd = 0;
                    try { hwnd = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle.ToInt32(); } catch { }
                    batchGet.CreateTree(hwnd, (int)EdmGetCmdFlags.Egcf_IncludeAutoCacheFiles);

                    // 诊断：树中有多少文件（装配体应包含子零件）
                    int treeCount = 0;
                    try { treeCount = batchGet.FileCount; } catch { }
                    diag.Append(string.Format("CreateTree完成(树中{0}个文件)。", treeCount));

                    // 3. GetFiles(hwnd, null)
                    batchGet.GetFiles(hwnd, null);

                    diag.Append("BatchGet强类型调用成功。");
                    if (File.Exists(item.LocalPath)) return true;
                    diag.Append("但文件仍不存在。");
                }
            }
            catch (Exception ex)
            {
                diag.Append("强类型BatchGet异常:").Append(ex.Message);
                if (ex.InnerException != null) diag.Append("[").Append(ex.InnerException.Message).Append("]");
                diag.Append("。");
            }

            // 方式2：尝试文件对象上可能的 Get 方法（2参/4参都试）
            try
            {
                dynamic f = item.File;
                string[] fileGetMethods = { "GetFileCopy", "GetFileCopy2", "GetLatestVersion", "GetLocalCopy", "Retrieve", "Get" };
                foreach (string m in fileGetMethods)
                {
                    try
                    {
                        f.GetType().InvokeMember(m,
                            System.Reflection.BindingFlags.InvokeMethod, null, f,
                            new object[] { (int)item.ParentFolderId, 0, item.LocalPath, 0 });
                        diag.Append("file.").Append(m).Append("(4参)。");
                        if (File.Exists(item.LocalPath)) return true;
                    }
                    catch { }
                    try
                    {
                        f.GetType().InvokeMember(m,
                            System.Reflection.BindingFlags.InvokeMethod, null, f,
                            new object[] { (int)item.ParentFolderId, 0 });
                        diag.Append("file.").Append(m).Append("(2参)。");
                        if (File.Exists(item.LocalPath)) return true;
                    }
                    catch { }
                }
            }
            catch { }

            return false;
        }

        /// <summary>用反射取枚举类型的所有整数值。</summary>
        private static int[] GetEnumAllValues(string typeName)
        {
            try
            {
                Type t = Type.GetType(typeName + ", EPDM.Interop.epdm");
                if (t != null)
                {
                    Array vals = Enum.GetValues(t);
                    int[] result = new int[vals.Length];
                    for (int i = 0; i < vals.Length; i++) result[i] = (int)vals.GetValue(i);
                    return result;
                }
            }
            catch { }
            return new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        }

        /// <summary>从已加载程序集中取枚举值，失败则用默认值。</summary>
        private static int GetEnumValue(string typeName, string fieldName, int def)
        {
            try
            {
                Type t = Type.GetType(typeName + ", EPDM.Interop.epdm");
                if (t != null) return (int)Enum.Parse(t, fieldName);
            }
            catch { }
            return def;
        }

        // 兼容旧调用（无 out 参数）
        public bool EnsureLocalCopy(EdmSearchItem item, bool forceLatest = false)
        {
            string err;
            return EnsureLocalCopy(item, forceLatest, out err);
        }

        /// <summary>判断是否为 SolidWorks 模型文件（零件/装配体）。</summary>
        public static bool IsModelFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return false;
            string ext = Path.GetExtension(fileName).ToLowerInvariant();
            return ext == ".sldprt" || ext == ".sldasm";
        }

        /// <summary>
        /// 根据 3D 文件本地路径，在 PDM 中查找同名 PDF 文件并 Get 到本地视图。
        /// </summary>
        public string FindAndGetPdf(string sldLocalPath, long parentFolderId, bool forceLatest, out string error)
        {
            error = "";
            if (string.IsNullOrEmpty(sldLocalPath))
            {
                error = "3D文件路径为空。";
                return null;
            }

            string pdfPath = Path.ChangeExtension(sldLocalPath, ".pdf");
            string diag = "";

            int pdfFileId = 0;
            int pdfParentId = (int)parentFolderId;

            // 强类型调用 GetFileFromPath（签名：GetFileFromPath(string, out IEdmFolder5)）
            try
            {
                IEdmFolder5 pdfFolder;
                IEdmFile5 pdfFile = _vault7.GetFileFromPath(pdfPath, out pdfFolder);
                if (pdfFile != null)
                {
                    pdfFileId = pdfFile.ID;
                    if (pdfFolder != null) pdfParentId = pdfFolder.ID;
                }
            }
            catch (Exception ex)
            {
                diag = "GetFileFromPath: " + ex.Message;
            }

            // 备选：用 GetObject 按 ID 获取（需要知道文件ID，这里不可用）
            // 备选：直接检查本地文件
            if (pdfFileId <= 0)
            {
                if (File.Exists(pdfPath)) return pdfPath;
                error = "未找到PDF(" + diag + ")";
                return null;
            }

            // 本地已有副本且不需要强制最新，直接返回
            if (!forceLatest && File.Exists(pdfPath))
            {
                return pdfPath;
            }

            // 用 IEdmBatchGet Get PDF 最新版
            try
            {
                IEdmVault7 vault7 = _vault7 ?? (IEdmVault7)_vault;
                IEdmBatchGet batchGet = (IEdmBatchGet)vault7.CreateUtility(EdmUtility.EdmUtil_BatchGet);
                EdmSelItem[] selItems = new EdmSelItem[1];
                selItems[0].mlDocID = pdfFileId;
                selItems[0].mlProjID = pdfParentId;
                batchGet.AddSelection((EdmVault5)_vault, ref selItems);
                int hwnd = 0;
                try { hwnd = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle.ToInt32(); } catch { }
                batchGet.CreateTree(hwnd, (int)EdmGetCmdFlags.Egcf_IncludeAutoCacheFiles);
                batchGet.GetFiles(hwnd, null);
            }
            catch (Exception ex)
            {
                error = "PDF Get失败: " + ex.Message;
                if (File.Exists(pdfPath)) return pdfPath;
                return null;
            }

            if (File.Exists(pdfPath)) return pdfPath;
            error = "PDF Get后本地仍不存在: " + pdfPath;
            return null;
        }
    }
}