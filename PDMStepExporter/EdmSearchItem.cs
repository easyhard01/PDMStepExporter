using EPDM.Interop.epdm;

namespace PDMStepExporter
{
    /// <summary>
    /// 一条搜索结果（对应 PDM 库中的一个文件）。
    /// </summary>
    public class EdmSearchItem
    {
        /// <summary>用户输入的检索词（物料编码或规格型号）。</summary>
        public string InputTerm { get; set; }

        /// <summary>命中的变量名（物料编码 / 规格型号）。</summary>
        public string MatchedField { get; set; }

        /// <summary>文件名（含扩展名）。</summary>
        public string FileName { get; set; }

        /// <summary>库内版本号。</summary>
        public int Version { get; set; }

        /// <summary>文件所在父文件夹 ID（用于计算本地路径）。</summary>
        public long ParentFolderId { get; set; }

        /// <summary>本地视图路径（根文件夹拼接，不一定真实存在）。</summary>
        public string LocalPath { get; set; }

        /// <summary>本地是否已有副本。</summary>
        public bool HasLocalCopy { get; set; }

        /// <summary>是否可导出（.sldprt / .sldasm）。</summary>
        public bool IsExportable { get; set; }

        /// <summary>对应的 EPDM 文件对象（供 Get 最新版本使用）。</summary>
        public IEdmFile5 File { get; set; }
    }
}
