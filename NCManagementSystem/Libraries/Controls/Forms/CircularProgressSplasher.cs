//#define ForTask

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NCManagementSystem.Libraries.Controls.Forms
{
    [ToolboxBitmap(typeof(System.Windows.Forms.ProgressBar)), ToolboxItemFilter("System.Windows.Forms")]
    public partial class CircularProgressSplasher : System.Windows.Forms.Panel
    {
        #region [ Constructor ]
        public CircularProgressSplasher()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, true);
            UpdateStyles();

            InitializeComponent();

            Initialize();
        }
        #endregion

        #region [ Member Variables / Fields / Properties ]
        private List<FormsConstsDefiner.Spoke> m_Spokes;
        private int m_iSpokeIndex = 0;

        private int m_iBackgroundOpacity = 50;
        [DefaultValue(50)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.Repaint)]
        public int BackgroundOpacity
        {
            get { return m_iBackgroundOpacity; }
            set
            {
                if (value < 0 || value > 100)
                {
                    return;
                }

                m_iBackgroundOpacity = value;
                Invalidate();
            }
        }

        private FormsConstsDefiner.SpokeStyles m_SpokeStyle = FormsConstsDefiner.SpokeStyles.Circle;
        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.Repaint)]
        [DefaultValue(typeof(FormsConstsDefiner.SpokeStyles), "Circle")]
        public FormsConstsDefiner.SpokeStyles SpokeStyle
        {
            get { return m_SpokeStyle; }
            set
            {
                m_SpokeStyle = value;
                SetSpokes();
                Invalidate();
            }
        }

        private FormsConstsDefiner.RotateDirections m_RotateDirection = FormsConstsDefiner.RotateDirections.Clockwise;
        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.Repaint)]
        [DefaultValue(typeof(FormsConstsDefiner.RotateDirections), "Clockwise")]
        public FormsConstsDefiner.RotateDirections RotateDirection
        {
            get { return m_RotateDirection; }
            set
            {
                m_RotateDirection = value;
                SetSpokes();
                Invalidate();
            }
        }

        private int m_iRotationSpeedPercent = 100;
        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.Repaint)]
        [DefaultValue(100)]
        public int RotationSpeedPercent
        {
            get { return m_iRotationSpeedPercent; }
            set
            {
                if (value < 1 || value > 200)
                {
                    return;
                }
                m_iRotationSpeedPercent = value;
                Invalidate();
            }
        }

        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public SpokeProperties SpokeProperties { get; set; }

#if ForTask
        private bool m_IsActive = false;
        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.Repaint)]
        [DefaultValue(false)]
        public bool IsActive
        {
            get { return m_IsActive; }
            set
            {
                m_IsActive = value;

                if (m_IsActive)
                {
                    RunActiveAsync();
                }
            }
        }

        private async void RunActiveAsync()
        {
            try
            {
                while (m_IsActive)
                {
                    await Task.Run(() =>
                    {
                        if (SpokeProperties.SpokeCount.Equals((m_iSpokePosition + 1)))
                        {
                            m_iSpokePosition = 0;
                        }
                        else
                        {
                            m_iSpokePosition++;
                        }
                    });

                    if (IsHandleCreated && !IsDisposed)
                    {
                        Invoke(new Action(() =>
                        {
                            Invalidate();

                        }));
                    }

                    await Task.Delay(CalculateRotationInterval());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int CalculateRotationInterval()
        {
            try
            {
                return (((1000 / SpokeProperties.SpokeCount) * m_iRotationSpeedRatio) / 100);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
#else
        private bool m_IsActive = false;
        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.Repaint)]
        [DefaultValue(false)]
        public bool IsActive
        {
            get { return m_IsActive; }
            set
            {
                m_IsActive = value;
                if (m_IsActive)
                {
                    m_Timer.Start();
                }
                else
                {
                    m_Timer.Stop();
                    m_iSpokeIndex = 0;
                    Invalidate();
                }
            }
        }

        private System.Timers.Timer m_Timer = new System.Timers.Timer();
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                m_Timer.Interval = CalculateRotationInterval();

                if (SpokeProperties.SpokeCount.Equals((m_iSpokeIndex + 1)))
                {
                    m_iSpokeIndex = 0;
                }
                else
                {
                    m_iSpokeIndex++;
                }
                Invalidate();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double CalculateRotationInterval()
        {
            try
            {
                return (((1000D / SpokeProperties.SpokeCount) * m_iRotationSpeedPercent) / 100D);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
#endif
        #endregion

        #region [ Override Events / Events / Methods ]
        private void Initialize()
        {
            try
            {
                SpokeProperties = new SpokeProperties();
                SpokeProperties.PropertyChanged += SpokeProperties_PropertyChanged;

                SetSpokes();
#if ForTask
#else
                m_Timer.Elapsed += Timer_Elapsed;
#endif
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SpokeProperties_PropertyChanged(object sender, ExtensionsPropertyEventArgs e)
        {
            try
            {
                SetSpokes();
                Invalidate();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetSpokes()
        {
            try
            {
                m_Spokes = new List<FormsConstsDefiner.Spoke>();

                int _iSizeOfSpokeBounds = (ClientRectangle.Width < ClientRectangle.Height) ? ClientRectangle.Width : ClientRectangle.Height;
                if (SpokeProperties.IsFixedSpokeSize)
                {
                    _iSizeOfSpokeBounds = SpokeProperties.FixedSpokeSize;
                }
                float _fIncrementAngle = (360F / SpokeProperties.SpokeCount);
                byte _IncrementAlpha = (byte)(byte.MaxValue / SpokeProperties.SpokeCount);

                float _fRatioOfInnerRadius = 0F;
                float _fRatioOfOuterRadius = 0F;
                switch (m_SpokeStyle)
                {
                    case FormsConstsDefiner.SpokeStyles.Circle:
                        {
                            _fRatioOfInnerRadius = ((_iSizeOfSpokeBounds * FormsConstsDefiner.FixedSpoke.CircleStyle.RatioOfInnerRadius) / 100F);
                            _fRatioOfOuterRadius = ((_iSizeOfSpokeBounds * FormsConstsDefiner.FixedSpoke.CircleStyle.RatioOuterRadius) / 100F);
                        }
                        break;

                    case FormsConstsDefiner.SpokeStyles.ShortRoundedRectangle:
                        {
                            _fRatioOfInnerRadius = ((_iSizeOfSpokeBounds * FormsConstsDefiner.FixedSpoke.ShortRoundedRectangle.RatioOfInnerRadius) / 100F);
                            _fRatioOfOuterRadius = ((_iSizeOfSpokeBounds * FormsConstsDefiner.FixedSpoke.ShortRoundedRectangle.RatioOuterRadius) / 100F);
                        }
                        break;

                    case FormsConstsDefiner.SpokeStyles.MediumRoundedRectangle:
                        {
                            _fRatioOfInnerRadius = ((_iSizeOfSpokeBounds * FormsConstsDefiner.FixedSpoke.MediumRoundedRectangle.RatioOfInnerRadius) / 100F);
                            _fRatioOfOuterRadius = ((_iSizeOfSpokeBounds * FormsConstsDefiner.FixedSpoke.MediumRoundedRectangle.RatioOuterRadius) / 100F);
                        }
                        break;

                    case FormsConstsDefiner.SpokeStyles.LongRoundedRectangle:
                        {
                            _fRatioOfInnerRadius = FormsConstsDefiner.FixedSpoke.LongRoundedRectangleStyle.RatioOfInnerRadius;
                            _fRatioOfOuterRadius = ((_iSizeOfSpokeBounds * FormsConstsDefiner.FixedSpoke.LongRoundedRectangleStyle.RatioOuterRadius) / 100F);
                        }
                        break;

                    default:
                        break;
                }

                for (int _iIdx = 0; _iIdx < SpokeProperties.SpokeCount; _iIdx++)
                {
                    FormsConstsDefiner.Spoke _Spoke;
                    PointF _InnerPoint;
                    PointF _OuterPoint;
                    float _fAngle = 0F;
                    Color _Color;
                    if (_iIdx.Equals(0))
                    {
                        _fAngle = (float)SpokeProperties.SpokeStartPosition;
                        _InnerPoint = new PointF(_fRatioOfInnerRadius * (float)Math.Cos(ConvertDegreesToRadians(_fAngle)), _fRatioOfInnerRadius * (float)Math.Sin(ConvertDegreesToRadians(_fAngle)));
                        _OuterPoint = new PointF(_fRatioOfOuterRadius * (float)Math.Cos(ConvertDegreesToRadians(_fAngle)), _fRatioOfOuterRadius * (float)Math.Sin(ConvertDegreesToRadians(_fAngle)));
                        _Color = DarkenSpokeColors(SpokeProperties.SpokeColor, byte.MaxValue);
                        _Spoke = new FormsConstsDefiner.Spoke(_InnerPoint, _OuterPoint, _fAngle, _Color);
                    }
                    else
                    {
                        switch (m_RotateDirection)
                        {
                            case FormsConstsDefiner.RotateDirections.Anticlockwise:
                                {
                                    _fAngle = (m_Spokes[(_iIdx - 1)].m_Angle - _fIncrementAngle);
                                }
                                break;

                            case FormsConstsDefiner.RotateDirections.Clockwise:
                            default:
                                {
                                    _fAngle = (m_Spokes[(_iIdx - 1)].m_Angle + _fIncrementAngle);
                                }
                                break;
                        }
                        _InnerPoint = new PointF(_fRatioOfInnerRadius * (float)Math.Cos(ConvertDegreesToRadians(_fAngle)), _fRatioOfInnerRadius * (float)Math.Sin(ConvertDegreesToRadians(_fAngle)));
                        _OuterPoint = new PointF(_fRatioOfOuterRadius * (float)Math.Cos(ConvertDegreesToRadians(_fAngle)), _fRatioOfOuterRadius * (float)Math.Sin(ConvertDegreesToRadians(_fAngle)));
                        _Color = DarkenSpokeColors(m_Spokes[(_iIdx - 1)].m_Color, (_IncrementAlpha * (_iIdx + 1)));
                        _Spoke = new FormsConstsDefiner.Spoke(_InnerPoint, _OuterPoint, _fAngle, _Color);
                    }
                    m_Spokes.Add(_Spoke);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double ConvertDegreesToRadians(float degrees)
        {
            try
            {
                return ((Math.PI / 180D) * degrees);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Color DarkenSpokeColors(Color color, int alpha)
        {
            try
            {
                int _iRed = color.R;
                int _iGreen = color.G;
                int _iBlue = color.B;
                return Color.FromArgb(alpha, Math.Min(_iRed, byte.MaxValue), Math.Min(_iGreen, byte.MaxValue), Math.Min(_iBlue, byte.MaxValue));
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
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var _SolidBrush = new SolidBrush(Color.FromArgb(((BackgroundOpacity * 255) / 100), BackColor)))
                {
                    e.Graphics.FillRectangle(_SolidBrush, ClientRectangle);
                }

                PointF _CenterPoint = new PointF((ClientRectangle.Width / 2), (ClientRectangle.Height / 2));

                float _fAngleOfSpoke = m_Spokes[m_iSpokeIndex].m_Angle;
                e.Graphics.TranslateTransform(_CenterPoint.X, _CenterPoint.Y, MatrixOrder.Prepend);
                e.Graphics.RotateTransform(_fAngleOfSpoke, MatrixOrder.Prepend);

                int _iSizeOfSpokeBounds = (ClientRectangle.Width < ClientRectangle.Height) ? ClientRectangle.Width : ClientRectangle.Height;
                if (SpokeProperties.IsFixedSpokeSize)
                {
                    _iSizeOfSpokeBounds = SpokeProperties.FixedSpokeSize;
                }

                float _fRatioOfSpokeThickness = 0F;
                switch (m_SpokeStyle)
                {
                    case FormsConstsDefiner.SpokeStyles.Circle:
                        {
                            _fRatioOfSpokeThickness = FormsConstsDefiner.FixedSpoke.CircleStyle.RatioOfThickness;
                        }
                        break;

                    case FormsConstsDefiner.SpokeStyles.ShortRoundedRectangle:
                        {
                            _fRatioOfSpokeThickness = FormsConstsDefiner.FixedSpoke.ShortRoundedRectangle.RatioOfThickness;
                        }
                        break;

                    case FormsConstsDefiner.SpokeStyles.MediumRoundedRectangle:
                        {
                            _fRatioOfSpokeThickness = FormsConstsDefiner.FixedSpoke.MediumRoundedRectangle.RatioOfThickness;
                        }
                        break;

                    case FormsConstsDefiner.SpokeStyles.LongRoundedRectangle:
                        {
                            _fRatioOfSpokeThickness = FormsConstsDefiner.FixedSpoke.LongRoundedRectangleStyle.RatioOfThickness;
                        }
                        break;

                    default:
                        break;
                }
                if (SpokeProperties.SpokeCount < FormsConstsDefiner.FixedSpoke.DefaultCount)
                {
                    _fRatioOfSpokeThickness = (_fRatioOfSpokeThickness + ((FormsConstsDefiner.FixedSpoke.DefaultCount - SpokeProperties.SpokeCount) / 10F));
                }
                for (int _iIdx = 0; _iIdx < SpokeProperties.SpokeCount; _iIdx++)
                {
                    using (Pen _PenSpoke = new Pen(new SolidBrush(m_Spokes[_iIdx].m_Color), ((_iSizeOfSpokeBounds / SpokeProperties.SpokeCount) / _fRatioOfSpokeThickness)))
                    {
                        _PenSpoke.StartCap = LineCap.Round;
                        _PenSpoke.EndCap = LineCap.Round;
                        e.Graphics.DrawLine(_PenSpoke, m_Spokes[_iIdx].m_InnerPoint, m_Spokes[_iIdx].m_OuterPoint);
                    }
                }

                e.Graphics.RotateTransform(-_fAngleOfSpoke, MatrixOrder.Append);
                e.Graphics.TranslateTransform(-_CenterPoint.X, -_CenterPoint.Y, MatrixOrder.Append);

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
                base.OnSizeChanged(e);

                SetSpokes();
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
                _CreateParams.ExStyle = _CreateParams.ExStyle | NativeMethods.WS_EX_TRANSPARENT | NativeMethods.WS_EX_COMPOSITED;
                return _CreateParams;
            }
        }
        #endregion
    }


    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SpokeProperties
    {
        public SpokeProperties()
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

        private int m_iSpokeCount = 12;
        [DefaultValue(12)]
        public int SpokeCount
        {
            get { return m_iSpokeCount; }
            set
            {
                if (value < FormsConstsDefiner.FixedSpoke.MinimumCount)
                {
                    return;
                }
                else if (value > (FormsConstsDefiner.FixedSpoke.DefaultCount * 5))
                {
                    return;
                }

                m_iSpokeCount = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.SpokeProperties.SpokeCount.ToString());
            }
        }

        private int m_iMultipleExtraSpokeCount = 8;
        [DefaultValue(8)]
        public int MultipleExtraSpokeCount
        {
            get { return m_iMultipleExtraSpokeCount; }
            set
            {
                m_iMultipleExtraSpokeCount = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.SpokeProperties.SpokeCount.ToString());
            }
        }

        private Color m_SpokeColor = Color.White;
        [DefaultValue(typeof(Color), "White")]
        public Color SpokeColor
        {
            get { return m_SpokeColor; }
            set
            {
                m_SpokeColor = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.SpokeProperties.SpokeColor.ToString());
            }
        }

        private bool m_IsFixedSpokeSize = false;
        [DefaultValue(false)]
        public bool IsFixedSpokeSize
        {
            get { return m_IsFixedSpokeSize; }
            set
            {
                m_IsFixedSpokeSize = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.SpokeProperties.IsFixedSpokeSize.ToString());
            }
        }

        private int m_iFixedSpokeSize = 100;
        [DefaultValue(100)]
        public int FixedSpokeSize
        {
            get { return m_iFixedSpokeSize; }
            set
            {
                m_iFixedSpokeSize = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.SpokeProperties.FixedSpokeSize.ToString());
            }
        }

        private FormsConstsDefiner.SpokeStartPositions m_SpokeStartPosition = FormsConstsDefiner.SpokeStartPositions.Top;
        [DefaultValue(typeof(FormsConstsDefiner.SpokeStartPositions), "Top")]
        public FormsConstsDefiner.SpokeStartPositions SpokeStartPosition
        {
            get { return m_SpokeStartPosition; }
            set
            {
                m_SpokeStartPosition = value;
                OnPropertyChanged(FormsConstsDefiner.ExtensionsPropertyNames.SpokeProperties.SpokeStartPosition.ToString());
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
