
namespace NCManagementSystem.Components.Controllers
{
    partial class FormAppSettings00
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tlpMain = new NCManagementSystem.Libraries.Controls.Panel.FwTableLayoutPanel();
            this.pnlPermit = new NCManagementSystem.Libraries.Controls.Panel.FwPanel();
            this.btnPermit = new System.Windows.Forms.Button();
            this.pnlCount = new NCManagementSystem.Libraries.Controls.Panel.FwPanel();
            this.pnlMachineCount = new NCManagementSystem.Libraries.Controls.Panel.FwPanel();
            this.txtMachineCount = new NCManagementSystem.Libraries.Controls.TextBox.FwTextBox();
            this.pnlTitle = new NCManagementSystem.Libraries.Controls.Panel.FwPanel();
            this.lblTitle = new NCManagementSystem.Libraries.Controls.Label.FwLabel();
            this.SkinContainer.SuspendLayout();
            this.tlpMain.SuspendLayout();
            this.pnlPermit.SuspendLayout();
            this.pnlCount.SuspendLayout();
            this.pnlMachineCount.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // SkinContainer
            // 
            this.SkinContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.SkinContainer.BorderProperties.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.SkinContainer.BorderProperties.BorderThickness = 1;
            this.SkinContainer.Controls.Add(this.tlpMain);
            this.SkinContainer.Location = new System.Drawing.Point(1, 1);
            this.SkinContainer.Size = new System.Drawing.Size(340, 150);
            this.SkinContainer.Text = "Machines Allocation";
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
            this.SkinContainer.TitleBarProperties.IsMinimizeBox = false;
            this.SkinContainer.TitleBarProperties.TitleBarBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.SkinContainer.TitleBarProperties.TitleBarIcon = global::NCManagementSystem.Properties.Resources.Settings_36W;
            this.SkinContainer.TitleBarProperties.TitleBarTextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SkinContainer.TitleBarProperties.TitleBarTextForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.Controls.Add(this.pnlPermit, 1, 1);
            this.tlpMain.Controls.Add(this.pnlCount, 0, 1);
            this.tlpMain.Controls.Add(this.pnlTitle, 0, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 30);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 2;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.Size = new System.Drawing.Size(340, 120);
            this.tlpMain.TabIndex = 0;
            // 
            // pnlPermit
            // 
            this.pnlPermit.Controls.Add(this.btnPermit);
            this.pnlPermit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPermit.Location = new System.Drawing.Point(160, 60);
            this.pnlPermit.Margin = new System.Windows.Forms.Padding(0);
            this.pnlPermit.Name = "pnlPermit";
            this.pnlPermit.Padding = new System.Windows.Forms.Padding(0, 1, 0, 25);
            this.pnlPermit.Size = new System.Drawing.Size(180, 60);
            this.pnlPermit.TabIndex = 3;
            // 
            // btnPermit
            // 
            this.btnPermit.BackColor = System.Drawing.Color.SteelBlue;
            this.btnPermit.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnPermit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(129)))), ((int)(((byte)(169)))));
            this.btnPermit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPermit.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPermit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnPermit.Location = new System.Drawing.Point(0, 1);
            this.btnPermit.Margin = new System.Windows.Forms.Padding(0);
            this.btnPermit.Name = "btnPermit";
            this.btnPermit.Size = new System.Drawing.Size(120, 34);
            this.btnPermit.TabIndex = 0;
            this.btnPermit.Text = "Permit";
            this.btnPermit.UseVisualStyleBackColor = false;
            this.btnPermit.Click += new System.EventHandler(this.btnPermit_Click);
            // 
            // pnlCount
            // 
            this.pnlCount.Controls.Add(this.pnlMachineCount);
            this.pnlCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCount.Location = new System.Drawing.Point(0, 60);
            this.pnlCount.Margin = new System.Windows.Forms.Padding(0);
            this.pnlCount.Name = "pnlCount";
            this.pnlCount.Padding = new System.Windows.Forms.Padding(20, 4, 10, 28);
            this.pnlCount.Size = new System.Drawing.Size(160, 60);
            this.pnlCount.TabIndex = 1;
            // 
            // pnlMachineCount
            // 
            this.pnlMachineCount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.pnlMachineCount.BorderColor = System.Drawing.Color.DimGray;
            this.pnlMachineCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMachineCount.Controls.Add(this.txtMachineCount);
            this.pnlMachineCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMachineCount.Location = new System.Drawing.Point(20, 4);
            this.pnlMachineCount.Name = "pnlMachineCount";
            this.pnlMachineCount.Padding = new System.Windows.Forms.Padding(1, 1, 20, 1);
            this.pnlMachineCount.Size = new System.Drawing.Size(130, 28);
            this.pnlMachineCount.TabIndex = 0;
            // 
            // txtMachineCount
            // 
            this.txtMachineCount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.txtMachineCount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMachineCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMachineCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.txtMachineCount.ForeColor = System.Drawing.Color.White;
            this.txtMachineCount.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtMachineCount.IsErrorProvider = true;
            this.txtMachineCount.IsValidateEmpty = true;
            this.txtMachineCount.Location = new System.Drawing.Point(1, 1);
            this.txtMachineCount.MaxLength = 36;
            this.txtMachineCount.Name = "txtMachineCount";
            this.txtMachineCount.Size = new System.Drawing.Size(107, 24);
            this.txtMachineCount.TabIndex = 0;
            this.txtMachineCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtMachineCount.TextBoxStyle = NCManagementSystem.Libraries.Controls.TextBox.TextBoxConstsDefiner.TextBoxStyles.Numeric;
            // 
            // pnlTitle
            // 
            this.pnlTitle.AutoSize = true;
            this.tlpMain.SetColumnSpan(this.pnlTitle, 2);
            this.pnlTitle.Controls.Add(this.lblTitle);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlTitle.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Padding = new System.Windows.Forms.Padding(10, 23, 10, 0);
            this.pnlTitle.Size = new System.Drawing.Size(340, 60);
            this.pnlTitle.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.LightGray;
            this.lblTitle.Location = new System.Drawing.Point(10, 23);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(320, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "● Number of allocable machines";
            // 
            // FormAppSettings00
            // 
            this.AcceptButton = this.btnPermit;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(342, 152);
            this.IsFixedWindow = true;
            this.MaximumSize = new System.Drawing.Size(2560, 1392);
            this.Name = "FormAppSettings00";
            this.SkinContainer.ResumeLayout(false);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.pnlPermit.ResumeLayout(false);
            this.pnlCount.ResumeLayout(false);
            this.pnlMachineCount.ResumeLayout(false);
            this.pnlMachineCount.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Libraries.Controls.Panel.FwTableLayoutPanel tlpMain;
        private Libraries.Controls.Panel.FwPanel pnlTitle;
        private Libraries.Controls.Label.FwLabel lblTitle;
        private Libraries.Controls.Panel.FwPanel pnlCount;
        private Libraries.Controls.Panel.FwPanel pnlMachineCount;
        private Libraries.Controls.TextBox.FwTextBox txtMachineCount;
        private Libraries.Controls.Panel.FwPanel pnlPermit;
        private System.Windows.Forms.Button btnPermit;
    }
}