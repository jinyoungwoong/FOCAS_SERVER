using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using NCManagementSystem.Libraries.Controls.Panel;
using NCManagementSystem.Components.Helpers;

namespace NCManagementSystem.Libraries.Controls.Forms
{
    [ToolboxItem(false), EditorBrowsable(EditorBrowsableState.Never)]
    public partial class SkinContainer : ContainerControl
    {
        #region [ Constructor ]
        public SkinContainer()
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
        private bool m_IsHandleCreated = false;

        private bool IsDesignMode
        {
            get { return (System.Reflection.Assembly.GetEntryAssembly() == null); }
        }

        private Rectangle m_TitleBarBounds;
        internal FwPanel pnlTitleBar;
        internal ControlBoxButton btnClose = new ControlBoxButton();
        internal ControlBoxButton btnMaximize = new ControlBoxButton();
        internal ControlBoxButton btnMinimize = new ControlBoxButton();
        internal CircularProgressSplasher cprgSplashScreen = new CircularProgressSplasher();

        [Browsable(false)]
        public Form OwnerForm { get; set; }

        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.Repaint)]
        [DefaultValue(true)]
        public bool IsDragMoving { get; set; } = true;

        private bool m_IsShowSplashScreen = false;
        [Browsable(false)]
        public bool IsShowSplashScreen
        {
            get { return m_IsShowSplashScreen; }
            set
            {
                m_IsShowSplashScreen = value;
                if (m_IsShowSplashScreen)
                {
                    ShowSplashScreen();
                }
                else
                {
                    HideSplashScreen();
                }
            }
        }

        private Color m_SkinBackColor = SystemColors.Control;
        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.Repaint)]
        [DefaultValue(typeof(Color), "Control")]
        public new Color BackColor
        {
            get { return m_SkinBackColor; }
            set
            {
                m_SkinBackColor = value;
                base.BackColor = m_SkinBackColor;
                cprgSplashScreen.BackColor = m_SkinBackColor;
                Invalidate();
            }
        }

        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.Repaint)]
        [DefaultValue("")]
        public new string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                Invalidate();
            }
        }

        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public BorderProperties BorderProperties { get; set; }

        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public TitleBarProperties TitleBarProperties { get; set; }

        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public SplashScreenProperties SplashScreenProperties { get; set; }
        #endregion

        #region [ Override Events / Events / Methods ]
        private void Initialize()
        {
            try
            {
                Dock = DockStyle.Fill;
                base.BackColor = m_SkinBackColor;

                BorderProperties = new BorderProperties();
                BorderProperties.PropertyChanged += BorderProperties_PropertyChanged;

                TitleBarProperties = new TitleBarProperties();
                TitleBarProperties.PropertyChanged += TitleBarProperties_PropertyChanged;

                SplashScreenProperties = new SplashScreenProperties();
                SplashScreenProperties.PropertyChanged += SplashScreenProperties_PropertyChanged;

                int _iThicknessOfTitleBarBottomBorder = !TitleBarProperties.IsTitleBarBottomBorder ? 0 : TitleBarProperties.TitleBarBottomBorderThickness;
                Padding = new Padding(0, (TitleBarProperties.TitleBarHeight + _iThicknessOfTitleBarBottomBorder), 0, 0);

                InitializeTitleBar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnParentChanged(EventArgs e)
        {
            try
            {
                base.OnParentChanged(e);

                OwnerForm = ParentForm;
                if (OwnerForm != null)
                {
                    int _iThicknessOfBorder = BorderProperties.BorderThickness;
                    OwnerForm.Padding = new Padding(_iThicknessOfBorder);
                    OwnerForm.MinimumSize = new Size(FormsConstsDefiner.FixedSize.Forms.Minimum.FormWidth, (TitleBarProperties.TitleBarHeight + (_iThicknessOfBorder * 2)));
                    OwnerForm.BackColor = BorderProperties.BorderColor;

                    CalculateTitleBar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            try
            {
                if (m_IsHandleCreated)
                {
                    return;
                }

                base.OnHandleCreated(e);

                InitializeSplashScreen();

                m_IsHandleCreated = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BorderProperties_PropertyChanged(object sender, ExtensionsPropertyEventArgs e)
        {
            try
            {
                if (Enum.TryParse(e.PropertyName, out FormsConstsDefiner.ExtensionsPropertyNames.Forms.BorderProperties _Property))
                {
                    switch (_Property)
                    {
                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.BorderProperties.FormBorderThickness:
                            {
                                int _iThicknessOfBorder = BorderProperties.BorderThickness;
                                OwnerForm.Padding = new Padding(_iThicknessOfBorder);
                            }
                            break;

                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.BorderProperties.FormBorderColor:
                            {
                                OwnerForm.BackColor = BorderProperties.BorderColor;
                            }
                            break;

                        default:
                            break;
                    }
                }
                Invalidate();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TitleBarProperties_PropertyChanged(object sender, ExtensionsPropertyEventArgs e)
        {
            try
            {
                if (Enum.TryParse(e.PropertyName, out FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties _Property))
                {
                    switch (_Property)
                    {
                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.TitleBarHeight:
                            {
                                int _iThicknessTitleBarBottomBorder = !TitleBarProperties.IsTitleBarBottomBorder ? 0 : TitleBarProperties.TitleBarBottomBorderThickness;
                                Padding = new Padding(0, (TitleBarProperties.TitleBarHeight + _iThicknessTitleBarBottomBorder), 0, 0);
                                OwnerForm.MinimumSize = new Size(FormsConstsDefiner.FixedSize.Forms.Minimum.FormWidth, TitleBarProperties.TitleBarHeight);
                                CalculateTitleBar();
                            }
                            break;

                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.TitleBarBackColor:
                            {
                                btnClose.BackColor = TitleBarProperties.TitleBarBackColor;
                                btnMaximize.BackColor = TitleBarProperties.TitleBarBackColor;
                                btnMinimize.BackColor = TitleBarProperties.TitleBarBackColor;
                            }
                            break;

                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.IsControlBox:
                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.IsMaximizeBox:
                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.IsMinimizeBox:
                            {
                                CalculateTitleBar();
                            }
                            break;

                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.ControlBoxMaximumSize:
                            {
                                btnClose.MaximumSize = new Size(TitleBarProperties.ControlBoxMaximumSize, TitleBarProperties.ControlBoxMaximumSize);
                                btnMaximize.MaximumSize = new Size(TitleBarProperties.ControlBoxMaximumSize, TitleBarProperties.ControlBoxMaximumSize);
                                btnMinimize.MaximumSize = new Size(TitleBarProperties.ControlBoxMaximumSize, TitleBarProperties.ControlBoxMaximumSize);
                                CalculateTitleBar();
                            }
                            break;

                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.IsShowTitleBarIcon:
                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.TitleBarIcon:
                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.TitleBarIconSize:
                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.TitleBarTextIndent:
                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.TitleBarTextFont:
                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.TitleBarTextForeColor:
                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.TitleBarTextAlignment:
                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.IsTitleBarTextEllipsis:
                            break;

                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.BackColorHover:
                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.BackColorDown:
                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.IsAlphaColorOnDown:
                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.Alpha:
                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.ForeColor:
                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.ForeColorHover:
                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.ForeColorDown:
                            {
                                btnClose.BackColor = TitleBarProperties.TitleBarBackColor;
                                btnClose.ColorProperties.BackColors.Hover = TitleBarProperties.ControlBoxColors.CloseButtonColors.BackColors.Hover;
                                btnClose.ColorProperties.BackColors.IsAlphaColorOnDown = TitleBarProperties.ControlBoxColors.CloseButtonColors.BackColors.IsAlphaColorOnDown;
                                btnClose.ColorProperties.BackColors.Alpha = TitleBarProperties.ControlBoxColors.CloseButtonColors.BackColors.Alpha;
                                btnClose.ColorProperties.BackColors.Down = TitleBarProperties.ControlBoxColors.CloseButtonColors.BackColors.Down;
                                btnClose.ForeColor = TitleBarProperties.ControlBoxColors.CloseButtonColors.ForeColor;
                                btnClose.ColorProperties.ForeColors.Hover = TitleBarProperties.ControlBoxColors.CloseButtonColors.ForeColors.Hover;
                                btnClose.ColorProperties.ForeColors.Down = TitleBarProperties.ControlBoxColors.CloseButtonColors.ForeColors.Down;
                                btnMaximize.BackColor = TitleBarProperties.TitleBarBackColor;
                                btnMaximize.ColorProperties.BackColors.Hover = TitleBarProperties.ControlBoxColors.MaximizeButtonColors.BackColors.Hover;
                                btnMaximize.ColorProperties.BackColors.IsAlphaColorOnDown = TitleBarProperties.ControlBoxColors.MaximizeButtonColors.BackColors.IsAlphaColorOnDown;
                                btnMaximize.ColorProperties.BackColors.Alpha = TitleBarProperties.ControlBoxColors.MaximizeButtonColors.BackColors.Alpha;
                                btnMaximize.ColorProperties.BackColors.Down = TitleBarProperties.ControlBoxColors.MaximizeButtonColors.BackColors.Down;
                                btnMaximize.ForeColor = TitleBarProperties.ControlBoxColors.MaximizeButtonColors.ForeColor;
                                btnMaximize.ColorProperties.ForeColors.Hover = TitleBarProperties.ControlBoxColors.MaximizeButtonColors.ForeColors.Hover;
                                btnMaximize.ColorProperties.ForeColors.Down = TitleBarProperties.ControlBoxColors.MaximizeButtonColors.ForeColors.Down;
                                btnMinimize.BackColor = TitleBarProperties.TitleBarBackColor;
                                btnMinimize.ColorProperties.BackColors.Hover = TitleBarProperties.ControlBoxColors.MinimizeButtonColors.BackColors.Hover;
                                btnMinimize.ColorProperties.BackColors.IsAlphaColorOnDown = TitleBarProperties.ControlBoxColors.MinimizeButtonColors.BackColors.IsAlphaColorOnDown;
                                btnMinimize.ColorProperties.BackColors.Alpha = TitleBarProperties.ControlBoxColors.MinimizeButtonColors.BackColors.Alpha;
                                btnMinimize.ColorProperties.BackColors.Down = TitleBarProperties.ControlBoxColors.MinimizeButtonColors.BackColors.Down;
                                btnMinimize.ForeColor = TitleBarProperties.ControlBoxColors.MinimizeButtonColors.ForeColor;
                                btnMinimize.ColorProperties.ForeColors.Hover = TitleBarProperties.ControlBoxColors.MinimizeButtonColors.ForeColors.Hover;
                                btnMinimize.ColorProperties.ForeColors.Down = TitleBarProperties.ControlBoxColors.MinimizeButtonColors.ForeColors.Down;
                            }
                            break;

                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.IsTitleBarBottomBorder:
                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.TitleBarBottomBorderThickness:
                            {
                                int _iThicknessOfTitleBarBottomBorder = !TitleBarProperties.IsTitleBarBottomBorder ? 0 : TitleBarProperties.TitleBarBottomBorderThickness;
                                Padding = new Padding(0, (TitleBarProperties.TitleBarHeight + _iThicknessOfTitleBarBottomBorder), 0, 0);
                            }
                            break;

                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.TitleBarBottomBorderColor:
                            break;

                        default:
                            break;
                    }
                    Invalidate();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SplashScreenProperties_PropertyChanged(object sender, ExtensionsPropertyEventArgs e)
        {
            try
            {
                if (Enum.TryParse(e.PropertyName, out FormsConstsDefiner.ExtensionsPropertyNames.Forms.SplashScreenProperties _Property))
                {
                    switch (_Property)
                    {
                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.SplashScreenProperties.SplashScreenBackColor:
                            {
                                cprgSplashScreen.BackColor = SplashScreenProperties.BackColor;
                            }
                            break;

                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.SplashScreenProperties.SplashScreenBackgroundOpacity:
                            {
                                cprgSplashScreen.BackgroundOpacity = SplashScreenProperties.BackgroundOpacity;
                            }
                            break;

                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.SplashScreenProperties.SplashScreenSpokeColor:
                            {
                                cprgSplashScreen.SpokeProperties.SpokeColor = SplashScreenProperties.SpokeColor;
                            }
                            break;

                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.SplashScreenProperties.RotationSpeedPercent:
                            {
                                cprgSplashScreen.RotationSpeedPercent = SplashScreenProperties.RotationSpeedPercent;
                            }
                            break;

                        case FormsConstsDefiner.ExtensionsPropertyNames.Forms.SplashScreenProperties.MaximumSpokeSize:
                            break;

                        default:
                            break;
                    }
                    Invalidate();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeTitleBar()
        {
            try
            {
                m_TitleBarBounds = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, TitleBarProperties.TitleBarHeight);
                btnClose.Visible = false;
                btnClose.MaximumSize = new Size(TitleBarProperties.ControlBoxMaximumSize, TitleBarProperties.ControlBoxMaximumSize);
                btnClose.Size = new Size(TitleBarProperties.TitleBarHeight, TitleBarProperties.TitleBarHeight);
                btnClose.Location = new Point((m_TitleBarBounds.Width - btnClose.Width), m_TitleBarBounds.Y);
                btnClose.ControlBoxButtonType = FormsConstsDefiner.ControlBoxButtonTypes.Close;
                btnClose.BackColor = TitleBarProperties.TitleBarBackColor;
                btnClose.ColorProperties.BackColors.Hover = TitleBarProperties.ControlBoxColors.CloseButtonColors.BackColors.Hover;
                btnClose.ColorProperties.BackColors.IsAlphaColorOnDown = TitleBarProperties.ControlBoxColors.CloseButtonColors.BackColors.IsAlphaColorOnDown;
                btnClose.ColorProperties.BackColors.Alpha = TitleBarProperties.ControlBoxColors.CloseButtonColors.BackColors.Alpha;
                btnClose.ColorProperties.BackColors.Down = TitleBarProperties.ControlBoxColors.CloseButtonColors.BackColors.Down;
                btnClose.ForeColor = TitleBarProperties.ControlBoxColors.CloseButtonColors.ForeColor;
                btnClose.ColorProperties.ForeColors.Hover = TitleBarProperties.ControlBoxColors.CloseButtonColors.ForeColors.Hover;
                btnClose.ColorProperties.ForeColors.Down = TitleBarProperties.ControlBoxColors.CloseButtonColors.ForeColors.Down;
                Controls.Add(btnClose);
                btnMaximize.Visible = false;
                btnMaximize.MaximumSize = new Size(TitleBarProperties.ControlBoxMaximumSize, TitleBarProperties.ControlBoxMaximumSize);
                btnMaximize.Size = new Size(TitleBarProperties.TitleBarHeight, TitleBarProperties.TitleBarHeight);
                btnMaximize.Location = new Point((m_TitleBarBounds.Width - (btnClose.Width + btnMaximize.Width)), m_TitleBarBounds.Y);
                btnMaximize.ControlBoxButtonType = FormsConstsDefiner.ControlBoxButtonTypes.Maximum;
                btnMaximize.BackColor = TitleBarProperties.TitleBarBackColor;
                btnMaximize.ColorProperties.BackColors.Hover = TitleBarProperties.ControlBoxColors.MaximizeButtonColors.BackColors.Hover;
                btnMaximize.ColorProperties.BackColors.IsAlphaColorOnDown = TitleBarProperties.ControlBoxColors.MaximizeButtonColors.BackColors.IsAlphaColorOnDown;
                btnMaximize.ColorProperties.BackColors.Alpha = TitleBarProperties.ControlBoxColors.MaximizeButtonColors.BackColors.Alpha;
                btnMaximize.ColorProperties.BackColors.Down = TitleBarProperties.ControlBoxColors.MaximizeButtonColors.BackColors.Down;
                btnMaximize.ForeColor = TitleBarProperties.ControlBoxColors.MaximizeButtonColors.ForeColor;
                btnMaximize.ColorProperties.ForeColors.Hover = TitleBarProperties.ControlBoxColors.MaximizeButtonColors.ForeColors.Hover;
                btnMaximize.ColorProperties.ForeColors.Down = TitleBarProperties.ControlBoxColors.MaximizeButtonColors.ForeColors.Down;
                Controls.Add(btnMaximize);
                btnMinimize.Visible = false;
                btnMinimize.MaximumSize = new Size(TitleBarProperties.ControlBoxMaximumSize, TitleBarProperties.ControlBoxMaximumSize);
                btnMinimize.Size = new Size(TitleBarProperties.TitleBarHeight, TitleBarProperties.TitleBarHeight);
                btnMinimize.Location = new Point((m_TitleBarBounds.Width - (btnClose.Width + btnMaximize.Width + btnMinimize.Width)), m_TitleBarBounds.Y);
                btnMinimize.ControlBoxButtonType = FormsConstsDefiner.ControlBoxButtonTypes.Minimize;
                btnMinimize.BackColor = TitleBarProperties.TitleBarBackColor;
                btnMinimize.ColorProperties.BackColors.Hover = TitleBarProperties.ControlBoxColors.MinimizeButtonColors.BackColors.Hover;
                btnMinimize.ColorProperties.BackColors.IsAlphaColorOnDown = TitleBarProperties.ControlBoxColors.MinimizeButtonColors.BackColors.IsAlphaColorOnDown;
                btnMinimize.ColorProperties.BackColors.Alpha = TitleBarProperties.ControlBoxColors.MinimizeButtonColors.BackColors.Alpha;
                btnMinimize.ColorProperties.BackColors.Down = TitleBarProperties.ControlBoxColors.MinimizeButtonColors.BackColors.Down;
                btnMinimize.ForeColor = TitleBarProperties.ControlBoxColors.MinimizeButtonColors.ForeColor;
                btnMinimize.ColorProperties.ForeColors.Hover = TitleBarProperties.ControlBoxColors.MinimizeButtonColors.ForeColors.Hover;
                btnMinimize.ColorProperties.ForeColors.Down = TitleBarProperties.ControlBoxColors.MinimizeButtonColors.ForeColors.Down;
                Controls.Add(btnMinimize);
                pnlTitleBar = new FwPanel
                {
                    BackColor = Color.Transparent,
                    Location = new Point(m_TitleBarBounds.X, m_TitleBarBounds.Y),
                    Size = new Size(m_TitleBarBounds.Width, m_TitleBarBounds.Height)
                };
                pnlTitleBar.MouseDown += TitleBar_MouseDown;
                Controls.Add(pnlTitleBar);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CalculateTitleBar()
        {
            try
            {
                m_TitleBarBounds = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, TitleBarProperties.TitleBarHeight);
                pnlTitleBar.BackColor = Color.Transparent;
                pnlTitleBar.Location = new Point(m_TitleBarBounds.X, m_TitleBarBounds.Y);
                pnlTitleBar.Size = new Size(m_TitleBarBounds.Width, m_TitleBarBounds.Height);
                if (TitleBarProperties.IsControlBox)
                {
                    btnClose.Visible = true;
                    btnClose.Size = new Size(TitleBarProperties.TitleBarHeight, TitleBarProperties.TitleBarHeight);
                    btnClose.Location = new Point((m_TitleBarBounds.Width - btnClose.Width), m_TitleBarBounds.Y);
                    if (TitleBarProperties.IsMaximizeBox)
                    {
                        btnMaximize.Visible = true;
                        btnMaximize.Size = new Size(TitleBarProperties.TitleBarHeight, TitleBarProperties.TitleBarHeight);
                        btnMaximize.Location = new Point((m_TitleBarBounds.Width - (btnClose.Width + btnMaximize.Width)), m_TitleBarBounds.Y);
                    }
                    else
                    {
                        btnMaximize.Visible = false;
                    }
                    if (TitleBarProperties.IsMinimizeBox)
                    {
                        btnMinimize.Visible = true;
                        btnMinimize.Size = new Size(TitleBarProperties.TitleBarHeight, TitleBarProperties.TitleBarHeight);
                        if (TitleBarProperties.IsMaximizeBox)
                        {
                            btnMinimize.Location = new Point((m_TitleBarBounds.Width - (btnClose.Width + btnMaximize.Width + btnMinimize.Width)), m_TitleBarBounds.Y);
                        }
                        else
                        {
                            btnMinimize.Location = new Point((m_TitleBarBounds.Width  - (btnClose.Width + btnMinimize.Width)), m_TitleBarBounds.Y);
                        }
                    }
                    else
                    {
                        btnMinimize.Visible = false;
                    }
                }
                else
                {
                    btnClose.Visible = false;
                    btnMaximize.Visible = false;
                    btnMinimize.Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                OnMouseDown(e);

                if (e.Button.Equals(MouseButtons.Left) && e.Clicks >= 2)
                {
                    if (OwnerForm.WindowState.Equals(FormWindowState.Normal))
                    {
                        if (!TitleBarProperties.IsMaximizeBox)
                        {
                            return;
                        }
                        OwnerForm.WindowState = FormWindowState.Maximized;
                        Invalidate();
                    }
                    else if (OwnerForm.WindowState.Equals(FormWindowState.Maximized))
                    {
                        if (!TitleBarProperties.IsMaximizeBox)
                        {
                            return;
                        }
                        OwnerForm.WindowState = FormWindowState.Normal;
                        Invalidate();
                    }
                    return;
                }
                else if (e.Button.Equals(MouseButtons.Left))
                {
                    if (!IsDragMoving)
                    {
                        return;
                    }
                    NativeMethods.ReleaseCapture();
                    NativeMethods.SendMessage(OwnerForm.Handle, NativeMethods.WM_NCLBUTTONDOWN, NativeMethods.HTCAPTION, 0);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeSplashScreen()
        {
            try
            {
                cprgSplashScreen.Dock = DockStyle.Fill;
                cprgSplashScreen.BackColor = SplashScreenProperties.BackColor;
                cprgSplashScreen.BackgroundOpacity = SplashScreenProperties.BackgroundOpacity;
                cprgSplashScreen.RotateDirection = FormsConstsDefiner.RotateDirections.Clockwise;
                cprgSplashScreen.SpokeStyle = FormsConstsDefiner.SpokeStyles.MediumRoundedRectangle;
                cprgSplashScreen.SpokeProperties.IsFixedSpokeSize = true;
                cprgSplashScreen.SpokeProperties.SpokeCount = 12;
                cprgSplashScreen.SpokeProperties.SpokeStartPosition = FormsConstsDefiner.SpokeStartPositions.Top;
                cprgSplashScreen.SpokeProperties.SpokeColor = SplashScreenProperties.SpokeColor;
                cprgSplashScreen.RotationSpeedPercent = SplashScreenProperties.RotationSpeedPercent;
                cprgSplashScreen.Visible = false;
                Controls.Add(cprgSplashScreen);
                cprgSplashScreen.BringToFront();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                e.Graphics.Clear(BackColor);

                using (SolidBrush _TitleBarBackColor = new SolidBrush(TitleBarProperties.TitleBarBackColor))
                {
                    e.Graphics.FillRectangle(_TitleBarBackColor, m_TitleBarBounds);
                }
                if (TitleBarProperties.IsTitleBarBottomBorder)
                {
                    using (Pen _TitleBarBottomBorder = new Pen(TitleBarProperties.TitleBarBottomBorderColor, TitleBarProperties.TitleBarBottomBorderThickness))
                    {
                        e.Graphics.DrawLine(_TitleBarBottomBorder, m_TitleBarBounds.X, (m_TitleBarBounds.Y + m_TitleBarBounds.Height + (TitleBarProperties.TitleBarBottomBorderThickness / 2F)), (m_TitleBarBounds.X + m_TitleBarBounds.Width), (m_TitleBarBounds.Y + m_TitleBarBounds.Height + (TitleBarProperties.TitleBarBottomBorderThickness / 2F)));
                    }
                }

                int _iTextIndentOfTitleBar = TitleBarProperties.TitleBarTextIndent;
                TextFormatFlags _TextFormatFlagsOfTitleBar = TextFormatFlags.VerticalCenter;
                if (TitleBarProperties.IsTitleBarTextEllipsis)
                {
                    _TextFormatFlagsOfTitleBar |= TextFormatFlags.EndEllipsis;
                }

                RectangleF _TitleBarIconBounds = RectangleF.Empty;
                if (TitleBarProperties.IsShowTitleBarIcon)
                {
                    Image _TitleBarIcon = null;
                    if (TitleBarProperties.TitleBarIcon == null)
                    {
                        _TitleBarIcon = Bitmap.FromHicon(OwnerForm.Icon.Handle);
                        _TitleBarIcon = SizeHelper.ResizeImage(_TitleBarIcon, TitleBarProperties.TitleBarIconSize.Width, TitleBarProperties.TitleBarIconSize.Height);
                    }
                    else
                    {
                        _TitleBarIcon = SizeHelper.ResizeImage(TitleBarProperties.TitleBarIcon, TitleBarProperties.TitleBarIconSize.Width, TitleBarProperties.TitleBarIconSize.Height);
                    }
                    float _fPaddingOfTitleBarIcon = ((TitleBarProperties.TitleBarHeight - TitleBarProperties.TitleBarIconSize.Height) / 2F);
                    _TitleBarIconBounds = new RectangleF((m_TitleBarBounds.X + _fPaddingOfTitleBarIcon), (m_TitleBarBounds.Y + _fPaddingOfTitleBarIcon), TitleBarProperties.TitleBarIconSize.Width, TitleBarProperties.TitleBarIconSize.Height);
                    e.Graphics.DrawImage(_TitleBarIcon, _TitleBarIconBounds);

                    _iTextIndentOfTitleBar += (TitleBarProperties.TitleBarIconSize.Width + (TitleBarProperties.TitleBarHeight - TitleBarProperties.TitleBarIconSize.Height));
                }

                Rectangle _TitleBarTextBounds = new Rectangle((m_TitleBarBounds.X + _iTextIndentOfTitleBar), m_TitleBarBounds.Y, (m_TitleBarBounds.Width - (_iTextIndentOfTitleBar + GetControlBoxBoundsWidth())), m_TitleBarBounds.Height);
                switch (TitleBarProperties.TitleBarTextAlignment)
                {
                    case FormsConstsDefiner.ColumnAlignment.center:
                        {
                            _TextFormatFlagsOfTitleBar |= TextFormatFlags.HorizontalCenter;
                            TextRenderer.DrawText(e.Graphics, Text, TitleBarProperties.TitleBarTextFont, _TitleBarTextBounds, TitleBarProperties.TitleBarTextForeColor, _TextFormatFlagsOfTitleBar);
                        }
                        break;

                    case FormsConstsDefiner.ColumnAlignment.right:
                        {
                            _TextFormatFlagsOfTitleBar |= TextFormatFlags.Right;
                            TextRenderer.DrawText(e.Graphics, Text, TitleBarProperties.TitleBarTextFont, _TitleBarTextBounds, TitleBarProperties.TitleBarTextForeColor, _TextFormatFlagsOfTitleBar);
                        }
                        break;

                    case FormsConstsDefiner.ColumnAlignment.left:
                    default:
                        {
                            _TextFormatFlagsOfTitleBar |= TextFormatFlags.Left;
                            TextRenderer.DrawText(e.Graphics, Text, TitleBarProperties.TitleBarTextFont, _TitleBarTextBounds, TitleBarProperties.TitleBarTextForeColor, _TextFormatFlagsOfTitleBar);
                        }
                        break;
                }

                base.OnPaint(e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int GetControlBoxBoundsWidth()
        {
            try
            {
                int _iWidth = btnClose.Width;
                if (!TitleBarProperties.IsMaximizeBox && !TitleBarProperties.IsMinimizeBox)
                {
                    return _iWidth;
                }
                else if (TitleBarProperties.IsMaximizeBox && !TitleBarProperties.IsMinimizeBox)
                {
                    return (_iWidth + btnMaximize.Width);
                }
                else if (!TitleBarProperties.IsMaximizeBox && TitleBarProperties.IsMinimizeBox)
                {
                    return (_iWidth + btnMinimize.Width);
                }
                else
                {
                    return (_iWidth + btnMaximize.Width + btnMinimize.Width);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            try
            {
                base.OnSizeChanged(e);

                if (!IsDisposed)
                {
                    CalculateTitleBar();

                    CalculateFixedSpokeSize();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ShowSplashScreen()
        {
            try
            {
                if (m_IsHandleCreated)
                {
                    Invoke(new Action(() =>
                    {
                        ActiveControl = null;
                        Cursor = Cursors.WaitCursor;

                        CalculateFixedSpokeSize();

                        cprgSplashScreen.IsActive = cprgSplashScreen.Visible = true;
                        cprgSplashScreen.BringToFront();
                    }));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void HideSplashScreen()
        {
            try
            {
                if (m_IsHandleCreated)
                {
                    Invoke(new Action(() =>
                    {
                        cprgSplashScreen.Visible = cprgSplashScreen.IsActive = false;
                        Cursor = Cursors.Default;
                    }));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CalculateFixedSpokeSize()
        {
            try
            {
                int _iFixedSpokeSize = (int)((Width * 20F) / 100F);
                if (_iFixedSpokeSize > SplashScreenProperties.MaximumSpokeSize)
                {
                    cprgSplashScreen.SpokeProperties.FixedSpokeSize = SplashScreenProperties.MaximumSpokeSize;
                }
                else
                {
                    cprgSplashScreen.SpokeProperties.FixedSpokeSize = _iFixedSpokeSize;
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
                if (!IsDesignMode)
                {
                    CreateParams _CreateParams = base.CreateParams;
                    _CreateParams.ExStyle = _CreateParams.ExStyle | NativeMethods.WS_EX_COMPOSITED;
                    return _CreateParams;
                }
                else
                {
                    return base.CreateParams;
                }
            }
        }
        #endregion
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class BorderProperties
    {
        public BorderProperties()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override string ToString()
        {
            return string.Empty;
        }

        private int m_iFormBorderThickness = 2;
        [DefaultValue(2)]
        public int BorderThickness
        {
            get { return m_iFormBorderThickness; }
            set
            {
                if (value <= 0)
                {
                    return;
                }
                m_iFormBorderThickness = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.Forms.BorderProperties.FormBorderThickness.ToString());
            }
        }

        private Color m_FormBorderColor = SystemColors.ActiveCaption;
        [DefaultValue(typeof(Color), "ActiveCaption")]
        public Color BorderColor
        {
            get { return m_FormBorderColor; }
            set
            {
                m_FormBorderColor = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.Forms.BorderProperties.FormBorderColor.ToString());
            }
        }

        [Browsable(false)]
        public event EventHandler<ExtensionsPropertyEventArgs> PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new ExtensionsPropertyEventArgs(propertyName));
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TitleBarProperties
    {
        public TitleBarProperties()
        {
            try
            {
                ControlBoxColors = new ControlBoxColorProperties();
                ControlBoxColors.PropertyChanged += OnSubPropertyChanged;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override string ToString()
        {
            return string.Empty;
        }

        private int m_iTitleBarHeight = 30;
        [DefaultValue(30)]
        public int TitleBarHeight
        {
            get { return m_iTitleBarHeight; }
            set
            {
                if (value < FormsConstsDefiner.FixedSize.Forms.Minimum.TitleBarHeight)
                {
                    value = FormsConstsDefiner.FixedSize.Forms.Minimum.TitleBarHeight; // 최소 높이 비교
                }
                else if (value < m_TitleBarIconSize.Height)
                {
                    TitleBarIconSize = new Size(m_TitleBarIconSize.Width, value); // 아이콘 높이 비교
                }

                m_iTitleBarHeight = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.TitleBarHeight.ToString());
            }
        }

        private Color m_TitleBarBackColor = SystemColors.ActiveCaption;
        [DefaultValue(typeof(Color), "ActiveCaption")]
        public Color TitleBarBackColor
        {
            get { return m_TitleBarBackColor; }
            set
            {
                m_TitleBarBackColor = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.TitleBarBackColor.ToString());
            }
        }

        private bool m_IsTitleBarBottomBorder = false;
        [DefaultValue(false)]
        public bool IsTitleBarBottomBorder
        {
            get { return m_IsTitleBarBottomBorder; }
            set
            {
                m_IsTitleBarBottomBorder = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.IsTitleBarBottomBorder.ToString());
            }
        }

        private int m_iTitleBarBottomBorderThickness = 1;
        [DefaultValue(1)]
        public int TitleBarBottomBorderThickness
        {
            get { return m_iTitleBarBottomBorderThickness; }
            set
            {
                m_iTitleBarBottomBorderThickness = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.TitleBarBottomBorderThickness.ToString());
            }
        }

        private Color m_TitleBarBottomBorderColor = SystemColors.ActiveBorder;
        [DefaultValue(typeof(Color), "ActiveBorder")]
        public Color TitleBarBottomBorderColor
        {
            get { return m_TitleBarBottomBorderColor; }
            set
            {
                m_TitleBarBottomBorderColor = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.TitleBarBottomBorderColor.ToString());
            }
        }

        private bool m_IsControlBox = true;
        [DefaultValue(true)]
        public bool IsControlBox
        {
            get { return m_IsControlBox; }
            set
            {
                m_IsControlBox = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.IsControlBox.ToString());
            }
        }

        private bool m_IsMaximizeBox = true;
        [DefaultValue(true)]
        public bool IsMaximizeBox
        {
            get { return m_IsMaximizeBox; }
            set
            {
                m_IsMaximizeBox = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.IsMaximizeBox.ToString());
            }
        }

        private bool m_IsMinimizeBox = true;
        [DefaultValue(true)]
        public bool IsMinimizeBox
        {
            get { return m_IsMinimizeBox; }
            set
            {
                m_IsMinimizeBox = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.IsMinimizeBox.ToString());
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public ControlBoxColorProperties ControlBoxColors { get; set; }

        private int m_iControlBoxMaximumSize = 30;
        [DefaultValue(30)]
        public int ControlBoxMaximumSize
        {
            get { return m_iControlBoxMaximumSize; }
            set
            {
                m_iControlBoxMaximumSize = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.ControlBoxMaximumSize.ToString());
            }
        }

        private bool m_IsShowTitleBarIcon = true;
        [DefaultValue(true)]
        public bool IsShowTitleBarIcon
        {
            get { return m_IsShowTitleBarIcon; }
            set
            {
                m_IsShowTitleBarIcon = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.IsShowTitleBarIcon.ToString());
            }
        }

        private Image m_TitleBarIcon = null;
        [DefaultValue(typeof(Image), "null")]
        public Image TitleBarIcon
        {
            get { return m_TitleBarIcon; }
            set
            {
                m_TitleBarIcon = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.TitleBarIcon.ToString());
            }
        }

        private Size m_TitleBarIconSize = new Size(20, 20);
        [DefaultValue(typeof(Size), "20, 20")]
        public Size TitleBarIconSize
        {
            get { return m_TitleBarIconSize; }
            set
            {
                if (value.Height < FormsConstsDefiner.FixedSize.Forms.Minimum.IconHeight)
                {
                    value.Height = FormsConstsDefiner.FixedSize.Forms.Minimum.IconHeight; // 아이콘 최소 높이 비교
                }
                else if (value.Height > m_iTitleBarHeight)
                {
                    value.Height = m_iTitleBarHeight; // 타이틀바 높이 비교
                }

                m_TitleBarIconSize = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.TitleBarIconSize.ToString());
            }
        }

        private int m_iTitleBarTextIndent = 0;
        [DefaultValue(0)]
        public int TitleBarTextIndent
        {
            get { return m_iTitleBarTextIndent; }
            set
            {
                m_iTitleBarTextIndent = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.TitleBarTextIndent.ToString());
            }
        }

        private Font m_TitleBarTextFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
        [DefaultValue(typeof(Font), "Microsoft Sans Serif, 9pt, style=Bold")]
        public Font TitleBarTextFont
        {
            get { return m_TitleBarTextFont; }
            set
            {
                m_TitleBarTextFont = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.TitleBarTextFont.ToString());
            }
        }

        private Color m_TitleBarTextForeColor = SystemColors.ControlText;
        [DefaultValue(typeof(Color), "ControlText")]
        public Color TitleBarTextForeColor
        {
            get { return m_TitleBarTextForeColor; }
            set
            {
                m_TitleBarTextForeColor = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.TitleBarTextForeColor.ToString());
            }
        }

        private FormsConstsDefiner.ColumnAlignment m_TitleBarTextAlignment = FormsConstsDefiner.ColumnAlignment.left;
        [DefaultValue(typeof(FormsConstsDefiner.ColumnAlignment), "Left")]
        public FormsConstsDefiner.ColumnAlignment TitleBarTextAlignment
        {
            get { return m_TitleBarTextAlignment; }
            set
            {
                m_TitleBarTextAlignment = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.TitleBarTextAlignment.ToString());
            }
        }

        private bool m_IsTitleBarTextEllipsis = true;
        [DefaultValue(true)]
        public bool IsTitleBarTextEllipsis
        {
            get { return m_IsTitleBarTextEllipsis; }
            set
            {
                m_IsTitleBarTextEllipsis = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.IsTitleBarTextEllipsis.ToString());
            }
        }

        private void OnSubPropertyChanged(object sender, ExtensionsPropertyEventArgs e)
        {
            try
            {
                OnPropertyChanged(e.PropertyName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Browsable(false)]
        public event EventHandler<ExtensionsPropertyEventArgs> PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new ExtensionsPropertyEventArgs(propertyName));
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ControlBoxColorProperties
    {
        public ControlBoxColorProperties()
        {
            try
            {
                CloseButtonColors = new CloseButtonColorProperties();
                CloseButtonColors.PropertyChanged += OnSubPropertyChanged;
                MaximizeButtonColors = new MaximizeButtonColorProperties();
                MaximizeButtonColors.PropertyChanged += OnSubPropertyChanged;
                MinimizeButtonColors = new MinimizeButtonColorProperties();
                MinimizeButtonColors.PropertyChanged += OnSubPropertyChanged;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override string ToString()
        {
            return string.Empty;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public CloseButtonColorProperties CloseButtonColors { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public MaximizeButtonColorProperties MaximizeButtonColors { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public MinimizeButtonColorProperties MinimizeButtonColors { get; set; }

        private void OnSubPropertyChanged(object sender, ExtensionsPropertyEventArgs e)
        {
            try
            {
                OnPropertyChanged(e.PropertyName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Browsable(false)]
        public event EventHandler<ExtensionsPropertyEventArgs> PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new ExtensionsPropertyEventArgs(propertyName));
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CloseButtonColorProperties
    {
        public CloseButtonColorProperties()
        {
            try
            {
                BackColors = new BackColorProperties();
                BackColors.PropertyChanged += OnSubPropertyChanged;
                ForeColors = new ForeColorProperties();
                ForeColors.PropertyChanged += OnSubPropertyChanged;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override string ToString()
        {
            return string.Empty;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public BackColorProperties BackColors { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public ForeColorProperties ForeColors { get; set; }

        private Color m_ForeColor = SystemColors.GrayText;
        [DefaultValue(typeof(Color), "GrayText")]
        public Color ForeColor
        {
            get { return m_ForeColor; }
            set
            {
                m_ForeColor = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.ForeColor.ToString());
            }
        }

        private void OnSubPropertyChanged(object sender, ExtensionsPropertyEventArgs e)
        {
            try
            {
                OnPropertyChanged(e.PropertyName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Browsable(false)]
        public event EventHandler<ExtensionsPropertyEventArgs> PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new ExtensionsPropertyEventArgs(propertyName));
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class MaximizeButtonColorProperties
    {
        public MaximizeButtonColorProperties()
        {
            try
            {
                BackColors = new BackColorProperties();
                BackColors.PropertyChanged += OnSubPropertyChanged;
                ForeColors = new ForeColorProperties();
                ForeColors.PropertyChanged += OnSubPropertyChanged;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override string ToString()
        {
            return string.Empty;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public BackColorProperties BackColors { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public ForeColorProperties ForeColors { get; set; }

        private Color m_ForeColor = SystemColors.GrayText;
        [DefaultValue(typeof(Color), "GrayText")]
        public Color ForeColor
        {
            get { return m_ForeColor; }
            set
            {
                m_ForeColor = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.ForeColor.ToString());
            }
        }

        private void OnSubPropertyChanged(object sender, ExtensionsPropertyEventArgs e)
        {
            try
            {
                OnPropertyChanged(e.PropertyName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Browsable(false)]
        public event EventHandler<ExtensionsPropertyEventArgs> PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new ExtensionsPropertyEventArgs(propertyName));
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class MinimizeButtonColorProperties
    {
        public MinimizeButtonColorProperties()
        {
            try
            {
                BackColors = new BackColorProperties();
                BackColors.PropertyChanged += OnSubPropertyChanged;
                ForeColors = new ForeColorProperties();
                ForeColors.PropertyChanged += OnSubPropertyChanged;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override string ToString()
        {
            return string.Empty;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public BackColorProperties BackColors { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public ForeColorProperties ForeColors { get; set; }

        private Color m_ForeColor = SystemColors.GrayText;
        [DefaultValue(typeof(Color), "GrayText")]
        public Color ForeColor
        {
            get { return m_ForeColor; }
            set
            {
                m_ForeColor = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.Forms.TitleBarProperties.ForeColor.ToString());
            }
        }

        private void OnSubPropertyChanged(object sender, ExtensionsPropertyEventArgs e)
        {
            try
            {
                OnPropertyChanged(e.PropertyName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Browsable(false)]
        public event EventHandler<ExtensionsPropertyEventArgs> PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new ExtensionsPropertyEventArgs(propertyName));
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SplashScreenProperties
    {
        public SplashScreenProperties()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override string ToString()
        {
            return string.Empty;
        }

        private Color m_BackColor = SystemColors.WindowFrame;
        [DefaultValue(typeof(Color), "WindowFrame")]
        public Color BackColor
        {
            get { return m_BackColor; }
            set
            {
                m_BackColor = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.Forms.SplashScreenProperties.SplashScreenBackColor.ToString());
            }
        }

        private int m_BackgroundOpacity = 20;
        [DefaultValue(20)]
        public int BackgroundOpacity
        {
            get { return m_BackgroundOpacity; }
            set
            {
                m_BackgroundOpacity = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.Forms.SplashScreenProperties.SplashScreenBackgroundOpacity.ToString());
            }
        }

        private Color m_SpokeColor = SystemColors.Highlight;
        [DefaultValue(typeof(Color), "Highlight")]
        public Color SpokeColor
        {
            get { return m_SpokeColor; }
            set
            {
                m_SpokeColor = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.Forms.SplashScreenProperties.SplashScreenSpokeColor.ToString());
            }
        }

        private int m_iRotationSpeedPercent = 80;
        [DefaultValue(80)]
        public int RotationSpeedPercent
        {
            get { return m_iRotationSpeedPercent; }
            set
            {
                m_iRotationSpeedPercent = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.Forms.SplashScreenProperties.RotationSpeedPercent.ToString());
            }
        }

        private int m_iMaximumSpokeSize = 250;
        [DefaultValue(250)]
        public int MaximumSpokeSize
        {
            get { return m_iMaximumSpokeSize; }
            set
            {
                m_iMaximumSpokeSize = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.Forms.SplashScreenProperties.MaximumSpokeSize.ToString());
            }
        }

        [Browsable(false)]
        public event EventHandler<ExtensionsPropertyEventArgs> PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new ExtensionsPropertyEventArgs(propertyName));
        }
    }
}
