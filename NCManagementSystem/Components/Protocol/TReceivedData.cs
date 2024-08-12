using System.Collections.Generic;

namespace NCManagementSystem.Components.Protocol
{
    public class TReceivedData
    {
        /// <summary>
        /// 설비 가동 시간.
        /// </summary>
        public double running_time { get; set; } = 0.0;

        /// <summary>
        /// 고정 데이터 할당 여부
        /// </summary>
        public bool IsAssigned { get; set; } = false;

        /// <summary>
        /// Additional information
        /// </summary>
        public short AddInfo { get; set; }

        /// <summary>
        /// Current controlled axes (ASCII)
        /// </summary>
        public string Axes { get; set; }

        /// <summary>
        /// Maximum controlled axes
        /// </summary>
        public string MaxAxis { get; set; }

        /// <summary>
        /// Kind of M/T (ASCII)
        /// </summary>
        public string MTType { get; set; }

        /// <summary>
        /// Series number of CNC (ASCII)
        /// </summary>
        public string Series { get; set; }

        /// <summary>
        /// Kind of CNC (ASCII)
        /// </summary>
        public string CNCType { get; set; }

        /// <summary>
        /// Version number of CNC (ASCII)
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// AUTOMATIC/MANUAL mode selection
        /// </summary>
        public short Aut { get; set; }

        /// <summary>
        /// Status of automatic operation  
        /// </summary>
        public short Run { get; set; }

        /// <summary>
        /// Status of axis movement, dwell
        /// </summary>
        public short Motion { get; set; }

        /// <summary>
        /// Status of M,S,T,B function
        /// </summary>
        public short MSTB { get; set; }

        /// <summary>
        /// Status of alarm
        /// </summary>
        public short Alarm { get; set; }

        /// <summary>
        /// Program number under execution
        /// </summary>
        public short Prgnum { get; set; }

        /// <summary>
        /// Main program number 
        /// </summary>
        public short Prgmnum { get; set; }

        /// <summary>
        /// Actual feed rate of the controlled axes
        /// </summary>
        public int ActF { get; set; }

        /// <summary>
        /// Actual spindle speed data
        /// </summary>
        public int ActS { get; set; }

        /// <summary>
        /// 스핀들 로드 미터 데이터(TORQ)
        /// </summary>
        public int SpindleLoad { get; set; }

        /// <summary>
        /// 스핀들 모터 데이터
        /// </summary>
        public int SpindleSpeed { get; set; }

        /// <summary>
        /// Axis Information(name, position, diagnosis)
        /// </summary>
        public SortedDictionary<int, TAxisStatus> AxisStatus { get; set; } = new SortedDictionary<int, TAxisStatus>();

        /// <summary>
        /// 수집된 설비 진단
        /// </summary>
        public List<TResponseDiagnosis> ResponseDiagnosis { get; set; } = new List<TResponseDiagnosis>();

        /// <summary>
        /// 수집된 설비 매개변수(파라미터)
        /// </summary>
        public List<TResponseParameter> ResponseParameter { get; set; } = new List<TResponseParameter>();
    }

    public class TAxisStatus
    {
        /// <summary>
        /// 축 이름
        /// </summary>
        public string AxisName { get; set; }

        /// <summary>
        /// Absolute position of the controlled axes
        /// </summary>
        public string Absolute { get; set; }

        /// <summary>
        /// Relative position of the controlled axes
        /// </summary>
        public string Relative { get; set; }

        /// <summary>
        /// Machine position of the controlled axes
        /// </summary>
        public string Machine { get; set; }

        /// <summary>
        /// Amount of distance to go of the controlled axes
        /// </summary>
        public string Distance { get; set; }

        /*
        /// <summary>
        /// 서보 위치 편차
        /// </summary>
        public string MotorDeviation { get; set; }

        /// <summary>
        /// 서보 모터 온도
        /// </summary>
        public string MotorTemperature { get; set; }

        /// <summary>
        /// 펄스코더 온도
        /// </summary>
        public string EncodeTemperature { get; set; }

        /// <summary>
        /// 서보 절연 저항치
        /// </summary>
        public string MotorResistance { get; set; }
        */

        /// <summary>
        /// 좌표값 소숫점 위치
        /// </summary>
        public int AxisPositionDotPoint { get; set; } = 3;

        /// <summary>
        /// 수집된 설비 진단
        /// </summary>
        public List<TResponseDiagnosis> ResponseDiagnosis { get; set; } = new List<TResponseDiagnosis>();

        /// <summary>
        /// 수집된 설비 매개변수(파라미터)
        /// </summary>
        public List<TResponseParameter> ResponseParameter { get; set; } = new List<TResponseParameter>();
    }

    public class TRequestDiagnosis
    {
        public TRequestDiagnosis()
        {
        }

        public short DiagnosisNumber { get; set; }

        public string Name { get; set; } = string.Empty;

        public ConstsDefiner.DiagnosisTypes DiagnosisType { get; set; } = ConstsDefiner.DiagnosisTypes.NoAxis;

        public short AxisSeq { get; set; } = -1;

        public bool IsRealData { get; set; } = false;
    }

    public class TResponseDiagnosis
    {
        public TResponseDiagnosis()
        {
        }

        public short DiagnosisNumber { get; set; }

        public string Name { get; set; } = string.Empty;

        public ConstsDefiner.DiagnosisTypes DiagnosisType { get; set; } = ConstsDefiner.DiagnosisTypes.NoAxis;

        public string Data { get; set; }
    }

    public class TRequestParameter
    {
        public TRequestParameter()
        {
        }

        public int ParameterNumber { get; set; } = 0;

        public string Name { get; set; } = string.Empty;
    }

    public class TResponseParameter
    {
        public TResponseParameter()
        {
        }

        public int ParameterNumber { get; set; } = 0;

        public string Name { get; set; } = string.Empty;

        public string Data { get; set; }
    }
}
