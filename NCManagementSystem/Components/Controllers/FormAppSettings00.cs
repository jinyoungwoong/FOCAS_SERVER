﻿using System;
using System.Windows.Forms;
using NCManagementSystem.Components.Handlers.Log;
using NCManagementSystem.Libraries.Controls.Forms;

namespace NCManagementSystem.Components.Controllers
{
    public partial class FormAppSettings00 : FwSkinForm
    {
        #region [ Constructor ]
        public FormAppSettings00()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();

            InitializeComponent();

            Initialize();
        }
        #endregion

        #region [ Member Variables / Fields / Properties / EventHandler ]
        #endregion

        #region [ Override Events / Events / Methods ]
        private void Initialize()
        {
            try
            {
                Icon = Properties.Resources.logo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            ActiveControl = txtMachineCount;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                if (DialogResult.Equals(DialogResult.Retry))
                {
                    e.Cancel = true;
                }
                
                base.OnFormClosing(e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (ModifierKeys.Equals(Keys.None) && keyData.Equals(Keys.Escape))
            {
                Close();
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }
        
        private void btnPermit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!txtMachineCount.RunValidate())
                {
                    return;
                }

                if (!int.TryParse(txtMachineCount.Text.Trim(), out int _iControllerCount))
                {
                    _iControllerCount = 0;
                }
                if (_iControllerCount > (ConstsDefiner.LimitedCountOfControllerRows * 2))
                {
                    MessageBox.Show($"최대 설비 수는 {(ConstsDefiner.LimitedCountOfControllerRows * 2)}대 입니다.", MessageBoxIcon.Warning.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMachineCount.Focus();

                    DialogResult = DialogResult.Retry;
                    return;
                }

                Properties.Settings.Default.CONTROLLER_COUNT = _iControllerCount;
                Properties.Settings.Default.Save();

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                LogHandler.Fatal(LogHandler.SerializeException(ex));
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}
