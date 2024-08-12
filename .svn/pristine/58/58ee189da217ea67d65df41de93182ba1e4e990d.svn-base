using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NCManagementSystem.Libraries.Controls.Label
{
    [ToolboxBitmap(typeof(System.Windows.Forms.Label)), ToolboxItemFilter("System.Windows.Forms")]
    public partial class FwLabel : System.Windows.Forms.Label
    {
        #region [ Constructor ]
        public FwLabel()
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
        private LabelConstsDefiner.LabelStyles m_LabelStyle = LabelConstsDefiner.LabelStyles.Default;
        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All)]
        [DefaultValue(LabelConstsDefiner.LabelStyles.Default)]
        public LabelConstsDefiner.LabelStyles LabelStyle
        {
            get { return m_LabelStyle; }
            set
            {
                m_LabelStyle = value;
                switch (m_LabelStyle)
                {
                    case LabelConstsDefiner.LabelStyles.Gradient:
                        {
                            GradientStyleProperties.PropertyChanged += GradientStyleProperties_PropertyChanged;
                        }
                        break;

                    case LabelConstsDefiner.LabelStyles.Default:
                    default:
                        {
                            GradientStyleProperties.PropertyChanged -= GradientStyleProperties_PropertyChanged;
                        }
                        break;
                }
                Invalidate();
            }
        }

        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public GradientStyleProperties GradientStyleProperties { get; set; }
        #endregion

        #region [ Override Events / Events / Methods ]
        private void Initialize()
        {
            try
            {
                GradientStyleProperties = new GradientStyleProperties();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GradientStyleProperties_PropertyChanged(object sender, ExtensionsPropertyEventArgs e)
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

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            try
            {
                pevent.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                pevent.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                pevent.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                Rectangle _ClientRectangle = ClientRectangle;
                switch (LabelStyle)
                {
                    case LabelConstsDefiner.LabelStyles.Gradient:
                        {
                            using (LinearGradientBrush _LinearGradientBrush = new LinearGradientBrush(_ClientRectangle, GradientStyleProperties.BackColor.FirstColor, GradientStyleProperties.BackColor.LastColor, GradientStyleProperties.GradientMode))
                            {
                                pevent.Graphics.FillRectangle(_LinearGradientBrush, _ClientRectangle);
                            }

                            if (base.BorderStyle.Equals(BorderStyle.None))
                            {
                                if (GradientStyleProperties.IsBorder)
                                {
                                    _ClientRectangle = new Rectangle(ClientRectangle.X, ClientRectangle.Y, (ClientRectangle.Width + 1), (ClientRectangle.Height + 1));
                                    ControlPaint.DrawBorder(pevent.Graphics, _ClientRectangle, GradientStyleProperties.BorderColor, ButtonBorderStyle.Solid);
                                }
                                else
                                {
                                    ControlPaint.DrawBorder3D(pevent.Graphics, _ClientRectangle, GradientStyleProperties.Border3DStyle);
                                }
                            }
                        }
                        break;

                    case LabelConstsDefiner.LabelStyles.Default:
                    default:
                        {
                            base.OnPaintBackground(pevent);
                        }
                        break;
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
    public class GradientStyleProperties
    {
        public GradientStyleProperties()
        {
            try
            {
                BackColor = new GradientBackColorProperties();
                BackColor.PropertyChanged += BackColor_PropertyChanged;
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
        public GradientBackColorProperties BackColor { get; set; }

        private void BackColor_PropertyChanged(object sender, ExtensionsPropertyEventArgs e)
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

        private LinearGradientMode m_GradientMode = LinearGradientMode.Vertical;
        [DefaultValue(typeof(LinearGradientMode), "Vertical")]
        public LinearGradientMode GradientMode
        {
            get { return m_GradientMode; }
            set
            {
                m_GradientMode = value;
                OnPropertyChanged(LabelConstsDefiner.ExtensionsPropertyNames.GradientStyleProperties.GradientMode.ToString());
            }
        }

        private Border3DStyle m_Border3DStyle = Border3DStyle.Adjust;
        [DefaultValue(typeof(Border3DStyle), "Adjust")]
        public Border3DStyle Border3DStyle
        {
            get { return m_Border3DStyle; }
            set
            {
                m_Border3DStyle = value;
                OnPropertyChanged(LabelConstsDefiner.ExtensionsPropertyNames.GradientStyleProperties.Border3DStyle.ToString());
            }
        }

        private bool m_IsBorder = false;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All)]
        [DefaultValue(false)]
        public bool IsBorder
        {
            get { return m_IsBorder; }
            set
            {
                m_IsBorder = value;
                OnPropertyChanged(LabelConstsDefiner.ExtensionsPropertyNames.GradientStyleProperties.IsBorder.ToString());
            }
        }

        private Color m_BorderColor = SystemColors.ActiveBorder;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All)]
        [DefaultValue(typeof(Color), "ActiveBorder")]
        public Color BorderColor
        {
            get { return m_BorderColor; }
            set
            {
                m_BorderColor = value;
                OnPropertyChanged(LabelConstsDefiner.ExtensionsPropertyNames.GradientStyleProperties.BorderColor.ToString());
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
    public class GradientBackColorProperties
    {
        public GradientBackColorProperties()
        {
        }

        public override string ToString()
        {
            return string.Empty;
        }

        private Color m_FirstColor = SystemColors.Control;
        [DefaultValue(typeof(Color), "Control")]
        public Color FirstColor
        {
            get { return m_FirstColor; }
            set
            {
                if (value.Equals(Color.Transparent))
                {
                    return;
                }

                m_FirstColor = value;
                OnPropertyChanged(LabelConstsDefiner.ExtensionsPropertyNames.GradientStyleProperties.FirstBackColor.ToString());
            }
        }

        private Color m_LastColor = SystemColors.ControlDark;
        [DefaultValue(typeof(Color), "ControlDark")]
        public Color LastColor
        {
            get { return m_LastColor; }
            set
            {
                if (value.Equals(Color.Transparent))
                {
                    return;
                }

                m_LastColor = value;
                OnPropertyChanged(LabelConstsDefiner.ExtensionsPropertyNames.GradientStyleProperties.LastBackColor.ToString());
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