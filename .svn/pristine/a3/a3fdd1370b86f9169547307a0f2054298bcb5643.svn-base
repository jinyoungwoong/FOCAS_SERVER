using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using NCManagementSystem.Components.Handlers.Log;
using NCManagementSystem.Components.Models;
using NCManagementSystem.Libraries.Controls;

namespace NCManagementSystem.Components.Controllers
{
    public partial class ucController00 : UserControl
    {
        #region [ Constructor ]
        public ucController00()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();

            InitializeComponent();
        }
        #endregion

        #region [ Member Variables / Fields / Properties / EventHandler ]
        public event EventHandler<ControllerStateChangedEventArgs> OnControllerStateChanged;
        public event EventHandler<ControllerSettingsChangedEventArgs> OnControllerSettingsChanged;
        public event BindingControllerRequestedEvnetHandler OnBindingControllerRequested;
        
        private bool m_IsSkipEvent = false;
        private PControllerContent ControllerContent { get; set; }
        #endregion

        #region [ Override Events / Events / Methods ]
        public void Initialize(PControllerContent content)
        {
            try
            {
                ControllerContent = content;
                if (content.IsTitle)
                {
                    Font _Font = new Font(lblID.Font.FontFamily, 15.75F, FontStyle.Bold);
                    Color _ForeColor = Color.Orange;
                    lblID.LabelStyle = Libraries.Controls.Label.LabelConstsDefiner.LabelStyles.Gradient;
                    lblID.Font = _Font;
                    lblID.ForeColor = _ForeColor;
                    lblID.Text = ConstsDefiner.ControllerHeaders.ID;
                    lblCode.LabelStyle = Libraries.Controls.Label.LabelConstsDefiner.LabelStyles.Gradient;
                    lblCode.Font = _Font;
                    lblCode.ForeColor = _ForeColor;
                    lblCode.Text = ConstsDefiner.ControllerHeaders.Code;
                    lblIPAdr.LabelStyle = Libraries.Controls.Label.LabelConstsDefiner.LabelStyles.Gradient;
                    lblIPAdr.Font = _Font;
                    lblIPAdr.ForeColor = _ForeColor;
                    lblIPAdr.Text = ConstsDefiner.ControllerHeaders.IP;
                    lblState.LabelStyle = Libraries.Controls.Label.LabelConstsDefiner.LabelStyles.Gradient;
                    lblState.Font = _Font;
                    lblState.ForeColor = _ForeColor;
                    lblState.Image = null;
                    lblState.Text = ConstsDefiner.ControllerHeaders.State;
                    chkController.Visible = false;
                    chkController.Enabled = false;
                    btnSettings.Visible = false;
                    btnSettings.Enabled = false;
                }
                else
                {
                    lblID.LabelStyle = Libraries.Controls.Label.LabelConstsDefiner.LabelStyles.Default;
                    lblID.Text = ControllerContent.ID;
                    lblCode.LabelStyle = Libraries.Controls.Label.LabelConstsDefiner.LabelStyles.Default;
                    lblCode.Text = string.IsNullOrEmpty(ControllerContent.CODE) ? ConstsDefiner.FixedString.BlankSymbol : ControllerContent.CODE;
                    lblIPAdr.LabelStyle = Libraries.Controls.Label.LabelConstsDefiner.LabelStyles.Default;
                    lblIPAdr.Text = string.IsNullOrEmpty(ControllerContent.IP) ? ConstsDefiner.FixedString.BlankSymbol : ControllerContent.IP;
                    chkController.Text = ConstsDefiner.ControllerActions.Start;
                    if (!ControllerContent.IsBinding)
                    {
                        lblState.LabelStyle = Libraries.Controls.Label.LabelConstsDefiner.LabelStyles.Default;
                        lblState.Image = null;
                        lblState.Text = ConstsDefiner.FixedString.BlankSymbol;
                        chkController.Enabled = false;
                    }
                    else
                    {
                        lblState.LabelStyle = Libraries.Controls.Label.LabelConstsDefiner.LabelStyles.Default;
                        lblState.Image = Properties.Resources.LampOff_32R;
                        lblState.ResetText();
                        chkController.Enabled = true;
                    }
                    btnSettings.Visible = true;
                    btnSettings.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AutoStart()
        {
            try
            {
                chkController.Checked = true;
            }
            catch (Exception ex)
            {
                LogHandler.Fatal(LogHandler.SerializeException(ex));
                MessageBox.Show(ex.Message);
            }
        }

        private void chkController_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_IsSkipEvent)
                {
                    return;
                }

                OnControllerStateChanged?.Invoke(this, new ControllerStateChangedEventArgs(Name, chkController.Checked));
            }
            catch (Exception ex)
            {
                LogHandler.Fatal(LogHandler.SerializeException(ex));
                MessageBox.Show(ex.Message);
            }
        }

        public void UpdateControllerStatus(ConstsDefiner.ControllerStatuses status)
        {
            try
            {
                switch (status)
                {
                    case ConstsDefiner.ControllerStatuses.Waiting:
                        {
                            lblState.Image = Properties.Resources.LampOnOff_24W;
                            chkController.Text = ConstsDefiner.ControllerActions.Stop;
                            btnSettings.Enabled = false;
                        }
                        break;

                    case ConstsDefiner.ControllerStatuses.Connected:
                        {
                            lblState.Image = Properties.Resources.LampOn_32G;
                            chkController.Text = ConstsDefiner.ControllerActions.Stop;
                            btnSettings.Enabled = false;
                        }
                        break;

                    case ConstsDefiner.ControllerStatuses.Disconnected:
                    default:
                        {
                            if (chkController.Checked)
                            {
                                m_IsSkipEvent = true;
                                chkController.Checked = false;
                                m_IsSkipEvent = false;
                            }
                            lblState.Image = Properties.Resources.LampOff_32R;
                            chkController.Text = ConstsDefiner.ControllerActions.Start;
                            btnSettings.Enabled = true;
                        }
                        break;
                }
                Invalidate();
            }
            catch (Exception ex)
            {
                if (chkController.Checked)
                {
                    m_IsSkipEvent = true;
                    chkController.Checked = false;
                    m_IsSkipEvent = false;
                }
                lblState.Image = Properties.Resources.LampOff_32R;
                chkController.Text = ConstsDefiner.ControllerActions.Start;
                btnSettings.Enabled = true;

                throw ex;
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, string> _BindingControllers = OnBindingControllerRequested?.Invoke();
                ControllerContent.BindingControllers = _BindingControllers;

                using (FormControllerSettings01 _ControllerSettingsDialog = new FormControllerSettings01(ControllerContent))
                {
                    if (_ControllerSettingsDialog.ShowDialog().Equals(DialogResult.OK))
                    {
                        OnControllerSettingsChanged?.Invoke(this, new ControllerSettingsChangedEventArgs(Name, _ControllerSettingsDialog.ControllerContent));
                    }
                }

                ControllerContent.BindingControllers = null;
            }
            catch (Exception ex)
            {
                LogHandler.Fatal(LogHandler.SerializeException(ex));
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSettings_EnabledChanged(object sender, EventArgs e)
        {
            try
            {
                if (btnSettings.Enabled)
                {
                    btnSettings.Image = Properties.Resources.Settings_24W;
                }
                else
                {
                    btnSettings.Image = Properties.Resources.Settings_24G;
                }
            }
            catch (Exception ex)
            {
                LogHandler.Fatal(LogHandler.SerializeException(ex));
                MessageBox.Show(ex.Message);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams _CreateParams = base.CreateParams;
                _CreateParams.ExStyle = _CreateParams.ExStyle | NativeMethods.WS_EX_COMPOSITED;
                return _CreateParams;
            }
        }
        #endregion
    }

}
