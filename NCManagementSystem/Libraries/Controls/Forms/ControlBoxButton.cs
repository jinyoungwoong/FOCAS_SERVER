using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NCManagementSystem.Libraries.Controls.Forms
{
    [ToolboxBitmap(typeof(System.Windows.Forms.Button)), ToolboxItemFilter("System.Windows.Forms")]
    public partial class ControlBoxButton : Control
    {
        #region [ Constructor ]
        public ControlBoxButton()
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
        private FormsConstsDefiner.MouseState m_MouseState;

        private FormsConstsDefiner.ControlBoxButtonTypes m_ControlBoxButtonType = FormsConstsDefiner.ControlBoxButtonTypes.Close;
        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All)]
        [DefaultValue(FormsConstsDefiner.ControlBoxButtonTypes.Close)]
        public FormsConstsDefiner.ControlBoxButtonTypes ControlBoxButtonType
        {
            get { return m_ControlBoxButtonType; }
            set
            {
                m_ControlBoxButtonType = value;
                Invalidate();
            }
        }

        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public ColorProperties ColorProperties { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All)]
        [DefaultValue(typeof(Size), "24, 24")]
        public new Size Size { get { return base.Size; } set { base.Size = value; } }
        #endregion

        #region [ Override Events / Events / Methods ]
        private void Initialize()
        {
            try
            {
                // 최소 사이즈
                MinimumSize = new Size(FormsConstsDefiner.FixedSize.ControlBoxButton.Minimum, FormsConstsDefiner.FixedSize.ControlBoxButton.Minimum);
                Size = new Size(FormsConstsDefiner.FixedSize.ControlBoxButton.Default, FormsConstsDefiner.FixedSize.ControlBoxButton.Default);

                ColorProperties = new ColorProperties();
                ColorProperties.PropertyChanged += ColorProperties_PropertyChanged;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ColorProperties_PropertyChanged(object sender, ExtensionsPropertyEventArgs e)
        {
            try
            {
                Invalidate();
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
                base.OnHandleCreated(e);

                Form _OwnerForm = FindForm();
                if (_OwnerForm != null)
                {
                    _OwnerForm.SizeChanged += OwnerForm_SizeChanged;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void OwnerForm_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                Invalidate();
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
                e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                e.Graphics.Clear(BackColor);

                Rectangle _ClientRectangle = ClientRectangle;
                switch (m_ControlBoxButtonType)
                {
                    case FormsConstsDefiner.ControlBoxButtonTypes.Minimize:
                        {
                            Color _ActionBackColor = BackColor;
                            Color _ActionForeColor = ForeColor;
                            switch (m_MouseState)
                            {
                                case FormsConstsDefiner.MouseState.Hover:
                                    {
                                        _ActionBackColor = ColorProperties.BackColors.Hover;
                                        _ActionForeColor = ColorProperties.ForeColors.Hover;
                                    }
                                    break;

                                case FormsConstsDefiner.MouseState.Down:
                                    {
                                        if (ColorProperties.BackColors.IsAlphaColorOnDown)
                                        {
                                            _ActionBackColor = Color.FromArgb(ColorProperties.BackColors.Alpha, ColorProperties.BackColors.Down);
                                        }
                                        else
                                        {
                                            _ActionBackColor = ColorProperties.BackColors.Down;
                                        }
                                        _ActionForeColor = ColorProperties.ForeColors.Down;
                                    }
                                    break;

                                case FormsConstsDefiner.MouseState.None:
                                default:
                                    {
                                        _ActionBackColor = BackColor;
                                        _ActionForeColor = ForeColor;
                                    }
                                    break;
                            }
                            using (SolidBrush _BackColor = new SolidBrush(_ActionBackColor))
                            {
                                e.Graphics.FillRectangle(_BackColor, _ClientRectangle);
                            }

                            float _fMargin = ((_ClientRectangle.Height * 26F) / 100F);
                            float _fRatioOfMargin = ((Width - Height) / 2F);
                            int _iThicknessOfSymbol = (int)(_fMargin / 3F);
                            using (Pen _Symbol = new Pen(_ActionForeColor, _iThicknessOfSymbol))
                            {
                                _Symbol.Alignment = PenAlignment.Center;
                                _Symbol.StartCap = LineCap.Flat;
                                _Symbol.EndCap = LineCap.Flat;

                                RectangleF _SymbolBounds = new RectangleF((_ClientRectangle.X + (_fMargin + _fRatioOfMargin)), (_ClientRectangle.Y + _fMargin), (_ClientRectangle.Width - (_fMargin + _fRatioOfMargin)), (_ClientRectangle.Height - _fMargin));
                                PointF[] _PointsOfSymbolBounds = new PointF[2]
                                {
                                    new PointF(_SymbolBounds.X, _SymbolBounds.Height),
                                    new PointF(_SymbolBounds.Width, _SymbolBounds.Height)
                                };
                                e.Graphics.DrawLines(_Symbol, _PointsOfSymbolBounds);
                            }
                        }
                        break;

                    case FormsConstsDefiner.ControlBoxButtonTypes.Maximum:
                        {
                            Color _ActionBackColor = BackColor;
                            Color _ActionForeColor = ForeColor;
                            switch (m_MouseState)
                            {
                                case FormsConstsDefiner.MouseState.Hover:
                                    {
                                        _ActionBackColor = ColorProperties.BackColors.Hover;
                                        _ActionForeColor = ColorProperties.ForeColors.Hover;
                                    }
                                    break;

                                case FormsConstsDefiner.MouseState.Down:
                                    {
                                        if (ColorProperties.BackColors.IsAlphaColorOnDown)
                                        {
                                            _ActionBackColor = Color.FromArgb(ColorProperties.BackColors.Alpha, ColorProperties.BackColors.Down);
                                        }
                                        else
                                        {
                                            _ActionBackColor = ColorProperties.BackColors.Down;
                                        }
                                        _ActionForeColor = ColorProperties.ForeColors.Down;
                                    }
                                    break;

                                case FormsConstsDefiner.MouseState.None:
                                default:
                                    {
                                        _ActionBackColor = BackColor;
                                        _ActionForeColor = ForeColor;
                                    }
                                    break;
                            }
                            using (SolidBrush _BackColor = new SolidBrush(_ActionBackColor))
                            {
                                e.Graphics.FillRectangle(_BackColor, _ClientRectangle);
                            }

                            float _fMargin = ((_ClientRectangle.Height * 26F) / 100F);
                            float _fRatioOfMargin = ((Width - Height) / 2F);
                            int _iThicknessOfSymbol  = (int)(_fMargin / 3F);
                            using (Pen _Symbol = new Pen(_ActionForeColor, _iThicknessOfSymbol))
                            {
                                _Symbol.Alignment = PenAlignment.Center;
                                _Symbol.StartCap = LineCap.Flat;
                                _Symbol.EndCap = LineCap.Flat;

                                RectangleF _SymbolBounds = new RectangleF((_ClientRectangle.X + (_fMargin + _fRatioOfMargin)), (_ClientRectangle.Y + _fMargin), (_ClientRectangle.Width - ((_fMargin * 2 ) + _fRatioOfMargin)), (_ClientRectangle.Height - (_fMargin * 2)));
                                Form _OwnerForm = FindForm();
                                if (_OwnerForm != null)
                                {
                                    if (_OwnerForm.WindowState.Equals(FormWindowState.Maximized))
                                    {
                                        float _fOverlapMargin = ((_SymbolBounds.Width * 30F) / 100F);
                                        RectangleF _OverlapSymbolBounds = new RectangleF((_SymbolBounds.X + _fOverlapMargin), _SymbolBounds.Y, (_SymbolBounds.Width - _fOverlapMargin), (_SymbolBounds.Height - _fOverlapMargin));
                                        PointF[] _PointsOfOverlapSymbolBounds = new PointF[8]
                                        {
                                            new PointF(_OverlapSymbolBounds.X, (_OverlapSymbolBounds.Y + _fOverlapMargin)),
                                            new PointF(_OverlapSymbolBounds.X, _OverlapSymbolBounds.Y),
                                            new PointF(_OverlapSymbolBounds.X, _OverlapSymbolBounds.Y),
                                            new PointF((_OverlapSymbolBounds.X + _OverlapSymbolBounds.Width), _OverlapSymbolBounds.Y),
                                            new PointF((_OverlapSymbolBounds.X + _OverlapSymbolBounds.Width), _OverlapSymbolBounds.Y),
                                            new PointF((_OverlapSymbolBounds.X + _OverlapSymbolBounds.Width), (_OverlapSymbolBounds.Y + _OverlapSymbolBounds.Height)),
                                            new PointF((_OverlapSymbolBounds.X + _OverlapSymbolBounds.Width), _OverlapSymbolBounds.Y + _OverlapSymbolBounds.Height),
                                            new PointF(((_OverlapSymbolBounds.X + _OverlapSymbolBounds.Width) - _fOverlapMargin), (_OverlapSymbolBounds.Y + _OverlapSymbolBounds.Height))
                                        };
                                        e.Graphics.DrawLines(_Symbol, _PointsOfOverlapSymbolBounds);
                                        _OverlapSymbolBounds = new RectangleF(_SymbolBounds.X, (_SymbolBounds.Y + _fOverlapMargin), (_SymbolBounds.Width - _fOverlapMargin), (_SymbolBounds.Height - _fOverlapMargin));
                                        _PointsOfOverlapSymbolBounds = new PointF[6]
                                        {
                                            new PointF(_OverlapSymbolBounds.X, _OverlapSymbolBounds.Y),
                                            new PointF((_OverlapSymbolBounds.X + _OverlapSymbolBounds.Width), _OverlapSymbolBounds.Y),
                                            new PointF((_OverlapSymbolBounds.X + _OverlapSymbolBounds.Width), _OverlapSymbolBounds.Y),
                                            new PointF((_OverlapSymbolBounds.X + _OverlapSymbolBounds.Width), (_OverlapSymbolBounds.Y + _OverlapSymbolBounds.Height)),
                                            new PointF((_OverlapSymbolBounds.X + _OverlapSymbolBounds.Width), (_OverlapSymbolBounds.Y + _OverlapSymbolBounds.Height)),
                                            new PointF(_OverlapSymbolBounds.X, (_OverlapSymbolBounds.Y + _OverlapSymbolBounds.Height))
                                        };
                                        e.Graphics.DrawPolygon(_Symbol, _PointsOfOverlapSymbolBounds);
                                    }
                                    else if (_OwnerForm.WindowState.Equals(FormWindowState.Normal))
                                    {
                                        PointF[] _PointFs = new PointF[6]
                                        {
                                            new PointF(_SymbolBounds.X, _SymbolBounds.Y),
                                            new PointF((_SymbolBounds.X + _SymbolBounds.Width), _SymbolBounds.Y),
                                            new PointF((_SymbolBounds.X + _SymbolBounds.Width), _SymbolBounds.Y),
                                            new PointF((_SymbolBounds.X + _SymbolBounds.Width), (_SymbolBounds.Y + _SymbolBounds.Height)),
                                            new PointF((_SymbolBounds.X + _SymbolBounds.Width), (_SymbolBounds.Y + _SymbolBounds.Height)),
                                            new PointF(_SymbolBounds.X, (_SymbolBounds.Y + _SymbolBounds.Height)),
                                        };
                                        e.Graphics.DrawPolygon(_Symbol, _PointFs);
                                    }
                                }
                                else
                                {
                                    PointF[] _PointsOfSymbolBounds = new PointF[6]
                                    {
                                        new PointF(_SymbolBounds.X, _SymbolBounds.Y),
                                        new PointF((_SymbolBounds.X + _SymbolBounds.Width), _SymbolBounds.Y),
                                        new PointF((_SymbolBounds.X + _SymbolBounds.Width), _SymbolBounds.Y),
                                        new PointF((_SymbolBounds.X + _SymbolBounds.Width), (_SymbolBounds.Y + _SymbolBounds.Height)),
                                        new PointF((_SymbolBounds.X + _SymbolBounds.Width), (_SymbolBounds.Y + _SymbolBounds.Height)),
                                        new PointF(_SymbolBounds.X, (_SymbolBounds.Y + _SymbolBounds.Height)),
                                    };
                                    e.Graphics.DrawPolygon(_Symbol, _PointsOfSymbolBounds);
                                }
                            }
                        }
                        break;

                    case FormsConstsDefiner.ControlBoxButtonTypes.Close:
                    default:
                        {
                            Color _ActionBackColor = BackColor;
                            Color _ActionForeColor = ForeColor;
                            switch (m_MouseState)
                            {
                                case FormsConstsDefiner.MouseState.Hover:
                                    {
                                        _ActionBackColor = ColorProperties.BackColors.Hover;
                                        _ActionForeColor = ColorProperties.ForeColors.Hover;
                                    }
                                    break;

                                case FormsConstsDefiner.MouseState.Down:
                                    {
                                        if (ColorProperties.BackColors.IsAlphaColorOnDown)
                                        {
                                            _ActionBackColor = Color.FromArgb(ColorProperties.BackColors.Alpha, ColorProperties.BackColors.Down);
                                        }
                                        else
                                        {
                                            _ActionBackColor = ColorProperties.BackColors.Down;
                                        }
                                        _ActionForeColor = ColorProperties.ForeColors.Down;
                                    }
                                    break;

                                case FormsConstsDefiner.MouseState.None:
                                default:
                                    {
                                        _ActionBackColor = BackColor;
                                        _ActionForeColor = ForeColor;
                                    }
                                    break;
                            }
                            using (SolidBrush _BackColor = new SolidBrush(_ActionBackColor))
                            {
                                e.Graphics.FillRectangle(_BackColor, _ClientRectangle);
                            }

                            float _fMargin = ((_ClientRectangle.Height * 26F) / 100F);
                            float _fRatioOfMargin = ((Width - Height) / 2F);
                            int _iThicknessOfSymbol = (int)(_fMargin / 3F);
                            using (Pen _Symbol = new Pen(_ActionForeColor, _iThicknessOfSymbol))
                            {
                                _Symbol.Alignment = PenAlignment.Center;
                                _Symbol.StartCap = LineCap.Triangle;
                                _Symbol.EndCap = LineCap.Triangle;

                                RectangleF _SymbolBounds = new RectangleF((_ClientRectangle.X + (_fMargin + _fRatioOfMargin)), (_ClientRectangle.Y + _fMargin), (_ClientRectangle.Width - (_fMargin + _fRatioOfMargin)), (_ClientRectangle.Height - _fMargin));
                                PointF[] _PointsOfSymbolBounds = new PointF[2]
                                {
                                    new PointF(_SymbolBounds.X, _SymbolBounds.Y),
                                    new PointF(_SymbolBounds.Width, _SymbolBounds.Height)
                                };
                                e.Graphics.DrawLines(_Symbol, _PointsOfSymbolBounds);
                                _PointsOfSymbolBounds = new PointF[2]
                                {
                                    new PointF(_SymbolBounds.Width, _SymbolBounds.Y),
                                    new PointF(_SymbolBounds.X, _SymbolBounds.Height)
                                };
                                e.Graphics.DrawLines(_Symbol, _PointsOfSymbolBounds);
                            }
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

        protected override void OnSizeChanged(EventArgs e)
        {
            try
            {
                if (!Width.Equals(Height))
                {
                    Width = Height;
                }
                else
                {
                    base.OnSizeChanged(e);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnMouseEnter(EventArgs eventargs)
        {
            try
            {
                base.OnMouseEnter(eventargs);

                m_MouseState = FormsConstsDefiner.MouseState.Hover;
                Invalidate();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnMouseLeave(EventArgs eventargs)
        {
            try
            {
                base.OnMouseLeave(eventargs);

                m_MouseState = FormsConstsDefiner.MouseState.None;
                Invalidate();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            try
            {
                base.OnMouseDown(e);

                m_MouseState = FormsConstsDefiner.MouseState.Down;
                Invalidate();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            try
            {
                base.OnMouseUp(e);

                m_MouseState = FormsConstsDefiner.MouseState.Hover;
                Invalidate();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnClick(EventArgs e)
        {
            try
            {
                base.OnClick(e);

                Form _OwnerForm = FindForm();
                if (_OwnerForm != null)
                {
                    switch (m_ControlBoxButtonType)
                    {
                        case FormsConstsDefiner.ControlBoxButtonTypes.Minimize:
                            {
                                _OwnerForm.WindowState = FormWindowState.Minimized;
                                Invalidate();
                            }
                            break;

                        case FormsConstsDefiner.ControlBoxButtonTypes.Maximum:
                            {
                                switch (_OwnerForm.WindowState)
                                {
                                    case FormWindowState.Normal:
                                        {
                                            _OwnerForm.WindowState = FormWindowState.Maximized;
                                        }
                                        break;

                                    case FormWindowState.Maximized:
                                    default:
                                        {
                                            _OwnerForm.WindowState = FormWindowState.Normal;
                                        }
                                        break;
                                }
                                Invalidate();
                            }
                            break;

                        case FormsConstsDefiner.ControlBoxButtonTypes.Close:
                        default:
                            {
                                if (_OwnerForm != null)
                                {
                                    _OwnerForm.Close();
                                }
                                else
                                {
                                    Application.DoEvents();
                                    Application.Exit();
                                }
                            }
                            break;
                    }
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

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ColorProperties
    {
        public ColorProperties()
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
    public class BackColorProperties
    {
        public BackColorProperties()
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

        private Color m_HoverColor = SystemColors.ControlLight;
        [DefaultValue(typeof(Color), "ControlLight")]
        public Color Hover
        {
            get { return m_HoverColor; }
            set
            {
                m_HoverColor = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.ControlBoxButton.ColorProperties.BackColorHover.ToString());
            }
        }

        private Color m_DownColor = SystemColors.ControlDark;
        [DefaultValue(typeof(Color), "ControlDark")]
        public Color Down
        {
            get { return m_DownColor; }
            set
            {
                m_DownColor = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.ControlBoxButton.ColorProperties.BackColorDown.ToString());
            }
        }

        private bool m_IsAlphaColorOnDown = true;
        [DefaultValue(true)]
        public bool IsAlphaColorOnDown
        {
            get { return m_IsAlphaColorOnDown; }
            set
            {
                m_IsAlphaColorOnDown = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.ControlBoxButton.ColorProperties.IsAlphaColorOnDown.ToString());
            }
        }

        private int m_iAlpha = 90;
        [DefaultValue(90)]
        public int Alpha
        {
            get { return m_iAlpha; }
            set
            {
                m_iAlpha = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.ControlBoxButton.ColorProperties.Alpha.ToString());
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
    public class ForeColorProperties
    {
        public ForeColorProperties()
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

        private Color m_HoverColor = Color.FromArgb(64, 64, 64);
        [DefaultValue(typeof(Color), "64, 64, 64")]
        public Color Hover
        {
            get { return m_HoverColor; }
            set
            {
                m_HoverColor = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.ControlBoxButton.ColorProperties.ForeColorHover.ToString());
            }
        }

        private Color m_DownColor = Color.FromArgb(64, 64, 64);
        [DefaultValue(typeof(Color), "64, 64, 64")]
        public Color Down
        {
            get { return m_DownColor; }
            set
            {
                m_DownColor = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.ControlBoxButton.ColorProperties.ForeColorDown.ToString());
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
