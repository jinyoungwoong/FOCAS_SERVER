using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NCManagementSystem.Libraries.Controls.TextBox
{
    [ToolboxBitmap(typeof(System.Windows.Forms.TextBox)), ToolboxItemFilter("System.Windows.Forms")]
    public partial class FwTextBox : System.Windows.Forms.TextBox
    {
        #region [ Constructor ]
        public FwTextBox()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            UpdateStyles();
            DoubleBuffered = true;

            InitializeComponent();

            Initialize();
        }
        #endregion

        #region [ Member Variables / Fields / Properties ]
        private ErrorProvider m_ErrorProvider;
        private string m_sErrorMessage = string.Empty;
        private bool m_IsCustomError = false;
        private string m_sCustomErrorMessage = string.Empty;
        private bool m_IsFocusedOnWaterMark = true;

        private TextBoxConstsDefiner.TextBoxStyles m_TextBoxStyle = TextBoxConstsDefiner.TextBoxStyles.Default;
        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All)]
        [DefaultValue(TextBoxConstsDefiner.TextBoxStyles.Default)]
        public TextBoxConstsDefiner.TextBoxStyles TextBoxStyle
        {
            get { return m_TextBoxStyle; }
            set
            {
                m_TextBoxStyle = value;
                switch (m_TextBoxStyle)
                {
                    case TextBoxConstsDefiner.TextBoxStyles.Numeric:
                    case TextBoxConstsDefiner.TextBoxStyles.Decimal:
                    case TextBoxConstsDefiner.TextBoxStyles.Alphabet:
                    case TextBoxConstsDefiner.TextBoxStyles.ExceptHangul:
                    case TextBoxConstsDefiner.TextBoxStyles.EMail:
                        {
                            ImeMode = ImeMode.Disable;
                            if (m_TextBoxStyle.Equals(TextBoxConstsDefiner.TextBoxStyles.Decimal))
                            {
                                DecimalStyleProperties.PropertyChanged += DecimalStyleProperties_PropertyChanged;
                            }
                            else
                            {
                                DecimalStyleProperties.PropertyChanged -= DecimalStyleProperties_PropertyChanged;
                            }
                        }
                        break;

                    case TextBoxConstsDefiner.TextBoxStyles.Default:
                    default:
                        {
                            ImeMode = ImeMode.NoControl;
                            DecimalStyleProperties.PropertyChanged -= DecimalStyleProperties_PropertyChanged;
                        }
                        break;
                }
                Invalidate();
            }
        }

        private bool m_IsErrorProvider = false;
        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All)]
        [DefaultValue(false)]
        public bool IsErrorProvider
        {
            get { return m_IsErrorProvider; }
            set
            {
                m_IsErrorProvider = value;
                if (m_IsErrorProvider)
                {
                    m_ErrorProvider = new ErrorProvider()
                    {
                        BlinkStyle = ErrorProviderProperties.ErrorBlinkStyle
                    };
                    m_ErrorProvider.SetIconAlignment(this, ErrorProviderProperties.ErrorIconAlignment);
                    m_ErrorProvider.SetIconPadding(this, ErrorProviderProperties.Margin);
                    ErrorProviderProperties.PropertyChanged += ErrorProviderProperties_PropertyChanged;
                }
                else
                {
                    m_ErrorProvider = null;
                    ErrorProviderProperties.PropertyChanged -= ErrorProviderProperties_PropertyChanged;
                }
                Invalidate();
            }
        }

        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public ErrorProviderProperties ErrorProviderProperties { get; set; }

        private bool m_IsWaterMark = false;
        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All)]
        [DefaultValue(false)]
        public bool IsWaterMark
        {
            get { return m_IsWaterMark; }
            set
            {
                m_IsWaterMark = value;
                if (m_IsWaterMark)
                {
                    WaterMarkProperties.PropertyChanged += WaterMarkProperties_PropertyChanged;
                }
                else
                {
                    WaterMarkProperties.PropertyChanged -= WaterMarkProperties_PropertyChanged;
                }
                Invalidate();
            }
        }

        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public WaterMarkProperties WaterMarkProperties { get; set; }

        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public DecimalStyleProperties DecimalStyleProperties { get; set; }

        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All)]
        [DefaultValue(false)]
        public bool IsValidateEmpty { get; set; } = false;

        private int m_iMinLength = 0;
        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All)]
        [DefaultValue(0)]
        public int MinLength
        {
            get { return m_iMinLength; }
            set
            {
                if (value > MaxLength)
                {
                    return;
                }

                m_iMinLength = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All)]
        public new int MaxLength
        {
            get { return base.MaxLength; }
            set
            {
                if (value < MinLength)
                {
                    return;
                }

                base.MaxLength = value;
            }
        }
        #endregion

        #region [ Override Events / Events / Methods ]
        private void Initialize()
        {
            try
            {
                ErrorProviderProperties = new ErrorProviderProperties();
                WaterMarkProperties = new WaterMarkProperties();
                DecimalStyleProperties = new DecimalStyleProperties();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ErrorProviderProperties_PropertyChanged(object sender, ExtensionsPropertyEventArgs e)
        {
            try
            {
                string _sPropertyName = e.PropertyName;
                if (Enum.TryParse(_sPropertyName, out TextBoxConstsDefiner.ExtensionsPropertyNames.ErrorProviderProperties _PropertyName))
                {
                    switch (_PropertyName)
                    {
                        case TextBoxConstsDefiner.ExtensionsPropertyNames.ErrorProviderProperties.ErrorBlinkStyle:
                            {
                                if (m_ErrorProvider != null)
                                {
                                    m_ErrorProvider.BlinkStyle = ErrorProviderProperties.ErrorBlinkStyle;
                                }
                            }
                            break;

                        case TextBoxConstsDefiner.ExtensionsPropertyNames.ErrorProviderProperties.ErrorIconAlignment:
                            {
                                if (m_ErrorProvider != null)
                                {
                                    m_ErrorProvider.SetIconAlignment(this, ErrorProviderProperties.ErrorIconAlignment);
                                }
                            }
                            break;

                        case TextBoxConstsDefiner.ExtensionsPropertyNames.ErrorProviderProperties.ErrorProviderMargin:
                            {
                                if (m_ErrorProvider != null)
                                {
                                    m_ErrorProvider.SetIconPadding(this, ErrorProviderProperties.Margin);
                                }
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void WaterMarkProperties_PropertyChanged(object sender, ExtensionsPropertyEventArgs e)
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

        private void DecimalStyleProperties_PropertyChanged(object sender, ExtensionsPropertyEventArgs e)
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

        protected override void OnLeave(EventArgs e)
        {
            try
            {
                base.OnLeave(e);


                if (m_IsCustomError)
                {
                    return;
                }

                bool _IsHasError = false;
                int _iTextLength = Encoding.Default.GetByteCount(Text.Trim());
                switch (TextBoxStyle)
                {
                    case TextBoxConstsDefiner.TextBoxStyles.Numeric:
                        {
                            if (_iTextLength > 0)
                            {
                                if (!Regex.IsMatch(Text, "^[+-]?\\d+$"))
                                {
                                    _IsHasError = true;
                                    m_sErrorMessage = TextBoxConstsDefiner.MessageSet.VALID_NUMBER;
                                }

                                if (!_IsHasError && _iTextLength < MinLength)
                                {
                                    _IsHasError = true;
                                    m_sErrorMessage = TextBoxConstsDefiner.MessageSet.INVALID_LESS_MININUM;
                                }
                            }
                        }
                        break;

                    case TextBoxConstsDefiner.TextBoxStyles.Decimal:
                        {
                            if (_iTextLength > 0)
                            {
                                bool _IsContainsDot = Text.Contains(".");
                                if (_IsContainsDot)
                                {
                                    int _iDotPosition = Text.IndexOf('.');
                                    string[] _sSplitValues = Text.Split('.');
                                    if (DecimalStyleProperties.DotPostLength > 0 && _sSplitValues[1].Length > DecimalStyleProperties.DotPostLength)
                                    {
                                        _IsHasError = true;
                                        m_sErrorMessage = TextBoxConstsDefiner.MessageSet.INVALID_DOT_POST_EXCEEDED;
                                    }
                                }

                                if (decimal.TryParse(Text, out decimal _mConvertValue))
                                {
                                    if (DecimalStyleProperties.IsFloatingPoint)
                                    {
                                        if (DecimalStyleProperties.IsCommaSeparator)
                                        {
                                            if (_mConvertValue.Equals(0))
                                            {
                                                string _sFormat = string.Concat("0.");
                                                _sFormat = _sFormat.PadRight((2 + DecimalStyleProperties.DotPostLength), '0');
                                                Text = _mConvertValue.ToString(_sFormat);
                                            }
                                            else
                                            {
                                                string _sFormat = string.Concat("#,0.");
                                                _sFormat = _sFormat.PadRight((4 + DecimalStyleProperties.DotPostLength), '0');
                                                Text = _mConvertValue.ToString(_sFormat);
                                            }
                                        }
                                        else
                                        {
                                            string _sFormat = string.Concat("0.");
                                            _sFormat = _sFormat.PadRight((2 + DecimalStyleProperties.DotPostLength), '0');
                                            Text = _mConvertValue.ToString(_sFormat);
                                        }
                                    }
                                    else
                                    {
                                        if (_mConvertValue.Equals(0))
                                        {
                                            Text = _mConvertValue.ToString();
                                        }
                                        else
                                        {
                                            if (DecimalStyleProperties.IsCommaSeparator)
                                            {
                                                string _sFormat = string.Concat("#,0.");
                                                _sFormat = _sFormat.PadRight((4 + DecimalStyleProperties.DotPostLength), '#');
                                                Text = _mConvertValue.ToString(_sFormat);
                                            }
                                            else
                                            {
                                                string _sFormat = string.Concat("0.");
                                                _sFormat = _sFormat.PadRight((2 + DecimalStyleProperties.DotPostLength), '#');
                                                Text = _mConvertValue.ToString(_sFormat);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    _IsHasError = true;
                                    m_sErrorMessage = TextBoxConstsDefiner.MessageSet.VALID_DECIMAL;
                                }

                                if (!Regex.IsMatch(Text, "^\\$?[+-]?[\\d,]*(\\.\\d*)?$"))
                                {
                                    _IsHasError = true;
                                    m_sErrorMessage = TextBoxConstsDefiner.MessageSet.VALID_DECIMAL;
                                }

                                if (!_IsHasError && _iTextLength < MinLength)
                                {
                                    _IsHasError = true;
                                    m_sErrorMessage = TextBoxConstsDefiner.MessageSet.INVALID_LESS_MININUM;
                                }
                            }
                        }
                        break;

                    case TextBoxConstsDefiner.TextBoxStyles.Alphabet:
                        {
                            if (_iTextLength > 0)
                            {
                                if (!Regex.IsMatch(Text, "^[a-zA-Z]*$"))
                                {
                                    _IsHasError = true;
                                    m_sErrorMessage = TextBoxConstsDefiner.MessageSet.VALID_ALPHABET;
                                }

                                if (!_IsHasError && _iTextLength < MinLength)
                                {
                                    _IsHasError = true;
                                    m_sErrorMessage = TextBoxConstsDefiner.MessageSet.INVALID_LESS_MININUM;
                                }
                            }
                        }
                        break;

                    case TextBoxConstsDefiner.TextBoxStyles.ExceptHangul:
                        {
                            if (_iTextLength > 0)
                            {
                                if (!Regex.IsMatch(Text, "^[a-zA-Z0-9]*$"))
                                {
                                    _IsHasError = true;
                                    m_sErrorMessage = TextBoxConstsDefiner.MessageSet.VALID_NUMBER_AND_ALPHABET;
                                }

                                if (!_IsHasError && _iTextLength < MinLength)
                                {
                                    _IsHasError = true;
                                    m_sErrorMessage = TextBoxConstsDefiner.MessageSet.INVALID_LESS_MININUM;
                                }
                            }
                        }
                        break;

                    case TextBoxConstsDefiner.TextBoxStyles.EMail:
                        {
                            Regex _regexPattern = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
                            if (Text.Length > 0 && !_regexPattern.IsMatch(Text))
                            {
                                _IsHasError = true;
                                m_sErrorMessage = TextBoxConstsDefiner.MessageSet.VALID_EMAIL;
                            }
                        }
                        break;

                    case TextBoxConstsDefiner.TextBoxStyles.Default:
                    default:
                        {
                            if (_iTextLength > 0 && _iTextLength < MinLength)
                            {
                                _IsHasError = true;
                                m_sErrorMessage = TextBoxConstsDefiner.MessageSet.INVALID_LESS_MININUM;
                            }
                        }
                        break;
                }
                if (_IsHasError)
                {
                    if (m_ErrorProvider != null && IsErrorProvider)
                    {
                        m_ErrorProvider.SetError(this, m_sErrorMessage);
                    }
                    Focus();
                }
                else
                {
                    if (!string.IsNullOrEmpty(m_sErrorMessage))
                    {
                        m_sErrorMessage = string.Empty;
                        if (m_ErrorProvider != null && IsErrorProvider)
                        {
                            m_ErrorProvider.SetError(this, string.Empty);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            try
            {
                if ((e.KeyChar.Equals((char)3)) || (e.KeyChar.Equals((char)22)) || (e.KeyChar.Equals((char)24)) || (e.KeyChar.Equals((char)26)) || (e.KeyChar.Equals((char)1)))
                {
                    e.Handled = false;
                    return;
                }

                int _iTextLength = Encoding.Default.GetByteCount(Text);
                int _iKeyCharLength = Encoding.Default.GetByteCount(e.KeyChar.ToString());
                int _iCursorPosition = SelectionStart;
                switch (TextBoxStyle)
                {
                    case TextBoxConstsDefiner.TextBoxStyles.Numeric:
                        {
                            if (!e.KeyChar.Equals((char)Keys.Back) && !e.KeyChar.Equals(Convert.ToChar(Keys.Enter)))
                            {
                                if (((_iKeyCharLength + _iTextLength) > MaxLength) && SelectionLength <= 0)
                                {
                                    m_sErrorMessage = TextBoxConstsDefiner.MessageSet.INVALID_EXCEEDED_MAXIMUM;
                                    e.Handled = true;
                                }

                                if (!e.Handled && !char.IsDigit(e.KeyChar) && !e.KeyChar.Equals('-'))
                                {
                                    m_sErrorMessage = TextBoxConstsDefiner.MessageSet.VALID_NUMBER;
                                    e.Handled = true;
                                }

                                if (!e.Handled && e.KeyChar.Equals('-') && _iTextLength > 0 && _iCursorPosition != 0)
                                {
                                    m_sErrorMessage = TextBoxConstsDefiner.MessageSet.INVALID_MINUS_POSITION;
                                    e.Handled = true;
                                }
                            }
                        }
                        break;

                    case TextBoxConstsDefiner.TextBoxStyles.Decimal:
                        {
                            if (!e.KeyChar.Equals((char)Keys.Back) && !e.KeyChar.Equals(Convert.ToChar(Keys.Enter)))
                            {
                                if ((_iKeyCharLength + _iTextLength) > MaxLength && SelectionLength <= 0)
                                {
                                    m_sErrorMessage = TextBoxConstsDefiner.MessageSet.INVALID_EXCEEDED_MAXIMUM;
                                    e.Handled = true;
                                }

                                if (!e.Handled && !char.IsDigit(e.KeyChar) && !e.KeyChar.Equals('.') && !e.KeyChar.Equals('-'))
                                {
                                    m_sErrorMessage = TextBoxConstsDefiner.MessageSet.VALID_NUMBER_AND_DOT;
                                    e.Handled = true;
                                }

                                if (!e.Handled && e.KeyChar.Equals('-') && _iTextLength > 0 && _iCursorPosition != 0)
                                {
                                    m_sErrorMessage = TextBoxConstsDefiner.MessageSet.INVALID_MINUS_POSITION;
                                    e.Handled = true;
                                }

                                if (!e.Handled && e.KeyChar.Equals('.'))
                                {
                                    int _iIndexOfDot = Text.IndexOf('.', 0);
                                    if (_iCursorPosition.Equals(0))
                                    {
                                        m_sErrorMessage = TextBoxConstsDefiner.MessageSet.INVALID_DOT_POSITION;
                                        e.Handled = true;
                                    }
                                    else if (_iTextLength <= 0)
                                    {
                                        m_sErrorMessage = TextBoxConstsDefiner.MessageSet.INVALID_DOT_POSITION;
                                        e.Handled = true;
                                    }
                                    else if (_iIndexOfDot > 0)
                                    {
                                        m_sErrorMessage = TextBoxConstsDefiner.MessageSet.INVALID_DOT_COUNT;
                                        e.Handled = true;
                                    }
                                    else if (Text.Substring(_iCursorPosition - 1).Equals("-"))
                                    {
                                        m_sErrorMessage = TextBoxConstsDefiner.MessageSet.VALID_NUMBER;
                                        e.Handled = true;
                                    }
                                    else if (DecimalStyleProperties.DotPostLength > 0 && ((_iTextLength - _iCursorPosition) > DecimalStyleProperties.DotPostLength))
                                    {
                                        m_sErrorMessage = TextBoxConstsDefiner.MessageSet.INVALID_DOT_POST_EXCEEDED;
                                        e.Handled = true;
                                    }
                                }

                                if (DecimalStyleProperties.DotPostLength > 0)
                                {
                                    bool _IsContainsDot = Text.Contains(".") && !SelectedText.Contains(".");
                                    if (_IsContainsDot)
                                    {
                                        int _iDotPosition = Text.IndexOf('.');
                                        string[] _sSplitValues = Text.Split('.');
                                        if (_iDotPosition < _iCursorPosition && _sSplitValues[1].Length >= DecimalStyleProperties.DotPostLength)
                                        {
                                            m_sErrorMessage = TextBoxConstsDefiner.MessageSet.INVALID_DOT_POST_EXCEEDED;
                                            e.Handled = true;
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    case TextBoxConstsDefiner.TextBoxStyles.Alphabet:
                        {
                            if (!e.KeyChar.Equals((char)Keys.Back) && !e.KeyChar.Equals(Convert.ToChar(Keys.Enter)))
                            {
                                if (((_iKeyCharLength + _iTextLength) > MaxLength) && SelectionLength <= 0)
                                {
                                    m_sErrorMessage = TextBoxConstsDefiner.MessageSet.INVALID_EXCEEDED_MAXIMUM;
                                    e.Handled = true;
                                }

                                if (!e.Handled && !(char.IsLetter(e.KeyChar)))
                                {
                                    m_sErrorMessage = TextBoxConstsDefiner.MessageSet.VALID_ALPHABET;
                                    e.Handled = true;
                                }
                            }
                        }
                        break;

                    case TextBoxConstsDefiner.TextBoxStyles.EMail:
                        {
                            MaxLength = 32767;
                            MinLength = 0;
                        }
                        break;

                    case TextBoxConstsDefiner.TextBoxStyles.ExceptHangul:
                        {
                            if (!e.KeyChar.Equals((char)Keys.Back) && !e.KeyChar.Equals(Convert.ToChar(Keys.Enter)))
                            {
                                if (((_iKeyCharLength + _iTextLength) > MaxLength) && SelectionLength <= 0)
                                {
                                    m_sErrorMessage = TextBoxConstsDefiner.MessageSet.INVALID_EXCEEDED_MAXIMUM;
                                    e.Handled = true;
                                }

                                if (!e.Handled && !char.IsLetterOrDigit(e.KeyChar))
                                {
                                    m_sErrorMessage = TextBoxConstsDefiner.MessageSet.VALID_NUMBER_AND_ALPHABET;
                                    e.Handled = true;
                                }
                                else
                                {
                                    if ((0xAC00 <= e.KeyChar && e.KeyChar <= 0xD7A3) || (0x3131 <= e.KeyChar && e.KeyChar <= 0x318E))
                                    {
                                        e.Handled = true;
                                    }
                                }
                            }
                        }
                        break;

                    default:
                        {
                            if (!e.KeyChar.Equals((char)Keys.Back) && !e.KeyChar.Equals(Convert.ToChar(Keys.Enter)) && (_iKeyCharLength + _iTextLength > MaxLength) && SelectionLength <= 0)
                            {
                                m_sErrorMessage = TextBoxConstsDefiner.MessageSet.INVALID_EXCEEDED_MAXIMUM;
                                e.Handled = true;
                            }
                        }
                        break;
                };

                if (e.Handled)
                {
                    if (m_ErrorProvider != null && IsErrorProvider)
                    {
                        m_ErrorProvider.SetError(this, m_sErrorMessage);

                        Invalidate();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(m_sErrorMessage))
                    {
                        m_sErrorMessage = string.Empty;
                        if (m_ErrorProvider != null && IsErrorProvider)
                        {
                            m_ErrorProvider.SetError(this, string.Empty);
                            Invalidate();
                        }
                    }

                    if (m_IsCustomError)
                    {
                        if (m_ErrorProvider != null && IsErrorProvider)
                        {
                            m_ErrorProvider.SetError(this, m_sCustomErrorMessage);
                            Invalidate();
                        }
                    }
                }

                base.OnKeyPress(e);
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
                if (IsWaterMark && m_IsFocusedOnWaterMark && Text.Length.Equals(0))
                {
                    DrawWaterMark(e.Graphics);
                }

                base.OnPaint(e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DrawWaterMark(Graphics g)
        {
            try
            {
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                Rectangle _ClientRectangle = ClientRectangle;
                TextFormatFlags _TextFormatFlags = TextFormatFlags.NoPadding | TextFormatFlags.Top | TextFormatFlags.EndEllipsis;
                int _iOffsetY = ((_ClientRectangle.Size.Height - WaterMarkProperties.Font.Height) / 2);
                switch (WaterMarkProperties.TextAlignment)
                {
                    case HorizontalAlignment.Center:
                        {
                            _TextFormatFlags = _TextFormatFlags | TextFormatFlags.HorizontalCenter;
                            _ClientRectangle.Offset(0, _iOffsetY);
                        }
                        break;

                    case HorizontalAlignment.Left:
                        {
                            _TextFormatFlags = _TextFormatFlags | TextFormatFlags.Left;
                            _ClientRectangle.Offset(1, _iOffsetY);
                        }
                        break;

                    case HorizontalAlignment.Right:
                        {
                            _TextFormatFlags = _TextFormatFlags | TextFormatFlags.Right;
                            _ClientRectangle.Offset(0, _iOffsetY);
                        }
                        break;

                    default:
                        break;
                }
                TextRenderer.DrawText(g, WaterMarkProperties.Text, WaterMarkProperties.Font, _ClientRectangle, WaterMarkProperties.ForeColor, BackColor, _TextFormatFlags);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DrawWaterMark()
        {
            try
            {
                using (Graphics _Graphics = CreateGraphics())
                {
                    DrawWaterMark(_Graphics);
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
                switch (m.Msg)
                {
                    case NativeMethods.WM_SETFOCUS:
                        {
                            m_IsFocusedOnWaterMark = false;
                        }
                        break;

                    case NativeMethods.WM_KILLFOCUS:
                        {
                            m_IsFocusedOnWaterMark = true;
                        }
                        break;
                }

                base.WndProc(ref m);

                if (m.Msg.Equals(NativeMethods.WM_PAINT) && IsWaterMark && m_IsFocusedOnWaterMark && Text.Length.Equals(0) && !GetStyle(ControlStyles.UserPaint))
                {
                    DrawWaterMark();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetCustomError(string error)
        {
            try
            {
                m_IsCustomError = true;
                m_sCustomErrorMessage = error;
                if (m_ErrorProvider != null && IsErrorProvider)
                {
                    m_ErrorProvider.Icon = new ErrorProvider().Icon;
                    m_ErrorProvider.SetError(this, m_sCustomErrorMessage);
                }
                Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ClearCustomError()
        {
            try
            {
                if (m_IsCustomError)
                {
                    m_IsCustomError = false;
                    m_sCustomErrorMessage = string.Empty;
                    if (m_ErrorProvider != null && IsErrorProvider)
                    {
                        m_ErrorProvider.Icon = new ErrorProvider().Icon;
                        m_ErrorProvider.SetError(this, string.Empty);
                    }
                    Focus();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetError()
        {
            try
            {
                if (IsErrorProvider)
                {
                    if (!string.IsNullOrEmpty(m_sErrorMessage))
                    {
                        return m_sErrorMessage;
                    }

                    if (m_IsCustomError && !string.IsNullOrEmpty(m_sCustomErrorMessage))
                    {
                        return m_sCustomErrorMessage;
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool RunValidate(bool isCustomErrorClear = false)
        {
            try
            {
                if (isCustomErrorClear)
                {
                    ClearCustomError();
                }

                m_sErrorMessage = string.Empty;
                if (IsValidateEmpty)
                {
                    if (Text.Length <= 0 || string.IsNullOrEmpty(Text.Trim()))
                    {
                        m_sErrorMessage = TextBoxConstsDefiner.MessageSet.REQUIRED_INPUT;
                        if (m_ErrorProvider != null && IsErrorProvider)
                        {
                            m_ErrorProvider.SetError(this, m_sErrorMessage);
                        }
                        Focus();
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(m_sErrorMessage))
                {
                    Focus();
                    return false;
                }

                if (m_IsCustomError)
                {
                    Focus();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class DecimalStyleProperties
    {
        public DecimalStyleProperties()
        {
        }

        public override string ToString()
        {
            return string.Empty;
        }

        private int m_iDotPostLength = 0;
        [DefaultValue(0)]
        public int DotPostLength
        {
            get { return m_iDotPostLength; }
            set
            {
                m_iDotPostLength = value;
                OnPropertyChanged(TextBoxConstsDefiner.ExtensionsPropertyNames.DecimalStyleProperties.DotPostLength.ToString());
            }
        }

        private bool m_IsFloatingPoint = true;
        [DefaultValue(true)]
        public bool IsFloatingPoint
        {
            get { return m_IsFloatingPoint; }
            set
            {
                m_IsFloatingPoint = value;
                OnPropertyChanged(TextBoxConstsDefiner.ExtensionsPropertyNames.DecimalStyleProperties.IsFloatingPoint.ToString());
            }
        }

        private bool m_IsCommaSeparator = false;
        [DefaultValue(false)]
        public bool IsCommaSeparator
        {
            get { return m_IsCommaSeparator; }
            set
            {
                m_IsCommaSeparator = value;
                OnPropertyChanged(TextBoxConstsDefiner.ExtensionsPropertyNames.DecimalStyleProperties.IsCommaSeparator.ToString());
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
    public class ErrorProviderProperties
    {
        public ErrorProviderProperties()
        {
        }

        public override string ToString()
        {
            return string.Empty;
        }

        private ErrorBlinkStyle m_ErrorBlinkStyle = ErrorBlinkStyle.NeverBlink;
        [DefaultValue(ErrorBlinkStyle.NeverBlink)]
        public ErrorBlinkStyle ErrorBlinkStyle
        {
            get { return m_ErrorBlinkStyle; }
            set
            {
                m_ErrorBlinkStyle = value;
                OnPropertyChanged(TextBoxConstsDefiner.ExtensionsPropertyNames.ErrorProviderProperties.ErrorBlinkStyle.ToString());
            }
        }

        private ErrorIconAlignment m_ErrorProviderIconAlignment = ErrorIconAlignment.MiddleRight;
        [DefaultValue(ErrorIconAlignment.MiddleRight)]
        public ErrorIconAlignment ErrorIconAlignment
        {
            get { return m_ErrorProviderIconAlignment; }
            set
            {
                m_ErrorProviderIconAlignment = value;
                OnPropertyChanged(TextBoxConstsDefiner.ExtensionsPropertyNames.ErrorProviderProperties.ErrorIconAlignment.ToString());
            }
        }

        private int m_iMargin = 0;
        [DefaultValue(0)]
        public int Margin
        {
            get { return m_iMargin; }
            set
            {
                m_iMargin = value;
                OnPropertyChanged(TextBoxConstsDefiner.ExtensionsPropertyNames.ErrorProviderProperties.ErrorProviderMargin.ToString());
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
    public class WaterMarkProperties
    {
        public WaterMarkProperties()
        {
        }

        public override string ToString()
        {
            return string.Empty;
        }

        private Font m_Font = new Font("맑은 고딕", 9F);
        [DefaultValue(typeof(Font), "맑은 고딕, 9pt")]
        public Font Font
        {
            get { return m_Font; }
            set
            {
                m_Font = value;
                OnPropertyChanged(TextBoxConstsDefiner.ExtensionsPropertyNames.WaterMarkProperties.WaterMarkFont.ToString());
            }
        }

        private HorizontalAlignment m_TextAlignment = HorizontalAlignment.Left;
        [DefaultValue(HorizontalAlignment.Left)]
        public HorizontalAlignment TextAlignment
        {
            get { return m_TextAlignment; }
            set
            {
                m_TextAlignment = value;
                OnPropertyChanged(TextBoxConstsDefiner.ExtensionsPropertyNames.WaterMarkProperties.WaterMarkTextAlignment.ToString());
            }
        }

        private string m_sText = string.Empty;
        [DefaultValue(typeof(string), "")]
        public string Text
        {
            get { return m_sText; }
            set
            {
                m_sText = value;
                OnPropertyChanged(TextBoxConstsDefiner.ExtensionsPropertyNames.WaterMarkProperties.WaterMarkText.ToString());
            }
        }

        private Color m_ForeColor = SystemColors.GrayText;
        [DefaultValue(typeof(Color), "GrayText")]
        public Color ForeColor
        {
            get { return m_ForeColor; }
            set
            {
                m_ForeColor = value;
                OnPropertyChanged(TextBoxConstsDefiner.ExtensionsPropertyNames.WaterMarkProperties.WaterMarkForeColor.ToString());
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
