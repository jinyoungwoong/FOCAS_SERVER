
namespace NCManagementSystem.Libraries.Controls.Forms
{
    partial class FwSkinForm
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
            this.SkinContainer = new NCManagementSystem.Libraries.Controls.Forms.SkinContainer();
            this.SuspendLayout();
            // 
            // SkinContainer
            // 
            this.SkinContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SkinContainer.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SkinContainer.Location = new System.Drawing.Point(2, 2);
            this.SkinContainer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SkinContainer.Name = "SkinContainer";
            this.SkinContainer.OwnerForm = this;
            this.SkinContainer.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.SkinContainer.Size = new System.Drawing.Size(796, 596);
            this.SkinContainer.TabIndex = 0;
            // 
            // FwSkinForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.SkinContainer);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(150, 34);
            this.Name = "FwSkinForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);

        }

        #endregion

        public SkinContainer SkinContainer;
    }
}