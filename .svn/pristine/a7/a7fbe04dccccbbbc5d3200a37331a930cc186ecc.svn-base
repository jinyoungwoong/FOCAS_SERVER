using System;
using System.Drawing;
using System.Windows.Forms;

namespace NCManagementSystem.Components.Handlers.TrayNotify
{
    public class TrayNotifyHandler
    {
        #region [ Constructor ]
        private static readonly object m_SyncLock = new object();

        private static TrayNotifyHandler m_Instance = null;
        public static TrayNotifyHandler GetInstance()
        {
            if (m_Instance == null)
            {
                lock (m_SyncLock)
                {
                    m_Instance = new TrayNotifyHandler();
                }
            }

            return m_Instance;
        }

        public TrayNotifyHandler()
        {
        }
        #endregion

        #region [ Member Variables / Fields / Properties ]
        public event EventHandler OnBalloonTipClicked;
        public event MouseEventHandler OnMouseDoubleClick;

        private NotifyIcon Notifier = null;
        public bool IsApplicationExit { get; set; } = false;

        public Icon NotifierIcon
        {
            get { return Notifier != null ? Notifier.Icon : Icon.FromHandle(SystemIcons.Application.Handle); }
            set { if (Notifier != null) { Notifier.Icon = value; } }
        }

        public string NotifierTitle
        {
            get { return Notifier != null ? Notifier.Text : string.Empty; }
            set { if (Notifier != null) { Notifier.Text = value; } }
        }

        public ContextMenuStrip ContextMenuStrip
        {
            get { return Notifier?.ContextMenuStrip; }
            set { if (Notifier != null) { Notifier.ContextMenuStrip = value; } }
        }

        public bool IsShowNotifier
        {
            get { return Notifier != null ? Notifier.Visible : false; }
            set { if (Notifier != null) { Notifier.Visible = value; } }
        }
        #endregion

        #region [ Override Events / Events / Methods ]
        public void Initialize(bool isVisible = false, string text = "", Icon icon = null)
        {
            try
            {
                if (Notifier != null)
                {
                    Notifier.MouseDoubleClick -= Notifier_MouseDoubleClick;
                    Notifier.BalloonTipClicked -= Notifier_BalloonTipClicked;
                    Notifier.Dispose();
                    Notifier = null;
                }

                Notifier = new NotifyIcon
                {
                    Icon = icon ?? NotifierIcon,
                    Text = text,
                    Visible = isVisible
                };
                Notifier.MouseDoubleClick += Notifier_MouseDoubleClick;
                Notifier.BalloonTipClicked += Notifier_BalloonTipClicked;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Deinitialize()
        {
            try
            {
                if (Notifier != null)
                {
                    Notifier.Dispose();
                    Notifier = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ShowBalloonTip(string tipText, ToolTipIcon tipIcon = ToolTipIcon.None)
        {
            try
            {
                Notifier.ShowBalloonTip(1, Notifier.Text.Trim(), tipText, tipIcon);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ShowBalloonTip(string tipTitle, string tipText, ToolTipIcon tipIcon = ToolTipIcon.None)
        {
            try
            {
                Notifier.ShowBalloonTip(1, tipTitle, tipText, tipIcon);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Notifier_BalloonTipClicked(object sender, EventArgs e)
        {
            try
            {
                OnBalloonTipClicked?.Invoke(sender, e);
            }
            catch
            {}
        }

        private void Notifier_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                OnMouseDoubleClick?.Invoke(sender, e);
            }
            catch
            {}
        }
        #endregion
    }
}
