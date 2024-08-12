using System;
using System.Windows.Forms;
using NCManagementSystem.Libraries.Controls;

namespace NCManagementSystem.Components.Helpers
{
    public static class SuspendHelper
    {
        public static IDisposable BeginSuspend(this Control control)
        {
            return new Suspender(control);
        }

        private class Suspender : IDisposable
        {
            private Control m_Control;

            public Suspender(Control control)
            {
                m_Control = control;
                NativeMethods.SendMessage(m_Control.Handle, NativeMethods.WM_SETREDRAW, false, 0);
            }

            public void Dispose()
            {
                NativeMethods.SendMessage(m_Control.Handle, NativeMethods.WM_SETREDRAW, true, 0);
                m_Control.Refresh();
            }
        }
    }
}
