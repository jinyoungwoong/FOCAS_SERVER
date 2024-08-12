
namespace NCManagementSystem.Components.Controllers
{
    partial class ucController00
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
            this.tlpMain = new NCManagementSystem.Libraries.Controls.Panel.FwTableLayoutPanel();
            this.pnlEquipCode = new NCManagementSystem.Libraries.Controls.Panel.FwPanel();
            this.lblCode = new NCManagementSystem.Libraries.Controls.Label.FwLabel();
            this.pnlIPAdr = new NCManagementSystem.Libraries.Controls.Panel.FwPanel();
            this.lblIPAdr = new NCManagementSystem.Libraries.Controls.Label.FwLabel();
            this.pnlID = new NCManagementSystem.Libraries.Controls.Panel.FwPanel();
            this.lblID = new NCManagementSystem.Libraries.Controls.Label.FwLabel();
            this.tlpController = new NCManagementSystem.Libraries.Controls.Panel.FwTableLayoutPanel();
            this.btnSettings = new System.Windows.Forms.Button();
            this.chkController = new System.Windows.Forms.CheckBox();
            this.pnlState = new NCManagementSystem.Libraries.Controls.Panel.FwPanel();
            this.lblState = new NCManagementSystem.Libraries.Controls.Label.FwLabel();
            this.tlpMain.SuspendLayout();
            this.pnlEquipCode.SuspendLayout();
            this.pnlIPAdr.SuspendLayout();
            this.pnlID.SuspendLayout();
            this.tlpController.SuspendLayout();
            this.pnlState.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 4;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.Controls.Add(this.pnlEquipCode, 1, 0);
            this.tlpMain.Controls.Add(this.pnlIPAdr, 2, 0);
            this.tlpMain.Controls.Add(this.pnlID, 0, 0);
            this.tlpMain.Controls.Add(this.tlpController, 3, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 1;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Size = new System.Drawing.Size(792, 45);
            this.tlpMain.TabIndex = 0;
            // 
            // pnlEquipCode
            // 
            this.pnlEquipCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.pnlEquipCode.Controls.Add(this.lblCode);
            this.pnlEquipCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlEquipCode.Location = new System.Drawing.Point(52, 1);
            this.pnlEquipCode.Margin = new System.Windows.Forms.Padding(1, 1, 0, 1);
            this.pnlEquipCode.Name = "pnlEquipCode";
            this.pnlEquipCode.Size = new System.Drawing.Size(297, 43);
            this.pnlEquipCode.TabIndex = 1;
            // 
            // lblCode
            // 
            this.lblCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCode.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold);
            this.lblCode.ForeColor = System.Drawing.Color.White;
            this.lblCode.GradientStyleProperties.BackColor.FirstColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.lblCode.GradientStyleProperties.BackColor.LastColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.lblCode.Location = new System.Drawing.Point(0, 0);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(297, 43);
            this.lblCode.TabIndex = 0;
            this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlIPAdr
            // 
            this.pnlIPAdr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.pnlIPAdr.Controls.Add(this.lblIPAdr);
            this.pnlIPAdr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlIPAdr.Location = new System.Drawing.Point(350, 1);
            this.pnlIPAdr.Margin = new System.Windows.Forms.Padding(1, 1, 0, 1);
            this.pnlIPAdr.Name = "pnlIPAdr";
            this.pnlIPAdr.Size = new System.Drawing.Size(200, 43);
            this.pnlIPAdr.TabIndex = 2;
            // 
            // lblIPAdr
            // 
            this.lblIPAdr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIPAdr.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold);
            this.lblIPAdr.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblIPAdr.GradientStyleProperties.BackColor.FirstColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.lblIPAdr.GradientStyleProperties.BackColor.LastColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.lblIPAdr.Location = new System.Drawing.Point(0, 0);
            this.lblIPAdr.Name = "lblIPAdr";
            this.lblIPAdr.Size = new System.Drawing.Size(200, 43);
            this.lblIPAdr.TabIndex = 0;
            this.lblIPAdr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlID
            // 
            this.pnlID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.pnlID.Controls.Add(this.lblID);
            this.pnlID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlID.Location = new System.Drawing.Point(1, 1);
            this.pnlID.Margin = new System.Windows.Forms.Padding(1, 1, 0, 1);
            this.pnlID.Name = "pnlID";
            this.pnlID.Size = new System.Drawing.Size(50, 43);
            this.pnlID.TabIndex = 0;
            // 
            // lblID
            // 
            this.lblID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblID.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblID.ForeColor = System.Drawing.Color.White;
            this.lblID.GradientStyleProperties.BackColor.FirstColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.lblID.GradientStyleProperties.BackColor.LastColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.lblID.Location = new System.Drawing.Point(0, 0);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(50, 43);
            this.lblID.TabIndex = 0;
            this.lblID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tlpController
            // 
            this.tlpController.ColumnCount = 3;
            this.tlpController.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpController.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpController.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpController.Controls.Add(this.btnSettings, 2, 0);
            this.tlpController.Controls.Add(this.chkController, 1, 0);
            this.tlpController.Controls.Add(this.pnlState, 0, 0);
            this.tlpController.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpController.Location = new System.Drawing.Point(551, 1);
            this.tlpController.Margin = new System.Windows.Forms.Padding(1);
            this.tlpController.Name = "tlpController";
            this.tlpController.RowCount = 1;
            this.tlpController.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpController.Size = new System.Drawing.Size(240, 43);
            this.tlpController.TabIndex = 4;
            // 
            // btnSettings
            // 
            this.btnSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.btnSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSettings.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.btnSettings.FlatAppearance.BorderSize = 0;
            this.btnSettings.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Image = global::NCManagementSystem.Properties.Resources.Settings_24W;
            this.btnSettings.Location = new System.Drawing.Point(190, 0);
            this.btnSettings.Margin = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(50, 43);
            this.btnSettings.TabIndex = 4;
            this.btnSettings.UseVisualStyleBackColor = false;
            this.btnSettings.EnabledChanged += new System.EventHandler(this.btnSettings_EnabledChanged);
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // chkController
            // 
            this.chkController.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkController.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.chkController.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkController.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkController.FlatAppearance.BorderSize = 0;
            this.chkController.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.chkController.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.chkController.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkController.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold);
            this.chkController.ForeColor = System.Drawing.Color.White;
            this.chkController.Location = new System.Drawing.Point(51, 0);
            this.chkController.Margin = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.chkController.Name = "chkController";
            this.chkController.Size = new System.Drawing.Size(138, 43);
            this.chkController.TabIndex = 3;
            this.chkController.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkController.UseVisualStyleBackColor = false;
            this.chkController.CheckedChanged += new System.EventHandler(this.chkController_CheckedChanged);
            // 
            // pnlState
            // 
            this.pnlState.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.pnlState.Controls.Add(this.lblState);
            this.pnlState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlState.Location = new System.Drawing.Point(0, 0);
            this.pnlState.Margin = new System.Windows.Forms.Padding(0);
            this.pnlState.Name = "pnlState";
            this.pnlState.Size = new System.Drawing.Size(50, 43);
            this.pnlState.TabIndex = 0;
            // 
            // lblState
            // 
            this.lblState.AutoEllipsis = true;
            this.lblState.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.lblState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblState.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblState.ForeColor = System.Drawing.Color.White;
            this.lblState.GradientStyleProperties.BackColor.FirstColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.lblState.GradientStyleProperties.BackColor.LastColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.lblState.Image = global::NCManagementSystem.Properties.Resources.LampOff_32R;
            this.lblState.Location = new System.Drawing.Point(0, 0);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(50, 43);
            this.lblState.TabIndex = 1;
            this.lblState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ucController00
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.Controls.Add(this.tlpMain);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ucController00";
            this.Size = new System.Drawing.Size(792, 45);
            this.tlpMain.ResumeLayout(false);
            this.pnlEquipCode.ResumeLayout(false);
            this.pnlIPAdr.ResumeLayout(false);
            this.pnlID.ResumeLayout(false);
            this.tlpController.ResumeLayout(false);
            this.pnlState.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Libraries.Controls.Panel.FwTableLayoutPanel tlpMain;
        private Libraries.Controls.Panel.FwPanel pnlIPAdr;
        private Libraries.Controls.Panel.FwPanel pnlID;
        private Libraries.Controls.Panel.FwPanel pnlEquipCode;
        private Libraries.Controls.Panel.FwTableLayoutPanel tlpController;
        private Libraries.Controls.Panel.FwPanel pnlState;
        private Libraries.Controls.Label.FwLabel lblID;
        private Libraries.Controls.Label.FwLabel lblIPAdr;
        private Libraries.Controls.Label.FwLabel lblCode;
        private Libraries.Controls.Label.FwLabel lblState;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.CheckBox chkController;
    }
}
