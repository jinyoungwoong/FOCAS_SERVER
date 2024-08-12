using System;
using System.Collections.Generic;
using NCManagementSystem.Components.Protocol;
using Newtonsoft.Json;

namespace NCManagementSystem.Components.Models
{
    public class PControllerContent
    {
        public string ID { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public string MODEL { get; set; }
        public string IP { get; set; }
        public int PORT { get; set; }

        public string EMP_CD { get; set; }
        public DateTime RUNTIME { get; set; }
        
        public int CONNECT_TIMEOUT { get; set; }  // * 초

        public int CONNECT_RETRY_COUNT { get; set; }
        public int RECEIVE_INTERVAL { get; set; }  // * 밀리초
        public bool IS_AUTO_START { get; set; } // 자동시작

        [JsonIgnore]
        public bool IsTitle { get; set; } = false;
        [JsonIgnore]
        public bool IsBinding { get; set; } = false;
        [JsonIgnore]
        public FocasMaster FocasMaster { get; set; } = null;
        [JsonIgnore]
        public Dictionary<string, string> BindingControllers { get; set; } = null;
    }
}
