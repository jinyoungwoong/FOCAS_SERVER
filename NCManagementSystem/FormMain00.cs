using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using NCManagementSystem.Components;
using NCManagementSystem.Components.Controllers;
using NCManagementSystem.Components.Handlers.DB.Repositories;
using NCManagementSystem.Components.Handlers.Log;
using NCManagementSystem.Components.Handlers.Publish;
using NCManagementSystem.Components.Handlers.TrayNotify;
using NCManagementSystem.Components.Models;
using NCManagementSystem.Components.Protocol;
using NCManagementSystem.Libraries.Controls.Forms;
using Newtonsoft.Json;
using NCManagementSystem.Components.License;
using Newtonsoft.Json.Linq;
using NCManagementSystem.Components.License;

namespace NCManagementSystem
{
    public partial class FormMain00 : FwSkinForm
    {
        #region [ Constructor ]
        /// <summary>
        /// 생성자 입니다.
        /// </summary>
        public FormMain00()
        {
            ResetLicense();

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
        /// <summary>
        /// 설비 진단
        /// </summary>
        private List<TRequestDiagnosis> m_RequestDiagnoses = new List<TRequestDiagnosis>();

        /// <summary>
        /// 설비 매개변수(파라미터)
        /// </summary>
        private List<TRequestParameter> m_RequestParameters = new List<TRequestParameter>();

        /// <summary>
        /// 설비(컨트롤러)
        /// </summary>
        private Dictionary<string, PControllerContent> Controllers = new Dictionary<string, PControllerContent>();
        #endregion

        #region [ Override Events / Events / Methods ]
        /// <summary>
        /// 초기화.
        /// </summary>
        private void Initialize()
        {
            try
            {             
                Icon = Properties.Resources.logo;

                string _sFormText = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title; // * 어플리케이션명
                if (PublishHandler.GetInstance().IsDeployed)
                {
                    _sFormText = string.Concat(_sFormText, string.Format(Components.ConstsDefiner.FixedString.Deployed, PublishHandler.GetInstance().Version.ToString()));
                }
                else
                {
                    _sFormText = string.Concat(_sFormText, Components.ConstsDefiner.FixedString.NotDeployed);
                }
                Text = SkinContainer.Text = _sFormText;

                TrayNotifyHandler.GetInstance().ContextMenuStrip = cmsNotifier;
                TrayNotifyHandler.GetInstance().OnMouseDoubleClick += Notifier_OnMouseDoubleClick;

                InitializeAppConfiguration();

                InitializeDataMemoryMap();

                InitializeControllers();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 어플리케이션 구성 초기화.
        /// </summary>
        private void InitializeAppConfiguration()
        {
            try
            {
                string _sConfigurePath = PublishHandler.GetInstance().FilePath;
                string _sConfigureFile = PublishHandler.GetInstance().FileNames.Split(',')[0];
                string _sConfigureFullPath = Path.Combine(_sConfigurePath, _sConfigureFile);
                if (!File.Exists(_sConfigureFullPath))
                {
                    throw new Exception(string.Format(Components.ConstsDefiner.MessageSet.NOT_FOUND_CONFIG, _sConfigureFile));
                }
                XmlDocument _XmlDocument = new XmlDocument();
                _XmlDocument.Load(_sConfigureFullPath);
                XmlNode _XmlNodeDB = _XmlDocument.SelectSingleNode("configuration/database");
                string _sDataSource = _XmlNodeDB.Attributes["datasource"].Value;
                string _sDataBaseName = _XmlNodeDB.Attributes["name"].Value;
                string _sUser = Properties.Settings.Default.DB_USER;
                string _sPassword = Properties.Settings.Default.DB_PASSWORD;
                RunSQL.GetInstance().SetConnectionProperties(DBTypes.PostgreSQL, _sDataSource, _sDataBaseName, _sUser, _sPassword);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 수집 대상 데이터 메모리 맵(설비 진단, 매개변수(파라미터))
        /// </summary>
        private void InitializeDataMemoryMap()
        {
            try
            {
                List<string> _Diagnoss = new List<string>(Enum.GetNames(typeof(DataMemoryMap.Diagnoses)));
                foreach (string _sDiagnosis in _Diagnoss)
                {
                    DataMemoryMap.Diagnoses _Diagnosis = (DataMemoryMap.Diagnoses)Enum.Parse(typeof(DataMemoryMap.Diagnoses), _sDiagnosis);
                    Components.Protocol.ConstsDefiner.DiagnosisTypes _DiagnosisType = Components.Protocol.ConstsDefiner.DiagnosisTypes.NoAxis;
                    if (_sDiagnosis.Contains("AXIS_"))
                    {
                        _DiagnosisType = Components.Protocol.ConstsDefiner.DiagnosisTypes.Axis;
                    }
                    else if (_sDiagnosis.Contains("SPINDLE_"))
                    {
                        _DiagnosisType = Components.Protocol.ConstsDefiner.DiagnosisTypes.Spindle;
                    }
                    bool _IsReal = false;
                    if (_sDiagnosis.Contains("_REAL"))
                    {
                        _IsReal = true;
                    }

                    TRequestDiagnosis _RequestDiagnosis = new TRequestDiagnosis
                    {
                        Name = _sDiagnosis,
                        DiagnosisNumber = (short)_Diagnosis,
                        DiagnosisType = _DiagnosisType,
                        IsRealData = _IsReal,
                        //AxisSeq = -1
                    };
                    m_RequestDiagnoses.Add(_RequestDiagnosis);
                }

                foreach (DataMemoryMap.Parameters _Parameter in Enum.GetValues(typeof(DataMemoryMap.Parameters)))
                {
                    m_RequestParameters.Add(new TRequestParameter { Name = _Parameter.ToString(), ParameterNumber = (int)_Parameter });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 컨트롤러 초기화.
        /// </summary>
        private void InitializeControllers()
        {
            try
            {
                List<PControllerContent> _DefinedControllers = new List<PControllerContent>();
                string _sFileFullPath = Path.Combine(PublishHandler.GetInstance().FilePath, PublishHandler.GetInstance().FileNames.Split(',')[1]);
                if (File.Exists(_sFileFullPath))
                {
                    string _sDefinedControllers = File.ReadAllText(_sFileFullPath);
                    if (!string.IsNullOrEmpty(_sDefinedControllers))
                    {
                        _DefinedControllers = JsonConvert.DeserializeObject<List<PControllerContent>>(_sDefinedControllers);
                    }
                }
                
                int _iControllerCount = Properties.Settings.Default.CONTROLLER_COUNT; // 컨트롤러 수
                if (_iControllerCount > (Components.ConstsDefiner.LimitedCountOfControllerRows * 2))
                {
                    _iControllerCount = (Components.ConstsDefiner.LimitedCountOfControllerRows * 2);
                }
                int _iColumnCount = 1;
                if (Components.ConstsDefiner.LimitedCountOfControllerRows < _iControllerCount)
                {
                    _iColumnCount = 2;
                }
                int _iFullControllerCount = (_iControllerCount + _iColumnCount);
                int _iRowCount = Components.ConstsDefiner.LimitedCountOfControllerRows;
                int _iQuotient = (_iFullControllerCount / _iColumnCount);
                double _dRemainder = (_iFullControllerCount % _iColumnCount);
                if (_dRemainder > 0)
                {
                    _iRowCount = (_iQuotient + 1);
                }
                else
                {
                    _iRowCount = _iQuotient;
                }
                int _iFormBorderThickness = (SkinContainer.BorderProperties.BorderThickness * 2);
                int _iFormPaddingWidth = (flpCanvas.Margin.Left + flpCanvas.Margin.Right + flpCanvas.Padding.Left + flpCanvas.Padding.Right);
                int _iFormPaddingHeight = (flpCanvas.Margin.Top + flpCanvas.Margin.Bottom + flpCanvas.Padding.Top + flpCanvas.Padding.Bottom);
                Width = ((_iFormBorderThickness + _iFormPaddingWidth) + (Components.ConstsDefiner.ControllerWidth * _iColumnCount) + (Components.ConstsDefiner.ControllerColumnPadding * (_iColumnCount - 1)));
                Height = ((_iFormBorderThickness + _iFormPaddingHeight + SkinContainer.Padding.Top + pnlBottom.Height) + (Components.ConstsDefiner.ControllerHeight * _iRowCount) + ((_iRowCount - 1) * Components.ConstsDefiner.ControllerRowPadding));

                for (int _iIdx = 0; _iIdx < _iFullControllerCount; _iIdx++)
                {
                    ucController00 _ControllerCtrl = new ucController00();
                    if (_iIdx.Equals(0))
                    {
                        _ControllerCtrl.Name = string.Concat(Components.ConstsDefiner.FixedString.PrefixTitle, _iIdx.ToString());
                        _ControllerCtrl.Initialize(new PControllerContent() { IsTitle = true, IsBinding = false });
                        _ControllerCtrl.Margin = new Padding(0);
                    }
                    else if (_iIdx.Equals(_iRowCount))
                    {
                        _ControllerCtrl.Name = string.Concat(Components.ConstsDefiner.FixedString.PrefixTitle, _iIdx.ToString());
                        _ControllerCtrl.Initialize(new PControllerContent() { IsTitle = true, IsBinding = false });
                        _ControllerCtrl.Margin = new Padding(Components.ConstsDefiner.ControllerColumnPadding, 0, 0, 0);
                    }
                    else
                    {
                        string _sControllerID = _iIdx.ToString();
                        if (_iIdx > _iRowCount)
                        {
                            _sControllerID = (_iIdx - 1).ToString();
                            _ControllerCtrl.Margin = new Padding(Components.ConstsDefiner.ControllerColumnPadding, Components.ConstsDefiner.ControllerRowPadding, 0, 0);
                        }
                        else
                        {
                            _ControllerCtrl.Margin = new Padding(0, Components.ConstsDefiner.ControllerRowPadding, 0, 0);
                        }

                        bool _IsBinding = false;
                        foreach (PControllerContent _ControllerContent in _DefinedControllers)
                        {
                            if (string.Compare(_sControllerID, _ControllerContent.ID, false).Equals(0))
                            {
                                _ControllerContent.IsTitle = false;
                                _ControllerContent.IsBinding = true;
                                _ControllerCtrl.Name = _ControllerContent.ID;
                                _ControllerCtrl.Initialize(_ControllerContent);
                                _ControllerCtrl.OnControllerStateChanged += ControllerCtrl_OnControllerStateChanged;
                                Controllers.Add(_ControllerContent.ID, _ControllerContent);

                                FocasMaster _FocasMaster = new FocasMaster
                                {
                                    MasterID = _ControllerContent.ID,
                                    RemoteIP = _ControllerContent.IP,
                                    RemotePort = _ControllerContent.PORT,
                                    PingTimeout = 10,
                                    AllocateTimeout = Components.ConstsDefiner.CommunicationSettings.Timeout,
                                    ConnectRetryCount = _ControllerContent.CONNECT_RETRY_COUNT,
                                    ReceiveInterval = _ControllerContent.RECEIVE_INTERVAL
                                };
                                _FocasMaster.RequestDiagnosis(m_RequestDiagnoses);
                                _FocasMaster.RequestParameter(m_RequestParameters);
                                _FocasMaster.OnStatusChanged += FocasMaster_OnStatusChanged;
                                _FocasMaster.OnReceived += FocasMaster_OnReceived;
                                _FocasMaster.OnErrorOccurred += FocasMaster_OnErrorOccurred;
                                _FocasMaster.OnFocasException += FocasMaster_OnFocasException;
                                Controllers[_ControllerContent.ID].FocasMaster = _FocasMaster;

                                _IsBinding = true;
                                break;
                            }
                        }
                        if (!_IsBinding)
                        {
                            _ControllerCtrl.Name = _sControllerID;
                            _ControllerCtrl.Initialize(new PControllerContent() { IsTitle = false, IsBinding = false, ID = _sControllerID });
                        }
                        _ControllerCtrl.OnControllerSettingsChanged += ControllerCtrl_OnControllerSettingsChanged;
                        _ControllerCtrl.OnBindingControllerRequested += ControllerCtrl_OnBindingControllerRequested;
                    }

                    flpCanvas.Controls.Add(_ControllerCtrl);
                    flpCanvas.Controls.SetChildIndex(_ControllerCtrl, _iIdx);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Form.OnShown 이벤트를 발생시킵니다.
        /// </summary>
        /// <param name="e">이벤트 데이터를 포함하는 System.EventArgs입니다.</param>
        protected async override void OnShown(EventArgs e)
        {
            try
            {
                base.OnShown(e);

                RunSQL.GetInstance().OnDBStateMonitoringException += OnGlobalException;
                RunSQL.GetInstance().OnDBStateChanged += OnDBStateChanged;

                chkDBState.Checked = true; // * DB 상태 체크

                // * 시작프로그램 등록
                RegistryWindowsStartUp();

                // * 데이터 수집 자동 시작
                foreach (KeyValuePair<string, PControllerContent> _Controller in Controllers)
                {
                    bool _IsAutoStart = _Controller.Value.IS_AUTO_START;
                    if (_IsAutoStart)
                    {
                        if (flpCanvas.Controls[_Controller.Key] is ucController00 _ControllerCtrl)
                        {
                            _ControllerCtrl.AutoStart();
                            await Task.Delay(Components.ConstsDefiner.CommunicationSettings.AutoStartInterval);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OnGlobalException(this, ex);
            }
        }

        /// <summary>
        /// Form.OnFormClosing 이벤트를 발생시킵니다.
        /// </summary>
        /// <param name="e">이벤트 데이터를 포함하는 System.Windows.Forms.FormClosingEventArgs입니다.</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                if (!TrayNotifyHandler.GetInstance().IsApplicationExit)
                {
                    try
                    {
                        Hide();
                        TrayNotifyHandler.GetInstance().IsShowNotifier = true;
                        e.Cancel = true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    RunSQL.GetInstance().OnDBStateChanged -= OnDBStateChanged;
                    RunSQL.GetInstance().StopDBStateMonitoring();

                    TrayNotifyHandler.GetInstance().OnMouseDoubleClick -= Notifier_OnMouseDoubleClick;
                }

                base.OnFormClosing(e);
            }
            catch (Exception ex)
            {
                LogHandler.Fatal(LogHandler.SerializeException(ex));
            }
        }

        /// <summary>
        /// 설비(컨트롤러) 바인딩
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> ControllerCtrl_OnBindingControllerRequested()
        {
            try
            {
                Dictionary<string, string> _BindingControllers = new Dictionary<string, string>();
                foreach (KeyValuePair<string, PControllerContent> _Controller in Controllers)
                {
                    if (_Controller.Value.IsBinding)
                    {
                        string _sID = _Controller.Key;
                        string _sCode = _Controller.Value.CODE;
                        _BindingControllers.Add(_sID, _sCode);
                    }
                }
                return _BindingControllers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 설비(컨트롤러) 상태 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControllerCtrl_OnControllerStateChanged(object sender, ControllerStateChangedEventArgs e)
        {
            try
            {
                if (Controllers.ContainsKey(e.ID))
                {
                    if (flpCanvas.Controls[e.ID] is ucController00 _ControllerCtrl)
                    {
                        _ControllerCtrl.UpdateControllerStatus(Components.ConstsDefiner.ControllerStatuses.Waiting);
                    }

                    if (e.IsState)
                    {
                        Controllers[e.ID].FocasMaster.Start();
                    }
                    else
                    {
                        Controllers[e.ID].FocasMaster.Stop();
                    }
                }
            }
            catch (Exception ex)
            {
                OnGlobalException(this, ex);
            }
        }

        /// <summary>
        /// 설비(컨트롤러) 설정 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControllerCtrl_OnControllerSettingsChanged(object sender, ControllerSettingsChangedEventArgs e)
        {
            try
            {
                if (Controllers.ContainsKey(e.ID))
                {
                    if (e.ControllerContent.IsBinding)
                    {
                        Controllers[e.ID] = e.ControllerContent;
                        if (flpCanvas.Controls[e.ID] is ucController00 _ControllerCtrl)
                        {
                            _ControllerCtrl.Initialize(e.ControllerContent);
                        }
                        Controllers[e.ID].FocasMaster.OnStatusChanged -= FocasMaster_OnStatusChanged;
                        Controllers[e.ID].FocasMaster.OnReceived -= FocasMaster_OnReceived;
                        Controllers[e.ID].FocasMaster.OnErrorOccurred -= FocasMaster_OnErrorOccurred;
                        Controllers[e.ID].FocasMaster.OnFocasException -= FocasMaster_OnFocasException;
                        Controllers[e.ID].FocasMaster = null;

                        FocasMaster _FocasMaster = new FocasMaster
                        {
                            MasterID = e.ControllerContent.ID,
                            RemoteIP = e.ControllerContent.IP,
                            RemotePort = e.ControllerContent.PORT,
                            PingTimeout = 10,
                            AllocateTimeout = Components.ConstsDefiner.CommunicationSettings.Timeout,
                            ConnectRetryCount = e.ControllerContent.CONNECT_RETRY_COUNT,
                            ReceiveInterval = e.ControllerContent.RECEIVE_INTERVAL
                        };
                        _FocasMaster.RequestDiagnosis(m_RequestDiagnoses);
                        _FocasMaster.RequestParameter(m_RequestParameters);
                        _FocasMaster.OnStatusChanged += FocasMaster_OnStatusChanged;
                        _FocasMaster.OnReceived += FocasMaster_OnReceived;
                        _FocasMaster.OnErrorOccurred += FocasMaster_OnErrorOccurred;
                        _FocasMaster.OnFocasException += FocasMaster_OnFocasException;
                        Controllers[e.ControllerContent.ID].FocasMaster = _FocasMaster;
                    }
                    else
                    {
                        Controllers.Remove(e.ID);
                        if (flpCanvas.Controls[e.ID] is ucController00 _ControllerCtrl)
                        {
                            _ControllerCtrl.Initialize(new PControllerContent() { IsTitle = false, IsBinding = false, ID = e.ID });
                        }
                    }
                }
                else
                {
                    if (e.ControllerContent.IsBinding)
                    {
                        Controllers.Add(e.ControllerContent.ID, e.ControllerContent);
                        if (flpCanvas.Controls[e.ID] is ucController00 _ControllerCtrl)
                        {
                            _ControllerCtrl.OnControllerStateChanged -= ControllerCtrl_OnControllerStateChanged;
                            _ControllerCtrl.Initialize(e.ControllerContent);
                            _ControllerCtrl.OnControllerStateChanged += ControllerCtrl_OnControllerStateChanged;
                        }

                        FocasMaster _FocasMaster = new FocasMaster
                        {
                            MasterID = e.ControllerContent.ID,
                            RemoteIP = e.ControllerContent.IP,
                            RemotePort = e.ControllerContent.PORT,
                            PingTimeout = 10,
                            AllocateTimeout = Components.ConstsDefiner.CommunicationSettings.Timeout,
                            ConnectRetryCount = e.ControllerContent.CONNECT_RETRY_COUNT,
                            ReceiveInterval = e.ControllerContent.RECEIVE_INTERVAL
                        };
                        _FocasMaster.RequestDiagnosis(m_RequestDiagnoses);
                        _FocasMaster.RequestParameter(m_RequestParameters);
                        _FocasMaster.OnStatusChanged += FocasMaster_OnStatusChanged;
                        _FocasMaster.OnReceived += FocasMaster_OnReceived;
                        _FocasMaster.OnErrorOccurred += FocasMaster_OnErrorOccurred;
                        _FocasMaster.OnFocasException += FocasMaster_OnFocasException;
                        Controllers[e.ControllerContent.ID].FocasMaster = _FocasMaster;
                    }
                }

                string _sFileFullPath = Path.Combine(PublishHandler.GetInstance().FilePath, PublishHandler.GetInstance().FileNames.Split(',')[1]);
                List<PControllerContent> _DefinedControllers = new List<PControllerContent>();
                foreach (KeyValuePair<string, PControllerContent> _ControllerContent in Controllers)
                {
                    _DefinedControllers.Add(_ControllerContent.Value);
                }
                string _sRedefinedControllers = JsonConvert.SerializeObject(_DefinedControllers);
                File.WriteAllText(_sFileFullPath, _sRedefinedControllers);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 포카스 마스터 상태 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FocasMaster_OnStatusChanged(object sender, StatusChangedEventArgs e)
        {
            try
            {
                Task.Run(() =>
                {
                    Invoke(new Action(async () =>
                    {
                        FocasMaster _Sender = (FocasMaster)sender;

                        switch (e.Status)
                        {
                            case Components.Protocol.ConstsDefiner.Statuses.Retrying:
                            case Components.Protocol.ConstsDefiner.Statuses.Retrying_PingTest:
                                {
                                    if (flpCanvas.Controls[_Sender.MasterID] is ucController00 _ControllerCtrl)
                                    {
                                        _ControllerCtrl.UpdateControllerStatus(Components.ConstsDefiner.ControllerStatuses.Waiting);
                                    }
                                }
                                break;

                            case Components.Protocol.ConstsDefiner.Statuses.Connected:
                                {
                                    if (flpCanvas.Controls[_Sender.MasterID] is ucController00 _ControllerCtrl)
                                    {
                                        _ControllerCtrl.UpdateControllerStatus(Components.ConstsDefiner.ControllerStatuses.Connected);
                                    }
                                }
                                break;

                            case Components.Protocol.ConstsDefiner.Statuses.Disconnected:
                            case Components.Protocol.ConstsDefiner.Statuses.Disconnected_PingTest:
                            case Components.Protocol.ConstsDefiner.Statuses.Disconnected_Cancel:
                                {
                                    if (Controllers.ContainsKey(_Sender.MasterID))
                                    {
                                        if (RunSQL.GetInstance().IsConnected)
                                        {
                                            try
                                            {
                                                TNCMS_STATUS _NCStatus = new TNCMS_STATUS
                                                {
                                                    MACHINE_CD = Controllers[_Sender.MasterID].CODE,
                                                    TRNX_TIME = DateTime.Now,
                                                    IS_CONNECTED = _Sender.IsConnected ? "1" : "0"
                                                };
                                                RunSQL.GetInstance().UpsertRecord(_NCStatus);
                                                TransferData(_NCStatus, 0);
                                            }
                                            catch (Exception ex)
                                            {
                                                LogHandler.Fatal(LogHandler.SerializeException(ex));
                                            }
                                        }
                                    }
                                    if (flpCanvas.Controls[_Sender.MasterID] is ucController00 _MachineControllerCtrl)
                                    {
                                        _MachineControllerCtrl.UpdateControllerStatus(Components.ConstsDefiner.ControllerStatuses.Disconnected);
                                    }
                                }
                                break;

                            default:
                                break;
                        }

                        string _sLog = $"<'FocasMaster_OnStatusChanged'> : {e.Status}";
                        FocasLogging(_Sender, Components.Protocol.ConstsDefiner.LogTypes.DEBUG, _sLog);
                    }));
                });
            }
            catch (Exception ex)
            {
                LogHandler.Fatal(LogHandler.SerializeException(ex));
                throw ex;
            }
        }

        /// <summary>
        /// 포카스 마스터 데이터 수신 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FocasMaster_OnReceived(object sender, ReceivedEventArgs e)
        {
            try
            {
                Task.Run(() =>
                {
                    Invoke(new Action(() =>
                    {
                        FocasMaster _Sender = (FocasMaster)sender;

                        if (Controllers.ContainsKey(_Sender.MasterID))
                        {
                            string _sSpindleDeviation = string.Empty;
                            string _sSpindleDeviationValue = string.Empty;
                            string _sSpindleResistance = string.Empty;
                            string _sSpindleTemperature = string.Empty;
                            foreach (TResponseDiagnosis _ResponseDiagnosis in _Sender.ReceivedData.ResponseDiagnosis)
                            {
                                string _sName = _ResponseDiagnosis.Name;
                                DataMemoryMap.Diagnoses _Diagnosis = (DataMemoryMap.Diagnoses)Enum.Parse(typeof(DataMemoryMap.Diagnoses), _sName);
                                if (_ResponseDiagnosis.DiagnosisType.Equals(Components.Protocol.ConstsDefiner.DiagnosisTypes.Spindle))
                                {
                                    switch (_Diagnosis)
                                    {
                                        case DataMemoryMap.Diagnoses.SPINDLE_MotorDeviation:
                                            _sSpindleDeviation = _ResponseDiagnosis.Data.ToString();
                                            break;

                                        case DataMemoryMap.Diagnoses.SPINDLE_MotorDeviationValue:
                                            _sSpindleDeviationValue = _ResponseDiagnosis.Data.ToString();
                                            break;

                                        case DataMemoryMap.Diagnoses.SPINDLE_MotorResistance_REAL:
                                            _sSpindleResistance = _ResponseDiagnosis.Data.ToString();
                                            break;

                                        case DataMemoryMap.Diagnoses.SPINDLE_MotorTemperature:
                                            _sSpindleTemperature = _ResponseDiagnosis.Data.ToString();
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            string _sSpindleData = $"{_sSpindleDeviation},{_sSpindleDeviationValue},{_sSpindleResistance},{_sSpindleTemperature}";

                            StringBuilder _sbAxisData = new StringBuilder();
                            foreach (KeyValuePair<int, TAxisStatus> _AxisStatus in _Sender.ReceivedData.AxisStatus)
                            {
                                if (!string.IsNullOrEmpty(_sbAxisData.ToString()))
                                {
                                    _sbAxisData.Append("/");
                                }

                                string _sMotorTemperature = string.Empty;
                                string _sEncodeTemperature = string.Empty;
                                foreach (TResponseDiagnosis _ResponseDiagnosis in _AxisStatus.Value.ResponseDiagnosis)
                                {
                                    string _sName = _ResponseDiagnosis.Name;
                                    DataMemoryMap.Diagnoses _Diagnosis = (DataMemoryMap.Diagnoses)Enum.Parse(typeof(DataMemoryMap.Diagnoses), _sName);
                                    if (_ResponseDiagnosis.DiagnosisType.Equals(Components.Protocol.ConstsDefiner.DiagnosisTypes.Axis))
                                    {
                                        switch (_Diagnosis)
                                        {
                                            case DataMemoryMap.Diagnoses.AXIS_MotorTemperature:
                                                _sMotorTemperature = _ResponseDiagnosis.Data.ToString();
                                                break;

                                            case DataMemoryMap.Diagnoses.AXIS_EncoderTemperature:
                                                _sEncodeTemperature = _ResponseDiagnosis.Data.ToString();
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }

                                _sbAxisData.Append($"{_AxisStatus.Value.AxisName},{_AxisStatus.Value.Absolute},{_AxisStatus.Value.Relative},{_AxisStatus.Value.Machine},{_AxisStatus.Value.Distance},{_sMotorTemperature},{_sEncodeTemperature}");
                            }

                            int _iPartCount = 0;
                            int _iPartsTotal = 0;
                            string _sOperTimeMin = string.Empty;
                            string _sOperTimeSec = string.Empty;
                            string _sCuttingTimeMin = string.Empty;
                            string _sCuttingTimeSec = string.Empty;
                            string _sCycleTimeMin = string.Empty;
                            string _sCycleTimeSec = string.Empty;
                            foreach (TResponseParameter _ResponseParameter in _Sender.ReceivedData.ResponseParameter)
                            {
                                string _sName = _ResponseParameter.Name;
                                switch (_ResponseParameter.ParameterNumber)
                                {
                                    case (int)DataMemoryMap.Parameters.PartsCount:
                                        {
                                            if (!int.TryParse(_ResponseParameter.Data, out _iPartCount))
                                            {
                                                _iPartCount = 0;
                                            }
                                        }
                                        break;
                                    case (int)DataMemoryMap.Parameters.PartsTotal:
                                        {
                                            if (!int.TryParse(_ResponseParameter.Data, out _iPartsTotal))
                                            {
                                                _iPartsTotal = 0;
                                            }
                                        }
                                        break;
                                    case (int)DataMemoryMap.Parameters.OperatingTime_Minute:
                                        _sOperTimeMin = _ResponseParameter.Data.ToString();
                                        break;
                                    case (int)DataMemoryMap.Parameters.OperatingTime_MilliSecond:
                                        _sOperTimeSec = _ResponseParameter.Data.ToString();
                                        break;
                                    case (int)DataMemoryMap.Parameters.CuttingTime_Minute:
                                        _sCuttingTimeMin = _ResponseParameter.Data.ToString();
                                        break;
                                    case (int)DataMemoryMap.Parameters.CuttingTime_MilliSecond:
                                        _sCuttingTimeSec = _ResponseParameter.Data.ToString();
                                        break;
                                    case (int)DataMemoryMap.Parameters.CycleTime_Minute:
                                        _sCycleTimeMin = _ResponseParameter.Data.ToString();
                                        break;
                                    case (int)DataMemoryMap.Parameters.CycleTime_MilliSecond:
                                        _sCycleTimeSec = _ResponseParameter.Data.ToString();
                                        break;
                                    default:
                                        break;
                                }
                            }
                            string _sOperationTime = $"{_sOperTimeMin}:{_sOperTimeSec}";
                            string _sCuttingTime = $"{_sCuttingTimeMin}:{_sCuttingTimeSec}";
                            string _sCycleTime = $"{_sCycleTimeMin}:{_sCycleTimeSec}";

                            string _sLog = $"<'FocasMaster_OnReceived'> : ";
                            _sLog += string.Concat($"{_Sender.ReceivedData.AddInfo} | {_Sender.ReceivedData.Axes} | {_Sender.ReceivedData.MaxAxis} | {_Sender.ReceivedData.MTType} | {_Sender.ReceivedData.Series} | {_Sender.ReceivedData.CNCType} | {_Sender.ReceivedData.Version} | ",
                                                    $"{_Sender.ReceivedData.Aut} | {_Sender.ReceivedData.Run} | {_Sender.ReceivedData.Motion} | {_Sender.ReceivedData.MSTB} | {_Sender.ReceivedData.Alarm} | {_Sender.ReceivedData.Prgnum} | {_Sender.ReceivedData.Prgmnum} | ",
                                                    $"{_Sender.ReceivedData.ActF} | {_Sender.ReceivedData.ActS} | {_Sender.ReceivedData.SpindleLoad} | ",
                                                    $"{_sSpindleData} | ",
                                                    $"{_sbAxisData} | ",
                                                    $"{_iPartCount} | {_iPartsTotal} | {_sOperationTime} | {_sCuttingTime} | {_sCycleTime}");
                            FocasLogging(_Sender, Components.Protocol.ConstsDefiner.LogTypes.DEBUG, _sLog);
                            //Console.WriteLine(_sOperationTime + ", " + _sCuttingTime + ", " + _sCycleTime);
                            RunSQL _sql_instance = RunSQL.GetInstance();
                            if (_sql_instance.IsConnected)
                            {
                                try
                                {

                                    //Console.WriteLine(r_diff.TotalSeconds);
                                    TNCMS_STATUS _NCStatus = new TNCMS_STATUS
                                    {
                                        MACHINE_CD = Controllers[_Sender.MasterID].CODE,
                                        TRNX_TIME = DateTime.Now,
                                        IS_CONNECTED = _Sender.IsConnected ? "1" : "0",
                                        ADDINFO = _Sender.ReceivedData.AddInfo.ToString(),
                                        AXES = _Sender.ReceivedData.Axes,
                                        MAX_AXIS = _Sender.ReceivedData.MaxAxis,
                                        MT_TYPE = _Sender.ReceivedData.MTType,
                                        SERIES = _Sender.ReceivedData.Series,
                                        CNC_TYPE = _Sender.ReceivedData.CNCType,
                                        VERSION = _Sender.ReceivedData.Version,
                                        AUT = _Sender.ReceivedData.Aut,
                                        RUN = _Sender.ReceivedData.Run,
                                        MOTION = _Sender.ReceivedData.Motion,
                                        MSTB = _Sender.ReceivedData.MSTB,
                                        ALARM = _Sender.ReceivedData.Alarm,
                                        PRGNUM = _Sender.ReceivedData.Prgnum,
                                        PRGMNUM = _Sender.ReceivedData.Prgmnum,
                                        ACTF = _Sender.ReceivedData.ActF,
                                        ACTS = _Sender.ReceivedData.ActS,
                                        SPLOAD = _Sender.ReceivedData.SpindleLoad,
                                        SPINDLE_DATA = _sSpindleData,
                                        AXIS_DATA = _sbAxisData.ToString(),
                                        PART_COUNT = _iPartCount,
                                        PARTS_TOTAL = _iPartsTotal,
                                        OPERATING_TIME = $"{_sOperationTime}",
                                        CUTTING_TIME = $"{_sCuttingTime}",
                                        CYCLE_TIME = $"{_sCycleTime}",
                                        RUNNING_TIME = _Sender.ReceivedData.running_time,
                                    };
                                    if (!_Sender.m_CancellationToken.IsCancellationRequested)
                                    {
                                        RunSQL.GetInstance().UpsertRecord(_NCStatus);
                                        TransferData(_NCStatus, 1);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogHandler.Fatal(LogHandler.SerializeException(ex));
                                }
                            }
                        }

                        e.WaitHandle.Set();
                    }));
                });
            }
            catch (Exception ex)
            {
                LogHandler.Fatal(LogHandler.SerializeException(ex));
                throw ex;
            }
        }

        public async Task<string> TransferData(TNCMS_STATUS data, int mode)
        {
            try
            {
                string licenseKey = Properties.Settings.Default.LICENSE_KEY;
                string baseUrl = "http://license.ksolution.kr/DataTransfer/";

                string requestParams;
                if (mode == 0)
                {
                    requestParams = $"LicenseKey={licenseKey}&MACHINE_CD={data.MACHINE_CD}&TRNX_TIME={data.TRNX_TIME}&IS_CONNECTED={data.IS_CONNECTED}";
                }
                else
                {
                    requestParams = $"LicenseKey={licenseKey}&MACHINE_CD={data.MACHINE_CD}&TRNX_TIME={data.TRNX_TIME}&IS_CONNECTED={data.IS_CONNECTED}&ADDINFO={data.ADDINFO}&AXES={data.AXES}&MAX_AXIS={data.MAX_AXIS}&MT_TYPE={data.MT_TYPE}&SERIES={data.SERIES}&CNC_TYPE={data.CNC_TYPE}&VERSION={data.VERSION}&AUT={data.AUT}&RUN={data.RUN}&MOTION={data.MOTION}&MSTB={data.MSTB}&ALARM={data.ALARM}&PRGNUM={data.PRGNUM}&PRGMNUM={data.PRGMNUM}&ACTF={data.ACTF}&ACTS={data.ACTS}&SPLOAD={data.SPLOAD}&SPINDLE_DATA={data.SPINDLE_DATA}&AXIS_DATA={data.AXIS_DATA}&PART_COUNT={data.PART_COUNT}&PARTS_TOTAL={data.PARTS_TOTAL}&OPERATING_TIME={data.OPERATING_TIME}&CUTTING_TIME={data.CUTTING_TIME}&CYCLE_TIME={data.CYCLE_TIME}";
                }

                using (HttpClient client = new HttpClient())
                {
                    var content = new StringContent(requestParams, Encoding.UTF8, "application/x-www-form-urlencoded");

                    HttpResponseMessage response = await client.PostAsync(baseUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        return result;
                    }
                    else
                    {
                        // 실패한 경우 처리
                        return string.Empty; // 혹은 null 또는 다른 값을 반환
                    }
                }
            }
            catch (Exception ex)
            {
                // 예외 처리
                return string.Empty; // 혹은 null 또는 다른 값을 반환
            }
        }


        /// <summary>
        /// 포카스 마스터 에러 발생 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FocasMaster_OnErrorOccurred(object sender, ErrorOccurredEventArgs e)
        {
            try
            {
                string _sLog = $"<'{e.StackTrace}'>";
                if (e.IsFocasFunctions)
                {
                    string _sErrorCode = Enum.GetName(typeof(Focas1.focas_ret), e.ErrorCode);
                    _sLog += $" 《{_sErrorCode}({e.ErrorCode})》";
                }
                _sLog += !string.IsNullOrEmpty(e.ErrorMessage) ? $" : {e.ErrorMessage}" : string.Empty;
                FocasMaster _Sender = (sender as FocasMaster);
                FocasLogging(_Sender, e.ErrorType, _sLog);
            }
            catch (Exception ex)
            {
                LogHandler.Fatal(LogHandler.SerializeException(ex));
                throw ex;
            }
        }

        /// <summary>
        /// 포카스 마스터 예외 발생 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="fex"></param>
        private void FocasMaster_OnFocasException(object sender, Exception fex)
        {
            if (!TrayNotifyHandler.GetInstance().IsApplicationExit)
            {
                Invoke(new Action(() =>
                {
                    LogHandler.Fatal(LogHandler.SerializeException(fex));
                    MessageBox.Show(fex.Message);
                    TrayNotifyHandler.GetInstance().IsApplicationExit = true;
                    Application.DoEvents();
                    Application.Exit();
                }));
            }
        }

        /// <summary>
        /// 포카스 마스터 로그 기록
        /// </summary>
        /// <param name="master"></param>
        /// <param name="logType"></param>
        /// <param name="log"></param>
        private void FocasLogging(FocasMaster master, Components.Protocol.ConstsDefiner.LogTypes logType, string log)
        {
            try
            {
                string _sControllerCode = string.Empty;
                if (Controllers.ContainsKey(master.MasterID))
                {
                    _sControllerCode = Controllers[master.MasterID].CODE;
                }
                string _sLog = $"[#{master.MasterID}({_sControllerCode})] {log}";
                switch (logType)
                {
                    case Components.Protocol.ConstsDefiner.LogTypes.INFO:
                        LogHandler.Info(Components.ConstsDefiner.FocasLogger, _sLog);
                        break;
                    case Components.Protocol.ConstsDefiner.LogTypes.WARN:
                        LogHandler.Warn(Components.ConstsDefiner.FocasLogger, _sLog);
                        break;
                    case Components.Protocol.ConstsDefiner.LogTypes.ERROR:
                        LogHandler.Error(Components.ConstsDefiner.FocasLogger, _sLog);
                        break;
                    case Components.Protocol.ConstsDefiner.LogTypes.FATAL:
                        LogHandler.Fatal(Components.ConstsDefiner.FocasLogger, _sLog);
                        break;
                    case Components.Protocol.ConstsDefiner.LogTypes.DEBUG:
                    default:
                        LogHandler.Debug(Components.ConstsDefiner.FocasLogger, _sLog);
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHandler.Fatal(LogHandler.SerializeException(ex));
                throw ex;
            }
        }

        /// <summary>
        /// 데이터 베이스 상태 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkDBState_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                chkDBState.Image = Properties.Resources.LampOnOff_24W;
                if (chkDBState.Checked)
                {
                    btnDBSettings.Enabled = false;
                    RunSQL.GetInstance().StartDBStateMonitoring();
                }
                else
                {
                    btnDBSettings.Enabled = true;
                    RunSQL.GetInstance().StopDBStateMonitoring();
                }
            }
            catch (Exception ex)
            {
                chkDBState.Image = Properties.Resources.LampOff_32R;
                if (chkDBState.Checked)
                {
                    chkDBState.CheckedChanged -= chkDBState_CheckedChanged;
                    chkDBState.Checked = false;
                    chkDBState.CheckedChanged += chkDBState_CheckedChanged;
                }
                btnDBSettings.Enabled = true;

                LogHandler.Fatal(LogHandler.SerializeException(ex));
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 데이터 베이스 상태 변경 이벤트
        /// </summary>
        private void OnDBStateChanged()
        {
            try
            {
                if (IsDisposed)
                {
                    return;
                }

                if (RunSQL.GetInstance().IsConnected)
                {
                    chkDBState.Image = Properties.Resources.LampOn_32G;
                }
                else
                {
                    if (chkDBState.Checked)
                    {
                        chkDBState.Image = Properties.Resources.LampOnOff_24W;
                    }
                    else
                    {
                        chkDBState.Image = Properties.Resources.LampOff_32R;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHandler.Fatal(LogHandler.SerializeException(ex));
                throw ex;
            }
        }

        /// <summary>
        /// 데이터 베이스 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDBSettings_Click(object sender, EventArgs e)
        {
            try
            {
                using (FormDBSettings00 _DBSettingsDialog = new FormDBSettings00())
                {
                    if (_DBSettingsDialog.ShowDialog().Equals(DialogResult.OK))
                    {
                        InitializeAppConfiguration();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHandler.Fatal(LogHandler.SerializeException(ex));
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 데이터 베이스 설정 버튼 활성화 속성 값이 변경되면 발생합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDBSettings_EnabledChanged(object sender, EventArgs e)
        {
            try
            {
                if (btnDBSettings.Enabled)
                {
                    btnDBSettings.Image = Properties.Resources.Settings_24W;
                }
                else
                {
                    btnDBSettings.Image = Properties.Resources.Settings_24G;
                }
            }
            catch (Exception ex)
            {
                LogHandler.Fatal(LogHandler.SerializeException(ex));
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Notifier 마우스 더블클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Notifier_OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                Show();
                if (WindowState.Equals(FormWindowState.Minimized))
                {
                    WindowState = FormWindowState.Normal;
                }
                Activate();
                TrayNotifyHandler.GetInstance().IsShowNotifier = false;
            }
            catch (Exception ex)
            {
                OnGlobalException(this, ex);
            }
        }

        /// <summary>
        /// 어플리케이션 보기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiShowApplicationUI_Click(object sender, EventArgs e)
        {
            try
            {
                Show();
                if (WindowState.Equals(FormWindowState.Minimized))
                {
                    WindowState = FormWindowState.Normal;
                }
                Activate();
                TrayNotifyHandler.GetInstance().IsShowNotifier = false;
            }
            catch (Exception ex)
            {
                OnGlobalException(this, ex);
            }
        }

        /// <summary>
        /// 어플리케이션 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiApplicationExit_Click(object sender, EventArgs e)
        {
            try
            {
                string _sCaption = (sender as ToolStripMenuItem).Text;
                if (MessageBox.Show(Components.ConstsDefiner.MessageSet.QST_EXIT, _sCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
                {
                    TrayNotifyHandler.GetInstance().IsApplicationExit = true;
                    Application.DoEvents();
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                OnGlobalException(this, ex);
            }
        }

        /// <summary>
        /// 전역 예외 처리.
        /// </summary>
        /// <param name="sender">예외 이벤트의 원본입니다.</param>
        /// <param name="gex">애플리케이션을 실행할 때 나타나는 오류를 나타냅니다.</param>
        private void OnGlobalException(object sender, Exception gex)
        {
            if (!TrayNotifyHandler.GetInstance().IsApplicationExit)
            {
                Invoke(new Action(() =>
                {
                    LogHandler.Fatal(LogHandler.SerializeException(gex));
                    MessageBox.Show(gex.Message);
                    TrayNotifyHandler.GetInstance().IsApplicationExit = true;
                    Application.DoEvents();
                    Application.Exit();
                }));
            }
        }

        /// <summary>
        /// 윈도우 시작 프로그램에 등록합니다.
        /// </summary>
        private static void RegistryWindowsStartUp()
        {
            try
            {
                if (PublishHandler.GetInstance().IsDeployed)
                {
                    using (RegistryKey _RegistryKey = Registry.CurrentUser.OpenSubKey(Components.ConstsDefiner.RegistryKeyPath, true))
                    {
                        string _sProgramPath = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
                        string _sPublisherName = Application.CompanyName;
                        string _sShortcutPath = Path.Combine(_sProgramPath, _sPublisherName);
                        string _sProductName = Application.ProductName;
                        string _sFullPath = string.Concat(Path.Combine(_sShortcutPath, _sProductName), ".appref-ms");
                        _RegistryKey.DeleteValue(_sProductName, false); // * 기존 레지스터리 삭제
                        _RegistryKey.SetValue(_sProductName, _sFullPath); // * 레지스터리 등록
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        private void txtJoin_Click(object sender, EventArgs e)
        {
            try
            {
                if (RunSQL.GetInstance().IsConnected)
                {
                    using (FormJoin _formJoin = new FormJoin())
                    {
                        if (_formJoin.ShowDialog().Equals(DialogResult.OK))
                        {
                            InitializeAppConfiguration();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("DB연결이 되어있지 않습니다.\nDB연결을 확인해주세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                LogHandler.Fatal(LogHandler.SerializeException(ex));
                MessageBox.Show(ex.Message);
            }
        }

        public async void ResetLicense()
        {

            string _sLicenseKey = Properties.Settings.Default.LICENSE_KEY.Trim();
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

                if (_iMachineCnt != Properties.Settings.Default.CONTROLLER_COUNT)
                {
                    if (_IsSuccess)
                    {
                        MessageBox.Show("라이센스의 설비 보유 대수가 수정되어 프로그램이 재실행됩니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Properties.Settings.Default.LICENSE_KEY = _sLicenseKey;
                        Properties.Settings.Default.CONTROLLER_COUNT = _iMachineCnt;
                        Properties.Settings.Default.Save();

                        Application.Exit();
                        Application.ExitThread();
                        Application.Restart();
                    }
                }
            }
        }
    }

}
