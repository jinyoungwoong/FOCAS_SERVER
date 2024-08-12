using System;

namespace NCManagementSystem.Components.Models
{
    public class TNCMS_STATUS
    {
        public string MACHINE_CD { get; set; }
        public DateTime TRNX_TIME { get; set; }
        public string IS_CONNECTED { get; set; }
        public string ADDINFO { get; set; }
        public string AXES { get; set; }
        public string MAX_AXIS { get; set; }
        public string MT_TYPE { get; set; }
        public string SERIES { get; set; }
        public string CNC_TYPE { get; set; }
        public string VERSION { get; set; }
        public int AUT { get; set; }
        public int RUN { get; set; }
        public int MOTION { get; set; }
        public int MSTB { get; set; }
        public int ALARM { get; set; }
        public int PRGNUM { get; set; }
        public int PRGMNUM { get; set; }
        public decimal ACTF { get; set; }
        public decimal ACTS { get; set; }
        public int SPLOAD { get; set; } // 로드미터(TORQ)
        public string SPINDLE_DATA { get; set; } // 주축위치편차(418 *ldata),주축위치편차량(450 *ldata),주축절연저항치(1703 *dgn_val + dec_val),주축모터온도(403 *ldata)
        public string AXIS_DATA { get; set; } // 축이름(*name),절대(*absolute),상대(*relative),기계(*machine),남은거리(*distance),서보모터온도(308 *cdata),펄스코더온도(309 *cdata)/...
        public decimal PART_COUNT { get; set; } // 6711 - prm_val
        public decimal PARTS_TOTAL { get; set; } // 6712 - prm_val 
        public string OPERATING_TIME { get; set; } // 6752(min):6751(ms) - prm_val
        public string CUTTING_TIME { get; set; } // 6754(min):6753(ms) - prm_val
        public string CYCLE_TIME { get; set; } // 6758(min):6757(ms) - prm_val
        public DateTime MODIFIED_ON { get; set; }
        public double RUNNING_TIME { get; set; }
    }
}
