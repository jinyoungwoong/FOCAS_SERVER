using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NCManagementSystem.Libraries.Controls.Panel
{
    [ToolboxBitmap(typeof(System.Windows.Forms.Panel)), ToolboxItemFilter("System.Windows.Forms")]
    public partial class FwPanel : System.Windows.Forms.Panel
    {
        #region [ Constructor ]
        public FwPanel()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();

            InitializeComponent();
        }
        #endregion

        #region [ DllImport ]
        /*
        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);
        */
        #endregion

        #region [ Member Variables / Fields / Properties ]
        private Color m_BorderColor = SystemColors.ActiveBorder;
        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All)]
        [DefaultValue(typeof(Color), "ActiveBorder")]
        public Color BorderColor
        {
            get { return m_BorderColor; }
            set
            {
                m_BorderColor = value;
                Invalidate();
            }
        }

        private ButtonBorderStyle m_BorderLineStyle = ButtonBorderStyle.Solid;
        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All)]
        [DefaultValue(typeof(ButtonBorderStyle), "Solid")]
        public ButtonBorderStyle BorderLineStyle
        {
            get { return m_BorderLineStyle; }
            set
            {
                m_BorderLineStyle = value;
                Invalidate();
            }
        }
        #endregion

        #region [ Override Events / Events / Methods ]
        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                if (BorderStyle.Equals(BorderStyle.FixedSingle))
                {
                    IntPtr _hDC = NativeMethods.GetWindowDC(Handle);
                    Graphics _Graphics = Graphics.FromHdc(_hDC);
                    _Graphics.Clear(BackColor);
                    ControlPaint.DrawBorder(_Graphics, new Rectangle(0, 0, Width, Height), m_BorderColor, m_BorderLineStyle);
                    NativeMethods.ReleaseDC(Handle, _hDC);
                    _Graphics.Dispose();
                }
                else
                {
                    base.OnPaint(e);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams _CreateParams = base.CreateParams;
                _CreateParams.ExStyle = _CreateParams.ExStyle | NativeMethods.WS_EX_COMPOSITED;
                return _CreateParams;
            }
        }
        #endregion
    }
}
