using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NCManagementSystem.Components.Protocol
{
    public class FocasMaster
    {
        private static Mutex singleInstanceMutex = new Mutex(true, "{YourUniqueMutexName}");

        #region [ Constructor ]
        /// <summary>
        /// 생성자
        /// </summary>
        public FocasMaster()
        {
        }
        #endregion

        #region [ Member Variables / Fields / Properties / EventHandler ]
        /// <summary>
        /// 예외 이벤트 핸들러
        /// </summary>
        public event FocasExceptionEventHandler OnFocasException;

        /// <summary>
        /// 상태 변경 이벤트 핸들러
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> OnStatusChanged;

        /// <summary>
        /// 수신 이벤트 핸들러
        /// </summary>
        public event EventHandler<ReceivedEventArgs> OnReceived;

        /// <summary>
        /// 에러 이벤트 핸들러
        /// </summary>
        public event EventHandler<ErrorOccurredEventArgs> OnErrorOccurred;

        private string m_sRemoteIP = "127.0.0.1";
        /// <summary>
        /// IP 주소
        /// </summary>
        public string RemoteIP
        {
            get { return m_sRemoteIP; }
            set { m_sRemoteIP = value; }
        }

        private int m_iRemotePort = 8193;
        /// <summary>
        /// 포트 번호 (기본:8193)
        /// </summary>
        public int RemotePort
        {
            get { return m_iRemotePort; }
            set { m_iRemotePort = value; }
        }

        private int m_iPingTimeout = 10;
        /// <summary>
        /// 핑 테스트 타임아웃 (밀리초)
        /// </summary>
        public int PingTimeout
        {
            get { return m_iPingTimeout; }
            set { m_iPingTimeout = value; }
        }

        private int m_iAllocateTimeout = 1;
        /// <summary>
        /// 포카스 핸들 할당 타임아웃 (초)
        /// </summary>
        public int AllocateTimeout
        {
            get { return m_iAllocateTimeout; }
            set { m_iAllocateTimeout = value; }
        }

        /// <summary>
        /// 연결 실패 횟수
        /// </summary>
        private int m_iConnectRefusedCount = 0;

        private int m_iConnectRetryCount = 0;
        /// <summary>
        /// 연결 재시도 횟수
        /// </summary>
        public int ConnectRetryCount
        {
            get { return m_iConnectRetryCount; }
            set { m_iConnectRetryCount = value; }
        }

        private bool m_IsConnected = false;
        /// <summary>
        /// 연결 여부
        /// </summary>
        public bool IsConnected
        {
            get { return m_IsConnected; }
            set { m_IsConnected = value; }
        }

        private int m_iReconnectInterval = 500;
        /// <summary>
        /// 재연결 간격 (밀리초)
        /// </summary>
        public int ReconnectInterval
        {
            get { return m_iReconnectInterval; }
            set { m_iReconnectInterval = value; }
        }

        private int m_iReceiveInterval = 500;
        /// <summary>
        /// 수신 간격 (밀리초)
        /// </summary>
        public int ReceiveInterval
        {
            get { return m_iReceiveInterval; }
            set { m_iReceiveInterval = value; }
        }

        private ushort m_FlibHndl = 0;
        /// <summary>
        /// 포카스 핸들
        /// </summary>
        public ushort FlibHndl
        {
            get { return m_FlibHndl; }
            set { m_FlibHndl = value; }
        }

        private string m_sMasterID = string.Empty;
        /// <summary>
        /// 포카스 마스터 아이디
        /// </summary>
        public string MasterID
        {
            get { return m_sMasterID; }
            set { m_sMasterID = value; }
        }

        /// <summary>
        /// 수신된 상태 데이터
        /// </summary>
        public TReceivedData ReceivedData { get; set; }

        //2022-09-26 고광진
        // 가동시간 활용
        // 설비 가동상태 확인용.
        public DateTime stDate = DateTime.Now;
        public bool runState = false;
        

        /// <summary>
        /// 요청된 설비 진단
        /// </summary>
        private List<TRequestDiagnosis> m_RequestDiagnoses = new List<TRequestDiagnosis>();

        /// <summary>
        /// 요청된 설비 매개변수(파라미터)
        /// </summary>
        private List<TRequestParameter> m_RequestParameters = new List<TRequestParameter>();

        /// <summary>
        /// 취소되도록 System.Threading.CancellationToken에 신호를 보냅니다.
        /// </summary>
        public CancellationTokenSource m_CancellationTokenSource = null;

        /// <summary>
        /// 작업을 취소하지 않아야 함을 전파합니다.
        /// </summary>
        public CancellationToken m_CancellationToken;

        /// <summary>
        /// 수집 작업 상태 여부
        /// </summary>
        private bool m_IsRunning = false;
        #endregion

        #region [ Override Events / Events / Methods ]
        /// <summary>
        /// 포카스 마스터 연결, 데이터 수신 시작
        /// </summary>
        public async void Start()
        {
            try
            {
                m_CancellationTokenSource = new CancellationTokenSource();
                m_CancellationToken = m_CancellationTokenSource.Token;

                await Task.Run(() =>
                {
                    m_IsRunning = true;

                    short _ReturnCode; // * 수신 반환 코드

                    while (m_IsRunning)
                    {
                        bool _IsErrorOccurred = false; // * 에러 발생 여부
                        DateTime _dtmStartTime = DateTime.Now; // * 작업(Task) 시작 시간

                        if (!IsConnected) // * 미 연결 시
                        {
                            ReceivedData = new TReceivedData(); // * 수신된 데이터 초기화

                            using (Ping _Ping = new Ping())
                            {
                                
                                PingReply _PingReply = _Ping.Send(RemoteIP, PingTimeout); // * 핑 테스트
                                if (_PingReply.Status.Equals(IPStatus.Success)) // * 핑 테스트 성공 시
                                {
                                    // * 재연결 시 기존 할당된 포카스 핸들 해제 및 초기화
                                    if (!FlibHndl.Equals(0))
                                    {
                                        _ReturnCode = Focas1.cnc_freelibhndl(FlibHndl); // * 포카스 핸들 해제
                                        if (_ReturnCode.Equals((int)Focas1.focas_ret.EW_OK))
                                        {
                                            FlibHndl = 0;
                                        }
                                        else
                                        {
                                            FlibHndl = 0; // * 강제로 핸들 해제

                                            OnErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs { IsFocasFunctions = true, StackTrace = "cnc_freelibhndl", ErrorCode = _ReturnCode });
                                        }
                                    }

                                    #region [ ***** cnc_allclibhndl3 ***** ]
                                    _ReturnCode = Focas1.cnc_allclibhndl3(RemoteIP, (ushort)RemotePort, AllocateTimeout, out m_FlibHndl); // * 핸들 할당
                                    if (_ReturnCode.Equals((short)Focas1.focas_ret.EW_OK))
                                    {
                                        IsConnected = true; // * 연결, 핸들 할당됨
                                        m_iConnectRefusedCount = 0;

                                        OnStatusChanged?.Invoke(this, new StatusChangedEventArgs(ConstsDefiner.Statuses.Connected)); // * 상태 이벤트
                                    }
                                    else
                                    {
                                        if (m_CancellationToken.IsCancellationRequested) // * 작업(Task) 취소 여부
                                        {
                                            break;
                                        }

                                        if (ConnectRetryCount.Equals(0) || !ConnectRetryCount.Equals(m_iConnectRefusedCount))
                                        {
                                            m_iConnectRefusedCount++; // * 재시도

                                            OnStatusChanged?.Invoke(this, new StatusChangedEventArgs(ConstsDefiner.Statuses.Retrying)); // * 상태 이벤트
                                            OnErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs { IsFocasFunctions = true, StackTrace = "cnc_allclibhndl3", ErrorCode = _ReturnCode, ErrorMessage = "Retrying", ErrorType = ConstsDefiner.LogTypes.INFO });
                                        }
                                        else
                                        {
                                            m_IsRunning = false; // * 연결끊김
                                            IsConnected = false;
                                            m_iConnectRefusedCount = 0;

                                            OnStatusChanged?.Invoke(this, new StatusChangedEventArgs(ConstsDefiner.Statuses.Disconnected)); // * 상태 이벤트
                                            OnErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs { IsFocasFunctions = true, StackTrace = "cnc_allclibhndl3", ErrorCode = _ReturnCode, ErrorMessage = "Disconnected" });

                                            break;
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    if (m_CancellationToken.IsCancellationRequested) // * 작업(Task) 취소 여부
                                    {
                                        break;
                                    }

                                    if (ConnectRetryCount.Equals(0) || !ConnectRetryCount.Equals(m_iConnectRefusedCount))
                                    {
                                        m_iConnectRefusedCount++; // * 재시도(PingTest)

                                        OnStatusChanged?.Invoke(this, new StatusChangedEventArgs(ConstsDefiner.Statuses.Retrying_PingTest)); // * 상태 이벤트
                                        OnErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs { IsFocasFunctions = false, StackTrace = "PingTest", ErrorMessage = "Retrying", ErrorType = ConstsDefiner.LogTypes.INFO });
                                    }
                                    else
                                    {
                                        m_IsRunning = false; // * 연결끊김(PingTest)
                                        IsConnected = false;
                                        m_iConnectRefusedCount = 0;

                                        OnStatusChanged?.Invoke(this, new StatusChangedEventArgs(ConstsDefiner.Statuses.Disconnected_PingTest)); // * 상태 이벤트
                                        OnErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs { IsFocasFunctions = false, StackTrace = "PingTest", ErrorMessage = "Disconnected" });

                                        break;
                                    }
                                }
                            }

                            DateTime _dtmFinishTime = DateTime.Now; // * 작업(Task) 완료 시간
                            TimeSpan _tsInterval = (_dtmFinishTime - _dtmStartTime); // * 대기(재연결) 간격 계산
                            if (_tsInterval.TotalMilliseconds < ReconnectInterval)
                            {
                                int _iCalculatedInterval = (ReconnectInterval - (int)_tsInterval.TotalMilliseconds); // * 남은 대기(재연결) 간격 계산
                                bool _IsCancellation = m_CancellationToken.WaitHandle.WaitOne(_iCalculatedInterval); // * 대기(재연결)
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            if (!short.TryParse(ReceivedData.Axes, out short _Axes))  // * 축 갯수
                            {
                                _Axes = 0;
                            }
                            
                            if (!short.TryParse(ReceivedData.MaxAxis, out short _MaxAxis))  // * 최대 축 갯수
                            {
                                _MaxAxis = 0;
                            }

                            if (!ReceivedData.IsAssigned) // * 최초 접속 연결 이후 데이터 변동 X 경우...
                            {
                                #region [ ***** cnc_sysinfo ***** ]
                                // * ※ Available : Series 15i, 16/18/21, 16i/18i/21i, 0i, 30i/31i/32i, Power Mate i, PMi-A
                                Focas1.ODBSYS _odbsys = new Focas1.ODBSYS();

                                _ReturnCode = Focas1.cnc_sysinfo(FlibHndl, _odbsys);
                                if (_ReturnCode.Equals((int)Focas1.focas_ret.EW_OK))
                                {
                                    ReceivedData.AddInfo = _odbsys.addinfo; // * Additional information
                                    //ReceiveDataParser.GetInstance().ParsingAdditionalInformation(_odbsys.addinfo);

                                    ReceivedData.Axes = string.Concat(_odbsys.axes); // * Current controlled axes (ASCII)
                                    _Axes = short.Parse(ReceivedData.Axes);

                                    ReceivedData.MaxAxis = _odbsys.max_axis.ToString(); // * Maximum controlled axes
                                    _MaxAxis = short.Parse(ReceivedData.MaxAxis);

                                    ReceivedData.MTType = string.Concat(_odbsys.mt_type); // * Kind of M/T (ASCII)
                                    if (string.IsNullOrEmpty(ReceivedData.MTType)) // * ※ [이우철차장] 참고
                                    {
                                        if (_Axes.Equals(2))
                                        {
                                            ReceivedData.MTType = "T"; // * Lathe
                                        }
                                        else
                                        {
                                            ReceivedData.MTType = "M"; // * Machining center
                                        }
                                    }

                                    ReceivedData.Series = string.Concat(_odbsys.series); // * Series number of CNC (ASCII)

                                    ReceivedData.CNCType = string.Concat(_odbsys.cnc_type); // * Kind of CNC (ASCII)

                                    ReceivedData.Version = string.Concat(_odbsys.version); // * Version number of CNC (ASCII)
                                }
                                else
                                {
                                    IsConnected = false; // * 재접속

                                    OnErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs { IsFocasFunctions = true, StackTrace = "cnc_sysinfo", ErrorCode = _ReturnCode });

                                    continue;
                                }
                                #endregion

                                #region [ ***** cnc_rdaxisname ***** ]
                                Focas1.ODBAXISNAME_data _odbaxisname_data = new Focas1.ODBAXISNAME_data();
                                byte[] _AxisNameBytes = new byte[(Marshal.SizeOf(_odbaxisname_data) * _Axes)];
                                short data_num = _Axes;

                                _ReturnCode = Focas1.cnc_rdaxisname(FlibHndl, ref data_num, _AxisNameBytes);
                                if (_ReturnCode.Equals((int)Focas1.focas_ret.EW_OK))
                                {
                                    ArrayList _AxisNameStructures = DeserializeToStructure(_odbaxisname_data, _AxisNameBytes, data_num);
                                    for (int _iAxisSeq = 1; _iAxisSeq <= data_num; _iAxisSeq++)
                                    {
                                        string _sAxisName = Convert.ToChar(((Focas1.ODBAXISNAME_data)_AxisNameStructures[_iAxisSeq - 1]).name).ToString(); // * 축 이름
                                        if (string.IsNullOrEmpty(_sAxisName.Trim('\0')))
                                        {
                                            _sAxisName = _iAxisSeq.ToString(); // * 축 이름 빈 값일 때 축 순번
                                        }

                                        ReceivedData.AxisStatus.Add(_iAxisSeq, new TAxisStatus() { AxisName = _sAxisName });
                                    }
                                }
                                else
                                {
                                    IsConnected = false; // * 재접속

                                    OnErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs { IsFocasFunctions = true, StackTrace = "cnc_rdaxisname", ErrorCode = _ReturnCode });

                                    continue;
                                }
                                #endregion

                                #region [ ***** cnc_rdparar ***** ]
                                /* * 좌표값 소숫점 위치 */
                                /* ※ [이우철차장] CNC Type 별 파라미터 번호가 1004 / 1013 인지 확인 */
                                Focas1.IODBPSD_1 _iodbpsd = new Focas1.IODBPSD_1();
                                short _length = (short)(Marshal.SizeOf(_iodbpsd));

                                switch (ReceivedData.CNCType)
                                {
                                    case "16":
                                    case "18":
                                        {
                                            short _number = 1004;
                                            short _axis = 0;
                                            int _iAxisPositionDotPoint = 3;

                                            _ReturnCode = Focas1.cnc_rdparar(FlibHndl, ref _number, _axis, ref _number, ref _length, _iodbpsd);
                                            if (_ReturnCode.Equals((int)Focas1.focas_ret.EW_OK))
                                            {
                                                BitArray _BitArray = new BitArray(new int[] { _iodbpsd.ldata });
                                                bool _ISA = _BitArray[0];
                                                bool _ISC = _BitArray[1];
                                                if (!_ISA && !_ISC)
                                                {
                                                    _iAxisPositionDotPoint = 3;
                                                }
                                                else if (_ISA && !_ISC)
                                                {
                                                    _iAxisPositionDotPoint = 2;
                                                }
                                                else if (!_ISA && _ISC)
                                                {
                                                    _iAxisPositionDotPoint = 4;
                                                }
                                                else
                                                {
                                                    _iAxisPositionDotPoint = 3;
                                                }

                                                for (short _AxisSeq = 1; _AxisSeq <= _Axes; _AxisSeq++)
                                                {
                                                    ReceivedData.AxisStatus[_AxisSeq].AxisPositionDotPoint = _iAxisPositionDotPoint;
                                                }
                                            }
                                            else
                                            {
                                                if (_ReturnCode.Equals((int)Focas1.focas_ret.EW_RANGE))
                                                {
                                                    string _sErrorMessage = $"*Axis: {_axis} | *Parameter Number: {_number}";
                                                    OnErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs { IsFocasFunctions = true, StackTrace = "cnc_rdparar", ErrorCode = _ReturnCode, ErrorMessage = _sErrorMessage, ErrorType = ConstsDefiner.LogTypes.INFO });

                                                    continue;
                                                }
                                                else
                                                {
                                                    _IsErrorOccurred = true;

                                                    string _sErrorMessage = $"*Axis: {_axis} | *Parameter Number: {_number}";
                                                    OnErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs { IsFocasFunctions = true, StackTrace = "cnc_rdparar", ErrorCode = _ReturnCode, ErrorMessage = _sErrorMessage });

                                                    break;
                                                }
                                            }
                                        }
                                        break;

                                    default:
                                        {
                                            short _number = 1013;

                                            for (short _AxisSeq = 1; _AxisSeq <= _Axes; _AxisSeq++)
                                            {
                                                short _axis = _AxisSeq;

                                                _ReturnCode = Focas1.cnc_rdparar(FlibHndl, ref _number, _axis, ref _number, ref _length, _iodbpsd);
                                                if (_ReturnCode.Equals((int)Focas1.focas_ret.EW_OK))
                                                {
                                                    BitArray _BitArray = new BitArray(new int[] { _iodbpsd.ldata });
                                                    bool _ISA = _BitArray[0];
                                                    bool _ISC = _BitArray[1];
                                                    if (!_ISA && !_ISC)
                                                    {
                                                        ReceivedData.AxisStatus[_AxisSeq].AxisPositionDotPoint = 3;
                                                    }
                                                    else if (_ISA && !_ISC)
                                                    {
                                                        ReceivedData.AxisStatus[_AxisSeq].AxisPositionDotPoint = 2;
                                                    }
                                                    else if (!_ISA && _ISC)
                                                    {
                                                        ReceivedData.AxisStatus[_AxisSeq].AxisPositionDotPoint = 4;
                                                    }
                                                    else
                                                    {
                                                        ReceivedData.AxisStatus[_AxisSeq].AxisPositionDotPoint = 3;
                                                    }
                                                }
                                                else
                                                {
                                                    if (_ReturnCode.Equals((int)Focas1.focas_ret.EW_RANGE))
                                                    {
                                                        string _sErrorMessage = $"*Axis: {_axis} | *Parameter Number: {_number}";
                                                        OnErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs { IsFocasFunctions = true, StackTrace = "cnc_rdparar", ErrorCode = _ReturnCode, ErrorMessage = _sErrorMessage, ErrorType = ConstsDefiner.LogTypes.INFO });

                                                        continue;
                                                    }
                                                    else
                                                    {
                                                        _IsErrorOccurred = true;

                                                        string _sErrorMessage = $"*Axis: {_axis} | *Parameter Number: {_number}";
                                                        OnErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs { IsFocasFunctions = true, StackTrace = "cnc_rdparar", ErrorCode = _ReturnCode, ErrorMessage = _sErrorMessage });

                                                        break;
                                                    }

                                                    //IsConnected = false; // * 재접속
                                                    //string _sErrorMessage = $"*Axis: {_AxisSeq} | *Parameter Number: {_number}";
                                                    //OnErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs { IsFocasFunctions = true, StackTrace = "cnc_rdparar", ErrorCode = _ReturnCode, ErrorMessage = _sErrorMessage });
                                                    //continue;
                                                }
                                            }
                                            if (_IsErrorOccurred)
                                            {
                                                IsConnected = false; // * 재접속
                                                continue;
                                            }
                                        }
                                        break;
                                }
                                #endregion

                                ReceivedData.IsAssigned = true; // * 접속 연결 후 한번만... 여부
                            }

                            #region [ ***** cnc_statinfo ***** ]
                            dynamic _odbst = new object();
                            switch (ReceivedData.CNCType)
                            {
                                case "15":
                                    {
                                        // * ※ Available: Series 15/15i
                                        _odbst = new Focas1.ODBST_FS15();
                                        _ReturnCode = Focas1.cnc_statinfo(FlibHndl, _odbst);
                                        if (_ReturnCode.Equals((int)Focas1.focas_ret.EW_OK))
                                        {
                                            //short _dummy = _odbst.dummy; // 2byte

                                            ReceivedData.Aut = _odbst.aut; // * AUTOMATIC/MANUAL mode selection
                                            //ReceiveDataParser.GetInstance().ParsingAutomaticMode(_odbst.aut);

                                            short _manual = _odbst.manual; // * MANUAL mode selection
                                            //ReceiveDataParser.GetInstance().ParsingManualMode(_odbst.manual);

                                            ReceivedData.Run = _odbst.run; // * Status of automatic operation
                                            //ReceiveDataParser.GetInstance().ParsingOperationStatus(_odbst.run);

                                            short _edit = _odbst.edit; // * Status of program editing
                                            //ReceiveDataParser.GetInstance().ParsingProgramEditingStatus(_odbst.edit);

                                            ReceivedData.Motion = _odbst.motion; // * Status of axis movement, dwell
                                            //ReceiveDataParser.GetInstance().ParsingMotion(_odbst.motion);

                                            ReceivedData.MSTB = _odbst.mstb; // * Status of M,S,T,B function
                                            //ReceiveDataParser.GetInstance().ParsingMSTBFunctionStatus(_odbst.mstb);

                                            short _emergency = _odbst.emergency; // * Status of emergency
                                            //ReceiveDataParser.GetInstance().ParsingEmergencyStatus(_odbst.emergency);

                                            short _write = _odbst.write; // * Status of writing backupped memory
                                            /*
                                            0 : (Not writing)
                                            1 : @(writing)
                                            */

                                            short _labelskip = _odbst.labelskip; // * Status of label skip
                                            /*
                                            0 : LABEL SKIP
                                            1 : (Not label skip)
                                            */

                                            ReceivedData.Alarm = _odbst.alarm; // * Status of alarm
                                            //ReceiveDataParser.GetInstance().ParsingAlarmStatus(_odbst.alarm);

                                            short _warning = _odbst.warning; // * Status of warning
                                            /*
                                            0 : (No warning)
                                            1 : WARNING
                                            */

                                            short _battery = _odbst.battery; // * Status of battery
                                            /*
                                            0 : (Normal)
                                            1 : BATTERY LOW(backupped memory)
                                            2 : BATTERY LOW(absolute position detector)
                                            */
                                        }
                                        else
                                        {
                                            _IsErrorOccurred = true;

                                            string _sErrorMessage = $"*CNC Type: {ReceivedData.CNCType}";
                                            OnErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs { IsFocasFunctions = true, StackTrace = "cnc_statinfo", ErrorCode = _ReturnCode, ErrorMessage = _sErrorMessage });

                                            break;
                                        }
                                    }
                                    break;

                                default:
                                    {
                                        // * ※ Available : Series 16i/18i-W
                                        // * ※ Available : Series 16/18/21, 16i/18i/21i, 0i, 30i/31i/32i, Power Mate i, PMi-A
                                        _odbst = new Focas1.ODBST();

                                        _ReturnCode = Focas1.cnc_statinfo(FlibHndl, _odbst);
                                        if (_ReturnCode.Equals((int)Focas1.focas_ret.EW_OK))
                                        {
                                            // * ※ Available: 16i/18i-W
                                            //short _dummy = _odbst.dummy; // 2byte

                                            // * ※ Available: Series 16/18/21, 16i/18i/21i, 0i, 30i/31i/32i, Power Mate i, PMi-A
                                            //short _hdck = _odbst.hdck; // * 30i/31i/32i, 0i-D/F only
                                            /* Status of manual handle re - trace
                                            0 : Invalid of manual handle re - trace
                                            1 : M.H.RTR.(Manual handle re - trace)
                                            2 : NO RVRS.(Backward movement prohibition)
                                            3 : NO CHAG.(Direction change prohibition)
                                            */
                                            short _tmmode = _odbst.tmmode; // * T/M mode selection (only with compound machining function) 

                                            ReceivedData.Aut = _odbst.aut; // * AUTOMATIC/MANUAL mode selection
                                            //ReceiveDataParser.GetInstance().ParsingAutomaticMode(_odbst.aut);

                                            ReceivedData.Run = _odbst.run; // * Status of automatic operation
                                            //ReceiveDataParser.GetInstance().ParsingOperationStatus(_odbst.run);

                                            ReceivedData.Motion = _odbst.motion; // * Status of axis movement, dwell
                                            //ReceiveDataParser.GetInstance().ParsingMotion(_odbst.motion);

                                            ReceivedData.MSTB = _odbst.mstb; // * Status of M,S,T,B function
                                            //ReceiveDataParser.GetInstance().ParsingMSTBFunctionStatus(_odbst.mstb);

                                            short _emergency = _odbst.emergency; // * Status of emergency
                                            //ReceiveDataParser.GetInstance().ParsingEmergencyStatus(_odbst.emergency);

                                            ReceivedData.Alarm = _odbst.alarm; // * Status of alarm
                                            //ReceiveDataParser.GetInstance().ParsingAlarmStatus(_odbst.alarm);

                                            short _edit = _odbst.edit; // * Status of others In case of 0i - D, the meaning of status(16,21,23,42,44,46) is changed according to the parameter No.13104#0.(same as 30i)
                                            //ReceiveDataParser.GetInstance().ParsingProgramEditingStatus(_odbst.edit);
                                        }
                                        else
                                        {
                                            _IsErrorOccurred = true;

                                            string _sErrorMessage = $"*CNC Type: {ReceivedData.CNCType}";
                                            OnErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs { IsFocasFunctions = true, StackTrace = "cnc_statinfo", ErrorCode = _ReturnCode, ErrorMessage = _sErrorMessage });

                                            break;
                                        }
                                    }
                                    break;
                            }

                            if (_IsErrorOccurred)
                            {
                                IsConnected = false;
                                continue;
                            }
                            #endregion

                            #region [ ***** cnc_rddynamic ***** ]
                            dynamic _odbdy = new object();
                            switch (_MaxAxis)
                            {
                                case 8:
                                    _odbdy = new Focas1.ODBDY_1_AX8();
                                    break;

                                case 32:
                                default:
                                    _odbdy = new Focas1.ODBDY_1();
                                    break;
                            }

                            _ReturnCode = Focas1.cnc_rddynamic(FlibHndl, -1, (short)Marshal.SizeOf(_odbdy), _odbdy);
                            if (_ReturnCode.Equals((int)Focas1.focas_ret.EW_OK))
                            {
                                short _axis = _odbdy.axis; // * Axis number

                                short _alarm = _odbdy.alarm; // * Alarm status;
                                //ReceiveDataParser.GetInstance().ParsingAlarm(_odbdy.alarm);

                                ReceivedData.Prgnum = _odbdy.prgnum; // * Program number under execution

                                ReceivedData.Prgmnum = _odbdy.prgmnum; // * Main program number 

                                int _seqnum = _odbdy.seqnum; // * Current sequence number

                                ReceivedData.ActF = _odbdy.actf; // * Actual feed rate of the controlled axes

                                ReceivedData.ActS = _odbdy.acts; // * Actual spindle speed data

                                for (int _iAxisSeq = 1; _iAxisSeq <= _Axes; _iAxisSeq++)
                                {
                                    /*
                                    ReceivedData.AxisStatus[_iAxisSeq].Absolute = (_odbdy.pos.absolute[(_iAxisSeq - 1)] / 1000d).ToString("#####0.000").ToString(); // * Absolute position of the controlled axes
                                    ReceivedData.AxisStatus[_iAxisSeq].Relative = (_odbdy.pos.relative[(_iAxisSeq - 1)] / 1000d).ToString("#####0.000").ToString(); // * Relative position of the controlled axes
                                    ReceivedData.AxisStatus[_iAxisSeq].Machine = (_odbdy.pos.machine[(_iAxisSeq - 1)] / 1000d).ToString("#####0.000").ToString(); // * Machine position of the controlled axes
                                    ReceivedData.AxisStatus[_iAxisSeq].Distance = (_odbdy.pos.distance[(_iAxisSeq - 1)] / 1000d).ToString("#####0.000").ToString(); // * Amount of distance to go of the controlled axes
                                    */
                                    /* * 좌표값 소숫점 위치 */
                                    int _iDotPoint = ReceivedData.AxisStatus[_iAxisSeq].AxisPositionDotPoint;
                                    double _Absolute = (_odbdy.pos.absolute[(_iAxisSeq - 1)] / Math.Pow(10, _iDotPoint)); // * Absolute position of the controlled axes
                                    double _Relative = (_odbdy.pos.relative[(_iAxisSeq - 1)] / Math.Pow(10, _iDotPoint)); // * Relative position of the controlled axes
                                    double _Machine = (_odbdy.pos.machine[(_iAxisSeq - 1)] / Math.Pow(10, _iDotPoint)); // * Machine position of the controlled axes
                                    double _Distance = (_odbdy.pos.distance[(_iAxisSeq - 1)] / Math.Pow(10, _iDotPoint)); // * Amount of distance to go of the controlled axes
                                    ReceivedData.AxisStatus[_iAxisSeq].Absolute = Helpers.ConvertHelper.ConvertToDecimal(_Absolute.ToString(), false, true, _iDotPoint);
                                    ReceivedData.AxisStatus[_iAxisSeq].Relative = Helpers.ConvertHelper.ConvertToDecimal(_Relative.ToString(), false, true, _iDotPoint);
                                    ReceivedData.AxisStatus[_iAxisSeq].Machine = Helpers.ConvertHelper.ConvertToDecimal(_Machine.ToString(), false, true, _iDotPoint);
                                    ReceivedData.AxisStatus[_iAxisSeq].Distance = Helpers.ConvertHelper.ConvertToDecimal(_Distance.ToString(), false, true, _iDotPoint);
                                    /* * 좌표값 소숫점 위치
                                    string _sStringFormat = "{0:#0.000}";
                                    double _MultiplyFormat = 1000d;
                                    int _iDotPoint = ReceivedData.AxisStatus[_iAxisSeq].AxisPositionDotPoint;
                                    switch (_iDotPoint)
                                    {
                                        case 2:
                                            {
                                                _sStringFormat = "{0:#0.00}";
                                                _MultiplyFormat = 100d;
                                            }
                                            break;
                                        case 4:
                                            {
                                                _sStringFormat = "{0:#0.0000}";
                                                _MultiplyFormat = 10000d;
                                            }
                                            break;
                                        case 3:
                                        default:
                                            {
                                                _sStringFormat = "{0:#0.000}";
                                                _MultiplyFormat = 1000d;
                                            }
                                            break;
                                    }
                                    ReceivedData.AxisStatus[_iAxisSeq].Absolute = string.Format(_sStringFormat, (_odbdy.pos.absolute[(_iAxisSeq - 1)] / _MultiplyFormat)); // * Absolute position of the controlled axes
                                    ReceivedData.AxisStatus[_iAxisSeq].Relative = string.Format(_sStringFormat, (_odbdy.pos.relative[(_iAxisSeq - 1)] / _MultiplyFormat)); // * Relative position of the controlled axes
                                    ReceivedData.AxisStatus[_iAxisSeq].Machine = string.Format(_sStringFormat, (_odbdy.pos.machine[(_iAxisSeq - 1)] / _MultiplyFormat)); // * Machine position of the controlled axes
                                    ReceivedData.AxisStatus[_iAxisSeq].Distance = string.Format(_sStringFormat, (_odbdy.pos.distance[(_iAxisSeq - 1)] / _MultiplyFormat)); // * Amount of distance to go of the controlled axes
                                    */
                                }
                            }
                            else
                            {
                                IsConnected = false;

                                OnErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs { IsFocasFunctions = true, StackTrace = "cnc_rddynamic", ErrorCode = _ReturnCode });

                                continue;
                            }
                            #endregion

                            #region [ ***** cnc_diagnoss  ***** ]
                            if (m_RequestDiagnoses.Count > 0)
                            {
                                foreach (TRequestDiagnosis _RequestDiagnosis in m_RequestDiagnoses)
                                {
                                    dynamic _odbdgn = new object();
                                    if (_RequestDiagnosis.IsRealData)
                                    {
                                        _odbdgn = new Focas1.ODBDGN_2();
                                    }
                                    else
                                    {
                                        _odbdgn = new Focas1.ODBDGN_1();
                                    }

                                    short _diagnoss_number = _RequestDiagnosis.DiagnosisNumber;
                                    string _sName = _RequestDiagnosis.Name;
                                    TResponseDiagnosis _ResponseDiagnosis = new TResponseDiagnosis
                                    {
                                        DiagnosisNumber = _diagnoss_number,
                                        Name = _sName,
                                        DiagnosisType = _RequestDiagnosis.DiagnosisType,
                                    };

                                    switch (_RequestDiagnosis.DiagnosisType)
                                    {
                                        case ConstsDefiner.DiagnosisTypes.Axis:
                                            {
                                                if (_RequestDiagnosis.AxisSeq.Equals(-1))
                                                {
                                                    for (short _AxisSeq = 1; _AxisSeq <= _Axes; _AxisSeq++)
                                                    {
                                                        short _axis = _AxisSeq;

                                                        _ReturnCode = Focas1.cnc_diagnoss(FlibHndl, _diagnoss_number, _axis, (short)Marshal.SizeOf(_odbdgn), _odbdgn);
                                                        if (_ReturnCode.Equals((int)Focas1.focas_ret.EW_OK))
                                                        {
                                                            short _datano = _odbdgn.datano; // * diagnosis data number which was read is stored.

                                                            short _type = _odbdgn.type; // * Attribute of diagnosis data which was read is stored.
                                                            /*
                                                            - Upper byte:type (Series 15/15i)
                                                                0 : byte type
                                                                1 : word type
                                                                2 : 2-word type
                                                                3 : bit type (8 bit)
                                                                4 : not used
                                                                5 : real type (Series 15i)
                                                            - Lower byte:axis
                                                                0 : no axis
                                                                1,..,m : 1 axis (m=maximum controlled axes)
                                                                ALL_AXES : all axes (ALL_AXES=-1)
                                                            */

                                                            if (_odbdgn.GetType().Equals(new Focas1.ODBDGN_1().GetType()))
                                                            {
                                                                switch (_type)
                                                                {
                                                                    case 0:
                                                                    case 3:
                                                                        {
                                                                            byte _cdata = _odbdgn.cdata;
                                                                            _ResponseDiagnosis.Data = _cdata.ToString();
                                                                        }
                                                                        break;
                                                                    case 1:
                                                                        {
                                                                            short _idata = _odbdgn.idata;
                                                                            _ResponseDiagnosis.Data = _idata.ToString();
                                                                        }
                                                                        break;
                                                                    case 2:
                                                                        {
                                                                            int _ldata = _odbdgn.ldata;
                                                                            _ResponseDiagnosis.Data = _ldata.ToString();
                                                                        }
                                                                        break;
                                                                    case 4:
                                                                    case 5:
                                                                    default:
                                                                        break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                double _data = (_odbdgn.rdata.dgn_val / Math.Pow(10, _odbdgn.rdata.dec_val));
                                                                _ResponseDiagnosis.Data = Helpers.ConvertHelper.ConvertToDecimal(_data.ToString(), false, true, _odbdgn.rdata.dec_val);
                                                            }

                                                            ReceivedData.AxisStatus[_AxisSeq].ResponseDiagnosis.Add(_ResponseDiagnosis);
                                                        }
                                                        else
                                                        {

                                                            if (_ReturnCode.Equals((int)Focas1.focas_ret.EW_RANGE))
                                                            {
                                                                string _sErrorMessage = $"*Diagnosis Type: {_RequestDiagnosis.DiagnosisType} | *Axis: {_axis} | *Diagnosis Number: {_diagnoss_number}({_sName})";
                                                                OnErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs { IsFocasFunctions = true, StackTrace = "cnc_diagnoss", ErrorCode = _ReturnCode, ErrorMessage = _sErrorMessage, ErrorType = ConstsDefiner.LogTypes.INFO });

                                                                continue;
                                                            }
                                                            else
                                                            {
                                                                _IsErrorOccurred = true;

                                                                string _sErrorMessage = $"*Diagnosis Type: {_RequestDiagnosis.DiagnosisType} | *Axis: {_axis} | *Diagnosis Number: {_diagnoss_number}({_sName})";
                                                                OnErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs { IsFocasFunctions = true, StackTrace = "cnc_diagnoss", ErrorCode = _ReturnCode, ErrorMessage = _sErrorMessage });

                                                                break;
                                                            }
                                                        }
                                                    }

                                                    if (_IsErrorOccurred)
                                                    {
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    short _axis = _RequestDiagnosis.AxisSeq;

                                                    _ReturnCode = Focas1.cnc_diagnoss(FlibHndl, _diagnoss_number, _axis, (short)Marshal.SizeOf(_odbdgn), _odbdgn);
                                                    if (_ReturnCode.Equals((int)Focas1.focas_ret.EW_OK))
                                                    {
                                                        short _datano = _odbdgn.datano; // * diagnosis data number which was read is stored.

                                                        short _type = _odbdgn.type; // * Attribute of diagnosis data which was read is stored.

                                                        if (_odbdgn.GetType().Equals(new Focas1.ODBDGN_1().GetType()))
                                                        {
                                                            switch (_type)
                                                            {
                                                                case 0:
                                                                case 3:
                                                                    {
                                                                        byte _cdata = _odbdgn.cdata;
                                                                        _ResponseDiagnosis.Data = _cdata.ToString();
                                                                    }
                                                                    break;
                                                                case 1:
                                                                    {
                                                                        short _idata = _odbdgn.idata;
                                                                        _ResponseDiagnosis.Data = _idata.ToString();
                                                                    }
                                                                    break;
                                                                case 2:
                                                                    {
                                                                        int _ldata = _odbdgn.ldata;
                                                                        _ResponseDiagnosis.Data = _ldata.ToString();
                                                                    }
                                                                    break;
                                                                case 4:
                                                                case 5:
                                                                default:
                                                                    break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            double _data = (_odbdgn.rdata.dgn_val / Math.Pow(10, _odbdgn.rdata.dec_val));
                                                            _ResponseDiagnosis.Data = Helpers.ConvertHelper.ConvertToDecimal(_data.ToString(), false, true, _odbdgn.rdata.dec_val);
                                                        }

                                                        ReceivedData.ResponseDiagnosis.Add(_ResponseDiagnosis);
                                                    }
                                                    else
                                                    {
                                                        if (_ReturnCode.Equals((int)Focas1.focas_ret.EW_RANGE))
                                                        {
                                                            string _sErrorMessage = $"*Diagnosis Type: {_RequestDiagnosis.DiagnosisType} | *Axis: {_axis} | *Diagnosis Number: {_diagnoss_number}({_sName})";
                                                            OnErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs { IsFocasFunctions = true, StackTrace = "cnc_diagnoss", ErrorCode = _ReturnCode, ErrorMessage = _sErrorMessage, ErrorType = ConstsDefiner.LogTypes.INFO });

                                                            continue;
                                                        }
                                                        else
                                                        {
                                                            _IsErrorOccurred = true;

                                                            string _sErrorMessage = $"*Diagnosis Type: {_RequestDiagnosis.DiagnosisType} | *Axis: {_axis} | *Diagnosis Number: {_diagnoss_number}({_sName})";
                                                            OnErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs { IsFocasFunctions = true, StackTrace = "cnc_diagnoss", ErrorCode = _ReturnCode, ErrorMessage = _sErrorMessage });

                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            break;

                                        case ConstsDefiner.DiagnosisTypes.Spindle:
                                            {
                                                short _axis = 1;

                                                _ReturnCode = Focas1.cnc_diagnoss(FlibHndl, _diagnoss_number, _axis, (short)Marshal.SizeOf(_odbdgn), _odbdgn);
                                                if (_ReturnCode.Equals((int)Focas1.focas_ret.EW_OK))
                                                {
                                                    short _datano = _odbdgn.datano; // * diagnosis data number which was read is stored.

                                                    short _type = _odbdgn.type; // * Attribute of diagnosis data which was read is stored.

                                                    if (_odbdgn.GetType().Equals(new Focas1.ODBDGN_1().GetType()))
                                                    {
                                                        switch (_type)
                                                        {
                                                            case 0:
                                                            case 3:
                                                                {
                                                                    byte _cdata = _odbdgn.cdata;
                                                                    _ResponseDiagnosis.Data = _cdata.ToString();
                                                                }
                                                                break;
                                                            case 1:
                                                                {
                                                                    short _idata = _odbdgn.idata;
                                                                    _ResponseDiagnosis.Data = _idata.ToString();
                                                                }
                                                                break;
                                                            case 2:
                                                                {
                                                                    int _ldata = _odbdgn.ldata;
                                                                    _ResponseDiagnosis.Data = _ldata.ToString();
                                                                }
                                                                break;
                                                            case 4:
                                                            case 5:
                                                            default:
                                                                break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        double _data = (_odbdgn.rdata.dgn_val / Math.Pow(10, _odbdgn.rdata.dec_val));
                                                        _ResponseDiagnosis.Data = Helpers.ConvertHelper.ConvertToDecimal(_data.ToString(), false, true, _odbdgn.rdata.dec_val);
                                                    }

                                                    ReceivedData.ResponseDiagnosis.Add(_ResponseDiagnosis);
                                                }
                                                else
                                                {
                                                    if (_ReturnCode.Equals((int)Focas1.focas_ret.EW_RANGE))
                                                    {
                                                        string _sErrorMessage = $"*Diagnosis Type: {_RequestDiagnosis.DiagnosisType} | *Axis: {_axis} | *Diagnosis Number: {_diagnoss_number}({_sName})";
                                                        OnErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs { IsFocasFunctions = true, StackTrace = "cnc_diagnoss", ErrorCode = _ReturnCode, ErrorMessage = _sErrorMessage, ErrorType = ConstsDefiner.LogTypes.INFO });

                                                        continue;
                                                    }
                                                    else
                                                    {
                                                        _IsErrorOccurred = true;

                                                        string _sErrorMessage = $"*Diagnosis Type: {_RequestDiagnosis.DiagnosisType} | *Axis: {_axis} | *Diagnosis Number: {_diagnoss_number}({_sName})";
                                                        OnErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs { IsFocasFunctions = true, StackTrace = "cnc_diagnoss", ErrorCode = _ReturnCode, ErrorMessage = _sErrorMessage });

                                                        break;
                                                    }
                                                }
                                            }
                                            break;

                                        case ConstsDefiner.DiagnosisTypes.NoAxis:
                                        default:
                                            {
                                                // * ※ 축, 주축 진단이 아닐 경우.
                                                short _axis = 0;

                                                _ReturnCode = Focas1.cnc_diagnoss(FlibHndl, _diagnoss_number, _axis, (short)Marshal.SizeOf(_odbdgn), _odbdgn);
                                                if (_ReturnCode.Equals((int)Focas1.focas_ret.EW_OK))
                                                {
                                                    short _datano = _odbdgn.datano; // * diagnosis data number which was read is stored.

                                                    short _type = _odbdgn.type; // * Attribute of diagnosis data which was read is stored.

                                                    if (_odbdgn.GetType().Equals(new Focas1.ODBDGN_1().GetType()))
                                                    {
                                                        switch (_type)
                                                        {
                                                            case 0:
                                                            case 3:
                                                                {
                                                                    byte _cdata = _odbdgn.cdata;
                                                                    _ResponseDiagnosis.Data = _cdata.ToString();
                                                                }
                                                                break;
                                                            case 1:
                                                                {
                                                                    short _idata = _odbdgn.idata;
                                                                    _ResponseDiagnosis.Data = _idata.ToString();
                                                                }
                                                                break;
                                                            case 2:
                                                                {
                                                                    int _ldata = _odbdgn.ldata;
                                                                    _ResponseDiagnosis.Data = _ldata.ToString();
                                                                }
                                                                break;
                                                            case 4:
                                                            case 5:
                                                            default:
                                                                break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        double _data = (_odbdgn.rdata.dgn_val / Math.Pow(10, _odbdgn.rdata.dec_val));
                                                        _ResponseDiagnosis.Data = Helpers.ConvertHelper.ConvertToDecimal(_data.ToString(), false, true, _odbdgn.rdata.dec_val);
                                                    }

                                                    ReceivedData.ResponseDiagnosis.Add(_ResponseDiagnosis);
                                                }
                                                else
                                                {
                                                    if (_ReturnCode.Equals((int)Focas1.focas_ret.EW_RANGE))
                                                    {
                                                        string _sErrorMessage = $"*Diagnosis Type: {_RequestDiagnosis.DiagnosisType} | *Axis: {_axis} | *Diagnosis Number: {_diagnoss_number}({_sName})";
                                                        OnErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs { IsFocasFunctions = true, StackTrace = "cnc_diagnoss", ErrorCode = _ReturnCode, ErrorMessage = _sErrorMessage, ErrorType = ConstsDefiner.LogTypes.INFO });

                                                        continue;
                                                    }
                                                    else
                                                    {
                                                        _IsErrorOccurred = true;

                                                        string _sErrorMessage = $"*Diagnosis Type: {_RequestDiagnosis.DiagnosisType} | *Axis: {_axis} | *Diagnosis Number: {_diagnoss_number}({_sName})";
                                                        OnErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs { IsFocasFunctions = true, StackTrace = "cnc_diagnoss", ErrorCode = _ReturnCode, ErrorMessage = _sErrorMessage });

                                                        break;
                                                    }
                                                }
                                            }
                                            break;
                                    }

                                    if (_IsErrorOccurred)
                                    {
                                        break;
                                    }
                                }

                                if (_IsErrorOccurred)
                                {
                                    IsConnected = false;
                                    continue;
                                }
                            }
                            #endregion

                            #region [ ***** cnc_rdspmeter ***** ]
                            Focas1.ODBSPLOAD_data _odbspload_data = new Focas1.ODBSPLOAD_data();
                            short _rdspmeter_data_num = 8; // * 최대 스핀들 수 
                            short _rdspmeter_type = -1; // * Specify the data type.
                            /* * Specify the data type.
                            0  : spindle load meter data
                            1  : spindle motor speed data
                            -1 : all type
                            */
                            byte[] _SpindleLoadMeterBytes = new byte[(Marshal.SizeOf(_odbspload_data) * _rdspmeter_data_num)];

                            _ReturnCode = Focas1.cnc_rdspmeter(FlibHndl, _rdspmeter_type, ref _rdspmeter_data_num, _SpindleLoadMeterBytes);
                            if (_ReturnCode.Equals((int)Focas1.focas_ret.EW_OK))
                            {
                                ArrayList _SpindleLoadMeterStructures = DeserializeToStructure(_odbspload_data, _SpindleLoadMeterBytes, _rdspmeter_data_num);
                                for (int _iSpindleSeq = 1; _iSpindleSeq <= _rdspmeter_data_num; _iSpindleSeq++)
                                {
                                    string _sSpindle_name = Convert.ToChar(((Focas1.ODBSPLOAD_data)_SpindleLoadMeterStructures[_iSpindleSeq - 1]).spload.name).ToString(); // * Spindle name

                                    string _sSpindle_suff1 = Convert.ToChar(((Focas1.ODBSPLOAD_data)_SpindleLoadMeterStructures[_iSpindleSeq - 1]).spload.suff1).ToString(); // * Subscript of spindle name 1 (ASCII)

                                    short _SpindleLoad_unit = ((Focas1.ODBSPLOAD_data)_SpindleLoadMeterStructures[_iSpindleSeq - 1]).spload.unit; // * Spindle Load Unit
                                    /*
                                    0 : %
                                    1 : rpm
                                    */

                                    short _SpindleLoad_dec = ((Focas1.ODBSPLOAD_data)_SpindleLoadMeterStructures[_iSpindleSeq - 1]).spload.dec; // * Spindle Load Place of decimal point

                                    ReceivedData.SpindleLoad = ((Focas1.ODBSPLOAD_data)_SpindleLoadMeterStructures[_iSpindleSeq - 1]).spload.data; // * 스핀들 로드 미터 데이터(TORQ)

                                    short _SpindleMotor_unit = ((Focas1.ODBSPLOAD_data)_SpindleLoadMeterStructures[_iSpindleSeq - 1]).spspeed.unit; // * Spindle Motor Unit
                                    /*
                                    0 : %
                                    1 : rpm
                                    */

                                    short _SpindleMotor_dec = ((Focas1.ODBSPLOAD_data)_SpindleLoadMeterStructures[_iSpindleSeq - 1]).spspeed.dec; // * Spindle Motor Place of decimal point

                                    ReceivedData.SpindleSpeed = ((Focas1.ODBSPLOAD_data)_SpindleLoadMeterStructures[_iSpindleSeq - 1]).spspeed.data; // * 스핀들 모터 데이터
                                }
                            }
                            else
                            {
                                IsConnected = false;

                                OnErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs { IsFocasFunctions = true, StackTrace = "cnc_rdspmeter", ErrorCode = _ReturnCode });

                                continue;
                            }
                            #endregion

                            #region [ ***** cnc_rdparam_ext ***** ]
                            if (m_RequestParameters.Count > 0)
                            {
                                Dictionary<int, TResponseParameter> _ResponseParameters = new Dictionary<int, TResponseParameter>();

                                List<int> _ParameterNumbers = new List<int>();
                                foreach (TRequestParameter _RequestParameter in m_RequestParameters)
                                {
                                    _ParameterNumbers.Add(_RequestParameter.ParameterNumber);

                                    TResponseParameter _ResponseParameter = new TResponseParameter
                                    {
                                        ParameterNumber = _RequestParameter.ParameterNumber,
                                        Name = _RequestParameter.Name
                                    };
                                    _ResponseParameters.Add(_RequestParameter.ParameterNumber, _ResponseParameter);
                                }
                                int[] _rdparam_ext_prm_no = _ParameterNumbers.ToArray();

                                Focas1.IODBPRM2 _iodbprm = new Focas1.IODBPRM2();
                                byte[] _ParameterBytes = new byte[(Marshal.SizeOf(_iodbprm) * _rdparam_ext_prm_no.Length)];

                                _ReturnCode = Focas1.cnc_rdparam_ext(FlibHndl, _rdparam_ext_prm_no, (short)_rdparam_ext_prm_no.Length, _ParameterBytes);
                                if (_ReturnCode.Equals((int)Focas1.focas_ret.EW_OK))
                                {
                                    ArrayList _ParameterStructures = DeserializeToStructure(_iodbprm, _ParameterBytes, _rdparam_ext_prm_no.Length);
                                    for (int _iParamIdx = 1; _iParamIdx <= _rdparam_ext_prm_no.Length; _iParamIdx++)
                                    {
                                        int _datano = ((Focas1.IODBPRM2)_ParameterStructures[_iParamIdx - 1]).datano; // * Parameter number which was read is stored.

                                        short _type = ((Focas1.IODBPRM2)_ParameterStructures[_iParamIdx - 1]).type; // * Type information of parameter which was read is stored.
                                        /*
                                        0  : bit type
                                        1  : byte type
                                        2  : word type
                                        3  : 2-word type
                                        4  : real type (Series 15i, 30i, 0i-D/F, PMi-A)
                                        -1 : invalid parameter
                                        */

                                        short _axis = ((Focas1.IODBPRM2)_ParameterStructures[_iParamIdx - 1]).axis; // * Axis information of parameter which was read is stored.
                                        BitArray _AxisBitArray = new BitArray(new int[] { _axis });
                                        bool _IsWithAxis = _AxisBitArray[0]; // * Axis Attribute
                                        bool _IsSpindle = _AxisBitArray[1]; // * Spindle Parameter
                                        /* bit2,..,15 : (reserve) */

                                        short _info = ((Focas1.IODBPRM2)_ParameterStructures[_iParamIdx - 1]).info; // * Attribute of parameter which was read is stored.
                                        BitArray _InfoBitArray = new BitArray(new int[] { _info });
                                        bool _IsWithSign = _InfoBitArray[0]; // * Sign
                                        bool _IsSettingsInput = _InfoBitArray[1]; // * Settings Input
                                        bool _IsWriteProtection = _InfoBitArray[2]; // * Write Protection
                                        bool _IsPowerMustBeOffAfterWriting = _InfoBitArray[3]; // * Power must be off after writing
                                        bool _IsReadProtection = _InfoBitArray[4]; // * read protection
                                        /* bit5,..,15 : (reserve) */

                                        short _unit = ((Focas1.IODBPRM2)_ParameterStructures[_iParamIdx - 1]).unit; // * Unit of real type parameter which was read is stored.
                                        BitArray _UnitBitArray = new BitArray(new int[] { _unit });
                                        /*
                                        bit 0, 1 : attribute
                                                    0 : no real type
                                                    1 : input unit
                                                    2 : output unit
                                        bit 2,..,4 : unit
                                                    0 : without unit
                                                    1 : unit of length
                                                    2 : unit of angle
                                                    3 : unit of length + angle
                                                    4 : unit of velocity
                                        bit 5,..,15 : (reserve)
                                        */

                                        /* * Value of parameter which was read is stored.
                                        prm_val : Value of parameter
                                        dec_val : Place of decimal point (only available for real type)
                                        */

                                        TResponseParameter _ResponseParameter = _ResponseParameters[_datano];
                                        if (_IsWithAxis)
                                        {
                                            // * 축 별 파라미터 값.
                                            FieldInfo[] _FieldInfos = typeof(Focas1.IODBPRM1)
                                                                      .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                                                      //.Where(f => f.GetCustomAttribute<System.Runtime.CompilerServices.CompilerGeneratedAttribute>() == null)
                                                                      .ToArray();
                                            for (short _AxisSeq = 1; _AxisSeq <= _Axes; _AxisSeq++)
                                            {
                                                Focas1.IODBPRM_data _iodbprm_data = ((Focas1.IODBPRM_data)_FieldInfos[(_AxisSeq - 1)].GetValue(((Focas1.IODBPRM2)_ParameterStructures[_iParamIdx - 1]).data));
                                                double _data = (_iodbprm_data.prm_val / Math.Pow(10, _iodbprm_data.dec_val));
                                                _ResponseParameter.Data = Helpers.ConvertHelper.ConvertToDecimal(_data.ToString(), false, true, _iodbprm_data.dec_val);

                                                ReceivedData.AxisStatus[_AxisSeq].ResponseParameter.Add(_ResponseParameter);
                                            }
                                        }
                                        else
                                        {
                                            double _data = (((Focas1.IODBPRM2)_ParameterStructures[_iParamIdx - 1]).data.data1.prm_val / Math.Pow(10, ((Focas1.IODBPRM2)_ParameterStructures[_iParamIdx - 1]).data.data1.dec_val));
                                            _ResponseParameter.Data = Helpers.ConvertHelper.ConvertToDecimal(_data.ToString(), false, true, ((Focas1.IODBPRM2)_ParameterStructures[_iParamIdx - 1]).data.data1.dec_val);

                                            ReceivedData.ResponseParameter.Add(_ResponseParameter);
                                        }
                                    }
                                }
                                else
                                {
                                    IsConnected = false;

                                    OnErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs { IsFocasFunctions = true, StackTrace = "cnc_rdparam_ext", ErrorCode = _ReturnCode });

                                    continue;
                                }
                            }

                            #region Running Time Calculate / 2022-09-26 고광진
                            
                            if(!runState)
                            {
                                if(ReceivedData.Run == 3 && (ReceivedData.Aut == 1 || ReceivedData.Aut==10))
                                {
                                    runState = true;
                                    stDate = DateTime.Now;
                                }
                            }

                            if(runState)
                            {
                                if (ReceivedData.Run == 3 && (ReceivedData.Aut == 1 || ReceivedData.Aut == 10))
                                {
                                    DateTime now = DateTime.Now;
                                    ReceivedData.running_time = (now - stDate).TotalSeconds;
                                    stDate = now;
                                }
                                else
                                {   
                                    
                                    ReceivedData.running_time = 0.0;
                                    runState = false;
                                }
                            }

                            #endregion

                            #endregion

                            if (!m_CancellationToken.IsCancellationRequested)
                            {
                                AutoResetEvent _AutoResetEvent = new AutoResetEvent(false); // * 수신 이벤트 처리 완료 대기
                                OnReceived?.Invoke(this, new ReceivedEventArgs() { WaitHandle = _AutoResetEvent });
                                _AutoResetEvent.WaitOne();
                            }

                            DateTime _dtmFinishTime = DateTime.Now; // * 작업(Task) 완료 시간
                            TimeSpan _tsInterval = (_dtmFinishTime - _dtmStartTime); // * 대기(수신) 간격 계산
                            if (_tsInterval.TotalMilliseconds < ReceiveInterval)
                            {
                                int _iCalculatedInterval = (ReceiveInterval - (int)_tsInterval.TotalMilliseconds); // * 남은 대기(수신) 간격 계산
                                bool _IsCancellation = m_CancellationToken.WaitHandle.WaitOne(_iCalculatedInterval); // * 대기(수신)
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }

                    // * 반복문(while문) 종료 시 기존 할당된 포카스 핸들 해제 및 초기화
                    if (!FlibHndl.Equals(0))
                    {
                        _ReturnCode = Focas1.cnc_freelibhndl(FlibHndl); // * 포카스 핸들 해제
                        if (_ReturnCode.Equals((int)Focas1.focas_ret.EW_OK))
                        {
                            FlibHndl = 0;
                        }
                        else
                        {
                            OnErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs { IsFocasFunctions = true, StackTrace = "cnc_freelibhndl", ErrorCode = _ReturnCode });
                        }
                    }

                }).ContinueWith(x =>
                {
                    if (x.IsFaulted)
                    {
        
                    }
                }, m_CancellationToken);
            }
            catch (OperationCanceledException) // * 작업 취소 시 예외 처리
            {
                m_IsRunning = false;  // * 연결끊김(Cancel)
                IsConnected = false;
                m_iConnectRefusedCount = 0;

                OnStatusChanged?.Invoke(this, new StatusChangedEventArgs(ConstsDefiner.Statuses.Disconnected_Cancel));
            }
            catch (Exception ex)
            {
                OnFocasException?.Invoke(this, ex);
            }
        }

        /// <summary>
        /// 포카스 마스터 종료
        /// </summary>
        public void Stop()
        {
            try
            {
                m_IsRunning = false;
                if (m_CancellationTokenSource != null)
                {
                    m_CancellationTokenSource.Cancel();
                }
            }
            catch (Exception ex)
            {
                OnFocasException?.Invoke(this, ex);
            }
        }

        /// <summary>
        /// 구조체로 역직렬화(마샬링)
        /// </summary>
        /// <param name="structure">구조체</param>
        /// <param name="source">복사할 1차원 배열</param>
        /// <param name="length">배열의 길이</param>
        /// <returns></returns>
        private ArrayList DeserializeToStructure(object structure, byte[] source, int length)
        {
            try
            {
                ArrayList _Structures = new ArrayList();

                int _iPointerIdx = 0;
                IntPtr _IntPtrDestinationPointer = Marshal.AllocCoTaskMem(Marshal.SizeOf(structure));
                for (int _iIdx = 0; _iIdx < length; _iIdx++)
                {
                    Marshal.Copy(source, _iPointerIdx, _IntPtrDestinationPointer, Marshal.SizeOf(structure));

                    object _Structure = Marshal.PtrToStructure(_IntPtrDestinationPointer, structure.GetType());
                    _Structures.Add(_Structure);

                    _iPointerIdx += Marshal.SizeOf(structure);
                }
                Marshal.FreeCoTaskMem(_IntPtrDestinationPointer);

                return _Structures;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 요청된 설비 진단 할당
        /// </summary>
        /// <param name="diagnoses">설비 진단</param>
        public void RequestDiagnosis(List<TRequestDiagnosis> diagnoses)
        {
            try
            {
                m_RequestDiagnoses = diagnoses.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 요청된 설비 매개변수(파라미터) 할당
        /// </summary>
        /// <param name="parameters">매개변수(파라미터)</param>
        public void RequestParameter(List<TRequestParameter> parameters)
        {
            try
            {
                m_RequestParameters = parameters.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}