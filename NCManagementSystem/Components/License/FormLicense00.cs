using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using NCManagementSystem.Components.Handlers.Log;
using NCManagementSystem.Libraries.Controls.Forms;
using Newtonsoft.Json.Linq;

namespace NCManagementSystem.Components.License
{
    public partial class FormLicense00 : FwSkinForm
    {
        #region [ Constructor ]
        public FormLicense00()
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

                SkinContainer.SplashScreenProperties.BackColor = tlpMain.BackColor;
                SkinContainer.SplashScreenProperties.BackgroundOpacity = 60;
                SkinContainer.SplashScreenProperties.SpokeColor = Color.Orange;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            ActiveControl = txtLicenseKey;
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

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!txtLicenseKey.RunValidate())
                {
                    return;
                }

                IsShowSplashScreen = true;

                string _sLicenseKey = txtLicenseKey.Text.Trim();
                string _sResult = string.Empty;
                await Task.Run(() =>
                {
                    _sResult = LicenseHandler.GetInstance().VerificationLicense(_sLicenseKey);
                }).ContinueWith(x =>
                {
                    if (x.IsFaulted)
                    {
                        throw x.Exception.InnerException;
                    }
                });
                JObject _JObject = JObject.Parse(_sResult);
                if (_JObject.Count > 0)
                {
                    bool _IsSuccess = bool.Parse(_JObject["success"].ToString());
                    string _sReturnMessage = _JObject["message"].ToString();
                    // 2022-09-21 추가 : 등록 설비 대수 API Return 및 자동 등록
                    int _iMachineCnt = int.Parse(_JObject["machineCnt"].ToString());
                    if (_IsSuccess)
                    {
                        MessageBox.Show(_sReturnMessage, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Properties.Settings.Default.LICENSE_KEY = _sLicenseKey;
                        Properties.Settings.Default.CONTROLLER_COUNT = _iMachineCnt;
                        Properties.Settings.Default.Save();
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show(_sReturnMessage, MessageBoxIcon.Warning.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtLicenseKey.Focus();
                        DialogResult = DialogResult.Retry;
                    }
                }
                else
                {
                    string _sError = "라이선스 등록 중 오류가 발생하였습니다.\n관리자에게 문의하시기 바랍니다.";
                    MessageBox.Show(_sError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DialogResult = DialogResult.Retry;
                }

                IsShowSplashScreen = false;
            }
            catch (Exception ex)
            {
                IsShowSplashScreen = false;

                LogHandler.Fatal(LogHandler.SerializeException(ex));
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}

