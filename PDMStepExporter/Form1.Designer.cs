namespace PDMStepExporter
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelHeader = new PDMStepExporter.GradientPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblStatusDot = new System.Windows.Forms.Label();
            this.lblStatusText = new System.Windows.Forms.Label();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.cardTips = new System.Windows.Forms.Panel();
            this.lblCard3 = new System.Windows.Forms.Label();
            this.lblTips = new System.Windows.Forms.Label();
            this.cardInput = new System.Windows.Forms.Panel();
            this.lblCard2 = new System.Windows.Forms.Label();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.lblHint = new System.Windows.Forms.Label();
            this.rbCode = new System.Windows.Forms.RadioButton();
            this.rbSpec = new System.Windows.Forms.RadioButton();
            this.rbAuto = new System.Windows.Forms.RadioButton();
            this.lblMatch = new System.Windows.Forms.Label();
            this.chkFuzzy = new System.Windows.Forms.CheckBox();
            this.chkRecurse = new System.Windows.Forms.CheckBox();
            this.btnSearch = new PDMStepExporter.FlatButton();
            this.btnImport = new PDMStepExporter.FlatButton();
            this.btnClearInput = new PDMStepExporter.FlatButton();
            this.cardConnection = new System.Windows.Forms.Panel();
            this.lblCard1 = new System.Windows.Forms.Label();
            this.cboVault = new System.Windows.Forms.ComboBox();
            this.btnRefresh = new PDMStepExporter.FlatButton();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.btnLogin = new PDMStepExporter.FlatButton();
            this.lblLoginState = new System.Windows.Forms.Label();
            this.panelResults = new System.Windows.Forms.Panel();
            this.lblResultCount = new System.Windows.Forms.Label();
            this.chkFirstOnly = new System.Windows.Forms.CheckBox();
            this.btnClearGrid = new PDMStepExporter.FlatButton();
            this.dgvResults = new System.Windows.Forms.DataGridView();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.lblOut = new System.Windows.Forms.Label();
            this.txtOutputDir = new System.Windows.Forms.TextBox();
            this.btnBrowse = new PDMStepExporter.FlatButton();
            this.chkUseInputName = new System.Windows.Forms.CheckBox();
            this.chkGetLatest = new System.Windows.Forms.CheckBox();
            this.chkCloseSw = new System.Windows.Forms.CheckBox();
            this.btnExportSel = new PDMStepExporter.FlatButton();
            this.btnExportAll = new PDMStepExporter.FlatButton();
            this.btnCopySel = new PDMStepExporter.FlatButton();
            this.btnCopyAll = new PDMStepExporter.FlatButton();
            this.lblProgress = new System.Windows.Forms.Label();
            this.pgbProgress = new PDMStepExporter.PurpleProgressBar();
            this.logRich = new System.Windows.Forms.RichTextBox();
            this.bwSearch = new System.ComponentModel.BackgroundWorker();
            this.bwExport = new System.ComponentModel.BackgroundWorker();
            this.bwCopy = new System.ComponentModel.BackgroundWorker();
            this.ofdImport = new System.Windows.Forms.OpenFileDialog();
            this.fbdOutput = new System.Windows.Forms.FolderBrowserDialog();
            this.tipMain = new System.Windows.Forms.ToolTip(this.components);
            this.panelHeader.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.cardTips.SuspendLayout();
            this.cardInput.SuspendLayout();
            this.cardConnection.SuspendLayout();
            this.panelResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.ColorBottom = System.Drawing.Color.FromArgb(155, 89, 182);
            this.panelHeader.ColorTop = System.Drawing.Color.FromArgb(102, 0, 153);
            this.panelHeader.Controls.Add(this.lblStatusText);
            this.panelHeader.Controls.Add(this.lblStatusDot);
            this.panelHeader.Controls.Add(this.lblSubtitle);
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1080, 84);
            this.panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 17F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(28, 14);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(380, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "PDM 物料检索 · STEP 批量导出";
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.5F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(233, 217, 247);
            this.lblSubtitle.Location = new System.Drawing.Point(30, 52);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(360, 17);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "SOLIDWORKS Enterprise PDM 2018 ｜ 按物料编码 / 规格型号批量搜索并导出 STEP";
            // 
            // lblStatusDot
            // 
            this.lblStatusDot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatusDot.AutoSize = true;
            this.lblStatusDot.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblStatusDot.ForeColor = System.Drawing.Color.FromArgb(255, 107, 107);
            this.lblStatusDot.Location = new System.Drawing.Point(1044, 30);
            this.lblStatusDot.Name = "lblStatusDot";
            this.lblStatusDot.Size = new System.Drawing.Size(22, 22);
            this.lblStatusDot.TabIndex = 2;
            this.lblStatusDot.Text = "●";
            this.lblStatusDot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStatusText
            // 
            this.lblStatusText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatusText.AutoSize = true;
            this.lblStatusText.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblStatusText.ForeColor = System.Drawing.Color.White;
            this.lblStatusText.Location = new System.Drawing.Point(958, 33);
            this.lblStatusText.Name = "lblStatusText";
            this.lblStatusText.Size = new System.Drawing.Size(44, 17);
            this.lblStatusText.TabIndex = 3;
            this.lblStatusText.Text = "未连接";
            // 
            // splitMain
            // 
            this.splitMain.BackColor = System.Drawing.Color.FromArgb(237, 227, 245);
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitMain.Location = new System.Drawing.Point(0, 84);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.panelLeft);
            this.splitMain.Panel1MinSize = 360;
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.panelResults);
            this.splitMain.Size = new System.Drawing.Size(1080, 480);
            this.splitMain.SplitterDistance = 400;
            this.splitMain.SplitterWidth = 6;
            this.splitMain.TabIndex = 1;
            // 
            // panelLeft
            // 
            this.panelLeft.AutoScroll = true;
            this.panelLeft.BackColor = System.Drawing.Color.FromArgb(244, 238, 249);
            this.panelLeft.Controls.Add(this.cardTips);
            this.panelLeft.Controls.Add(this.cardInput);
            this.panelLeft.Controls.Add(this.cardConnection);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Padding = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.panelLeft.Size = new System.Drawing.Size(400, 480);
            this.panelLeft.TabIndex = 0;
            // 
            // cardTips
            // 
            this.cardTips.BackColor = System.Drawing.Color.White;
            this.cardTips.Controls.Add(this.lblCard3);
            this.cardTips.Controls.Add(this.lblTips);
            this.cardTips.Dock = System.Windows.Forms.DockStyle.Top;
            this.cardTips.Location = new System.Drawing.Point(10, 486);
            this.cardTips.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.cardTips.Name = "cardTips";
            this.cardTips.Padding = new System.Windows.Forms.Padding(14, 10, 14, 10);
            this.cardTips.Size = new System.Drawing.Size(380, 106);
            this.cardTips.TabIndex = 2;
            // 
            // lblCard3
            // 
            this.lblCard3.AutoSize = true;
            this.lblCard3.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblCard3.ForeColor = System.Drawing.Color.FromArgb(102, 0, 153);
            this.lblCard3.Location = new System.Drawing.Point(14, 12);
            this.lblCard3.Name = "lblCard3";
            this.lblCard3.Size = new System.Drawing.Size(75, 19);
            this.lblCard3.TabIndex = 0;
            this.lblCard3.Text = "③ 使用提示";
            // 
            // lblTips
            // 
            this.lblTips.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.5F);
            this.lblTips.ForeColor = System.Drawing.Color.FromArgb(138, 111, 158);
            this.lblTips.Location = new System.Drawing.Point(14, 40);
            this.lblTips.Name = "lblTips";
            this.lblTips.Size = new System.Drawing.Size(352, 54);
            this.lblTips.TabIndex = 1;
            this.lblTips.Text = "· 精确匹配要求与库内变量值完全一致；模糊匹配使用“包含”逻辑\r\n· 结果仅保留 .sldprt / .sldasm 模型文件\r\n· 导出自动避开重名文件，同一输入多个匹配时默认仅导出第一个";
            // 
            // cardInput
            // 
            this.cardInput.BackColor = System.Drawing.Color.White;
            this.cardInput.Controls.Add(this.lblCard2);
            this.cardInput.Controls.Add(this.txtInput);
            this.cardInput.Controls.Add(this.lblHint);
            this.cardInput.Controls.Add(this.rbCode);
            this.cardInput.Controls.Add(this.rbSpec);
            this.cardInput.Controls.Add(this.rbAuto);
            this.cardInput.Controls.Add(this.lblMatch);
            this.cardInput.Controls.Add(this.chkFuzzy);
            this.cardInput.Controls.Add(this.chkRecurse);
            this.cardInput.Controls.Add(this.btnSearch);
            this.cardInput.Controls.Add(this.btnImport);
            this.cardInput.Controls.Add(this.btnClearInput);
            this.cardInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.cardInput.Location = new System.Drawing.Point(10, 200);
            this.cardInput.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.cardInput.Name = "cardInput";
            this.cardInput.Padding = new System.Windows.Forms.Padding(14, 10, 14, 10);
            this.cardInput.Size = new System.Drawing.Size(380, 286);
            this.cardInput.TabIndex = 1;
            // 
            // lblCard2
            // 
            this.lblCard2.AutoSize = true;
            this.lblCard2.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblCard2.ForeColor = System.Drawing.Color.FromArgb(102, 0, 153);
            this.lblCard2.Location = new System.Drawing.Point(14, 12);
            this.lblCard2.Name = "lblCard2";
            this.lblCard2.Size = new System.Drawing.Size(124, 19);
            this.lblCard2.TabIndex = 0;
            this.lblCard2.Text = "② 批量输入（每行一个）";
            // 
            // txtInput
            // 
            this.txtInput.AcceptsReturn = true;
            this.txtInput.BackColor = System.Drawing.Color.FromArgb(251, 249, 254);
            this.txtInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInput.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtInput.Location = new System.Drawing.Point(14, 38);
            this.txtInput.Multiline = true;
            this.txtInput.Name = "txtInput";
            this.txtInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInput.Size = new System.Drawing.Size(352, 112);
            this.txtInput.TabIndex = 1;
            this.tipMain.SetToolTip(this.txtInput, "每行输入一个物料编码或规格型号；可直接粘贴 Excel 中选中的多行内容");
            this.txtInput.WordWrap = false;
            // 
            // lblHint
            // 
            this.lblHint.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.5F);
            this.lblHint.ForeColor = System.Drawing.Color.FromArgb(154, 134, 173);
            this.lblHint.Location = new System.Drawing.Point(14, 156);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(352, 18);
            this.lblHint.TabIndex = 2;
            this.lblHint.Text = "支持粘贴 Excel 多行 / 制表符分隔内容，每行一个编码或型号";
            // 
            // rbCode
            // 
            this.rbCode.AutoSize = true;
            this.rbCode.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            this.rbCode.Location = new System.Drawing.Point(14, 182);
            this.rbCode.Name = "rbCode";
            this.rbCode.Size = new System.Drawing.Size(88, 21);
            this.rbCode.TabIndex = 3;
            this.rbCode.TabStop = true;
            this.rbCode.Text = "物料编码";
            this.rbCode.UseVisualStyleBackColor = true;
            // 
            // rbSpec
            // 
            this.rbSpec.AutoSize = true;
            this.rbSpec.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            this.rbSpec.Location = new System.Drawing.Point(108, 182);
            this.rbSpec.Name = "rbSpec";
            this.rbSpec.Size = new System.Drawing.Size(88, 21);
            this.rbSpec.TabIndex = 4;
            this.rbSpec.TabStop = true;
            this.rbSpec.Text = "规格型号";
            this.rbSpec.UseVisualStyleBackColor = true;
            // 
            // rbAuto
            // 
            this.rbAuto.AutoSize = true;
            this.rbAuto.Checked = true;
            this.rbAuto.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            this.rbAuto.Location = new System.Drawing.Point(206, 182);
            this.rbAuto.Name = "rbAuto";
            this.rbAuto.Size = new System.Drawing.Size(145, 21);
            this.rbAuto.TabIndex = 5;
            this.rbAuto.TabStop = true;
            this.rbAuto.Text = "自动判断（先编码后型号）";
            this.rbAuto.UseVisualStyleBackColor = true;
            // 
            // lblMatch
            // 
            this.lblMatch.AutoSize = true;
            this.lblMatch.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            this.lblMatch.Location = new System.Drawing.Point(14, 214);
            this.lblMatch.Name = "lblMatch";
            this.lblMatch.Size = new System.Drawing.Size(72, 17);
            this.lblMatch.TabIndex = 6;
            this.lblMatch.Text = "匹配方式：";
            // 
            // chkFuzzy
            // 
            this.chkFuzzy.AutoSize = true;
            this.chkFuzzy.Checked = true;
            this.chkFuzzy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFuzzy.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            this.chkFuzzy.Location = new System.Drawing.Point(92, 212);
            this.chkFuzzy.Name = "chkFuzzy";
            this.chkFuzzy.Size = new System.Drawing.Size(112, 21);
            this.chkFuzzy.TabIndex = 7;
            this.chkFuzzy.Text = "模糊匹配（包含）";
            this.tipMain.SetToolTip(this.chkFuzzy, "勾选后按“包含”逻辑匹配（如输入 AB 可命中 AB-001），不勾选要求完全一致");
            this.chkFuzzy.UseVisualStyleBackColor = true;
            // 
            // chkRecurse
            // 
            this.chkRecurse.AutoSize = true;
            this.chkRecurse.Checked = true;
            this.chkRecurse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRecurse.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            this.chkRecurse.Location = new System.Drawing.Point(224, 212);
            this.chkRecurse.Name = "chkRecurse";
            this.chkRecurse.Size = new System.Drawing.Size(88, 21);
            this.chkRecurse.TabIndex = 8;
            this.chkRecurse.Text = "递归搜索";
            this.chkRecurse.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(14, 242);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Primary = true;
            this.btnSearch.Size = new System.Drawing.Size(150, 36);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "开始搜索";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnImport
            // 
            this.btnImport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnImport.Location = new System.Drawing.Point(172, 242);
            this.btnImport.Name = "btnImport";
            this.btnImport.Primary = false;
            this.btnImport.Size = new System.Drawing.Size(72, 36);
            this.btnImport.TabIndex = 10;
            this.btnImport.Text = "导入…";
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnClearInput
            // 
            this.btnClearInput.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClearInput.Location = new System.Drawing.Point(252, 242);
            this.btnClearInput.Name = "btnClearInput";
            this.btnClearInput.Primary = false;
            this.btnClearInput.Size = new System.Drawing.Size(72, 36);
            this.btnClearInput.TabIndex = 11;
            this.btnClearInput.Text = "清空";
            this.btnClearInput.Click += new System.EventHandler(this.btnClearInput_Click);
            // 
            // cardConnection
            // 
            this.cardConnection.BackColor = System.Drawing.Color.White;
            this.cardConnection.Controls.Add(this.lblCard1);
            this.cardConnection.Controls.Add(this.cboVault);
            this.cardConnection.Controls.Add(this.btnRefresh);
            this.cardConnection.Controls.Add(this.txtUser);
            this.cardConnection.Controls.Add(this.txtPwd);
            this.cardConnection.Controls.Add(this.btnLogin);
            this.cardConnection.Controls.Add(this.lblLoginState);
            this.cardConnection.Dock = System.Windows.Forms.DockStyle.Top;
            this.cardConnection.Location = new System.Drawing.Point(10, 10);
            this.cardConnection.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.cardConnection.Name = "cardConnection";
            this.cardConnection.Padding = new System.Windows.Forms.Padding(14, 10, 14, 10);
            this.cardConnection.Size = new System.Drawing.Size(380, 180);
            this.cardConnection.TabIndex = 0;
            // 
            // lblCard1
            // 
            this.lblCard1.AutoSize = true;
            this.lblCard1.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblCard1.ForeColor = System.Drawing.Color.FromArgb(102, 0, 153);
            this.lblCard1.Location = new System.Drawing.Point(14, 12);
            this.lblCard1.Name = "lblCard1";
            this.lblCard1.Size = new System.Drawing.Size(105, 19);
            this.lblCard1.TabIndex = 0;
            this.lblCard1.Text = "① 连接 PDM 库";
            // 
            // cboVault
            // 
            this.cboVault.BackColor = System.Drawing.Color.FromArgb(251, 249, 254);
            this.cboVault.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboVault.IntegralHeight = false;
            this.cboVault.Location = new System.Drawing.Point(14, 40);
            this.cboVault.Name = "cboVault";
            this.cboVault.Size = new System.Drawing.Size(238, 25);
            this.cboVault.TabIndex = 1;
            this.tipMain.SetToolTip(this.cboVault, "本机已配置的 PDM 库视图（点击“刷新列表”重新扫描）");
            // 
            // btnRefresh
            // 
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.Location = new System.Drawing.Point(260, 38);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Primary = false;
            this.btnRefresh.Size = new System.Drawing.Size(80, 30);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "刷新列表";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // txtUser
            // 
            this.txtUser.BackColor = System.Drawing.Color.FromArgb(251, 249, 254);
            this.txtUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUser.Location = new System.Drawing.Point(14, 76);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(150, 27);
            this.txtUser.TabIndex = 3;
            this.tipMain.SetToolTip(this.txtUser, "留空则使用当前 Windows 身份登录");
            // 
            // txtPwd
            // 
            this.txtPwd.BackColor = System.Drawing.Color.FromArgb(251, 249, 254);
            this.txtPwd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPwd.Location = new System.Drawing.Point(172, 76);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Size = new System.Drawing.Size(160, 27);
            this.txtPwd.TabIndex = 4;
            this.tipMain.SetToolTip(this.txtPwd, "留空则使用当前 Windows 身份登录");
            // 
            // btnLogin
            // 
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnLogin.Location = new System.Drawing.Point(14, 114);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Primary = true;
            this.btnLogin.Size = new System.Drawing.Size(120, 34);
            this.btnLogin.TabIndex = 5;
            this.btnLogin.Text = "连接 PDM";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // lblLoginState
            // 
            this.lblLoginState.AutoEllipsis = true;
            this.lblLoginState.ForeColor = System.Drawing.Color.FromArgb(138, 111, 158);
            this.lblLoginState.Location = new System.Drawing.Point(144, 120);
            this.lblLoginState.Name = "lblLoginState";
            this.lblLoginState.Size = new System.Drawing.Size(192, 22);
            this.lblLoginState.TabIndex = 6;
            this.lblLoginState.Text = "未连接";
            this.lblLoginState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelResults
            // 
            this.panelResults.BackColor = System.Drawing.Color.FromArgb(251, 249, 254);
            this.panelResults.Controls.Add(this.lblResultCount);
            this.panelResults.Controls.Add(this.chkFirstOnly);
            this.panelResults.Controls.Add(this.btnClearGrid);
            this.panelResults.Controls.Add(this.dgvResults);
            this.panelResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelResults.Location = new System.Drawing.Point(0, 0);
            this.panelResults.Name = "panelResults";
            this.panelResults.Padding = new System.Windows.Forms.Padding(12, 40, 12, 10);
            this.panelResults.Size = new System.Drawing.Size(674, 480);
            this.panelResults.TabIndex = 0;
            // 
            // lblResultCount
            // 
            this.lblResultCount.AutoSize = true;
            this.lblResultCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblResultCount.ForeColor = System.Drawing.Color.FromArgb(102, 0, 153);
            this.lblResultCount.Location = new System.Drawing.Point(14, 12);
            this.lblResultCount.Name = "lblResultCount";
            this.lblResultCount.Size = new System.Drawing.Size(110, 20);
            this.lblResultCount.TabIndex = 0;
            this.lblResultCount.Text = "匹配结果：0 条";
            // 
            // chkFirstOnly
            // 
            this.chkFirstOnly.AutoSize = true;
            this.chkFirstOnly.Checked = true;
            this.chkFirstOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFirstOnly.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            this.chkFirstOnly.Location = new System.Drawing.Point(160, 14);
            this.chkFirstOnly.Name = "chkFirstOnly";
            this.chkFirstOnly.Size = new System.Drawing.Size(208, 21);
            this.chkFirstOnly.TabIndex = 1;
            this.chkFirstOnly.Text = "每个输入仅导出第一个匹配项";
            this.tipMain.SetToolTip(this.chkFirstOnly, "勾选时“导出全部”对每个输入只导出第一个匹配文件；取消勾选则导出全部匹配");
            this.chkFirstOnly.UseVisualStyleBackColor = true;
            // 
            // btnClearGrid
            // 
            this.btnClearGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearGrid.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClearGrid.Location = new System.Drawing.Point(582, 8);
            this.btnClearGrid.Name = "btnClearGrid";
            this.btnClearGrid.Primary = false;
            this.btnClearGrid.Size = new System.Drawing.Size(80, 26);
            this.btnClearGrid.TabIndex = 2;
            this.btnClearGrid.Text = "清空列表";
            this.btnClearGrid.Click += new System.EventHandler(this.btnClearGrid_Click);
            // 
            // dgvResults
            // 
            this.dgvResults.AllowUserToAddRows = false;
            this.dgvResults.AllowUserToDeleteRows = false;
            this.dgvResults.AllowUserToResizeRows = false;
            this.dgvResults.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(247, 242, 251);
            this.dgvResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvResults.BackgroundColor = System.Drawing.Color.White;
            this.dgvResults.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvResults.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvResults.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvResults.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(102, 0, 153);
            this.dgvResults.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.dgvResults.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvResults.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(102, 0, 153);
            this.dgvResults.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvResults.ColumnHeadersHeight = 34;
            this.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvResults.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvResults.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            this.dgvResults.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(233, 216, 245);
            this.dgvResults.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            this.dgvResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResults.EnableHeadersVisualStyles = false;
            this.dgvResults.GridColor = System.Drawing.Color.FromArgb(232, 224, 239);
            this.dgvResults.Location = new System.Drawing.Point(12, 40);
            this.dgvResults.MultiSelect = true;
            this.dgvResults.Name = "dgvResults";
            this.dgvResults.ReadOnly = true;
            this.dgvResults.RowHeadersVisible = false;
            this.dgvResults.RowTemplate.Height = 28;
            this.dgvResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResults.Size = new System.Drawing.Size(650, 430);
            this.dgvResults.TabIndex = 3;
            this.dgvResults.Columns.Add("colTerm", "输入项");
            this.dgvResults.Columns.Add("colField", "匹配字段");
            this.dgvResults.Columns.Add("colName", "文件名");
            this.dgvResults.Columns.Add("colVersion", "版本");
            this.dgvResults.Columns.Add("colLocal", "本地路径");
            this.dgvResults.Columns.Add("colHas", "本地副本");
            this.dgvResults.Columns.Add("colExport", "可导出");
            this.dgvResults.Columns[0].FillWeight = 100F;
            this.dgvResults.Columns[1].FillWeight = 80F;
            this.dgvResults.Columns[2].FillWeight = 120F;
            this.dgvResults.Columns[3].FillWeight = 50F;
            this.dgvResults.Columns[4].FillWeight = 200F;
            this.dgvResults.Columns[5].FillWeight = 70F;
            this.dgvResults.Columns[6].FillWeight = 65F;
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.White;
            this.panelBottom.Controls.Add(this.lblOut);
            this.panelBottom.Controls.Add(this.txtOutputDir);
            this.panelBottom.Controls.Add(this.btnBrowse);
            this.panelBottom.Controls.Add(this.chkUseInputName);
            this.panelBottom.Controls.Add(this.chkGetLatest);
            this.panelBottom.Controls.Add(this.chkCloseSw);
            this.panelBottom.Controls.Add(this.btnExportSel);
            this.panelBottom.Controls.Add(this.btnExportAll);
            this.panelBottom.Controls.Add(this.btnCopySel);
            this.panelBottom.Controls.Add(this.btnCopyAll);
            this.panelBottom.Controls.Add(this.lblProgress);
            this.panelBottom.Controls.Add(this.pgbProgress);
            this.panelBottom.Controls.Add(this.logRich);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 564);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Padding = new System.Windows.Forms.Padding(14, 8, 14, 8);
            this.panelBottom.Size = new System.Drawing.Size(1080, 232);
            this.panelBottom.TabIndex = 2;
            // 
            // lblOut
            // 
            this.lblOut.AutoSize = true;
            this.lblOut.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            this.lblOut.Location = new System.Drawing.Point(14, 15);
            this.lblOut.Name = "lblOut";
            this.lblOut.Size = new System.Drawing.Size(72, 17);
            this.lblOut.TabIndex = 0;
            this.lblOut.Text = "输出文件夹：";
            // 
            // txtOutputDir
            // 
            this.txtOutputDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputDir.BackColor = System.Drawing.Color.FromArgb(251, 249, 254);
            this.txtOutputDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOutputDir.Location = new System.Drawing.Point(92, 10);
            this.txtOutputDir.Name = "txtOutputDir";
            this.txtOutputDir.Size = new System.Drawing.Size(850, 27);
            this.txtOutputDir.TabIndex = 1;
            this.tipMain.SetToolTip(this.txtOutputDir, "STEP 文件输出目录；留空时导出到 桌面\\PDM_STEP导出");
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowse.Location = new System.Drawing.Point(950, 8);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Primary = false;
            this.btnBrowse.Size = new System.Drawing.Size(116, 30);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "浏览…";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // chkUseInputName
            // 
            this.chkUseInputName.AutoSize = true;
            this.chkUseInputName.Checked = true;
            this.chkUseInputName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseInputName.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            this.chkUseInputName.Location = new System.Drawing.Point(14, 48);
            this.chkUseInputName.Name = "chkUseInputName";
            this.chkUseInputName.Size = new System.Drawing.Size(232, 21);
            this.chkUseInputName.TabIndex = 3;
            this.chkUseInputName.Text = "用输入项命名 STEP（如物料编码）";
            this.tipMain.SetToolTip(this.chkUseInputName, "勾选时 STEP 文件以输入的物料编码/规格型号命名；取消勾选则使用原文件名");
            this.chkUseInputName.UseVisualStyleBackColor = true;
            // 
            // chkGetLatest
            // 
            this.chkGetLatest.AutoSize = true;
            this.chkGetLatest.Checked = true;
            this.chkGetLatest.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGetLatest.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            this.chkGetLatest.Location = new System.Drawing.Point(330, 48);
            this.chkGetLatest.Name = "chkGetLatest";
            this.chkGetLatest.Size = new System.Drawing.Size(164, 21);
            this.chkGetLatest.TabIndex = 4;
            this.chkGetLatest.Text = "导出前 Get 最新版本";
            this.tipMain.SetToolTip(this.chkGetLatest, "本地无副本时自动调用 EPDM 的 Get（取最新版本到本地视图）");
            this.chkGetLatest.UseVisualStyleBackColor = true;
            // 
            // chkCloseSw
            // 
            this.chkCloseSw.AutoSize = true;
            this.chkCloseSw.Checked = true;
            this.chkCloseSw.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCloseSw.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            this.chkCloseSw.Location = new System.Drawing.Point(510, 48);
            this.chkCloseSw.Name = "chkCloseSw";
            this.chkCloseSw.Size = new System.Drawing.Size(176, 21);
            this.chkCloseSw.TabIndex = 7;
            this.chkCloseSw.Text = "导出完成后关闭 SolidWorks";
            this.tipMain.SetToolTip(this.chkCloseSw, "批量导出完成后自动退出 SolidWorks（包括已打开的实例）");
            this.chkCloseSw.UseVisualStyleBackColor = true;
            // 
            // btnExportSel
            // 
            this.btnExportSel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportSel.Location = new System.Drawing.Point(14, 84);
            this.btnExportSel.Name = "btnExportSel";
            this.btnExportSel.Primary = false;
            this.btnExportSel.Size = new System.Drawing.Size(150, 36);
            this.btnExportSel.TabIndex = 5;
            this.btnExportSel.Text = "导出选中为 STEP";
            this.btnExportSel.Click += new System.EventHandler(this.btnExportSel_Click);
            // 
            // btnExportAll
            // 
            this.btnExportAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportAll.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnExportAll.Location = new System.Drawing.Point(172, 84);
            this.btnExportAll.Name = "btnExportAll";
            this.btnExportAll.Primary = true;
            this.btnExportAll.Size = new System.Drawing.Size(150, 36);
            this.btnExportAll.TabIndex = 6;
            this.btnExportAll.Text = "导出全部为 STEP";
            this.btnExportAll.Click += new System.EventHandler(this.btnExportAll_Click);
            // 
            // btnCopySel
            // 
            this.btnCopySel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCopySel.Location = new System.Drawing.Point(14, 124);
            this.btnCopySel.Name = "btnCopySel";
            this.btnCopySel.Primary = false;
            this.btnCopySel.Size = new System.Drawing.Size(150, 30);
            this.btnCopySel.TabIndex = 10;
            this.btnCopySel.Text = "复制选中源文件";
            this.btnCopySel.Click += new System.EventHandler(this.btnCopySel_Click);
            // 
            // btnCopyAll
            // 
            this.btnCopyAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCopyAll.Location = new System.Drawing.Point(172, 124);
            this.btnCopyAll.Name = "btnCopyAll";
            this.btnCopyAll.Primary = false;
            this.btnCopyAll.Size = new System.Drawing.Size(150, 30);
            this.btnCopyAll.TabIndex = 11;
            this.btnCopyAll.Text = "复制全部源文件";
            this.btnCopyAll.Click += new System.EventHandler(this.btnCopyAll_Click);
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.ForeColor = System.Drawing.Color.FromArgb(138, 111, 158);
            this.lblProgress.Location = new System.Drawing.Point(340, 92);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(32, 17);
            this.lblProgress.TabIndex = 7;
            this.lblProgress.Text = "就绪";
            // 
            // pgbProgress
            // 
            this.pgbProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pgbProgress.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.5F);
            this.pgbProgress.Location = new System.Drawing.Point(440, 88);
            this.pgbProgress.Name = "pgbProgress";
            this.pgbProgress.Size = new System.Drawing.Size(626, 20);
            this.pgbProgress.TabIndex = 8;
            // 
            // logRich
            // 
            this.logRich.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.logRich.BackColor = System.Drawing.Color.FromArgb(250, 247, 253);
            this.logRich.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logRich.Font = new System.Drawing.Font("Consolas", 9F);
            this.logRich.Location = new System.Drawing.Point(14, 160);
            this.logRich.Name = "logRich";
            this.logRich.ReadOnly = true;
            this.logRich.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.logRich.Size = new System.Drawing.Size(1052, 56);
            this.logRich.TabIndex = 9;
            this.logRich.Text = "";
            // 
            // bwSearch
            // 
            this.bwSearch.WorkerReportsProgress = true;
            this.bwSearch.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwSearch_DoWork);
            this.bwSearch.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwSearch_ProgressChanged);
            this.bwSearch.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwSearch_RunWorkerCompleted);
            // 
            // bwExport
            // 
            this.bwExport.WorkerReportsProgress = true;
            this.bwExport.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwExport_DoWork);
            this.bwExport.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwExport_ProgressChanged);
            this.bwExport.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwExport_RunWorkerCompleted);
            // 
            // bwCopy
            // 
            this.bwCopy.WorkerReportsProgress = true;
            this.bwCopy.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwCopy_DoWork);
            this.bwCopy.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwCopy_ProgressChanged);
            this.bwCopy.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwCopy_RunWorkerCompleted);
            // 
            // ofdImport
            // 
            this.ofdImport.Filter = "文本文件 (*.txt)|*.txt|所有文件 (*.*)|*.*";
            this.ofdImport.Title = "选择批量输入文件";
            // 
            // fbdOutput
            // 
            this.fbdOutput.Description = "选择 STEP 输出目录";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AcceptButton = this.btnSearch;
            this.BackColor = System.Drawing.Color.FromArgb(244, 238, 249);
            this.ClientSize = new System.Drawing.Size(1080, 760);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelHeader);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.MinimumSize = new System.Drawing.Size(960, 660);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PDM 物料检索 · STEP 批量导出工具（清华紫）";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            this.splitMain.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.cardTips.ResumeLayout(false);
            this.cardTips.PerformLayout();
            this.cardInput.ResumeLayout(false);
            this.cardInput.PerformLayout();
            this.cardConnection.ResumeLayout(false);
            this.cardConnection.PerformLayout();
            this.panelResults.ResumeLayout(false);
            this.panelResults.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private PDMStepExporter.GradientPanel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblStatusDot;
        private System.Windows.Forms.Label lblStatusText;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel cardTips;
        private System.Windows.Forms.Label lblCard3;
        private System.Windows.Forms.Label lblTips;
        private System.Windows.Forms.Panel cardInput;
        private System.Windows.Forms.Label lblCard2;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.RadioButton rbCode;
        private System.Windows.Forms.RadioButton rbSpec;
        private System.Windows.Forms.RadioButton rbAuto;
        private System.Windows.Forms.Label lblMatch;
        private System.Windows.Forms.CheckBox chkFuzzy;
        private System.Windows.Forms.CheckBox chkRecurse;
        private PDMStepExporter.FlatButton btnSearch;
        private PDMStepExporter.FlatButton btnImport;
        private PDMStepExporter.FlatButton btnClearInput;
        private System.Windows.Forms.Panel cardConnection;
        private System.Windows.Forms.Label lblCard1;
        private System.Windows.Forms.ComboBox cboVault;
        private PDMStepExporter.FlatButton btnRefresh;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtPwd;
        private PDMStepExporter.FlatButton btnLogin;
        private System.Windows.Forms.Label lblLoginState;
        private System.Windows.Forms.Panel panelResults;
        private System.Windows.Forms.Label lblResultCount;
        private System.Windows.Forms.CheckBox chkFirstOnly;
        private PDMStepExporter.FlatButton btnClearGrid;
        private System.Windows.Forms.DataGridView dgvResults;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblOut;
        private System.Windows.Forms.TextBox txtOutputDir;
        private PDMStepExporter.FlatButton btnBrowse;
        private System.Windows.Forms.CheckBox chkUseInputName;
        private System.Windows.Forms.CheckBox chkGetLatest;
        private System.Windows.Forms.CheckBox chkCloseSw;
        private PDMStepExporter.FlatButton btnExportSel;
        private PDMStepExporter.FlatButton btnExportAll;
        private PDMStepExporter.FlatButton btnCopySel;
        private PDMStepExporter.FlatButton btnCopyAll;
        private System.Windows.Forms.Label lblProgress;
        private PDMStepExporter.PurpleProgressBar pgbProgress;
        private System.Windows.Forms.RichTextBox logRich;
        private System.ComponentModel.BackgroundWorker bwSearch;
        private System.ComponentModel.BackgroundWorker bwExport;
        private System.ComponentModel.BackgroundWorker bwCopy;
        private System.Windows.Forms.OpenFileDialog ofdImport;
        private System.Windows.Forms.FolderBrowserDialog fbdOutput;
        private System.Windows.Forms.ToolTip tipMain;
    }
}
