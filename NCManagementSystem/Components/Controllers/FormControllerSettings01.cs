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
    public partial class FormControllerSettings01 : FwSkinForm
    {
        #region [ Constructor ]
        public FormControllerSettings01(PControllerContent content)
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
            
            InitializeComponent();

            ControllerContent = content;

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
                ColumnSchemaFactory _Columns = ConstsDefiner.GetColumnsSchema(dgvsData.GridView);
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
                        lblID.Text = $"#{ControllerContent.ID}";

                        DataTable _dtStoredMachineMasterCode = null;
                        if (RunSQL.GetInstance().IsConnected)
                        {
                            List<TNCMS_MACHINE> _StoredMachineMasterCode = RunSQL.GetInstance().SelectAllRecordsForMachineMaster();
                            _dtStoredMachineMasterCode = ConvertHelper.ConvertToDataTable(_StoredMachineMasterCode);
                            dgvsData.SetDataSource(_dtStoredMachineMasterCode);
                        }

                        chkBindingData.Checked = ControllerContent.IsBinding;
                        if (ControllerContent.IsBinding)
                        {
                            bool _IsSelected = false;

                            if (_dtStoredMachineMasterCode != null)
                            {
                                foreach (DataRow _drMachineMasterCode in _dtStoredMachineMasterCode.Rows)
                                {
                                    string _sStoredMachineCode = _drMachineMasterCode[ConstsDefiner.DataPropertyNames.MACHINE_CD.ToString()].ToString();
                                    if (string.Compare(_sStoredMachineCode, ControllerContent.CODE, false).Equals(0))
                                    {
                                        int _iRowIdx = _dtStoredMachineMasterCode.Rows.IndexOf(_drMachineMasterCode);
                                        dgvsData.GridView.Rows[_iRowIdx].Selected = true;
                                        dgvsData.GridView.Rows[_iRowIdx].Cells[0].Selected = true;
                                        dgvsData_CellClick(this, new DataGridViewCellEventArgs(0, _iRowIdx));
                                        dgvsData.GridView.FirstDisplayedScrollingRowIndex = _iRowIdx;
                                        dgvsData.vscVScrollBar.Value = _iRowIdx;
                                        _IsSelected = true;
                                        break;
                                    }
                                }
                            }

                            if (!_IsSelected)
                            {
                                lblStoredData.Visible = false;
                                txtMachineCode.Text = ControllerContent.CODE;
                                txtMachineCode.Enabled = true;
                                txtMachineName.Text = ControllerContent.NAME;
                                txtModel.Text = ControllerContent.MODEL;
                                txtIPAdr.Text = ControllerContent.IP.ToString();
                                txtPort.Text = ControllerContent.PORT.ToString();
                                txtEmpNm.Text = ControllerContent.EMP_CD == null ? "" : ControllerContent.EMP_CD.ToString();
                                txtRemarks.Clear();
                                chkAutoStart.Checked = ControllerContent.IS_AUTO_START;
                                txtConnectRetryCount.Text = ControllerContent.CONNECT_RETRY_COUNT.ToString();
                                txtReceiveInterval.Text = ControllerContent.RECEIVE_INTERVAL.ToString();
                                btnReset.Enabled = true;
                                btnCreate.Enabled = true;
                                btnUpdate.Enabled = false;
                                btnDelete.Enabled = false;
                            }
                        }
                        else
                        {
                            lblStoredData.Visible = false;
                            txtMachineCode.Clear();
                            txtMachineCode.Enabled = true;
                            txtMachineName.Clear();
                            txtModel.Clear();
                            txtIPAdr.Clear();
                            txtPort.Clear();
                            txtEmpNm.Clear();
                            txtRemarks.Clear();
                            chkAutoStart.Checked = false;
                            txtConnectRetryCount.Text = ConstsDefiner.CommunicationSettings.RetryCount.ToString();
                            txtReceiveInterval.Text = ConstsDefiner.CommunicationSettings.ReceiveInterval.ToString();
                            btnReset.Enabled = false;
                            btnCreate.Enabled = true;
                            btnUpdate.Enabled = false;
                            btnDelete.Enabled = false;
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
                    DataRow _drSelected = dgvsData.GridView.GetDataModel().GetDataSource().Rows[e.RowIndex];
                    string _sMachineCode = _drSelected[ConstsDefiner.DataPropertyNames.MACHINE_CD.ToString()].ToString();
                    txtMachineCode.Text = _sMachineCode;
                    txtMachineCode.Enabled = false;
                    txtMachineName.Text = _drSelected[ConstsDefiner.DataPropertyNames.MACHINE_NM.ToString()].ToString();
                    txtModel.Text = _drSelected[ConstsDefiner.DataPropertyNames.MODEL.ToString()].ToString();
                    txtIPAdr.Text = _drSelected[ConstsDefiner.DataPropertyNames.IP_ADDR.ToString()].ToString();
                    txtPort.Text = _drSelected[ConstsDefiner.DataPropertyNames.PORT.ToString()].ToString();
                    txtEmpNm.Text = _drSelected[ConstsDefiner.DataPropertyNames.EMP_CD.ToString()].ToString();
                    txtRemarks.Text = _drSelected[ConstsDefiner.DataPropertyNames.SCOMMENT.ToString()].ToString();
                    if (string.Compare(_sMachineCode, ControllerContent.CODE,false).Equals(0))
                    {
                        lblStoredData.Visible = true;
                        chkBindingData.Checked = ControllerContent.IsBinding;
                        chkAutoStart.Checked = ControllerContent.IS_AUTO_START;
                        txtConnectRetryCount.Text = ControllerContent.CONNECT_RETRY_COUNT.ToString();
                        txtReceiveInterval.Text = ControllerContent.RECEIVE_INTERVAL.ToString();
                    }
                    else
                    {
                        lblStoredData.Visible = true;
                        chkBindingData.Checked = false;
                        chkAutoStart.Checked = false;
                        txtConnectRetryCount.Text = ConstsDefiner.CommunicationSettings.RetryCount.ToString();
                        txtReceiveInterval.Text = ConstsDefiner.CommunicationSettings.ReceiveInterval.ToString();
                    }
                    if (ControllerContent.IsBinding)
                    {
                        btnReset.Enabled = true;
                    }
                    else
                    {
                        btnReset.Enabled = false;
                    }
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

        private async void btnClear_Click(object sender, EventArgs e)
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
                        chkBindingData.Checked = false;
                        txtMachineCode.Clear();
                        txtMachineCode.Enabled = true;
                        txtMachineName.Clear();
                        txtModel.Clear();
                        txtIPAdr.Clear();
                        txtPort.Clear();
                        txtEmpNm.Clear();
                        txtRemarks.Clear();
                        chkAutoStart.Checked = false;
                        txtConnectRetryCount.Text = ConstsDefiner.CommunicationSettings.RetryCount.ToString();
                        txtReceiveInterval.Text = ConstsDefiner.CommunicationSettings.ReceiveInterval.ToString();
                        if (ControllerContent.IsBinding)
                        {
                            btnReset.Enabled = true;
                        }
                        else
                        {
                            btnReset.Enabled = false;
                        }
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

        private async void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                await Task.Run(() =>
                {
                    IsShowSplashScreen = true;

                    Invoke(new Action(() =>
                    {
                        chkBindingData.Checked = ControllerContent.IsBinding;
                        if (ControllerContent.IsBinding)
                        {
                            bool _IsSelected = false;

                            DataTable _dtStoredMachineMasterCode = dgvsData.GridView.GetDataModel().GetDataSource();
                            if (_dtStoredMachineMasterCode != null)
                            {
                                foreach (DataRow _drMachineMasterCode in _dtStoredMachineMasterCode.Rows)
                                {
                                    string _sStoredMachineCode = _drMachineMasterCode[ConstsDefiner.DataPropertyNames.MACHINE_CD.ToString()].ToString();
                                    if (string.Compare(_sStoredMachineCode, ControllerContent.CODE, false).Equals(0))
                                    {
                                        int _iRowIdx = _dtStoredMachineMasterCode.Rows.IndexOf(_drMachineMasterCode);
                                        dgvsData.GridView.Rows[_iRowIdx].Cells[0].Selected = true;
                                        dgvsData_CellClick(this, new DataGridViewCellEventArgs(0, _iRowIdx));
                                        dgvsData.GridView.FirstDisplayedScrollingRowIndex = _iRowIdx;
                                        dgvsData.vscVScrollBar.Value = _iRowIdx;
                                        _IsSelected = true;
                                        break;
                                    }
                                }
                            }

                            if (!_IsSelected)
                            {
                                lblStoredData.Visible = false;
                                txtMachineCode.Text = ControllerContent.CODE;
                                txtMachineCode.Enabled = true;
                                txtMachineName.Text = ControllerContent.NAME;
                                txtModel.Text = ControllerContent.MODEL;
                                txtIPAdr.Text = ControllerContent.IP.ToString();
                                txtPort.Text = ControllerContent.PORT.ToString();
                                txtEmpNm.Text = ControllerContent.EMP_CD.ToString();
                                txtRemarks.Clear();
                                chkAutoStart.Checked = ControllerContent.IS_AUTO_START;
                                txtConnectRetryCount.Text = ControllerContent.CONNECT_RETRY_COUNT.ToString();
                                txtReceiveInterval.Text = ControllerContent.RECEIVE_INTERVAL.ToString();
                                btnReset.Enabled = true;
                                btnCreate.Enabled = true;
                                btnUpdate.Enabled = false;
                                btnDelete.Enabled = false;
                            }
                        }
                        else
                        {
                            lblStoredData.Visible = false;
                            txtMachineCode.Clear();
                            txtMachineCode.Enabled = true;
                            txtMachineName.Clear();
                            txtModel.Clear();
                            txtIPAdr.Clear();
                            txtPort.Clear();
                            txtEmpNm.Clear();
                            txtRemarks.Clear();
                            chkAutoStart.Checked = false;
                            txtConnectRetryCount.Text = ConstsDefiner.CommunicationSettings.RetryCount.ToString();
                            txtReceiveInterval.Text = ConstsDefiner.CommunicationSettings.ReceiveInterval.ToString();
                            btnReset.Enabled = false;
                            btnCreate.Enabled = true;
                            btnUpdate.Enabled = false;
                            btnDelete.Enabled = false;
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!txtMachineCode.RunValidate() || !txtIPAdr.RunValidate() || !txtPort.RunValidate())
                {
                    return;
                }

                if (!IsRegexIPAdr(txtIPAdr.Text.Trim()))
                {
                    MessageBox.Show(ConstsDefiner.MessageSet.CHECK_IPADR);
                    return;
                }

                DateTime currentDateTime = DateTime.Now;
                DateTime stDate = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 8, 0, 0);
                DateTime endDate = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 17, 0, 0);

                if (RunSQL.GetInstance().IsConnected)
                {
                    await Task.Run(() =>
                    {
                        IsShowSplashScreen = true;

                        Invoke(new Action(() =>
                        {
                            bool _IsExists = RunSQL.GetInstance().CountByRecordForMachineMaster(txtMachineCode.Text.Trim());
                            if (!_IsExists)
                            {
                                TNCMS_MACHINE _MachineMasterCode = new TNCMS_MACHINE
                                {
                                    MACHINE_CD = txtMachineCode.Text.Trim(),
                                    MACHINE_NM = txtMachineName.Text.Trim(),
                                    MODEL = txtModel.Text.Trim(),
                                    IP_ADDR = txtIPAdr.Text.Trim(),
                                    PORT = int.Parse(txtPort.Text.Trim()),
                                    EMP_CD = txtEmpNm.Text.Trim(),
                                    SCOMMENT = txtRemarks.Text.Trim(),
                                    STARTTIME = stDate,
                                    FINISHTIME = endDate
                                };
                                RunSQL.GetInstance().InsertRecordForMachineMaster(_MachineMasterCode);

                                List<TNCMS_MACHINE> _StoredMachineMasterCode = RunSQL.GetInstance().SelectAllRecordsForMachineMaster();
                                DataTable _dtStoredMachineMasterCode = ConvertHelper.ConvertToDataTable(_StoredMachineMasterCode);
                                if (_dtStoredMachineMasterCode != null)
                                {
                                    dgvsData.SetDataSource(_dtStoredMachineMasterCode);

                                    foreach (DataRow _drMachineMasterCode in _dtStoredMachineMasterCode.Rows)
                                    {
                                        string _sStoredMachineCode = _drMachineMasterCode[ConstsDefiner.DataPropertyNames.MACHINE_CD.ToString()].ToString();
                                        if (string.Compare(_sStoredMachineCode, _MachineMasterCode.MACHINE_CD, false).Equals(0))
                                        {
                                            int _iRowIdx = _dtStoredMachineMasterCode.Rows.IndexOf(_drMachineMasterCode);
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
                                MessageBox.Show(string.Format(ConstsDefiner.MessageSet.EXISTS_CODE, txtMachineCode.Text.Trim()));
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
                if (!txtMachineCode.RunValidate() || !txtIPAdr.RunValidate() || !txtPort.RunValidate())
                {
                    return;
                }

                if (!IsRegexIPAdr(txtIPAdr.Text.Trim()))
                {
                    MessageBox.Show(ConstsDefiner.MessageSet.CHECK_IPADR);
                    return;
                }

                if (RunSQL.GetInstance().IsConnected)
                {
                    await Task.Run(() =>
                    {
                        IsShowSplashScreen = true;

                        Invoke(new Action(() =>
                        {
                            bool _IsExists = RunSQL.GetInstance().CountByRecordForMachineMaster(txtMachineCode.Text.Trim());
                            if (_IsExists)
                            {
                                TNCMS_MACHINE _MachineMasterCode = new TNCMS_MACHINE
                                {
                                    MACHINE_CD = txtMachineCode.Text.Trim(),
                                    MACHINE_NM = txtMachineName.Text.Trim(),
                                    MODEL = txtModel.Text.Trim(),
                                    IP_ADDR = txtIPAdr.Text.Trim(),
                                    PORT = int.Parse(txtPort.Text.Trim()),
                                    EMP_CD = txtEmpNm.Text.Trim(),
                                    SCOMMENT = txtRemarks.Text.Trim()
                                };
                                RunSQL.GetInstance().UpdateRecordForMachineMaster(_MachineMasterCode);

                                List<TNCMS_MACHINE> _StoredMachineMasterCode = RunSQL.GetInstance().SelectAllRecordsForMachineMaster();
                                DataTable _dtStoredMachineMasterCode = ConvertHelper.ConvertToDataTable(_StoredMachineMasterCode);
                                if (_dtStoredMachineMasterCode != null)
                                {
                                    dgvsData.SetDataSource(_dtStoredMachineMasterCode);

                                    foreach (DataRow _drMachineMasterCode in _dtStoredMachineMasterCode.Rows)
                                    {
                                        string _sStoredMachineCode = _drMachineMasterCode[ConstsDefiner.DataPropertyNames.MACHINE_CD.ToString()].ToString();
                                        if (string.Compare(_sStoredMachineCode, _MachineMasterCode.MACHINE_CD, false).Equals(0))
                                        {
                                            int _iRowIdx = _dtStoredMachineMasterCode.Rows.IndexOf(_drMachineMasterCode);
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
                                MessageBox.Show(string.Format(ConstsDefiner.MessageSet.NOT_EXISTS_CODE, txtMachineCode.Text.Trim()));
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
                if (!txtMachineCode.RunValidate())
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
                            bool _IsExists = RunSQL.GetInstance().CountByRecordForMachineMaster(txtMachineCode.Text.Trim());
                            if (_IsExists)
                            {
                                RunSQL.GetInstance().DeleteRecordOfMachineMaster(txtMachineCode.Text.Trim());

                                List<TNCMS_MACHINE> _StoredMachineMasterCode = RunSQL.GetInstance().SelectAllRecordsForMachineMaster();
                                DataTable _dtStoredMachineMasterCode = ConvertHelper.ConvertToDataTable(_StoredMachineMasterCode);
                                dgvsData.SetDataSource(_dtStoredMachineMasterCode);

                                chkBindingData.Checked = ControllerContent.IsBinding;
                                if (ControllerContent.IsBinding)
                                {
                                    bool _IsSelected = false;

                                    if (_dtStoredMachineMasterCode != null)
                                    {
                                        foreach (DataRow _drData in _dtStoredMachineMasterCode.Rows)
                                        {
                                            string _sStoredMachineCode = _drData[ConstsDefiner.DataPropertyNames.MACHINE_CD.ToString()].ToString();
                                            if (string.Compare(_sStoredMachineCode, ControllerContent.CODE, false).Equals(0))
                                            {
                                                int _iRowIdx = _dtStoredMachineMasterCode.Rows.IndexOf(_drData);
                                                dgvsData.GridView.Rows[_iRowIdx].Cells[0].Selected = true;
                                                dgvsData_CellClick(this, new DataGridViewCellEventArgs(0, _iRowIdx));
                                                dgvsData.GridView.FirstDisplayedScrollingRowIndex = _iRowIdx;
                                                dgvsData.vscVScrollBar.Value = _iRowIdx;
                                                _IsSelected = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (!_IsSelected)
                                    {
                                        dgvsData.GridView.ClearRowSelection();
                                        lblStoredData.Visible = false;
                                        txtMachineCode.Text = ControllerContent.CODE;
                                        txtMachineCode.Enabled = true;
                                        txtMachineName.Text = ControllerContent.NAME;
                                        txtModel.Text = ControllerContent.MODEL;
                                        txtIPAdr.Text = ControllerContent.IP.ToString();
                                        txtPort.Text = ControllerContent.PORT.ToString();
                                        txtEmpNm.Text = ControllerContent.EMP_CD.ToString();
                                        txtRemarks.Clear();
                                        chkAutoStart.Checked = ControllerContent.IS_AUTO_START;
                                        txtConnectRetryCount.Text = ControllerContent.CONNECT_RETRY_COUNT.ToString();
                                        txtReceiveInterval.Text = ControllerContent.RECEIVE_INTERVAL.ToString();
                                        btnReset.Enabled = true;
                                        btnCreate.Enabled = true;
                                        btnUpdate.Enabled = false;
                                        btnDelete.Enabled = false;
                                    }
                                }
                                else
                                {
                                    dgvsData.GridView.ClearRowSelection();
                                    lblStoredData.Visible = false;
                                    txtMachineCode.Clear();
                                    txtMachineCode.Enabled = true;
                                    txtMachineName.Clear();
                                    txtModel.Clear();
                                    txtIPAdr.Clear();
                                    txtPort.Clear();
                                    txtEmpNm.Clear();
                                    txtRemarks.Clear();
                                    chkAutoStart.Checked = false;
                                    txtConnectRetryCount.Text = ConstsDefiner.CommunicationSettings.RetryCount.ToString();
                                    txtReceiveInterval.Text = ConstsDefiner.CommunicationSettings.ReceiveInterval.ToString();
                                    btnReset.Enabled = false;
                                    btnCreate.Enabled = true;
                                    btnUpdate.Enabled = false;
                                    btnDelete.Enabled = false;
                                }
                            }
                            else
                            {
                                MessageBox.Show(string.Format(ConstsDefiner.MessageSet.NOT_EXISTS_CODE, txtMachineCode.Text.Trim()));
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
            try
            {
                if (chkBindingData.Checked)
                {
                    if (!txtMachineCode.RunValidate() || !txtIPAdr.RunValidate() || !txtPort.RunValidate()
                        || !txtConnectRetryCount.RunValidate() || !txtReceiveInterval.RunValidate())
                    {
                        return;
                    }
                    
                    if (!IsRegexIPAdr(txtIPAdr.Text.Trim()))
                    {
                        MessageBox.Show(ConstsDefiner.MessageSet.CHECK_IPADR);
                        return;
                    }

                    if (ControllerContent.BindingControllers != null)
                    {
                        foreach (KeyValuePair<string, string> _BindingController in ControllerContent.BindingControllers)
                        {
                            if (!string.Compare(ControllerContent.ID, _BindingController.Key, false).Equals(0)
                                && string.Compare(txtMachineCode.Text.Trim(), _BindingController.Value, false).Equals(0))
                            {
                                MessageBox.Show(ConstsDefiner.MessageSet.ALREADY_ASSIGNED_MACHINE);
                                return;
                            }
                        }
                    }

                    ControllerContent.IsBinding = chkBindingData.Checked;
                    ControllerContent.CODE = txtMachineCode.Text.Trim();
                    ControllerContent.NAME = txtMachineName.Text.Trim();
                    ControllerContent.MODEL = txtModel.Text.Trim();
                    ControllerContent.IP = txtIPAdr.Text.Trim();
                    ControllerContent.PORT = int.Parse(txtPort.Text.Trim());
                    ControllerContent.EMP_CD = txtEmpNm.Text.Trim();
                    ControllerContent.IS_AUTO_START = chkAutoStart.Checked;
                    ControllerContent.CONNECT_RETRY_COUNT = int.Parse(txtConnectRetryCount.Text.Trim());
                    ControllerContent.RECEIVE_INTERVAL = int.Parse(txtReceiveInterval.Text.Trim());
                }
                else
                {
                    ControllerContent.IsBinding = chkBindingData.Checked;
                }

                DialogResult = DialogResult.OK;
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

        public static bool IsRegexIPAdr(string sIPAdr)
        {
            Regex _Regex = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
            return _Regex.IsMatch(sIPAdr);
        }
        #endregion
    }
}
