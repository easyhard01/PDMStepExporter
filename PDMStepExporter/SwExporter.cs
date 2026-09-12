using System;
using System.IO;
using System.Runtime.InteropServices;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace PDMStepExporter
{
    /// <summary>
    /// SolidWorks 封装：连接实例、打开模型并另存为 STEP。
    /// 早绑定，需引用 SolidWorks.Interop.sldworks.dll 和 SolidWorks.Interop.swconst.dll。
    /// 保存格式由文件扩展名决定（.STEP）。
    /// </summary>
    public class SwExporter
    {
        private SldWorks _sw;

        public bool IsConnected
        {
            get { return _sw != null; }
        }

        /// <summary>
        /// 连接正在运行的 SolidWorks；若未运行则启动一个新的实例。
        /// 返回 null 表示成功，否则返回错误信息。
        /// </summary>
        public string Connect()
        {
            if (_sw != null) return null;
            try
            {
                _sw = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
            }
            catch (Exception)
            {
                try
                {
                    _sw = new SldWorks();
                    // 由本工具启动的 SolidWorks 显示窗口，方便用户观察导出过程
                    _sw.Visible = true;
                }
                catch (Exception ex)
                {
                    _sw = null;
                    return "无法启动或连接 SolidWorks：" + ex.Message;
                }
            }
            // 连接成功后，强制 STEP 导出格式为 AP214（最通用的实体模型 STEP 格式）
            SetStepFormatAp214();
            return null;
        }

        /// <summary>
        /// 通过反射设置 SolidWorks STEP 导出格式为 AP214。
        /// 用反射取枚举值而非硬编码，兼容不同 SolidWorks 版本；设置失败不影响导出。
        /// </summary>
        private void SetStepFormatAp214()
        {
            try
            {
                Type tPref = Type.GetType(
                    "SolidWorks.Interop.swconst.swUserPreferenceIntegerValue_e, SolidWorks.Interop.swconst");
                Type tAp = Type.GetType(
                    "SolidWorks.Interop.swconst.swStepAP_e, SolidWorks.Interop.swconst");
                if (tPref == null || tAp == null) return;
                object prefVal = Enum.Parse(tPref, "swStepFormat");
                object apVal = Enum.Parse(tAp, "swStepAP214");
                _sw.SetUserPreferenceInteger((int)prefVal, (int)apVal);
            }
            catch (Exception)
            {
                // 设置失败忽略，使用 SolidWorks 系统默认 STEP 格式
            }
        }

        /// <summary>
        /// 将模型文件另存为 STEP。
        /// </summary>
        /// <param name="srcPath">SolidWorks 模型本地路径（.sldprt / .sldasm）。</param>
        /// <param name="stepPath">目标 STEP 文件完整路径。</param>
        /// <param name="message">失败时的错误信息。</param>
        /// <returns>是否成功。</returns>
        public bool ExportStep(string srcPath, string stepPath, out string message)
        {
            message = "";
            string err = Connect();
            if (err != null)
            {
                message = err;
                return false;
            }

            try
            {
                string ext = Path.GetExtension(srcPath).ToLowerInvariant();
                bool isAssembly = (ext == ".sldasm" || ext == ".asm");
                int docType = isAssembly
                    ? (int)swDocumentTypes_e.swDocASSEMBLY
                    : (int)swDocumentTypes_e.swDocPART;

                // 若该文档已在 SolidWorks 中打开，直接复用，导出后不关闭
                ModelDoc2 opened = null;
                try
                {
                    opened = (ModelDoc2)_sw.GetOpenDocumentByName(srcPath);
                }
                catch (Exception)
                {
                    opened = null;
                }
                bool alreadyOpen = opened != null;

                int openErrors = 0;
                int openWarnings = 0;
                ModelDoc2 doc = (ModelDoc2)_sw.OpenDoc6(
                    srcPath,
                    docType,
                    (int)swOpenDocOptions_e.swOpenDocOptions_Silent,
                    "",
                    ref openErrors,
                    ref openWarnings);

                if (doc == null)
                {
                    string errDetail = "打开模型失败（err=" + openErrors + ", warn=" + openWarnings + "）。";
                    if (isAssembly)
                    {
                        errDetail += "装配体可能缺少子零件引用，请确保所有引用文件已在 PDM 本地视图中。";
                    }
                    else
                    {
                        errDetail += "请检查文件是否被占用、是否缺少关联版本或模型文件损坏。";
                    }
                    message = errDetail;
                    return false;
                }

                ModelDocExtension modelExt = (ModelDocExtension)doc.Extension;
                int saveErrors = 0;
                int saveWarnings = 0;
                bool ok = modelExt.SaveAs(
                    stepPath,
                    (int)swSaveAsVersion_e.swSaveAsCurrentVersion,
                    (int)swSaveAsOptions_e.swSaveAsOptions_Silent,
                    null,
                    ref saveErrors,
                    ref saveWarnings);

                if (!alreadyOpen)
                {
                    try
                    {
                        _sw.CloseDoc(doc.GetTitle());
                    }
                    catch (Exception)
                    {
                        // 关闭失败不影响导出结果
                    }
                }

                if (!ok)
                {
                    message = "STEP 保存失败（errors=" + saveErrors + ", warnings=" + saveWarnings + "）。"
                        + "可尝试在 SolidWorks 中手动另存为 STEP 检查选项设置。";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 强制退出 SolidWorks 应用程序（不管是否由本工具启动）。
        /// </summary>
        public void CloseSolidWorks()
        {
            if (_sw == null) return;
            try
            {
                _sw.ExitApp();
            }
            catch (Exception)
            {
                // 退出失败忽略
            }
            finally
            {
                _sw = null;
            }
        }
    }
}
