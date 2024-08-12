using System;

namespace NCManagementSystem.Components.Models
{
    public class TNCMS_MACHINE
    {
        public string MACHINE_CD { get; set; }
        public string MACHINE_NM { get; set; }
        public string MODEL { get; set; }
        public string IP_ADDR { get; set; }
        public int PORT { get; set; }
        public string SCOMMENT { get; set; }
        public string EMP_CD { get; set; }
        public string REG_DATE { get; set; }
        public string MDFY_DATE { get; set; }
        public DateTime STARTTIME { get; set; }
        public DateTime FINISHTIME { get; set; }
    }

    public class TNCMS_USER
    {
        public string ID { get; set; }
        public string PASSWORD { get; set; }
        public string NAME { get; set; }
        public string EMAIL { get; set; }
        public string PHONE { get; set; }
        public string USE_YN { get; set; }
        public string USER_GBN { get; set; }
    }
}
