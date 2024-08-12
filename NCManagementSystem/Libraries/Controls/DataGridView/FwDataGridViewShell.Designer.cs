namespace NCManagementSystem.Libraries.Controls.DataGridView
{
    partial class FwDataGridViewShell
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlMain = new NCManagementSystem.Libraries.Controls.Panel.FwPanel();
            this.tlpMain = new NCManagementSystem.Libraries.Controls.Panel.FwTableLayoutPanel();
            this.tlpTopBar = new NCManagementSystem.Libraries.Controls.Panel.FwTableLayoutPanel();
            this.tlpTitle = new NCManagementSystem.Libraries.Controls.Panel.FwTableLayoutPanel();
            this.pnlTitleIcon = new NCManagementSystem.Libraries.Controls.Panel.FwPanel();
            this.lblTitle = new NCManagementSystem.Libraries.Controls.Label.FwLabel();
            this.tlpPageController = new NCManagementSystem.Libraries.Controls.Panel.FwTableLayoutPanel();
            this.btnSkipToNext = new System.Windows.Forms.Button();
            this.btnToNext = new System.Windows.Forms.Button();
            this.lblPageControllerStatus = new NCManagementSystem.Libraries.Controls.Label.FwLabel();
            this.btnToPrevious = new System.Windows.Forms.Button();
            this.btnSkipToPrevious = new System.Windows.Forms.Button();
            this.tlpContent = new NCManagementSystem.Libraries.Controls.Panel.FwTableLayoutPanel();
            this.dgvGridView = new NCManagementSystem.Libraries.Controls.DataGridView.FwDataGridView();
            this.tlpVScroll = new NCManagementSystem.Libraries.Controls.Panel.FwTableLayoutPanel();
            this.btnToDown = new System.Windows.Forms.Button();
            this.btnToUp = new System.Windows.Forms.Button();
            this.vscVScrollBar = new System.Windows.Forms.VScrollBar();
            this.tlpHScroll = new NCManagementSystem.Libraries.Controls.Panel.FwTableLayoutPanel();
            this.btnToBackward = new System.Windows.Forms.Button();
            this.btnToForward = new System.Windows.Forms.Button();
            this.hscHScrollBar = new System.Windows.Forms.HScrollBar();
            this.pnlScrollNC = new NCManagementSystem.Libraries.Controls.Panel.FwPanel();
            this.tlpBottomBar = new NCManagementSystem.Libraries.Controls.Panel.FwTableLayoutPanel();
            this.lblPageStatus = new NCManagementSystem.Libraries.Controls.Label.FwLabel();
            this.pnlOuterRowsPerPage = new NCManagementSystem.Libraries.Controls.Panel.FwPanel();
            this.pnlRowsPerPage = new NCManagementSystem.Libraries.Controls.Panel.FwPanel();
            this.txtRowsPerPage = new NCManagementSystem.Libraries.Controls.TextBox.FwTextBox();
            this.pnlMain.SuspendLayout();
            this.tlpMain.SuspendLayout();
            this.tlpTopBar.SuspendLayout();
            this.tlpTitle.SuspendLayout();
            this.tlpPageController.SuspendLayout();
            this.tlpContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGridView)).BeginInit();
            this.tlpVScroll.SuspendLayout();
            this.tlpHScroll.SuspendLayout();
            this.tlpBottomBar.SuspendLayout();
            this.pnlOuterRowsPerPage.SuspendLayout();
            this.pnlRowsPerPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.tlpMain);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(300, 300);
            this.pnlMain.TabIndex = 0;
            this.pnlMain.Visible = false;
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.tlpTopBar, 0, 0);
            this.tlpMain.Controls.Add(this.tlpContent, 0, 1);
            this.tlpMain.Controls.Add(this.tlpBottomBar, 0, 2);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 3;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.Size = new System.Drawing.Size(300, 300);
            this.tlpMain.TabIndex = 0;
            // 
            // tlpTopBar
            // 
            this.tlpTopBar.BackColor = System.Drawing.SystemColors.Control;
            this.tlpTopBar.ColumnCount = 2;
            this.tlpTopBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTopBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpTopBar.Controls.Add(this.tlpTitle, 0, 0);
            this.tlpTopBar.Controls.Add(this.tlpPageController, 1, 0);
            this.tlpTopBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTopBar.Location = new System.Drawing.Point(0, 0);
            this.tlpTopBar.Margin = new System.Windows.Forms.Padding(0);
            this.tlpTopBar.Name = "tlpTopBar";
            this.tlpTopBar.RowCount = 1;
            this.tlpTopBar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTopBar.Size = new System.Drawing.Size(300, 40);
            this.tlpTopBar.TabIndex = 0;
            this.tlpTopBar.Visible = false;
            // 
            // tlpTitle
            // 
            this.tlpTitle.ColumnCount = 2;
            this.tlpTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTitle.Controls.Add(this.pnlTitleIcon, 0, 0);
            this.tlpTitle.Controls.Add(this.lblTitle, 1, 0);
            this.tlpTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTitle.Location = new System.Drawing.Point(0, 0);
            this.tlpTitle.Margin = new System.Windows.Forms.Padding(0);
            this.tlpTitle.Name = "tlpTitle";
            this.tlpTitle.RowCount = 1;
            this.tlpTitle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTitle.Size = new System.Drawing.Size(136, 40);
            this.tlpTitle.TabIndex = 0;
            // 
            // pnlTitleIcon
            // 
            this.pnlTitleIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlTitleIcon.Location = new System.Drawing.Point(0, 0);
            this.pnlTitleIcon.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTitleIcon.Name = "pnlTitleIcon";
            this.pnlTitleIcon.Size = new System.Drawing.Size(5, 40);
            this.pnlTitleIcon.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Location = new System.Drawing.Point(8, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(125, 40);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tlpPageController
            // 
            this.tlpPageController.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.tlpPageController.ColumnCount = 5;
            this.tlpPageController.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPageController.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPageController.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPageController.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPageController.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPageController.Controls.Add(this.btnSkipToNext, 4, 0);
            this.tlpPageController.Controls.Add(this.btnToNext, 3, 0);
            this.tlpPageController.Controls.Add(this.lblPageControllerStatus, 2, 0);
            this.tlpPageController.Controls.Add(this.btnToPrevious, 1, 0);
            this.tlpPageController.Controls.Add(this.btnSkipToPrevious, 0, 0);
            this.tlpPageController.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPageController.Location = new System.Drawing.Point(136, 0);
            this.tlpPageController.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPageController.Name = "tlpPageController";
            this.tlpPageController.RowCount = 1;
            this.tlpPageController.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPageController.Size = new System.Drawing.Size(164, 40);
            this.tlpPageController.TabIndex = 1;
            this.tlpPageController.Visible = false;
            // 
            // btnSkipToNext
            // 
            this.btnSkipToNext.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnSkipToNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSkipToNext.Enabled = false;
            this.btnSkipToNext.FlatAppearance.BorderSize = 0;
            this.btnSkipToNext.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnSkipToNext.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnSkipToNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSkipToNext.Location = new System.Drawing.Point(124, 0);
            this.btnSkipToNext.Margin = new System.Windows.Forms.Padding(0);
            this.btnSkipToNext.Name = "btnSkipToNext";
            this.btnSkipToNext.Size = new System.Drawing.Size(40, 40);
            this.btnSkipToNext.TabIndex = 4;
            this.btnSkipToNext.UseVisualStyleBackColor = false;
            this.btnSkipToNext.EnabledChanged += new System.EventHandler(this.PageControllers_EnabledChanged);
            this.btnSkipToNext.Click += new System.EventHandler(this.btnSkipToNext_Click);
            // 
            // btnToNext
            // 
            this.btnToNext.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnToNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnToNext.Enabled = false;
            this.btnToNext.FlatAppearance.BorderSize = 0;
            this.btnToNext.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnToNext.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnToNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToNext.Location = new System.Drawing.Point(83, 0);
            this.btnToNext.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.btnToNext.Name = "btnToNext";
            this.btnToNext.Size = new System.Drawing.Size(40, 40);
            this.btnToNext.TabIndex = 3;
            this.btnToNext.UseVisualStyleBackColor = false;
            this.btnToNext.EnabledChanged += new System.EventHandler(this.PageControllers_EnabledChanged);
            this.btnToNext.Click += new System.EventHandler(this.btnToNext_Click);
            // 
            // lblPageControllerStatus
            // 
            this.lblPageControllerStatus.BackColor = System.Drawing.SystemColors.Control;
            this.lblPageControllerStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPageControllerStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageControllerStatus.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblPageControllerStatus.Location = new System.Drawing.Point(82, 0);
            this.lblPageControllerStatus.Margin = new System.Windows.Forms.Padding(0);
            this.lblPageControllerStatus.Name = "lblPageControllerStatus";
            this.lblPageControllerStatus.Size = new System.Drawing.Size(1, 40);
            this.lblPageControllerStatus.TabIndex = 2;
            this.lblPageControllerStatus.Text = "Page 0 of 0";
            this.lblPageControllerStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblPageControllerStatus.Visible = false;
            // 
            // btnToPrevious
            // 
            this.btnToPrevious.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnToPrevious.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnToPrevious.Enabled = false;
            this.btnToPrevious.FlatAppearance.BorderSize = 0;
            this.btnToPrevious.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnToPrevious.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnToPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToPrevious.Location = new System.Drawing.Point(41, 0);
            this.btnToPrevious.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.btnToPrevious.Name = "btnToPrevious";
            this.btnToPrevious.Size = new System.Drawing.Size(40, 40);
            this.btnToPrevious.TabIndex = 1;
            this.btnToPrevious.UseVisualStyleBackColor = false;
            this.btnToPrevious.EnabledChanged += new System.EventHandler(this.PageControllers_EnabledChanged);
            this.btnToPrevious.Click += new System.EventHandler(this.btnToPrevious_Click);
            // 
            // btnSkipToPrevious
            // 
            this.btnSkipToPrevious.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnSkipToPrevious.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSkipToPrevious.Enabled = false;
            this.btnSkipToPrevious.FlatAppearance.BorderSize = 0;
            this.btnSkipToPrevious.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnSkipToPrevious.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnSkipToPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSkipToPrevious.Location = new System.Drawing.Point(0, 0);
            this.btnSkipToPrevious.Margin = new System.Windows.Forms.Padding(0);
            this.btnSkipToPrevious.Name = "btnSkipToPrevious";
            this.btnSkipToPrevious.Size = new System.Drawing.Size(40, 40);
            this.btnSkipToPrevious.TabIndex = 0;
            this.btnSkipToPrevious.UseVisualStyleBackColor = false;
            this.btnSkipToPrevious.EnabledChanged += new System.EventHandler(this.PageControllers_EnabledChanged);
            this.btnSkipToPrevious.Click += new System.EventHandler(this.btnSkipToPrevious_Click);
            // 
            // tlpContent
            // 
            this.tlpContent.ColumnCount = 2;
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpContent.Controls.Add(this.dgvGridView, 0, 0);
            this.tlpContent.Controls.Add(this.tlpVScroll, 1, 0);
            this.tlpContent.Controls.Add(this.tlpHScroll, 0, 1);
            this.tlpContent.Controls.Add(this.pnlScrollNC, 1, 1);
            this.tlpContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpContent.Location = new System.Drawing.Point(0, 40);
            this.tlpContent.Margin = new System.Windows.Forms.Padding(0);
            this.tlpContent.Name = "tlpContent";
            this.tlpContent.RowCount = 2;
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.Size = new System.Drawing.Size(300, 230);
            this.tlpContent.TabIndex = 1;
            // 
            // dgvGridView
            // 
            this.dgvGridView.AllowUserToAddRows = false;
            this.dgvGridView.AllowUserToDeleteRows = false;
            this.dgvGridView.AllowUserToResizeRows = false;
            this.dgvGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGridView.HeaderProperties.BorderColor = System.Drawing.Color.Transparent;
            this.dgvGridView.Location = new System.Drawing.Point(0, 0);
            this.dgvGridView.Margin = new System.Windows.Forms.Padding(0);
            this.dgvGridView.Name = "dgvGridView";
            this.dgvGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvGridView.RowTemplate.Height = 23;
            this.dgvGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGridView.Size = new System.Drawing.Size(264, 194);
            this.dgvGridView.TabIndex = 0;
            this.dgvGridView.VirtualMode = true;
            this.dgvGridView.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvGridView_ColumnWidthChanged);
            this.dgvGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvGridView_KeyDown);
            this.dgvGridView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgvGridView_KeyUp);
            // 
            // tlpVScroll
            // 
            this.tlpVScroll.ColumnCount = 1;
            this.tlpVScroll.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpVScroll.Controls.Add(this.btnToDown, 0, 2);
            this.tlpVScroll.Controls.Add(this.btnToUp, 0, 1);
            this.tlpVScroll.Controls.Add(this.vscVScrollBar, 0, 0);
            this.tlpVScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpVScroll.Location = new System.Drawing.Point(264, 0);
            this.tlpVScroll.Margin = new System.Windows.Forms.Padding(0);
            this.tlpVScroll.Name = "tlpVScroll";
            this.tlpVScroll.RowCount = 3;
            this.tlpVScroll.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpVScroll.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpVScroll.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpVScroll.Size = new System.Drawing.Size(36, 194);
            this.tlpVScroll.TabIndex = 1;
            this.tlpVScroll.Visible = false;
            // 
            // btnToDown
            // 
            this.btnToDown.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnToDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnToDown.FlatAppearance.BorderSize = 0;
            this.btnToDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToDown.Location = new System.Drawing.Point(0, 158);
            this.btnToDown.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.btnToDown.Name = "btnToDown";
            this.btnToDown.Size = new System.Drawing.Size(36, 36);
            this.btnToDown.TabIndex = 2;
            this.btnToDown.UseVisualStyleBackColor = false;
            this.btnToDown.Click += new System.EventHandler(this.btnToDown_Click);
            // 
            // btnToUp
            // 
            this.btnToUp.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnToUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnToUp.FlatAppearance.BorderSize = 0;
            this.btnToUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToUp.Location = new System.Drawing.Point(0, 121);
            this.btnToUp.Margin = new System.Windows.Forms.Padding(0);
            this.btnToUp.Name = "btnToUp";
            this.btnToUp.Size = new System.Drawing.Size(36, 36);
            this.btnToUp.TabIndex = 1;
            this.btnToUp.UseVisualStyleBackColor = false;
            this.btnToUp.Click += new System.EventHandler(this.btnToUp_Click);
            // 
            // vscVScrollBar
            // 
            this.vscVScrollBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vscVScrollBar.Location = new System.Drawing.Point(0, 0);
            this.vscVScrollBar.Name = "vscVScrollBar";
            this.vscVScrollBar.Size = new System.Drawing.Size(36, 121);
            this.vscVScrollBar.TabIndex = 0;
            this.vscVScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vscVScrollBar_Scroll);
            // 
            // tlpHScroll
            // 
            this.tlpHScroll.ColumnCount = 3;
            this.tlpHScroll.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpHScroll.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpHScroll.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpHScroll.Controls.Add(this.btnToBackward, 2, 0);
            this.tlpHScroll.Controls.Add(this.btnToForward, 1, 0);
            this.tlpHScroll.Controls.Add(this.hscHScrollBar, 0, 0);
            this.tlpHScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpHScroll.Location = new System.Drawing.Point(0, 194);
            this.tlpHScroll.Margin = new System.Windows.Forms.Padding(0);
            this.tlpHScroll.Name = "tlpHScroll";
            this.tlpHScroll.RowCount = 1;
            this.tlpHScroll.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpHScroll.Size = new System.Drawing.Size(264, 36);
            this.tlpHScroll.TabIndex = 2;
            this.tlpHScroll.Visible = false;
            // 
            // btnToBackward
            // 
            this.btnToBackward.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnToBackward.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnToBackward.FlatAppearance.BorderSize = 0;
            this.btnToBackward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToBackward.Location = new System.Drawing.Point(228, 0);
            this.btnToBackward.Margin = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.btnToBackward.Name = "btnToBackward";
            this.btnToBackward.Size = new System.Drawing.Size(36, 36);
            this.btnToBackward.TabIndex = 3;
            this.btnToBackward.UseVisualStyleBackColor = false;
            this.btnToBackward.Click += new System.EventHandler(this.btnToBackward_Click);
            // 
            // btnToForward
            // 
            this.btnToForward.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnToForward.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnToForward.FlatAppearance.BorderSize = 0;
            this.btnToForward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToForward.Location = new System.Drawing.Point(191, 0);
            this.btnToForward.Margin = new System.Windows.Forms.Padding(0);
            this.btnToForward.Name = "btnToForward";
            this.btnToForward.Size = new System.Drawing.Size(36, 36);
            this.btnToForward.TabIndex = 2;
            this.btnToForward.UseVisualStyleBackColor = false;
            this.btnToForward.Click += new System.EventHandler(this.btnToForward_Click);
            // 
            // hscHScrollBar
            // 
            this.hscHScrollBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hscHScrollBar.Location = new System.Drawing.Point(0, 0);
            this.hscHScrollBar.Name = "hscHScrollBar";
            this.hscHScrollBar.Size = new System.Drawing.Size(191, 36);
            this.hscHScrollBar.TabIndex = 0;
            this.hscHScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hscHScrollBar_Scroll);
            // 
            // pnlScrollNC
            // 
            this.pnlScrollNC.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pnlScrollNC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlScrollNC.Location = new System.Drawing.Point(264, 194);
            this.pnlScrollNC.Margin = new System.Windows.Forms.Padding(0);
            this.pnlScrollNC.Name = "pnlScrollNC";
            this.pnlScrollNC.Size = new System.Drawing.Size(36, 36);
            this.pnlScrollNC.TabIndex = 3;
            this.pnlScrollNC.Visible = false;
            // 
            // tlpBottomBar
            // 
            this.tlpBottomBar.BackColor = System.Drawing.SystemColors.Control;
            this.tlpBottomBar.ColumnCount = 2;
            this.tlpBottomBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBottomBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpBottomBar.Controls.Add(this.lblPageStatus, 0, 0);
            this.tlpBottomBar.Controls.Add(this.pnlOuterRowsPerPage, 1, 0);
            this.tlpBottomBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBottomBar.Location = new System.Drawing.Point(0, 270);
            this.tlpBottomBar.Margin = new System.Windows.Forms.Padding(0);
            this.tlpBottomBar.Name = "tlpBottomBar";
            this.tlpBottomBar.RowCount = 1;
            this.tlpBottomBar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBottomBar.Size = new System.Drawing.Size(300, 30);
            this.tlpBottomBar.TabIndex = 2;
            this.tlpBottomBar.Visible = false;
            // 
            // lblPageStatus
            // 
            this.lblPageStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPageStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageStatus.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblPageStatus.Location = new System.Drawing.Point(3, 0);
            this.lblPageStatus.Name = "lblPageStatus";
            this.lblPageStatus.Size = new System.Drawing.Size(252, 30);
            this.lblPageStatus.TabIndex = 3;
            this.lblPageStatus.Text = "Items 0.";
            this.lblPageStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlOuterRowsPerPage
            // 
            this.pnlOuterRowsPerPage.Controls.Add(this.pnlRowsPerPage);
            this.pnlOuterRowsPerPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOuterRowsPerPage.Location = new System.Drawing.Point(258, 0);
            this.pnlOuterRowsPerPage.Margin = new System.Windows.Forms.Padding(0);
            this.pnlOuterRowsPerPage.Name = "pnlOuterRowsPerPage";
            this.pnlOuterRowsPerPage.Padding = new System.Windows.Forms.Padding(1);
            this.pnlOuterRowsPerPage.Size = new System.Drawing.Size(42, 30);
            this.pnlOuterRowsPerPage.TabIndex = 4;
            this.pnlOuterRowsPerPage.Visible = false;
            // 
            // pnlRowsPerPage
            // 
            this.pnlRowsPerPage.BackColor = System.Drawing.SystemColors.Window;
            this.pnlRowsPerPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRowsPerPage.Controls.Add(this.txtRowsPerPage);
            this.pnlRowsPerPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRowsPerPage.Location = new System.Drawing.Point(1, 1);
            this.pnlRowsPerPage.Name = "pnlRowsPerPage";
            this.pnlRowsPerPage.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.pnlRowsPerPage.Size = new System.Drawing.Size(40, 28);
            this.pnlRowsPerPage.TabIndex = 0;
            // 
            // txtRowsPerPage
            // 
            this.txtRowsPerPage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRowsPerPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRowsPerPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRowsPerPage.ForeColor = System.Drawing.SystemColors.GrayText;
            this.txtRowsPerPage.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtRowsPerPage.Location = new System.Drawing.Point(0, 2);
            this.txtRowsPerPage.Name = "txtRowsPerPage";
            this.txtRowsPerPage.Size = new System.Drawing.Size(38, 24);
            this.txtRowsPerPage.TabIndex = 0;
            this.txtRowsPerPage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtRowsPerPage.TextBoxStyle = NCManagementSystem.Libraries.Controls.TextBox.TextBoxConstsDefiner.TextBoxStyles.Numeric;
            this.txtRowsPerPage.Enter += new System.EventHandler(this.txtRowsPerPage_Enter);
            this.txtRowsPerPage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRowsPerPage_KeyDown);
            this.txtRowsPerPage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRowsPerPage_KeyPress);
            this.txtRowsPerPage.Leave += new System.EventHandler(this.txtRowsPerPage_Leave);
            // 
            // FwDataGridViewShell
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Controls.Add(this.pnlMain);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FwDataGridViewShell";
            this.Size = new System.Drawing.Size(300, 300);
            this.pnlMain.ResumeLayout(false);
            this.tlpMain.ResumeLayout(false);
            this.tlpTopBar.ResumeLayout(false);
            this.tlpTitle.ResumeLayout(false);
            this.tlpPageController.ResumeLayout(false);
            this.tlpContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGridView)).EndInit();
            this.tlpVScroll.ResumeLayout(false);
            this.tlpHScroll.ResumeLayout(false);
            this.tlpBottomBar.ResumeLayout(false);
            this.pnlOuterRowsPerPage.ResumeLayout(false);
            this.pnlRowsPerPage.ResumeLayout(false);
            this.pnlRowsPerPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel.FwPanel pnlMain;
        private Panel.FwTableLayoutPanel tlpMain;
        private Panel.FwTableLayoutPanel tlpTopBar;
        private Panel.FwTableLayoutPanel tlpTitle;
        private Panel.FwTableLayoutPanel tlpPageController;
        private Panel.FwTableLayoutPanel tlpContent;
        private Panel.FwTableLayoutPanel tlpVScroll;
        private Panel.FwTableLayoutPanel tlpHScroll;
        private Panel.FwPanel pnlScrollNC;
        private Panel.FwTableLayoutPanel tlpBottomBar;
        private Panel.FwPanel pnlTitleIcon;
        private Label.FwLabel lblTitle;
        private System.Windows.Forms.Button btnSkipToNext;
        private System.Windows.Forms.Button btnToNext;
        private Label.FwLabel lblPageControllerStatus;
        private System.Windows.Forms.Button btnToPrevious;
        private System.Windows.Forms.Button btnSkipToPrevious;
        private System.Windows.Forms.Button btnToDown;
        private System.Windows.Forms.Button btnToUp;
        private System.Windows.Forms.Button btnToBackward;
        private System.Windows.Forms.Button btnToForward;
        private System.Windows.Forms.HScrollBar hscHScrollBar;
        private Label.FwLabel lblPageStatus;
        private Panel.FwPanel pnlOuterRowsPerPage;
        private Panel.FwPanel pnlRowsPerPage;
        private TextBox.FwTextBox txtRowsPerPage;
        private FwDataGridView dgvGridView;
        public System.Windows.Forms.VScrollBar vscVScrollBar;
    }
}
