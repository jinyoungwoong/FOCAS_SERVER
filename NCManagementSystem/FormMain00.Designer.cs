
namespace NCManagementSystem
{
    partial class FormMain00
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

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tlpMain = new NCManagementSystem.Libraries.Controls.Panel.FwTableLayoutPanel();
            this.pnlBottom = new NCManagementSystem.Libraries.Controls.Panel.FwPanel();
            this.tlpBottom = new NCManagementSystem.Libraries.Controls.Panel.FwTableLayoutPanel();
            this.fwTableLayoutPanel1 = new NCManagementSystem.Libraries.Controls.Panel.FwTableLayoutPanel();
            this.txtJoin = new NCManagementSystem.Libraries.Controls.Label.FwLabel();
            this.tlpDBState = new NCManagementSystem.Libraries.Controls.Panel.FwTableLayoutPanel();
            this.chkDBState = new System.Windows.Forms.CheckBox();
            this.btnDBSettings = new System.Windows.Forms.Button();
            this.lblTDBState = new NCManagementSystem.Libraries.Controls.Label.FwLabel();
            this.flpCanvas = new NCManagementSystem.Libraries.Controls.Panel.FwFlowLayoutPanel();
            this.cmsNotifier = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiShowApplicationUI = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiApplicationExit = new System.Windows.Forms.ToolStripMenuItem();
            this.SkinContainer.SuspendLayout();
            this.tlpMain.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.tlpBottom.SuspendLayout();
            this.fwTableLayoutPanel1.SuspendLayout();
            this.tlpDBState.SuspendLayout();
            this.cmsNotifier.SuspendLayout();
            this.SuspendLayout();
            // 
            // SkinContainer
            // 
            this.SkinContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.SkinContainer.BorderProperties.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.SkinContainer.BorderProperties.BorderThickness = 1;
            this.SkinContainer.Controls.Add(this.tlpMain);
            this.SkinContainer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.SkinContainer.Location = new System.Drawing.Point(1, 1);
            this.SkinContainer.Padding = new System.Windows.Forms.Padding(0, 34, 0, 0);
            this.SkinContainer.Size = new System.Drawing.Size(822, 461);
            this.SkinContainer.TitleBarProperties.ControlBoxColors.CloseButtonColors.BackColors.Down = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
            this.SkinContainer.TitleBarProperties.ControlBoxColors.CloseButtonColors.BackColors.Hover = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
            this.SkinContainer.TitleBarProperties.ControlBoxColors.CloseButtonColors.BackColors.IsAlphaColorOnDown = false;
            this.SkinContainer.TitleBarProperties.ControlBoxColors.CloseButtonColors.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.SkinContainer.TitleBarProperties.ControlBoxColors.CloseButtonColors.ForeColors.Down = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.SkinContainer.TitleBarProperties.ControlBoxColors.CloseButtonColors.ForeColors.Hover = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.SkinContainer.TitleBarProperties.ControlBoxColors.MaximizeButtonColors.BackColors.Down = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.SkinContainer.TitleBarProperties.ControlBoxColors.MaximizeButtonColors.BackColors.Hover = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.SkinContainer.TitleBarProperties.ControlBoxColors.MaximizeButtonColors.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.SkinContainer.TitleBarProperties.ControlBoxColors.MaximizeButtonColors.ForeColors.Down = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.SkinContainer.TitleBarProperties.ControlBoxColors.MaximizeButtonColors.ForeColors.Hover = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.SkinContainer.TitleBarProperties.ControlBoxColors.MinimizeButtonColors.BackColors.Down = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.SkinContainer.TitleBarProperties.ControlBoxColors.MinimizeButtonColors.BackColors.Hover = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.SkinContainer.TitleBarProperties.ControlBoxColors.MinimizeButtonColors.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.SkinContainer.TitleBarProperties.ControlBoxColors.MinimizeButtonColors.ForeColors.Down = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.SkinContainer.TitleBarProperties.ControlBoxColors.MinimizeButtonColors.ForeColors.Hover = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.SkinContainer.TitleBarProperties.IsMaximizeBox = false;
            this.SkinContainer.TitleBarProperties.TitleBarBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.SkinContainer.TitleBarProperties.TitleBarHeight = 34;
            this.SkinContainer.TitleBarProperties.TitleBarTextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SkinContainer.TitleBarProperties.TitleBarTextForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // tlpMain
            // 
            this.tlpMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.pnlBottom, 0, 1);
            this.tlpMain.Controls.Add(this.flpCanvas, 0, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 34);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 2;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.Size = new System.Drawing.Size(822, 427);
            this.tlpMain.TabIndex = 0;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.tlpBottom);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottom.Location = new System.Drawing.Point(0, 357);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.pnlBottom.Size = new System.Drawing.Size(822, 70);
            this.pnlBottom.TabIndex = 3;
            // 
            // tlpBottom
            // 
            this.tlpBottom.ColumnCount = 2;
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpBottom.Controls.Add(this.fwTableLayoutPanel1, 0, 0);
            this.tlpBottom.Controls.Add(this.tlpDBState, 0, 0);
            this.tlpBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBottom.Location = new System.Drawing.Point(0, 0);
            this.tlpBottom.Name = "tlpBottom";
            this.tlpBottom.RowCount = 1;
            this.tlpBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBottom.Size = new System.Drawing.Size(822, 65);
            this.tlpBottom.TabIndex = 0;
            // 
            // fwTableLayoutPanel1
            // 
            this.fwTableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fwTableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.fwTableLayoutPanel1.ColumnCount = 3;
            this.fwTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.fwTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.fwTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.fwTableLayoutPanel1.Controls.Add(this.txtJoin, 0, 0);
            this.fwTableLayoutPanel1.Location = new System.Drawing.Point(552, 10);
            this.fwTableLayoutPanel1.Margin = new System.Windows.Forms.Padding(10);
            this.fwTableLayoutPanel1.Name = "fwTableLayoutPanel1";
            this.fwTableLayoutPanel1.RowCount = 1;
            this.fwTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.fwTableLayoutPanel1.Size = new System.Drawing.Size(260, 45);
            this.fwTableLayoutPanel1.TabIndex = 5;
            // 
            // txtJoin
            // 
            this.txtJoin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.txtJoin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtJoin.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtJoin.ForeColor = System.Drawing.Color.White;
            this.txtJoin.GradientStyleProperties.BackColor.FirstColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.txtJoin.GradientStyleProperties.BackColor.LastColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.txtJoin.LabelStyle = NCManagementSystem.Libraries.Controls.Label.LabelConstsDefiner.LabelStyles.Gradient;
            this.txtJoin.Location = new System.Drawing.Point(1, 1);
            this.txtJoin.Margin = new System.Windows.Forms.Padding(1, 1, 0, 1);
            this.txtJoin.Name = "txtJoin";
            this.txtJoin.Size = new System.Drawing.Size(259, 43);
            this.txtJoin.TabIndex = 3;
            this.txtJoin.Text = "사용자계정 추가";
            this.txtJoin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtJoin.Click += new System.EventHandler(this.txtJoin_Click);
            // 
            // tlpDBState
            // 
            this.tlpDBState.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.tlpDBState.ColumnCount = 3;
            this.tlpDBState.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDBState.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpDBState.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpDBState.Controls.Add(this.chkDBState, 1, 0);
            this.tlpDBState.Controls.Add(this.btnDBSettings, 2, 0);
            this.tlpDBState.Controls.Add(this.lblTDBState, 0, 0);
            this.tlpDBState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpDBState.Location = new System.Drawing.Point(10, 10);
            this.tlpDBState.Margin = new System.Windows.Forms.Padding(10);
            this.tlpDBState.Name = "tlpDBState";
            this.tlpDBState.RowCount = 1;
            this.tlpDBState.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDBState.Size = new System.Drawing.Size(260, 45);
            this.tlpDBState.TabIndex = 0;
            // 
            // chkDBState
            // 
            this.chkDBState.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkDBState.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.chkDBState.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkDBState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkDBState.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.chkDBState.FlatAppearance.BorderSize = 0;
            this.chkDBState.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.chkDBState.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.chkDBState.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkDBState.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold);
            this.chkDBState.ForeColor = System.Drawing.Color.White;
            this.chkDBState.Image = global::NCManagementSystem.Properties.Resources.LampOff_32R;
            this.chkDBState.Location = new System.Drawing.Point(157, 1);
            this.chkDBState.Margin = new System.Windows.Forms.Padding(1, 1, 0, 1);
            this.chkDBState.Name = "chkDBState";
            this.chkDBState.Size = new System.Drawing.Size(50, 43);
            this.chkDBState.TabIndex = 1;
            this.chkDBState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkDBState.UseVisualStyleBackColor = false;
            this.chkDBState.CheckedChanged += new System.EventHandler(this.chkDBState_CheckedChanged);
            // 
            // btnDBSettings
            // 
            this.btnDBSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.btnDBSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDBSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDBSettings.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.btnDBSettings.FlatAppearance.BorderSize = 0;
            this.btnDBSettings.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.btnDBSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDBSettings.Image = global::NCManagementSystem.Properties.Resources.Settings_24W;
            this.btnDBSettings.Location = new System.Drawing.Point(208, 1);
            this.btnDBSettings.Margin = new System.Windows.Forms.Padding(1);
            this.btnDBSettings.Name = "btnDBSettings";
            this.btnDBSettings.Size = new System.Drawing.Size(51, 43);
            this.btnDBSettings.TabIndex = 2;
            this.btnDBSettings.UseVisualStyleBackColor = false;
            this.btnDBSettings.EnabledChanged += new System.EventHandler(this.btnDBSettings_EnabledChanged);
            this.btnDBSettings.Click += new System.EventHandler(this.btnDBSettings_Click);
            // 
            // lblTDBState
            // 
            this.lblTDBState.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.lblTDBState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTDBState.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTDBState.ForeColor = System.Drawing.Color.White;
            this.lblTDBState.GradientStyleProperties.BackColor.FirstColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.lblTDBState.GradientStyleProperties.BackColor.LastColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.lblTDBState.LabelStyle = NCManagementSystem.Libraries.Controls.Label.LabelConstsDefiner.LabelStyles.Gradient;
            this.lblTDBState.Location = new System.Drawing.Point(1, 1);
            this.lblTDBState.Margin = new System.Windows.Forms.Padding(1, 1, 0, 1);
            this.lblTDBState.Name = "lblTDBState";
            this.lblTDBState.Size = new System.Drawing.Size(155, 43);
            this.lblTDBState.TabIndex = 0;
            this.lblTDBState.Text = "DB STATE";
            this.lblTDBState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flpCanvas
            // 
            this.flpCanvas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.flpCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpCanvas.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpCanvas.Location = new System.Drawing.Point(5, 5);
            this.flpCanvas.Margin = new System.Windows.Forms.Padding(5);
            this.flpCanvas.Name = "flpCanvas";
            this.flpCanvas.Padding = new System.Windows.Forms.Padding(10);
            this.flpCanvas.Size = new System.Drawing.Size(812, 347);
            this.flpCanvas.TabIndex = 1;
            // 
            // cmsNotifier
            // 
            this.cmsNotifier.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmsNotifier.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiShowApplicationUI,
            this.tsSeparator,
            this.tsmiApplicationExit});
            this.cmsNotifier.Name = "cmsNotifier";
            this.cmsNotifier.Size = new System.Drawing.Size(102, 54);
            // 
            // tsmiShowApplicationUI
            // 
            this.tsmiShowApplicationUI.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tsmiShowApplicationUI.Name = "tsmiShowApplicationUI";
            this.tsmiShowApplicationUI.Size = new System.Drawing.Size(101, 22);
            this.tsmiShowApplicationUI.Text = "보기";
            this.tsmiShowApplicationUI.Click += new System.EventHandler(this.tsmiShowApplicationUI_Click);
            // 
            // tsSeparator
            // 
            this.tsSeparator.Name = "tsSeparator";
            this.tsSeparator.Size = new System.Drawing.Size(98, 6);
            // 
            // tsmiApplicationExit
            // 
            this.tsmiApplicationExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tsmiApplicationExit.Name = "tsmiApplicationExit";
            this.tsmiApplicationExit.Size = new System.Drawing.Size(101, 22);
            this.tsmiApplicationExit.Text = "종료";
            this.tsmiApplicationExit.Click += new System.EventHandler(this.tsmiApplicationExit_Click);
            // 
            // FormMain00
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(824, 463);
            this.IsFixedWindow = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximumSize = new System.Drawing.Size(2560, 1392);
            this.Name = "FormMain00";
            this.Controls.SetChildIndex(this.SkinContainer, 0);
            this.SkinContainer.ResumeLayout(false);
            this.tlpMain.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.tlpBottom.ResumeLayout(false);
            this.fwTableLayoutPanel1.ResumeLayout(false);
            this.tlpDBState.ResumeLayout(false);
            this.cmsNotifier.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Libraries.Controls.Panel.FwTableLayoutPanel tlpMain;
        private Libraries.Controls.Panel.FwFlowLayoutPanel flpCanvas;
        private Libraries.Controls.Panel.FwPanel pnlBottom;
        private Libraries.Controls.Panel.FwTableLayoutPanel tlpBottom;
        private Libraries.Controls.Panel.FwTableLayoutPanel tlpDBState;
        private Libraries.Controls.Label.FwLabel lblTDBState;
        private System.Windows.Forms.Button btnDBSettings;
        private System.Windows.Forms.CheckBox chkDBState;
        private System.Windows.Forms.ContextMenuStrip cmsNotifier;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowApplicationUI;
        private System.Windows.Forms.ToolStripSeparator tsSeparator;
        private System.Windows.Forms.ToolStripMenuItem tsmiApplicationExit;
        private Libraries.Controls.Panel.FwTableLayoutPanel fwTableLayoutPanel1;
        private Libraries.Controls.Label.FwLabel txtJoin;
    }
}

