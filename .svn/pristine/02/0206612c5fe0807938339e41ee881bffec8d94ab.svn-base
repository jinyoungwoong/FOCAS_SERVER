using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NCManagementSystem.Libraries.Controls.Forms
{
    public partial class FwSkinForm : Form
    {
        #region [ Constructor ]
        public FwSkinForm()
        {
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

        #region [ Member Variables / Fields / Properties ]
        private int m_iGripSize = 5;

        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.Repaint)]
        [DefaultValue(false)]
        public bool IsFullMaximumWindow { get; set; } = false;

        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.Repaint)]
        [DefaultValue(false)]
        public bool IsFixedWindow { get; set; } = false;

        [Browsable(false)]
        public bool IsShowSplashScreen
        {
            get { return SkinContainer.IsShowSplashScreen; }
            set { SkinContainer.IsShowSplashScreen = value; }
        }

        [Browsable(false)]
        public new Padding Padding { get; set; }

        [Browsable(false)]
        public new Color BackColor { get; set; }

        [Browsable(false)]
        public new bool ControlBox { get; set; }

        [Browsable(false)]
        public new bool MinimizeBox { get; set; }

        [Browsable(false)]
        public new bool MaximizeBox { get; set; }
        #endregion

        #region [ Override Events / Events / Methods ]
        private void Initialize()
        {
            try
            {
                AutoScaleMode = AutoScaleMode.None;
                FormBorderStyle = FormBorderStyle.None;
                StartPosition = FormStartPosition.CenterScreen;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                base.OnLoad(e);

                m_iGripSize = SkinContainer.BorderProperties.BorderThickness;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                base.OnFormClosing(e);

                if (SkinContainer.IsShowSplashScreen)
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if (SkinContainer.IsShowSplashScreen)
                {
                    return true;
                }
                else
                {
                    return base.ProcessCmdKey(ref msg, keyData);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void WndProc(ref Message m)
        {
            try
            {
                if (IsFullMaximumWindow)
                {
                    MaximumSize = new Size();
                }
                else
                {
                    MaximumSize = Screen.PrimaryScreen.WorkingArea.Size;
                }

                if (!IsFixedWindow && !WindowState.Equals(FormWindowState.Maximized))
                {
                    if (m.Msg == 0x84)
                    {
                        uint _uLParam32 = (uint)m.LParam.ToInt64();
                        short _PointX = (short)(_uLParam32 & 0xffff);
                        short _PointY = (short)((_uLParam32 >> 16) & 0xffff);
                        Point _Point = new Point(_PointX, _PointY);
                        _Point = PointToClient(_Point);
                        Rectangle _LeftSide = new Rectangle(0, m_iGripSize, m_iGripSize, (ClientSize.Height - (m_iGripSize * 2)));
                        Rectangle _TopLeftSide = new Rectangle(0, 0, m_iGripSize, m_iGripSize);
                        Rectangle _TopSide = new Rectangle(m_iGripSize, 0, (ClientSize.Width - (m_iGripSize * 2)), m_iGripSize);
                        Rectangle _TopRightSide = new Rectangle((ClientSize.Width - m_iGripSize), 0, m_iGripSize, m_iGripSize);
                        Rectangle _RightSide = new Rectangle((ClientSize.Width - m_iGripSize), m_iGripSize, m_iGripSize, (ClientSize.Height - (m_iGripSize * 2)));
                        Rectangle _BottomRightSide = new Rectangle((ClientSize.Width - m_iGripSize), (ClientSize.Height - m_iGripSize), m_iGripSize, m_iGripSize);
                        Rectangle _BottomSide = new Rectangle(m_iGripSize, (ClientSize.Height - m_iGripSize), (ClientSize.Width - (m_iGripSize * 2)), m_iGripSize);
                        Rectangle _BottomLeftSide = new Rectangle(0, (ClientSize.Height - m_iGripSize), m_iGripSize, m_iGripSize);
                        if (_LeftSide.IntersectsWith(new Rectangle(_Point.X, _Point.Y, 1, 1)))
                        {
                            m.Result = (IntPtr)NativeMethods.HTLEFT;
                            return;
                        }
                        else if (_RightSide.IntersectsWith(new Rectangle(_Point.X, _Point.Y, 1, 1)))
                        {
                            m.Result = (IntPtr)NativeMethods.HTRIGHT;
                            return;
                        }
                        else if (_TopSide.IntersectsWith(new Rectangle(_Point.X, _Point.Y, 1, 1)))
                        {
                            m.Result = (IntPtr)NativeMethods.HTTOP;
                            return;
                        }
                        else if (_TopLeftSide.IntersectsWith(new Rectangle(_Point.X, _Point.Y, 1, 1)))
                        {
                            m.Result = (IntPtr)NativeMethods.HTTOPLEFT;
                            return;
                        }
                        else if (_TopRightSide.IntersectsWith(new Rectangle(_Point.X, _Point.Y, 1, 1)))
                        {
                            m.Result = (IntPtr)NativeMethods.HTTOPRIGHT;
                            return;
                        }
                        else if (_BottomSide.IntersectsWith(new Rectangle(_Point.X, _Point.Y, 1, 1)))
                        {
                            m.Result = (IntPtr)NativeMethods.HTBOTTOM;
                            return;
                        }
                        else if (_BottomLeftSide.IntersectsWith(new Rectangle(_Point.X, _Point.Y, 1, 1)))
                        {
                            m.Result = (IntPtr)NativeMethods.HTBOTTOMLEFT;
                            return;
                        }
                        else if (_BottomRightSide.IntersectsWith(new Rectangle(_Point.X, _Point.Y, 1, 1)))
                        {
                            m.Result = (IntPtr)NativeMethods.HTBOTTOMRIGHT;
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            base.WndProc(ref m);
        }
        #endregion
    }
}
