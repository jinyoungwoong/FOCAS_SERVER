using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using NCManagementSystem.Components.Handlers.Log;
using NCManagementSystem.Components.Handlers.Publish;
using NCManagementSystem.Components.Helpers;
using NCManagementSystem.Libraries.Controls.Forms;

namespace NCManagementSystem.Components.Controllers
{
    public partial class FormDBSettings00 : FwSkinForm
    {
        #region [ Constructor ]
        public FormDBSettings00()
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
        private ConfigureBuilder m_ConfigureBuilder = new ConfigureBuilder();
        #endregion

        #region [ Override Events / Events / Methods ]
        private void Initialize()
        {
            try
            {
                m_ConfigureBuilder.FilePath = PublishHandler.GetInstance().FilePath;
                m_ConfigureBuilder.FileName = PublishHandler.GetInstance().FileNames.Split(',')[0];

                InitializeConfiguration();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string DataSource { get; set; }
        private string DataBaseName { get; set; }

        private void InitializeConfiguration()
        {
            try
            {
                string _sConfigureFile = PublishHandler.GetInstance().FileNames.Split(',')[0];
                if (m_ConfigureBuilder.IsExistsConfigureFile())
                {
                    Hashtable _htConfigure = m_ConfigureBuilder.LoadConfigure();
                    if (_htConfigure.ContainsKey("database"))
                    {
                        Dictionary<string, string> _Element = _htConfigure["database"] as Dictionary<string, string>;
                        if (!_Element.ContainsKey("datasource"))
                        {
                            throw new Exception(string.Format(ConstsDefiner.MessageSet.INVALID_CONFIG, _sConfigureFile, "database"));
                        }
                        else if (!_Element.ContainsKey("name"))
                        {
                            throw new Exception(string.Format(ConstsDefiner.MessageSet.INVALID_CONFIG, _sConfigureFile, "database"));
                        }

                        foreach (KeyValuePair<string, string> _Attribute in _Element)
                        {
                            switch (_Attribute.Key)
                            {
                                case "datasource":
                                    {
                                        DataSource = _Attribute.Value;
                                    }
                                    break;

                                case "name":
                                    {
                                        DataBaseName = _Attribute.Value;
                                    }
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                    else
                    {
                        throw new Exception(string.Format(ConstsDefiner.MessageSet.INVALID_CONFIG, _sConfigureFile, "database"));
                    }

                    string _sUser = Properties.Settings.Default.DB_USER;
                    string _sPassword = Properties.Settings.Default.DB_PASSWORD;
                    txtDataSource.Text = DataSource;
                    txtDataBaseName.Text = DataBaseName;
                    txtUser.Text = _sUser;
                    txtPassword.Text = _sPassword;
                }
                else
                {
                    throw new Exception(string.Format(ConstsDefiner.MessageSet.NOT_FOUND_CONFIG, _sConfigureFile));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            try
            {
                base.OnShown(e);

                ActiveControl = null;
                Invalidate();
            }
            catch (Exception ex)
            {
                LogHandler.Fatal(LogHandler.SerializeException(ex));
                MessageBox.Show(ex.Message);

                DialogResult = DialogResult.Cancel;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (!txtDataSource.RunValidate() || !txtDataBaseName.RunValidate() || !txtUser.RunValidate() || !txtPassword.RunValidate())
                {
                    return; 
                }

                string _sConfigureFile = PublishHandler.GetInstance().FileNames.Split(',')[0];
                if (m_ConfigureBuilder.IsExistsConfigureFile())
                {
                    Hashtable _htConfigure = m_ConfigureBuilder.LoadConfigure();
                    if (_htConfigure.ContainsKey("database"))
                    {
                        if (((Dictionary<string, string>)_htConfigure["database"]).ContainsKey("datasource"))
                        {
                            ((Dictionary<string, string>)_htConfigure["database"])["datasource"] = txtDataSource.Text.Trim();
                        }
                        if (((Dictionary<string, string>)_htConfigure["database"]).ContainsKey("name"))
                        {
                            ((Dictionary<string, string>)_htConfigure["database"])["name"] = txtDataBaseName.Text.Trim();
                        }
                    }
                    else
                    {
                        throw new Exception(string.Format(ConstsDefiner.MessageSet.INVALID_CONFIG, m_ConfigureBuilder.FileName, "database"));
                    }

                    m_ConfigureBuilder.SaveConfigure(_htConfigure);

                    Properties.Settings.Default.DB_USER = txtUser.Text.Trim();
                    Properties.Settings.Default.DB_PASSWORD = txtPassword.Text.Trim();
                    Properties.Settings.Default.Save();

                    DialogResult = DialogResult.OK;
                }
                else
                {
                    throw new Exception(string.Format(ConstsDefiner.MessageSet.NOT_FOUND_CONFIG, _sConfigureFile));
                }
            }
            catch (Exception ex)
            {
                LogHandler.Fatal(LogHandler.SerializeException(ex));
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult = DialogResult.Cancel;
            }
            catch (Exception ex)
            {
                LogHandler.Fatal(LogHandler.SerializeException(ex));
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

    }
}
