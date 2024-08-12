using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using NCManagementSystem.Components.Handlers.DB.Repositories;
using NCManagementSystem.Components.Handlers.Log;
using NCManagementSystem.Components.Helpers;
using NCManagementSystem.Components.Models;
using NCManagementSystem.Libraries.Controls.DataGridView.Components;
using NCManagementSystem.Libraries.Controls.Forms;

namespace NCManagementSystem.Components.Controllers
{
    public partial class FormJoin : FwSkinForm
    {
        #region [ Constructor ]
        public FormJoin()
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
        public PControllerContent ControllerContent { get; set; }
        #endregion

        #region [ Override Events / Events / Methods ]
        private void Initialize()
        {
            try
            {
                SkinContainer.SplashScreenProperties.BackColor = tlpMain.BackColor;
                SkinContainer.SplashScreenProperties.BackgroundOpacity = 60;
                SkinContainer.SplashScreenProperties.SpokeColor = Color.Orange;

                InitializeGridView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeGridView()
        {
            try
            {
                ColumnSchemaFactory _Columns = ConstsDefiner.GetUserColumnsSchema(dgvsData.GridView);
                dgvsData.GridView.SetColumns(_Columns.ToList());
                dgvsData.GridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvsData.GridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dgvsData.GridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
                dgvsData.InitializeScrollBars();
                dgvsData.GridView.KeyDown += dgvsData_KeyDown;
                dgvsData.GridView.CellClick += dgvsData_CellClick;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected async override void OnShown(EventArgs e)
        {
            try
            {
                base.OnShown(e);

                await Task.Run(() =>
                {
                    IsShowSplashScreen = true;

                    Invoke(new Action(() =>
                    {
                        //lblID.Text = $"#{ControllerContent.ID}";

                        DataTable _dtStoredUserMasterCode = null;
                        if (RunSQL.GetInstance().IsConnected)
                        {
                            List<TNCMS_USER> _StoredUserMasterCode = RunSQL.GetInstance().SelectAllRecordsForUserMaster();
                            _dtStoredUserMasterCode = ConvertHelper.ConvertToDataTable(_StoredUserMasterCode);
                            dgvsData.SetDataSource(_dtStoredUserMasterCode);
                        }

                        if (_dtStoredUserMasterCode.Rows.Count == 0)
                        {
                            txtId.ReadOnly = false;
                            btnNew.Enabled = true;
                            btnCreate.Enabled = true;
                            btnUpdate.Enabled = false;
                            btnDelete.Enabled = false;
                        }
                        else
                        {
                            DataRow _drSelected = dgvsData.GridView.GetDataModel().GetDataSource().Rows[0];

                            txtId.Text = _drSelected[ConstsDefiner.UserDataPropertyNames.ID.ToString()].ToString();
                            txtPwd.Text = _drSelected[ConstsDefiner.UserDataPropertyNames.PASSWORD.ToString()].ToString();
                            txtPwdChk.Text = _drSelected[ConstsDefiner.UserDataPropertyNames.PASSWORD.ToString()].ToString();
                            txtName.Text = _drSelected[ConstsDefiner.UserDataPropertyNames.NAME.ToString()].ToString();
                            txtEmail.Text = _drSelected[ConstsDefiner.UserDataPropertyNames.EMAIL.ToString()].ToString();
                            txtPhone.Text = _drSelected[ConstsDefiner.UserDataPropertyNames.PHONE.ToString()].ToString();
                            chkUse.Checked = _drSelected[ConstsDefiner.UserDataPropertyNames.USE_YN.ToString()].ToString() == "True" ? true : false;
                            chkUserGbn.Checked = _drSelected[ConstsDefiner.UserDataPropertyNames.USER_GBN.ToString()].ToString() == "True" ? true : false;
                            txtId.ReadOnly = true;

                            btnCreate.Enabled = false;
                            btnUpdate.Enabled = true;
                            btnDelete.Enabled = true;

                            btnNew.Enabled = true;
                            btnCreate.Enabled = false;
                            btnUpdate.Enabled = true;
                            btnDelete.Enabled = true;
                        }


                    }));

                    IsShowSplashScreen = false;
                }).ContinueWith(x =>
                {
                    if (x.IsFaulted)
                    {
                        IsShowSplashScreen = false;
                        throw x.Exception.InnerException;
                    }
                });

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

        private void dgvsData_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                e.Handled = true;
                return;
            }
            catch (Exception ex)
            {
                LogHandler.Fatal(LogHandler.SerializeException(ex));
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvsData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex < 0 || e.RowIndex < 0)
                {
                    return;
                }

                if (dgvsData.GridView.SelectedRows.Count.Equals(1))
                {

                    txtId.ReadOnly = true;

                    DataRow _drSelected = dgvsData.GridView.GetDataModel().GetDataSource().Rows[e.RowIndex];

                    txtId.Text = _drSelected[ConstsDefiner.UserDataPropertyNames.ID.ToString()].ToString();
                    txtPwd.Text = _drSelected[ConstsDefiner.UserDataPropertyNames.PASSWORD.ToString()].ToString();
                    txtPwdChk.Text = _drSelected[ConstsDefiner.UserDataPropertyNames.PASSWORD.ToString()].ToString();
                    txtName.Text = _drSelected[ConstsDefiner.UserDataPropertyNames.NAME.ToString()].ToString();
                    txtEmail.Text = _drSelected[ConstsDefiner.UserDataPropertyNames.EMAIL.ToString()].ToString();
                    txtPhone.Text = _drSelected[ConstsDefiner.UserDataPropertyNames.PHONE.ToString()].ToString();
                    chkUse.Checked = _drSelected[ConstsDefiner.UserDataPropertyNames.USE_YN.ToString()].ToString() == "True" ? true : false;
                    chkUserGbn.Checked = _drSelected[ConstsDefiner.UserDataPropertyNames.USER_GBN.ToString()].ToString() == "True" ? true : false;

                    btnCreate.Enabled = false;
                    btnUpdate.Enabled = true;
                    btnDelete.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                LogHandler.Fatal(LogHandler.SerializeException(ex));
                MessageBox.Show(ex.Message);
            }
        }

        bool CheckCreate()
        {
            if (txtId.Text == "")
            {
                MessageBox.Show("ID는 필수입니다.\n", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtPwd.Text == "")
            {
                MessageBox.Show("PASSWORD는 필수입니다.\n", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtPwdChk.Text == "")
            {
                MessageBox.Show("PASSWORD CHECK는 필수입니다.\n", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtName.Text == "")
            {
                MessageBox.Show("NAME은 필수입니다.\n", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private async void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckCreate())
                    return;

                if (txtPwd.Text != txtPwdChk.Text)
                {
                    MessageBox.Show("비밀번호와 비밀번호확인 다릅니다.\n확인 후 다시 시도해주세요.\n\n비밀번호 : " + txtPwd.Text + "\n비밀번호확인 : " + txtPwdChk.Text, "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (RunSQL.GetInstance().IsConnected)
                {
                    await Task.Run(() =>
                    {
                        IsShowSplashScreen = true;

                        Invoke(new Action(() =>
                        {
                            bool _IsExists = RunSQL.GetInstance().ExistsCheckForUserMaster(txtId.Text.Trim());

                            if (!_IsExists)
                            {
                                TNCMS_USER _UserMasterCode = new TNCMS_USER
                                {
                                    ID = txtId.Text.Trim(),
                                    PASSWORD = txtPwd.Text.Trim(),
                                    NAME = txtName.Text.Trim(),
                                    EMAIL = txtEmail.Text.Trim(),
                                    PHONE = txtPhone.Text.Trim(),
                                    USE_YN = chkUse.Checked == true ? "1" : "0",
                                    USER_GBN = chkUserGbn.Checked == true ? "1" : "0"
                                };
                                RunSQL.GetInstance().InsertRecordForUserMaster(_UserMasterCode);

                                List<TNCMS_USER> _StoredUserMasterCode = RunSQL.GetInstance().SelectAllRecordsForUserMaster();
                                DataTable _dtStoredUserMasterCode = ConvertHelper.ConvertToDataTable(_StoredUserMasterCode);

                                if (_dtStoredUserMasterCode != null)
                                {
                                    dgvsData.SetDataSource(_dtStoredUserMasterCode);

                                    foreach (DataRow _drUserMasterCode in _dtStoredUserMasterCode.Rows)
                                    {
                                        string _sStoredUserCode = _drUserMasterCode[ConstsDefiner.UserDataPropertyNames.ID.ToString()].ToString();

                                        if (string.Compare(_sStoredUserCode, _UserMasterCode.ID, false).Equals(0))
                                        {
                                            int _iRowIdx = _dtStoredUserMasterCode.Rows.IndexOf(_drUserMasterCode);
                                            dgvsData.GridView.Rows[_iRowIdx].Cells[0].Selected = true;
                                            dgvsData_CellClick(this, new DataGridViewCellEventArgs(0, _iRowIdx));
                                            dgvsData.GridView.FirstDisplayedScrollingRowIndex = _iRowIdx;
                                            dgvsData.vscVScrollBar.Value = _iRowIdx;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    dgvsData.ClearDataSource();
                                }
                            }
                            else
                            {
                                MessageBox.Show(string.Format(ConstsDefiner.MessageSet.EXISTS_CODE, txtId.Text.Trim()));
                            }
                        }));

                        IsShowSplashScreen = false;
                    }).ContinueWith(x =>
                    {
                        if (x.IsFaulted)
                        {
                            IsShowSplashScreen = false;
                            throw x.Exception.InnerException;
                        }
                    });

                    MessageBox.Show("저장되었습니다.", "완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(ConstsDefiner.MessageSet.CHECK_DATABASE);
                }
            }
            catch (Exception ex)
            {
                LogHandler.Fatal(LogHandler.SerializeException(ex));
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!txtId.RunValidate() || !txtPwd.RunValidate() || !txtPwdChk.RunValidate() || !txtName.RunValidate())
                {
                    return;
                }

                if (txtPwd.Text != txtPwdChk.Text)
                {
                    MessageBox.Show("비밀번호와 비밀번호확인 다릅니다.\n확인 후 다시 시도해주세요.\n\n비밀번호 : " + txtPwd.Text + "\n비밀번호확인 : " + txtPwdChk.Text, "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (RunSQL.GetInstance().IsConnected)
                {
                    await Task.Run(() =>
                    {
                        IsShowSplashScreen = true;

                        Invoke(new Action(() =>
                        {
                            bool _IsExists = RunSQL.GetInstance().ExistsCheckForUserMaster(txtId.Text.Trim());

                            if (_IsExists)
                            {
                                TNCMS_USER _UserMasterCode = new TNCMS_USER
                                {
                                    ID = txtId.Text.Trim(),
                                    PASSWORD = txtPwd.Text.Trim(),
                                    NAME = txtName.Text.Trim(),
                                    EMAIL = txtEmail.Text.Trim(),
                                    PHONE = txtPhone.Text.Trim(),
                                    USE_YN = chkUse.Checked == true ? "1" : "0",
                                    USER_GBN = chkUserGbn.Checked == true ? "1" : "0"
                                };
                                RunSQL.GetInstance().UpdateRecordForUserMaster(_UserMasterCode);

                                List<TNCMS_USER> _StoredUserMasterCode = RunSQL.GetInstance().SelectAllRecordsForUserMaster();
                                DataTable _dtStoredUserMasterCode = ConvertHelper.ConvertToDataTable(_StoredUserMasterCode);

                                if (_dtStoredUserMasterCode != null)
                                {
                                    dgvsData.SetDataSource(_dtStoredUserMasterCode);

                                    foreach (DataRow _drUserMasterCode in _dtStoredUserMasterCode.Rows)
                                    {
                                        string _sStoredMachineCode = _drUserMasterCode[ConstsDefiner.UserDataPropertyNames.ID.ToString()].ToString();
                                        if (string.Compare(_sStoredMachineCode, _UserMasterCode.ID, false).Equals(0))
                                        {
                                            int _iRowIdx = _dtStoredUserMasterCode.Rows.IndexOf(_drUserMasterCode);
                                            dgvsData.GridView.Rows[_iRowIdx].Cells[0].Selected = true;
                                            dgvsData_CellClick(this, new DataGridViewCellEventArgs(0, _iRowIdx));
                                            dgvsData.GridView.FirstDisplayedScrollingRowIndex = _iRowIdx;
                                            dgvsData.vscVScrollBar.Value = _iRowIdx;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    dgvsData.ClearDataSource();
                                }
                            }
                            else
                            {
                                MessageBox.Show(string.Format(ConstsDefiner.MessageSet.NOT_EXISTS_CODE, txtPwd.Text.Trim()));
                            }
                        }));

                        IsShowSplashScreen = false;

                    }).ContinueWith(x =>
                    {
                        if (x.IsFaulted)
                        {
                            IsShowSplashScreen = false;
                            throw x.Exception.InnerException;
                        }
                    });

                    MessageBox.Show("수정되었습니다.", "완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(ConstsDefiner.MessageSet.CHECK_DATABASE);
                }
            }
            catch (Exception ex)
            {
                LogHandler.Fatal(LogHandler.SerializeException(ex));
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!txtPwd.RunValidate())
                {
                    return;
                }

                if (RunSQL.GetInstance().IsConnected)
                {
                    await Task.Run(() =>
                    {
                        IsShowSplashScreen = true;

                        Invoke(new Action(() =>
                        {
                            bool _IsExists = RunSQL.GetInstance().ExistsCheckForUserMaster(txtId.Text.Trim());
                            if (_IsExists)
                            {
                                RunSQL.GetInstance().DeleteRecordOfUserMaster(txtId.Text.Trim());

                                List<TNCMS_USER> _StoredUserMasterCode = RunSQL.GetInstance().SelectAllRecordsForUserMaster();
                                DataTable _dtStoredUserMasterCode = ConvertHelper.ConvertToDataTable(_StoredUserMasterCode);
                                dgvsData.SetDataSource(_dtStoredUserMasterCode);

                                if (dgvsData.GridView.Rows.Count > 0)
                                {
                                    DataRow _drSelected = dgvsData.GridView.GetDataModel().GetDataSource().Rows[0];

                                    txtId.Text = _drSelected[ConstsDefiner.UserDataPropertyNames.ID.ToString()].ToString();
                                    txtPwd.Text = _drSelected[ConstsDefiner.UserDataPropertyNames.PASSWORD.ToString()].ToString();
                                    txtPwdChk.Text = _drSelected[ConstsDefiner.UserDataPropertyNames.PASSWORD.ToString()].ToString();
                                    txtName.Text = _drSelected[ConstsDefiner.UserDataPropertyNames.NAME.ToString()].ToString();
                                    txtEmail.Text = _drSelected[ConstsDefiner.UserDataPropertyNames.EMAIL.ToString()].ToString();
                                    txtPhone.Text = _drSelected[ConstsDefiner.UserDataPropertyNames.PHONE.ToString()].ToString();
                                    chkUse.Checked = _drSelected[ConstsDefiner.UserDataPropertyNames.USE_YN.ToString()].ToString() == "True" ? true : false;
                                    chkUserGbn.Checked = _drSelected[ConstsDefiner.UserDataPropertyNames.USER_GBN.ToString()].ToString() == "True" ? true : false;

                                    btnCreate.Enabled = false;
                                    btnUpdate.Enabled = true;
                                    btnDelete.Enabled = true;
                                }
                                else
                                {
                                    txtId.Text = "";
                                    txtPwd.Text = "";
                                    txtPwdChk.Text = "";
                                    txtName.Text = "";
                                    txtEmail.Text = "";
                                    txtPhone.Text = "";
                                    chkUse.Checked = true;

                                    btnCreate.Enabled = true;
                                    btnUpdate.Enabled = false;
                                    btnDelete.Enabled = false;
                                }
                            }
                            else
                            {
                                MessageBox.Show(string.Format(ConstsDefiner.MessageSet.NOT_EXISTS_CODE, txtPwd.Text.Trim()));
                            }
                        }));

                        IsShowSplashScreen = false;
                    }).ContinueWith(x =>
                    {
                        if (x.IsFaulted)
                        {
                            IsShowSplashScreen = false;
                            throw x.Exception.InnerException;
                        }
                    });

                    MessageBox.Show("삭제되었습니다.", "완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(ConstsDefiner.MessageSet.CHECK_DATABASE);
                }
            }
            catch (Exception ex)
            {
                LogHandler.Fatal(LogHandler.SerializeException(ex));
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
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

        public static bool IsRegexIPAdr(string sIPAdr)
        {
            Regex _Regex = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
            return _Regex.IsMatch(sIPAdr);
        }
        #endregion

        private async void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                await Task.Run(() =>
                {
                    IsShowSplashScreen = true;

                    Invoke(new Action(() =>
                    {
                        dgvsData.GridView.ClearRowSelection();

                        lblStoredData.Visible = false;


                        txtId.ReadOnly = false;

                        txtId.Clear();
                        txtId.Enabled = true;
                        txtPwd.Clear();
                        txtPwdChk.Clear();
                        txtName.Clear();
                        txtEmail.Clear();
                        txtPhone.Clear();
                        chkUse.Checked = true;

                        btnNew.Enabled = true;
                        btnCreate.Enabled = true;
                        btnUpdate.Enabled = false;
                        btnDelete.Enabled = false;
                    }));

                    IsShowSplashScreen = false;
                }).ContinueWith(x =>
                {
                    if (x.IsFaulted)
                    {
                        IsShowSplashScreen = false;
                        throw x.Exception.InnerException;
                    }
                });
            }
            catch (Exception ex)
            {
                LogHandler.Fatal(LogHandler.SerializeException(ex));
                MessageBox.Show(ex.Message);
            }
        }
    }
}
