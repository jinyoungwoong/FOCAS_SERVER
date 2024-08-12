using System;
using System.Threading;

namespace NCManagementSystem.Components.Protocol
{
    public delegate void FocasExceptionEventHandler(object sender, Exception fex);

    public class StatusChangedEventArgs : EventArgs
    {
        public StatusChangedEventArgs(ConstsDefiner.Statuses status)
        {
            Status = status;
        }

        public ConstsDefiner.Statuses Status { get; private set; }
    }

    public class ReceivedEventArgs : EventArgs
    {
        public ReceivedEventArgs()
        {
        }

        public AutoResetEvent WaitHandle { get; set; } // * 쓰레드 대기 이벤트
    }

    public class ErrorOccurredEventArgs : EventArgs
    {
        public ErrorOccurredEventArgs()
        {
        }

        public bool IsFocasFunctions { get; set; } = false;
        public string StackTrace { get; set; } = string.Empty;
        public int ErrorCode { get; set; } = 0;
        public string ErrorMessage { get; set; } = string.Empty;
        public ConstsDefiner.LogTypes ErrorType { get; set; } = ConstsDefiner.LogTypes.ERROR;
    }
}
