using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using NCManagementSystem.Components.Helpers;

namespace NCManagementSystem.Libraries.Controls.CheckBox
{
    [ToolboxBitmap(typeof(System.Windows.Forms.CheckBox)), ToolboxItemFilter("System.Windows.Forms")]
    public partial class FwCheckBox : System.Windows.Forms.CheckBox
    {
        #region [ Constructor ]
        public FwCheckBox()
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
        private bool m_IsFocused = false;
        private CheckBoxConstsDefiner.MouseState m_MouseState = CheckBoxConstsDefiner.MouseState.None;

        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public CheckSquareProperties CheckSquareProperties { get; set; }

        private int m_iTextIndent = 3;
        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All)]
        [DefaultValue(3)]
        public int TextIndent
        {
            get { return m_iTextIndent; }
            set
            {
                m_iTextIndent = value;
                Invalidate();
            }
        }

        private bool m_IsTextBorderOnFocus = false;
        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All)]
        [DefaultValue(false)]
        public bool IsTextBorderOnFocus
        {
            get { return m_IsTextBorderOnFocus; }
            set
            {
                m_IsTextBorderOnFocus = value;
                Invalidate();
            }
        }

        [DefaultValue(true)]
        public new bool AutoSize
        {
            get { return base.AutoSize; }
            set
            {
                base.AutoSize = value;
                Invalidate();
            }
        }
        #endregion

        #region [ Override Events / Events / Methods ]
        private void Initialize()
        {
            try
            {
                Cursor = Cursors.Hand;

                CheckSquareProperties = new CheckSquareProperties();
                CheckSquareProperties.PropertyChanged += CheckBoxSquareProperties_PropertyChanged;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CheckBoxSquareProperties_PropertyChanged(object sender, ExtensionsPropertyEventArgs e)
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

        protected override void OnPaint(PaintEventArgs pevent)
        {
            try
            {
                if (AutoSize)
                {
                    // AutoSize = 'true' 시 기본 체크박스
                    base.OnPaint(pevent);
                }
                else
                {
                    if (Appearance.Equals(Appearance.Button))
                    {
                        // Appearance = 'Button' 시 기본 체크박스
                        base.OnPaint(pevent);
                    }
                    else
                    {
                        pevent.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                        pevent.Graphics.Clear(BackColor);

                        Rectangle _ClientRectangle = ClientRectangle;

                        int _iPointXOfCheckSquare = CheckSquareProperties.Margin;
                        int _iPointYOfCHeckSquare = CheckSquareProperties.Margin;

                        Size _SizeOfCheckSquare = CheckSquareProperties.Size;
                        if (CheckSquareProperties.IsAutoSize)
                        {
                            _SizeOfCheckSquare = new Size((_ClientRectangle.Height - (CheckSquareProperties.Margin * 2) - 1), ((_ClientRectangle.Height - (CheckSquareProperties.Margin * 2)) - 1));
                        }

                        int _iPointXOfCheckText = (_iPointXOfCheckSquare + _SizeOfCheckSquare.Width + m_iTextIndent);
                        int _iPointYOfCheckText = _ClientRectangle.Y;
                        Size _SizeOfCheckText = new Size((_ClientRectangle.Width - _iPointXOfCheckText), _ClientRectangle.Height);

                        switch (CheckAlign)
                        {
                            case ContentAlignment.TopLeft:
                            case ContentAlignment.MiddleLeft:
                            case ContentAlignment.BottomLeft:
                            default:
                                {
                                    if (CheckSquareProperties.IsAutoSize)
                                    {
                                        _iPointXOfCheckSquare = CheckSquareProperties.Margin;
                                        _iPointYOfCHeckSquare = CheckSquareProperties.Margin;
                                    }
                                    else
                                    {
                                        switch (CheckAlign)
                                        {
                                            case ContentAlignment.TopLeft:
                                                {
                                                    _iPointXOfCheckSquare = CheckSquareProperties.Margin;
                                                    _iPointYOfCHeckSquare = CheckSquareProperties.Margin;
                                                }
                                                break;

                                            case ContentAlignment.MiddleLeft:
                                            default:
                                                {
                                                    _iPointXOfCheckSquare = CheckSquareProperties.Margin;
                                                    _iPointYOfCHeckSquare = ((_ClientRectangle.Height / 2) - (_SizeOfCheckSquare.Height / 2));
                                                }
                                                break;

                                            case ContentAlignment.BottomLeft:
                                                {
                                                    _iPointXOfCheckSquare = CheckSquareProperties.Margin;
                                                    _iPointYOfCHeckSquare = (_ClientRectangle.Height - (_SizeOfCheckSquare.Height + CheckSquareProperties.Margin));
                                                }
                                                break;
                                        }
                                    }
                                    _iPointXOfCheckText = (CheckSquareProperties.Margin + _SizeOfCheckSquare.Width + m_iTextIndent);
                                    _iPointYOfCheckText = _ClientRectangle.Y;
                                    _SizeOfCheckText = new Size((_ClientRectangle.Width - _iPointXOfCheckText), _ClientRectangle.Height);
                                }
                                break;

                            case ContentAlignment.TopCenter:
                            case ContentAlignment.MiddleCenter:
                            case ContentAlignment.BottomCenter:
                                {
                                    if (CheckSquareProperties.IsAutoSize)
                                    {
                                        _iPointXOfCheckSquare = (_ClientRectangle.Width / 2) - (_SizeOfCheckSquare.Width / 2);
                                        _iPointYOfCHeckSquare = CheckSquareProperties.Margin;
                                    }
                                    else
                                    {
                                        switch (CheckAlign)
                                        {
                                            case ContentAlignment.TopCenter:
                                                {
                                                    _iPointXOfCheckSquare = (_ClientRectangle.Width / 2) - (_SizeOfCheckSquare.Width / 2);
                                                    _iPointYOfCHeckSquare = CheckSquareProperties.Margin;
                                                }
                                                break;

                                            case ContentAlignment.MiddleCenter:
                                                {
                                                    _iPointXOfCheckSquare = (_ClientRectangle.Width / 2) - (_SizeOfCheckSquare.Width / 2);
                                                    _iPointYOfCHeckSquare = ((_ClientRectangle.Height / 2) - (_SizeOfCheckSquare.Height / 2));
                                                }
                                                break;

                                            case ContentAlignment.BottomCenter:
                                                {
                                                    _iPointXOfCheckSquare = (_ClientRectangle.Width / 2) - (_SizeOfCheckSquare.Width / 2);
                                                    _iPointYOfCHeckSquare = (_ClientRectangle.Height - (_SizeOfCheckSquare.Height + CheckSquareProperties.Margin));
                                                }
                                                break;
                                        }
                                    }
                                    _iPointXOfCheckText = _ClientRectangle.X;
                                    _iPointYOfCheckText = _ClientRectangle.Y;
                                    _SizeOfCheckText = new Size(_ClientRectangle.Width, _ClientRectangle.Height);
                                }
                                break;

                            case ContentAlignment.TopRight:
                            case ContentAlignment.MiddleRight:
                            case ContentAlignment.BottomRight:
                                {
                                    if (CheckSquareProperties.IsAutoSize)
                                    {
                                        _iPointXOfCheckSquare = (_ClientRectangle.Width - (_SizeOfCheckSquare.Width + CheckSquareProperties.Margin));
                                        _iPointYOfCHeckSquare = (_ClientRectangle.Y + CheckSquareProperties.Margin);
                                    }
                                    else
                                    {
                                        switch (CheckAlign)
                                        {
                                            case ContentAlignment.TopRight:
                                                {
                                                    _iPointXOfCheckSquare = (_ClientRectangle.Width - (_SizeOfCheckSquare.Width + CheckSquareProperties.Margin));
                                                    _iPointYOfCHeckSquare = CheckSquareProperties.Margin;
                                                }
                                                break;

                                            case ContentAlignment.MiddleRight:
                                                {
                                                    _iPointXOfCheckSquare = (_ClientRectangle.Width - (_SizeOfCheckSquare.Width + CheckSquareProperties.Margin));
                                                    _iPointYOfCHeckSquare = ((_ClientRectangle.Height / 2) - (_SizeOfCheckSquare.Height / 2));
                                                }
                                                break;

                                            case ContentAlignment.BottomRight:
                                                {
                                                    _iPointXOfCheckSquare = (_ClientRectangle.Width - (_SizeOfCheckSquare.Width + CheckSquareProperties.Margin));
                                                    _iPointYOfCHeckSquare = (_ClientRectangle.Height - (_SizeOfCheckSquare.Height + CheckSquareProperties.Margin));
                                                }
                                                break;
                                        }
                                    }
                                    _iPointXOfCheckText = _ClientRectangle.X;
                                    _iPointYOfCheckText = _ClientRectangle.Y;
                                    _SizeOfCheckText = new Size((_ClientRectangle.Width - (CheckSquareProperties.Margin + _SizeOfCheckSquare.Width + m_iTextIndent)), _ClientRectangle.Height);
                                }
                                break;
                        }

                        Point _PointOfCheckSquare = new Point(_iPointXOfCheckSquare, _iPointYOfCHeckSquare);
                        Rectangle _CheckSquareBounds = new Rectangle(_PointOfCheckSquare, _SizeOfCheckSquare);
                        pevent.Graphics.FillRectangle(new SolidBrush(CheckSquareProperties.BackColor), _CheckSquareBounds);

                        Point _PointOfCheckText = new Point(_iPointXOfCheckText, _iPointYOfCheckText);
                        Rectangle _CheckTextBounds = new Rectangle(_PointOfCheckText, _SizeOfCheckText);

                        if (Focused && m_IsFocused)
                        {
                            float _fThicknessOfBorder = 1F;
                            RectangleF _BorderBounds = new RectangleF((_CheckTextBounds.X + _fThicknessOfBorder), (_CheckTextBounds.Y + _fThicknessOfBorder), (_CheckTextBounds.Width - (_fThicknessOfBorder * 2F)), (_CheckTextBounds.Height - (_fThicknessOfBorder * 2F)));
                            using (Pen _Border = new Pen(SystemColors.ControlDarkDark, _fThicknessOfBorder))
                            {
                                _Border.DashStyle = DashStyle.Dot;
                                pevent.Graphics.DrawRectangle(_Border, _BorderBounds.X, _BorderBounds.Y, _BorderBounds.Width, _BorderBounds.Height);
                            }
                        }

                        Color _ActionBorderColor = CheckSquareProperties.BorderColors.Normal;
                        Color _ActionCheckMarkColor = CheckSquareProperties.CheckMark.Colors.Checked;
                        if (Enabled)
                        {
                            if (Checked)
                            {
                                switch (m_MouseState)
                                {
                                    case CheckBoxConstsDefiner.MouseState.Hover:
                                        {
                                            _ActionBorderColor = CheckSquareProperties.BorderColors.MouseOver;
                                            _ActionCheckMarkColor = CheckSquareProperties.CheckMark.Colors.MouseOver;
                                        }
                                        break;

                                    case CheckBoxConstsDefiner.MouseState.Down:
                                    case CheckBoxConstsDefiner.MouseState.None:
                                    default:
                                        {
                                            _ActionBorderColor = CheckSquareProperties.BorderColors.Checked;
                                            _ActionCheckMarkColor = CheckSquareProperties.CheckMark.Colors.Checked;
                                        }
                                        break;
                                }

                                int _iMarginOfCheckMark = (int)Math.Floor(((_CheckSquareBounds.Height * 10F) / 100));
                                Point _PointCheckMark = new Point((_CheckSquareBounds.X + _iMarginOfCheckMark), (_CheckSquareBounds.Y + _iMarginOfCheckMark));
                                Size _SizeOfCheckMark = new Size((_CheckSquareBounds.Width - (_iMarginOfCheckMark * 2)), (_CheckSquareBounds.Height - (_iMarginOfCheckMark * 2)));
                                Rectangle _CheckMarkBounds = new Rectangle(_PointCheckMark, _SizeOfCheckMark);
                                FontStyle _FontStyleOfCheckMark = FontStyle.Regular;
                                if (CheckSquareProperties.CheckMark.IsBold)
                                {
                                    _FontStyleOfCheckMark = FontStyle.Bold;
                                }
                                Font _FontOfCheckMark = SizeHelper.AutoSizeFont(pevent.Graphics, new Font("Wingdings", 9, _FontStyleOfCheckMark), _CheckSquareBounds.Height, _CheckSquareBounds.Height, Convert.ToChar(252).ToString());
                                TextRenderer.DrawText(pevent.Graphics, Convert.ToChar(252).ToString(), _FontOfCheckMark, _CheckMarkBounds, _ActionCheckMarkColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                            }
                            else
                            {
                                switch (m_MouseState)
                                {
                                    case CheckBoxConstsDefiner.MouseState.Hover:
                                        {
                                            _ActionBorderColor = CheckSquareProperties.BorderColors.MouseOver;
                                        }
                                        break;

                                    case CheckBoxConstsDefiner.MouseState.Down:
                                    case CheckBoxConstsDefiner.MouseState.None:
                                    default:
                                        {
                                            _ActionBorderColor = CheckSquareProperties.BorderColors.Normal;
                                        }
                                        break;
                                }
                            }
                            pevent.Graphics.DrawRectangle(new Pen(_ActionBorderColor), _CheckSquareBounds);
                            TextRenderer.DrawText(pevent.Graphics, Text, Font, _CheckTextBounds, ForeColor, GetTextFormatFlag());
                        }
                        else
                        {
                            if (Checked)
                            {
                                int _iMarginOfCheckMark = (int)Math.Floor(((_CheckSquareBounds.Height * 10F) / 100));
                                Point _PointOfCheckMark = new Point((_CheckSquareBounds.X + _iMarginOfCheckMark), (_CheckSquareBounds.Y + _iMarginOfCheckMark));
                                Size _SizeOfCheckMark = new Size((_CheckSquareBounds.Width - (_iMarginOfCheckMark * 2)), (_CheckSquareBounds.Height - (_iMarginOfCheckMark * 2)));
                                Rectangle _CheckMarkBounds = new Rectangle(_PointOfCheckMark, _SizeOfCheckMark);
                                FontStyle _FontStyleOfCheckMark = FontStyle.Regular;
                                if (CheckSquareProperties.CheckMark.IsBold)
                                {
                                    _FontStyleOfCheckMark = FontStyle.Bold;
                                }
                                Font _FontOfCheckMark = SizeHelper.AutoSizeFont(pevent.Graphics, new Font("Wingdings", 9, _FontStyleOfCheckMark), _CheckSquareBounds.Height, _CheckSquareBounds.Height, Convert.ToChar(252).ToString());
                                TextRenderer.DrawText(pevent.Graphics, Convert.ToChar(252).ToString(), _FontOfCheckMark, _CheckMarkBounds, CheckSquareProperties.CheckMark.Colors.Disabled, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                            }
                            pevent.Graphics.DrawRectangle(new Pen(CheckSquareProperties.BorderColors.Disabled), _CheckSquareBounds);
                            TextRenderer.DrawText(pevent.Graphics, Text, Font, _CheckTextBounds, CheckSquareProperties.CheckMark.Colors.Disabled, GetTextFormatFlag());
                        }
                    }
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

                if (!AutoSize && !Appearance.Equals(Appearance.Button))
                {
                    m_MouseState = CheckBoxConstsDefiner.MouseState.Hover;
                }
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

                if (!AutoSize && !Appearance.Equals(Appearance.Button))
                {
                    m_MouseState = CheckBoxConstsDefiner.MouseState.None;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnKeyUp(KeyEventArgs kevent)
        {
            try
            {
                base.OnKeyUp(kevent);

                if (!AutoSize && !Appearance.Equals(Appearance.Button) && m_IsTextBorderOnFocus)
                {
                    m_IsFocused = true;
                    Invalidate();
                }
                else
                {
                    m_IsFocused = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TextFormatFlags GetTextFormatFlag()
        {
            try
            {
                TextFormatFlags _TextFormatFlag = TextFormatFlags.Default;
                if (AutoEllipsis)
                {
                    _TextFormatFlag |= TextFormatFlags.EndEllipsis;
                }
                switch (TextAlign)
                {
                    case ContentAlignment.TopLeft:
                        {
                            return _TextFormatFlag | TextFormatFlags.Left | TextFormatFlags.Top;
                        }

                    case ContentAlignment.MiddleLeft:
                        {
                            return _TextFormatFlag | TextFormatFlags.Left | TextFormatFlags.VerticalCenter;
                        }

                    case ContentAlignment.BottomLeft:
                        {
                            return _TextFormatFlag | TextFormatFlags.Left | TextFormatFlags.Bottom;
                        }

                    case ContentAlignment.TopCenter:
                        {
                            return _TextFormatFlag | TextFormatFlags.HorizontalCenter | TextFormatFlags.Top;
                        }

                    case ContentAlignment.MiddleCenter:
                        {
                            return _TextFormatFlag | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
                        }

                    case ContentAlignment.BottomCenter:
                        {
                            return _TextFormatFlag | TextFormatFlags.HorizontalCenter | TextFormatFlags.Bottom;
                        }

                    case ContentAlignment.TopRight:
                        {
                            return _TextFormatFlag | TextFormatFlags.Right | TextFormatFlags.Top;
                        }

                    case ContentAlignment.MiddleRight:
                        {
                            return _TextFormatFlag | TextFormatFlags.Right | TextFormatFlags.VerticalCenter;
                        }

                    case ContentAlignment.BottomRight:
                        {
                            return _TextFormatFlag | TextFormatFlags.Right | TextFormatFlags.Bottom;
                        }

                    default:
                        {
                            return _TextFormatFlag;
                        }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CheckSquareProperties
    {
        public CheckSquareProperties()
        {
            try
            {
                BorderColors = new BorderColorProperties();
                BorderColors.PropertyChanged += OnSubPropertyChanged;

                CheckMark = new CheckMarkProperties();
                CheckMark.PropertyChanged += OnSubPropertyChanged;
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

        private Color m_BackColor = SystemColors.Window;
        [DefaultValue(typeof(Color), "Window")]
        public Color BackColor
        {
            get { return m_BackColor; }
            set
            {
                m_BackColor = value;
                OnPropertyChanged(CheckBoxConstsDefiner.ExtensionsPropertyNames.CheckBoxSquareProperties.CheckBoxSquareBackColor.ToString());
            }
        }

        private bool m_IsAutoSize = false;
        [DefaultValue(false)]
        public bool IsAutoSize
        {
            get { return m_IsAutoSize; }
            set
            {
                m_IsAutoSize = value;
                OnPropertyChanged(CheckBoxConstsDefiner.ExtensionsPropertyNames.CheckBoxSquareProperties.IsCheckBoxSquareAutoSize.ToString());
            }
        }

        private Size m_Size = new Size(20, 20);
        [DefaultValue(typeof(Size), "20, 20")]
        public Size Size
        {
            get { return m_Size; }
            set
            {
                m_Size = value;
                OnPropertyChanged(CheckBoxConstsDefiner.ExtensionsPropertyNames.CheckBoxSquareProperties.CheckBoxSquareSize.ToString());
            }
        }

        private int m_iMargin = 3;
        [DefaultValue(3)]
        public int Margin
        {
            get { return m_iMargin; }
            set
            {
                m_iMargin = value;
                OnPropertyChanged(CheckBoxConstsDefiner.ExtensionsPropertyNames.CheckBoxSquareProperties.CheckBoxSquareMargin.ToString());
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public BorderColorProperties BorderColors { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public CheckMarkProperties CheckMark { get; set; }

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
    public class BorderColorProperties
    {
        public BorderColorProperties()
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

        private Color m_NormalColor = SystemColors.ControlText;
        [DefaultValue(typeof(Color), "ControlText")]
        public Color Normal
        {
            get { return m_NormalColor; }
            set
            {
                m_NormalColor = value;
                OnPropertyChanged(CheckBoxConstsDefiner.ExtensionsPropertyNames.CheckBoxSquareProperties.CheckBoxSquareBorderColorNormal.ToString());
            }
        }

        private Color m_MouseOverColor = SystemColors.Highlight;
        [DefaultValue(typeof(Color), "Highlight")]
        public Color MouseOver
        {
            get { return m_MouseOverColor; }
            set
            {
                m_MouseOverColor = value;
                OnPropertyChanged(CheckBoxConstsDefiner.ExtensionsPropertyNames.CheckBoxSquareProperties.CheckBoxSquareBorderColorMouseOver.ToString());
            }
        }

        private Color m_CheckedColor = SystemColors.ControlText;
        [DefaultValue(typeof(Color), "ControlText")]
        public Color Checked
        {
            get { return m_CheckedColor; }
            set
            {
                m_CheckedColor = value;
                OnPropertyChanged(CheckBoxConstsDefiner.ExtensionsPropertyNames.CheckBoxSquareProperties.CheckBoxSquareBorderColorChecked.ToString());
            }
        }

        private Color m_DisabledColor = SystemColors.ActiveBorder;
        [DefaultValue(typeof(Color), "ActiveBorder")]
        public Color Disabled
        {
            get { return m_DisabledColor; }
            set
            {
                m_DisabledColor = value;
                OnPropertyChanged(CheckBoxConstsDefiner.ExtensionsPropertyNames.CheckBoxSquareProperties.CheckBoxSquareBorderColorDisabled.ToString());
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
    public class CheckMarkProperties
    {
        public CheckMarkProperties()
        {
            try
            {
                Colors = new CheckMarkColorProperties();
                Colors.PropertyChanged += OnSubPropertyChanged;
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

        private bool m_IsBold = false;
        [DefaultValue(false)]
        public bool IsBold
        {
            get { return m_IsBold; }
            set
            {
                m_IsBold = value;
                OnPropertyChanged(CheckBoxConstsDefiner.ExtensionsPropertyNames.CheckBoxSquareProperties.IsCheckMarkBold.ToString());
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public CheckMarkColorProperties Colors { get; set; }

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
    public class CheckMarkColorProperties
    {
        public CheckMarkColorProperties()
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

        private Color m_CheckedColor = SystemColors.ControlText;
        [DefaultValue(typeof(Color), "ControlText")]
        public Color Checked
        {
            get { return m_CheckedColor; }
            set
            {
                m_CheckedColor = value;
                OnPropertyChanged(CheckBoxConstsDefiner.ExtensionsPropertyNames.CheckBoxSquareProperties.CheckMarkColorChecked.ToString());
            }
        }

        private Color m_MouseOverColor = SystemColors.Highlight;
        [DefaultValue(typeof(Color), "Highlight")]
        public Color MouseOver
        {
            get { return m_MouseOverColor; }
            set
            {
                m_MouseOverColor = value;
                OnPropertyChanged(CheckBoxConstsDefiner.ExtensionsPropertyNames.CheckBoxSquareProperties.CheckMarkColorMouseOver.ToString());
            }
        }

        private Color m_DisabledColor = SystemColors.GrayText;
        [DefaultValue(typeof(Color), "GrayText")]
        public Color Disabled
        {
            get { return m_DisabledColor; }
            set
            {
                m_DisabledColor = value;
                OnPropertyChanged(CheckBoxConstsDefiner.ExtensionsPropertyNames.CheckBoxSquareProperties.CheckMarkColorDisabled.ToString());
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
