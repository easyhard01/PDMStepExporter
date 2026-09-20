using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PDMStepExporter
{
    public partial class Form1 : Form
    {
        private readonly EdmClient _edm = new EdmClient();
        private readonly SwExporter _sw = new SwExporter();
        private bool _closeSwAfterExport = false;
        private readonly List<EdmSearchItem> _items = new List<EdmSearchItem>();
        private bool _searchRunning;
        private bool _exportRunning;

        // 变量名与 PDM 库数据卡一致，可在 App.config 中修改
        private string _varCode = "物料编码";
        private string _varSpec = "规格型号";

        /// <summary>搜索参数（在 UI 线程捕获后传给后台线程）。</summary>
        private sealed class SearchOptions
        {
            public bool Fuzzy;
            public bool Recurse;
            public bool Code;
            public bool Spec;
            public bool Auto;
        }

        /// <summary>导出参数（在 UI 线程捕获后传给后台线程）。</summary>
        private sealed class ExportOptions
        {
            public bool UseInputName;
            public bool GetLatest;
            public bool CloseSw;
            public bool ExportPdf;
        }

        public Form1()
        {
            InitializeComponent();
        }

        #region 生命周期

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadConfig();
            ApplyTheme();
            pgbProgress.SetRange(0, 1);
            pgbProgress.Value = 0;
            Log("程序已启动：正在枚举 PDM 库视图……", Theme.TextSub);
            LoadVaultList();
            // 自动登录（Windows 身份）：与你旧项目 EdmBOM_CSharp 中 LoginAuto 方式一致
            AutoLogin();
        }

        /// <summary>
        /// 启动时自动登录 PDM（Windows 身份）。
        /// 优先使用 App.config 中保存的 VaultName，否则用下拉框第一个库。
        /// </summary>
        private void AutoLogin()
        {
            if (cboVault.Items.Count == 0) return;
            string vaultName = cboVault.Text.Trim();
            if (vaultName.Length == 0)
            {
                vaultName = cboVault.Items[0].ToString();
                cboVault.SelectedIndex = 0;
            }
            try
            {
                _edm.Login(vaultName, "", "", Handle); // 用户名/密码留空 = LoginAuto
                SetConnectedState(true);
                Log("自动登录成功：" + vaultName, Theme.OkColor);
            }
            catch (Exception ex)
            {
                SetConnectedState(false);
                Log("自动登录失败（可手动点「连接 PDM」重试）：" + ex.Message, Theme.ErrColor);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfig();
            try
            {
                _edm.Logout();
            }
            catch (Exception)
            {
                // 忽略退出时的登出异常
            }
        }

        #endregion

        #region 主题与配置

        private void ApplyTheme()
        {
            Theme.MakeRounded(cardConnection, 10);
            Theme.MakeRounded(cardInput, 10);
            Theme.MakeRounded(cardTips, 10);
            Resize += delegate
            {
                Theme.MakeRounded(cardConnection, 10);
                Theme.MakeRounded(cardInput, 10);
                Theme.MakeRounded(cardTips, 10);
            };

            Theme.SetPlaceholder(txtUser, "用户名（留空=Windows身份）", false);
            Theme.SetPlaceholder(txtPwd, "密码（留空=Windows身份）", true);
        }

        private void LoadConfig()
        {
            try
            {
                Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection kv = cfg.AppSettings.Settings;
                string v;

                v = GetCfg(kv, "VaultName");
                if (!string.IsNullOrEmpty(v)) cboVault.Items.Add(v);

                v = GetCfg(kv, "VariableCode");
                if (!string.IsNullOrEmpty(v)) _varCode = v;
                v = GetCfg(kv, "VariableSpec");
                if (!string.IsNullOrEmpty(v)) _varSpec = v;

                v = GetCfg(kv, "OutputDir");
                if (!string.IsNullOrEmpty(v)) txtOutputDir.Text = v;

                chkUseInputName.Checked = GetBoolCfg(kv, "UseInputName", true);
                chkFuzzy.Checked = GetBoolCfg(kv, "FuzzyMatch", true);
                chkRecurse.Checked = GetBoolCfg(kv, "Recursive", true);
                chkGetLatest.Checked = GetBoolCfg(kv, "GetLatest", true);
                chkCloseSw.Checked = GetBoolCfg(kv, "CloseSwAfterExport", true);
                chkFirstOnly.Checked = GetBoolCfg(kv, "FirstOnly", true);

                int mode = GetIntCfg(kv, "SearchMode", 2);
                rbAuto.Checked = mode == 2;
                rbCode.Checked = mode == 0;
                rbSpec.Checked = mode == 1;
            }
            catch (Exception)
            {
                // 配置读取失败时使用默认值
            }
        }

        private void SaveConfig()
        {
            try
            {
                Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection kv = cfg.AppSettings.Settings;
                SetCfg(kv, "VaultName", cboVault.Text.Trim());
                SetCfg(kv, "VariableCode", _varCode);
                SetCfg(kv, "VariableSpec", _varSpec);
                SetCfg(kv, "OutputDir", txtOutputDir.Text.Trim());
                SetCfg(kv, "UseInputName", chkUseInputName.Checked.ToString());
                SetCfg(kv, "FuzzyMatch", chkFuzzy.Checked.ToString());
                SetCfg(kv, "Recursive", chkRecurse.Checked.ToString());
                SetCfg(kv, "GetLatest", chkGetLatest.Checked.ToString());
                SetCfg(kv, "CloseSwAfterExport", chkCloseSw.Checked.ToString());
                SetCfg(kv, "FirstOnly", chkFirstOnly.Checked.ToString());
                int mode = rbCode.Checked ? 0 : (rbSpec.Checked ? 1 : 2);
                SetCfg(kv, "SearchMode", mode.ToString());
                cfg.Save(ConfigurationSaveMode.Modified);
            }
            catch (Exception)
            {
                // 保存失败不影响使用
            }
        }

        private static string GetCfg(KeyValueConfigurationCollection kv, string key)
        {
            KeyValueConfigurationElement el = kv[key];
            return el == null ? null : el.Value;
        }

        private static void SetCfg(KeyValueConfigurationCollection kv, string key, string value)
        {
            KeyValueConfigurationElement el = kv[key];
            if (el == null) kv.Add(key, value);
            else el.Value = value;
        }

        private static bool GetBoolCfg(KeyValueConfigurationCollection kv, string key, bool def)
        {
            string v = GetCfg(kv, key);
            if (string.IsNullOrEmpty(v)) return def;
            bool r;
            return bool.TryParse(v, out r) ? r : def;
        }

        private static int GetIntCfg(KeyValueConfigurationCollection kv, string key, int def)
        {
            string v = GetCfg(kv, key);
            if (string.IsNullOrEmpty(v)) return def;
            int r;
            return int.TryParse(v, out r) ? r : def;
        }

        #endregion

        #region 日志与状态

        private void Log(string msg, Color color)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { AppendLog(msg, color); });
                return;
            }
            AppendLog(msg, color);
        }

        private void AppendLog(string msg, Color color)
        {
            logRich.SelectionStart = logRich.TextLength;
            logRich.SelectionLength = 0;
            logRich.SelectionColor = color;
            logRich.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + msg + Environment.NewLine);
            logRich.SelectionStart = logRich.TextLength;
            logRich.ScrollToCaret();
        }

        private void SetConnectedState(bool connected)
        {
            if (connected)
            {
                lblStatusDot.ForeColor = Theme.OkColor;
                lblStatusText.Text = "已连接：" + _edm.VaultName;
                lblLoginState.ForeColor = Theme.OkColor;
                lblLoginState.Text = "✓ 已连接";
            }
            else
            {
                lblStatusDot.ForeColor = Color.FromArgb(255, 107, 107);
                lblStatusText.Text = "未连接";
                lblLoginState.ForeColor = Theme.ErrColor;
                lblLoginState.Text = "未连接";
            }
        }

        private void SetBusy(bool busy)
        {
            btnSearch.Enabled = !busy;
            btnLogin.Enabled = !busy;
            btnRefresh.Enabled = !busy;
            btnImport.Enabled = !busy;
            btnClearInput.Enabled = !busy;
            btnClearGrid.Enabled = !busy;
            btnExportSel.Enabled = !busy;
            btnExportAll.Enabled = !busy;
            btnCopySel.Enabled = !busy;
            btnCopyAll.Enabled = !busy;
            btnPdfSel.Enabled = !busy;
            btnPdfAll.Enabled = !busy;
            txtInput.Enabled = !busy;
            cboVault.Enabled = !busy;
            txtUser.Enabled = !busy;
            txtPwd.Enabled = !busy;
            lblProgress.Text = busy ? "处理中…" : "就绪";
        }

        #endregion

        #region 库连接

        private void LoadVaultList()
        {
            try
            {
                string[] names = EdmClient.GetVaultList();
                cboVault.Items.Clear();
                foreach (string n in names) cboVault.Items.Add(n);
                if (cboVault.Items.Count > 0)
                {
                    // 恢复上次选择的库
                    string last = GetCfg(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).AppSettings.Settings, "VaultName");
                    if (!string.IsNullOrEmpty(last) && cboVault.Items.Contains(last))
                    {
                        cboVault.SelectedItem = last;
                    }
                    else
                    {
                        cboVault.SelectedIndex = 0;
                    }
                    Log("已发现 " + cboVault.Items.Count + " 个库视图：" + string.Join("、", names), Theme.TextSub);
                }
                else
                {
                    Log("未发现可用的 PDM 库视图，请确认已安装 EPDM 客户端并配置本地视图。", Theme.ErrColor);
                }
            }
            catch (Exception ex)
            {
                Log("读取库列表失败：" + ex.Message, Theme.ErrColor);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadVaultList();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string vaultName = cboVault.Text.Trim();
            if (vaultName.Length == 0)
            {
                Log("请先选择或刷新 PDM 库名称。", Theme.ErrColor);
                return;
            }

            try
            {
                string user = Theme.IsPlaceholder(txtUser) ? "" : txtUser.Text.Trim();
                string pwd = Theme.IsPlaceholder(txtPwd) ? "" : txtPwd.Text;
                _edm.Login(vaultName, user, pwd, Handle);
                SetConnectedState(true);
                Log("已连接 PDM 库：" + vaultName, Theme.OkColor);
            }
            catch (Exception ex)
            {
                SetConnectedState(false);
                Log("连接失败：" + ex.Message, Theme.ErrColor);
            }
        }

        #endregion

        #region 搜索

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (_searchRunning || _exportRunning) return;

            List<string> terms = ParseInput(txtInput.Text);
            if (terms.Count == 0)
            {
                Log("请输入至少一个物料编码或规格型号（每行一个）。", Theme.ErrColor);
                return;
            }
            if (!_edm.IsLoggedIn)
            {
                Log("请先连接 PDM 库。", Theme.ErrColor);
                return;
            }

            _searchRunning = true;
            SetBusy(true);
            _items.Clear();
            dgvResults.Rows.Clear();
            lblResultCount.Text = "匹配结果：0 条";
            pgbProgress.SetRange(0, terms.Count);
            pgbProgress.Value = 0;
            Log("开始搜索 " + terms.Count + " 个条目（字段："
                + (rbCode.Checked ? _varCode : (rbSpec.Checked ? _varSpec : _varCode + "→" + _varSpec))
                + "，匹配：" + (chkFuzzy.Checked ? "包含" : "精确") + "）……", Theme.TextMain);

            // 在 UI 线程捕获选项，避免后台线程跨线程读取控件
            SearchOptions opt = new SearchOptions();
            opt.Fuzzy = chkFuzzy.Checked;
            opt.Recurse = chkRecurse.Checked;
            opt.Code = rbCode.Checked;
            opt.Spec = rbSpec.Checked;
            opt.Auto = rbAuto.Checked;
            bwSearch.RunWorkerAsync(new object[] { terms, opt });
        }

        /// <summary>解析批量输入：按行拆分，兼容 Excel 粘贴的制表符。</summary>
        private static List<string> ParseInput(string text)
        {
            List<string> result = new List<string>();
            if (string.IsNullOrEmpty(text)) return result;

            HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            string[] lines = text.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
            foreach (string line in lines)
            {
                string[] parts = line.Split('\t');
                foreach (string part in parts)
                {
                    string t = part.Trim();
                    if (t.Length > 0 && seen.Add(t)) result.Add(t);
                }
            }
            return result;
        }

        private void bwSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] args = (object[])e.Argument;
            List<string> terms = (List<string>)args[0];
            SearchOptions opt = (SearchOptions)args[1];
            List<EdmSearchItem> found = new List<EdmSearchItem>();
            HashSet<string> keys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            bool fuzzy = opt.Fuzzy;
            bool recurse = opt.Recurse;
            bool searchCode = opt.Code || opt.Auto;
            bool searchSpec = opt.Spec || opt.Auto;

            int done = 0;
            foreach (string term in terms)
            {
                done++;
                int before = CountForTerm(found, term);
                string error = null;
                try
                {
                    if (searchCode)
                    {
                        AddResults(found, keys, _edm.Search(_varCode, term, fuzzy, recurse));
                    }
                    if (searchSpec && (opt.Spec || CountForTerm(found, term) == before))
                    {
                        // 自动判断：编码未命中时再按规格型号搜索
                        AddResults(found, keys, _edm.Search(_varSpec, term, fuzzy, recurse));
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }

                int got = CountForTerm(found, term) - before;
                bwSearch.ReportProgress(done, new object[] { term, got, error });
            }

            e.Result = found;
        }

        private void AddResults(List<EdmSearchItem> list, HashSet<string> keys, List<EdmSearchItem> adds)
        {
            if (adds == null) return;
            foreach (EdmSearchItem it in adds)
            {
                string key = it.FileName + "|" + it.Version;
                if (keys.Add(key)) list.Add(it);
            }
        }

        private static int CountForTerm(List<EdmSearchItem> list, string term)
        {
            int n = 0;
            foreach (EdmSearchItem it in list)
            {
                if (string.Equals(it.InputTerm, term, StringComparison.OrdinalIgnoreCase)) n++;
            }
            return n;
        }

        private void bwSearch_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pgbProgress.Value = e.ProgressPercentage;
            object[] st = (object[])e.UserState;
            string term = (string)st[0];
            int count = (int)st[1];
            string err = st[2] as string;
            if (err != null)
            {
                Log("  「" + term + "」搜索出错：" + err, Theme.ErrColor);
            }
            else
            {
                Log("  「" + term + "」匹配 " + count + " 个文件", count > 0 ? Theme.OkColor : Theme.TextSub);
            }
        }

        private void bwSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _items.Clear();
            if (e.Error != null)
            {
                Log("搜索过程发生异常：" + e.Error.Message, Theme.ErrColor);
            }
            else if (e.Result != null)
            {
                foreach (EdmSearchItem it in (List<EdmSearchItem>)e.Result) _items.Add(it);
            }
            FillGrid();
            SetBusy(false);
            _searchRunning = false;
            Log("搜索完成，共 " + _items.Count + " 个匹配。可在右侧列表勾选后导出。", Theme.TextMain);
        }

        private void FillGrid()
        {
            dgvResults.Rows.Clear();
            foreach (EdmSearchItem it in _items)
            {
                int idx = dgvResults.Rows.Add(
                    it.InputTerm,
                    it.MatchedField,
                    it.FileName,
                    it.Version.ToString(),
                    it.LocalPath,
                    it.HasLocalCopy ? "本地" : "需Get",
                    it.IsExportable ? "可导出" : "跳过");
                dgvResults.Rows[idx].Tag = it;
                dgvResults.Rows[idx].DefaultCellStyle.ForeColor = it.IsExportable ? Theme.TextMain : Color.FromArgb(160, 160, 160);
            }
            lblResultCount.Text = "匹配结果：" + _items.Count + " 条";
        }

        #endregion

        #region 导出

        private void btnExportSel_Click(object sender, EventArgs e)
        {
            List<EdmSearchItem> list = new List<EdmSearchItem>();
            foreach (DataGridViewRow row in dgvResults.SelectedRows)
            {
                EdmSearchItem it = row.Tag as EdmSearchItem;
                if (it != null) list.Add(it);
            }
            if (list.Count == 0)
            {
                Log("请先在结果列表中选择要导出的行。", Theme.ErrColor);
                return;
            }
            StartExport(list);
        }

        private void btnExportAll_Click(object sender, EventArgs e)
        {
            List<EdmSearchItem> list = new List<EdmSearchItem>();
            if (chkFirstOnly.Checked)
            {
                HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (EdmSearchItem it in _items)
                {
                    if (seen.Add(it.InputTerm)) list.Add(it);
                }
            }
            else
            {
                foreach (EdmSearchItem it in _items) list.Add(it);
            }
            if (list.Count == 0)
            {
                Log("当前没有可导出的匹配结果。", Theme.ErrColor);
                return;
            }
            StartExport(list);
        }

        private void StartExport(List<EdmSearchItem> list)
        {
            if (_searchRunning || _exportRunning) return;

            List<EdmSearchItem> exportable = list.FindAll(delegate(EdmSearchItem x) { return x.IsExportable; });
            if (exportable.Count == 0)
            {
                Log("所选条目中没有可导出的模型文件（.sldprt / .sldasm）。", Theme.ErrColor);
                return;
            }

            string outDir = txtOutputDir.Text.Trim();
            if (outDir.Length == 0)
            {
                outDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "PDM_STEP导出");
            }
            try
            {
                Directory.CreateDirectory(outDir);
            }
            catch (Exception ex)
            {
                Log("无法创建输出目录：" + ex.Message, Theme.ErrColor);
                return;
            }
            txtOutputDir.Text = outDir;

            _exportRunning = true;
            SetBusy(true);
            pgbProgress.SetRange(0, exportable.Count);
            pgbProgress.Value = 0;
            Log("开始导出 " + exportable.Count + " 个文件 → " + outDir, Theme.TextMain);

            // 在 UI 线程捕获选项，避免后台线程跨线程读取控件
            ExportOptions opt = new ExportOptions();
            opt.UseInputName = chkUseInputName.Checked;
            opt.GetLatest = chkGetLatest.Checked;
            opt.CloseSw = chkCloseSw.Checked;
            _closeSwAfterExport = opt.CloseSw;
            bwExport.RunWorkerAsync(new object[] { exportable, outDir, opt });
        }

        private void bwExport_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] args = (object[])e.Argument;
            List<EdmSearchItem> list = (List<EdmSearchItem>)args[0];
            string outDir = (string)args[1];
            ExportOptions opt = (ExportOptions)args[2];
            bool useInputName = opt.UseInputName;
            bool getLatest = opt.GetLatest;

            int okCount = 0;
            int failCount = 0;
            int skipCount = 0;
            int i = 0;

            foreach (EdmSearchItem it in list)
            {
                i++;
                try
                {
                    // 1) 确保本地副本（勾选 Get 最新版本时，无论本地是否已有副本都强制取最新版）
                    if (getLatest || !it.HasLocalCopy)
                    {
                        string getErr;
                        bool got = _edm.EnsureLocalCopy(it, getLatest, out getErr);
                        if (!got || !File.Exists(it.LocalPath))
                        {
                            skipCount++;
                            string detail = string.IsNullOrEmpty(getErr)
                                ? string.Format("本地路径='{0}'", it.LocalPath)
                                : getErr;
                            bwExport.ReportProgress(i, new object[] { it, "跳过：本地无副本。" + detail, false });
                            continue;
                        }
                    }
                    if (!File.Exists(it.LocalPath))
                    {
                        skipCount++;
                        bwExport.ReportProgress(i, new object[] { it, "跳过：本地文件不存在：" + it.LocalPath, false });
                        continue;
                    }

                    // 2) 计算 STEP 文件名（自动避开重名）
                    string baseName = useInputName ? it.InputTerm : Path.GetFileNameWithoutExtension(it.FileName);
                    baseName = SanitizeFileName(baseName);
                    if (baseName.Length == 0) baseName = Path.GetFileNameWithoutExtension(it.FileName);
                    string stepPath = UniquePath(Path.Combine(outDir, baseName + ".STEP"));

                    // 3) 调用 SolidWorks 导出
                    string msg;
                    bool ok = _sw.ExportStep(it.LocalPath, stepPath, out msg);
                    if (ok)
                    {
                        okCount++;
                        bwExport.ReportProgress(i, new object[] { it, "OK → " + Path.GetFileName(stepPath), true });
                    }
                    else
                    {
                        failCount++;
                        bwExport.ReportProgress(i, new object[] { it, "失败：" + msg, false });
                    }
                }
                catch (Exception ex)
                {
                    failCount++;
                    bwExport.ReportProgress(i, new object[] { it, "异常：" + ex.Message, false });
                }
            }

            e.Result = new object[] { okCount, failCount, skipCount };
        }

        private void bwExport_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pgbProgress.Value = e.ProgressPercentage;
            lblProgress.Text = e.ProgressPercentage + " / " + pgbProgress.Maximum;
            object[] st = (object[])e.UserState;
            EdmSearchItem it = (EdmSearchItem)st[0];
            string msg = (string)st[1];
            bool ok = (bool)st[2];
            if (ok)
            {
                Log("  ✓ " + it.FileName + "：" + msg, Theme.OkColor);
            }
            else
            {
                Log("  ✗ " + it.FileName + "：" + msg, Theme.ErrColor);
            }
        }

        private void bwExport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            object[] r = (object[])e.Result;
            int okCount = (int)r[0];
            int failCount = (int)r[1];
            int skipCount = (int)r[2];
            SetBusy(false);
            _exportRunning = false;
            Log("导出完成：成功 " + okCount + "，失败 " + failCount + "，跳过 " + skipCount + "。",
                failCount == 0 ? Theme.OkColor : Theme.ErrColor);

            // 导出完成后自动关闭 SolidWorks
            if (_closeSwAfterExport)
            {
                try
                {
                    _sw.CloseSolidWorks();
                    Log("已自动关闭 SolidWorks。", Theme.TextSub);
                }
                catch (Exception ex)
                {
                    Log("关闭 SolidWorks 失败：" + ex.Message, Theme.TextSub);
                }
            }
        }

        private static string SanitizeFileName(string name)
        {
            if (string.IsNullOrEmpty(name)) return "";
            char[] invalid = Path.GetInvalidFileNameChars();
            System.Text.StringBuilder sb = new System.Text.StringBuilder(name.Length);
            foreach (char c in name)
            {
                bool bad = false;
                foreach (char ic in invalid)
                {
                    if (c == ic) { bad = true; break; }
                }
                if (!bad) sb.Append(c);
            }
            return sb.ToString().Trim();
        }

        private static string UniquePath(string path)
        {
            if (!File.Exists(path)) return path;
            string dir = Path.GetDirectoryName(path);
            string name = Path.GetFileNameWithoutExtension(path);
            string ext = Path.GetExtension(path);
            int n = 2;
            while (File.Exists(Path.Combine(dir, name + "_" + n + ext)))
            {
                n++;
            }
            return Path.Combine(dir, name + "_" + n + ext);
        }

        #endregion

        #region 复制源文件

        private void btnCopySel_Click(object sender, EventArgs e)
        {
            List<EdmSearchItem> list = new List<EdmSearchItem>();
            foreach (DataGridViewRow row in dgvResults.SelectedRows)
            {
                EdmSearchItem it = row.Tag as EdmSearchItem;
                if (it != null) list.Add(it);
            }
            if (list.Count == 0)
            {
                Log("请先在结果列表中选择要复制的行。", Theme.ErrColor);
                return;
            }
            StartCopy(list, false);
        }

        private void btnCopyAll_Click(object sender, EventArgs e)
        {
            List<EdmSearchItem> list = new List<EdmSearchItem>();
            if (chkFirstOnly.Checked)
            {
                HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (EdmSearchItem it in _items)
                {
                    if (seen.Add(it.InputTerm)) list.Add(it);
                }
            }
            else
            {
                foreach (EdmSearchItem it in _items) list.Add(it);
            }
            if (list.Count == 0)
            {
                Log("当前没有可复制的匹配结果。", Theme.ErrColor);
                return;
            }
            StartCopy(list, false);
        }

        private void btnPdfSel_Click(object sender, EventArgs e)
        {
            List<EdmSearchItem> list = new List<EdmSearchItem>();
            foreach (DataGridViewRow row in dgvResults.SelectedRows)
            {
                EdmSearchItem it = row.Tag as EdmSearchItem;
                if (it != null) list.Add(it);
            }
            if (list.Count == 0)
            {
                Log("请先在右侧列表勾选要导出PDF的条目。", Theme.ErrColor);
                return;
            }
            StartCopy(list, true);
        }

        private void btnPdfAll_Click(object sender, EventArgs e)
        {
            List<EdmSearchItem> list = new List<EdmSearchItem>();
            if (chkFirstOnly.Checked)
            {
                HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (EdmSearchItem it in _items)
                {
                    if (seen.Add(it.InputTerm)) list.Add(it);
                }
            }
            else
            {
                foreach (EdmSearchItem it in _items) list.Add(it);
            }
            if (list.Count == 0)
            {
                Log("当前没有可导出PDF的匹配结果。", Theme.ErrColor);
                return;
            }
            StartCopy(list, true);
        }

        private void StartCopy(List<EdmSearchItem> list, bool exportPdf)
        {
            if (_searchRunning || _exportRunning) return;

            List<EdmSearchItem> copyable = list.FindAll(delegate(EdmSearchItem x) { return x.IsExportable; });
            if (copyable.Count == 0)
            {
                Log("所选条目中没有模型文件（.sldprt / .sldasm）。", Theme.ErrColor);
                return;
            }

            string outDir = txtOutputDir.Text.Trim();
            if (outDir.Length == 0)
            {
                outDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "PDM_STEP导出");
            }
            try
            {
                Directory.CreateDirectory(outDir);
            }
            catch (Exception ex)
            {
                Log("无法创建输出目录：" + ex.Message, Theme.ErrColor);
                return;
            }
            txtOutputDir.Text = outDir;

            _exportRunning = true;
            SetBusy(true);
            pgbProgress.SetRange(0, copyable.Count);
            pgbProgress.Value = 0;
            Log((exportPdf ? "开始导出 " : "开始复制 ") + copyable.Count + (exportPdf ? " 个PDF图纸 → " : " 个源文件 → ") + outDir + (exportPdf ? "（根据3D文件定位PDF）" : "（无需 SolidWorks）"), Theme.TextMain);

            ExportOptions opt = new ExportOptions();
            opt.UseInputName = chkUseInputName.Checked;
            opt.GetLatest = chkGetLatest.Checked;
            opt.ExportPdf = exportPdf;
            bwCopy.RunWorkerAsync(new object[] { copyable, outDir, opt });
        }

        private void bwCopy_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] args = (object[])e.Argument;
            List<EdmSearchItem> list = (List<EdmSearchItem>)args[0];
            string outDir = (string)args[1];
            ExportOptions opt = (ExportOptions)args[2];
            bool useInputName = opt.UseInputName;
            bool getLatest = opt.GetLatest;

            int okCount = 0;
            int failCount = 0;
            int skipCount = 0;
            int i = 0;

            foreach (EdmSearchItem it in list)
            {
                i++;
                try
                {
                    // 1) 确保本地副本（勾选 Get 最新版本时，无论本地是否已有副本都强制取最新版）
                    if (getLatest || !it.HasLocalCopy)
                    {
                        string getErr;
                        bool got = _edm.EnsureLocalCopy(it, getLatest, out getErr);
                        if (!got || !File.Exists(it.LocalPath))
                        {
                            skipCount++;
                            string detail = string.IsNullOrEmpty(getErr)
                                ? string.Format("本地路径='{0}'", it.LocalPath)
                                : getErr;
                            bwCopy.ReportProgress(i, new object[] { it, "跳过：本地无副本。" + detail, false });
                            continue;
                        }
                    }
                    if (!File.Exists(it.LocalPath))
                    {
                        skipCount++;
                        bwCopy.ReportProgress(i, new object[] { it, "跳过：本地文件不存在：" + it.LocalPath, false });
                        continue;
                    }

                    string srcPath = it.LocalPath;
                    string srcFileName = it.FileName;

                    // PDF 模式：根据3D文件定位PDF并Get
                    if (opt.ExportPdf)
                    {
                        string pdfErr;
                        string pdfPath = _edm.FindAndGetPdf(it.LocalPath, it.ParentFolderId, getLatest, out pdfErr);
                        if (string.IsNullOrEmpty(pdfPath) || !File.Exists(pdfPath))
                        {
                            skipCount++;
                            bwCopy.ReportProgress(i, new object[] { it, "跳过：" + (string.IsNullOrEmpty(pdfErr) ? "PDF未找到" : pdfErr), false });
                            continue;
                        }
                        srcPath = pdfPath;
                        srcFileName = Path.GetFileName(pdfPath);
                    }

                    // 2) 计算目标文件名（自动避开重名）
                    string ext = Path.GetExtension(srcFileName);
                    string baseName = useInputName ? it.InputTerm : Path.GetFileNameWithoutExtension(srcFileName);
                    baseName = SanitizeFileName(baseName);
                    if (baseName.Length == 0) baseName = Path.GetFileNameWithoutExtension(srcFileName);
                    string destPath = UniquePath(Path.Combine(outDir, baseName + ext));

                    // 3) 复制文件
                    File.Copy(srcPath, destPath, true);
                    okCount++;
                    bwCopy.ReportProgress(i, new object[] { it, "OK → " + Path.GetFileName(destPath), true });
                }
                catch (Exception ex)
                {
                    failCount++;
                    bwCopy.ReportProgress(i, new object[] { it, "异常：" + ex.Message, false });
                }
            }

            e.Result = new object[] { okCount, failCount, skipCount };
        }

        private void bwCopy_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pgbProgress.Value = e.ProgressPercentage;
            lblProgress.Text = e.ProgressPercentage + " / " + pgbProgress.Maximum;
            object[] st = (object[])e.UserState;
            EdmSearchItem it = (EdmSearchItem)st[0];
            string msg = (string)st[1];
            bool ok = (bool)st[2];
            if (ok)
            {
                Log("  ✓ " + it.FileName + "：" + msg, Theme.OkColor);
            }
            else
            {
                Log("  ✗ " + it.FileName + "：" + msg, Theme.ErrColor);
            }
        }

        private void bwCopy_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            object[] r = (object[])e.Result;
            int okCount = (int)r[0];
            int failCount = (int)r[1];
            int skipCount = (int)r[2];
            SetBusy(false);
            _exportRunning = false;
            Log("复制完成：成功 " + okCount + "，失败 " + failCount + "，跳过 " + skipCount + "。",
                failCount == 0 ? Theme.OkColor : Theme.ErrColor);
        }

        #endregion

        #region 其他界面事件

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (fbdOutput.ShowDialog(this) == DialogResult.OK)
            {
                txtOutputDir.Text = fbdOutput.SelectedPath;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (ofdImport.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    string content = File.ReadAllText(ofdImport.FileName, System.Text.Encoding.UTF8);
                    if (txtInput.Text.Trim().Length > 0) txtInput.AppendText(Environment.NewLine);
                    txtInput.AppendText(content);
                    Log("已从文件导入：" + ofdImport.FileName, Theme.TextSub);
                }
                catch (Exception ex)
                {
                    Log("导入文件失败：" + ex.Message, Theme.ErrColor);
                }
            }
        }

        private void btnClearInput_Click(object sender, EventArgs e)
        {
            txtInput.Clear();
        }

        private void btnClearGrid_Click(object sender, EventArgs e)
        {
            _items.Clear();
            dgvResults.Rows.Clear();
            lblResultCount.Text = "匹配结果：0 条";
        }

        #endregion
    }
}
