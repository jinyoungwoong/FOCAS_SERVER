using System;
using System.Collections.Generic;
using NCManagementSystem.Components.Models;

namespace NCManagementSystem.Components.Controllers
{
    public delegate Dictionary<string, string> BindingControllerRequestedEvnetHandler();

    public class ControllerStateChangedEventArgs : EventArgs
    {
        public ControllerStateChangedEventArgs(string id, bool isState)
        {
            ID = id;
            IsState = isState;
        }

        public string ID { get; }
        public bool IsState { get; }
    }

    public class ControllerSettingsChangedEventArgs : EventArgs
    {
        public ControllerSettingsChangedEventArgs(string id, PControllerContent content)
        {
            ID = id;
            ControllerContent = content;
        }

        public string ID { get; }
        public PControllerContent ControllerContent { get; }
    }
}
