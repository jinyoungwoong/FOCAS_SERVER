using System.Drawing;

namespace NCManagementSystem.Libraries.Controls.Forms
{
    public class FormsConstsDefiner
    {
        public enum MouseState : byte
        {
            None = 0,
            Hover = 1,
            Down = 2
        }

        public enum ControlBoxButtonTypes
        {
            Close,
            Maximum,
            Minimize
        }

        public enum ColumnAlignment
        {
            left,
            center,
            right,
        }

        public enum SpokeStyles
        {
            Circle,
            ShortRoundedRectangle,
            MediumRoundedRectangle,
            LongRoundedRectangle,
            Continuous
        }

        public enum RotateDirections
        {
            Clockwise = 1,
            Anticlockwise = -1
        }

        public enum SpokeStartPositions
        {
            Top = 315,
            Right = 0,
            Bottom = 45,
            Left = 90
        }

        internal struct Spoke
        {
            public PointF m_InnerPoint;
            public PointF m_OuterPoint;
            public float m_Angle;
            public Color m_Color;

            public Spoke(PointF inner, PointF outer, float angle, Color color)
            {
                m_InnerPoint = inner;
                m_OuterPoint = outer;
                m_Angle = angle;
                m_Color = color;
            }
        }

        internal struct FixedSpoke
        {
            internal const int DefaultCount = 12;
            internal const int MinimumCount = 3;

            internal const int MinimumOfRotationSpeedPercent = 1;
            internal const int MaximumOfRotationSpeedPercent = 200;

            internal struct CircleStyle
            {
                internal const float RatioOfThickness = 0.5F;
                internal const float RatioOfInnerRadius = 34.9F;
                internal const float RatioOuterRadius = 35F;
            }

            internal struct ShortRoundedRectangle
            {
                internal const float RatioOfThickness = 0.75F;
                internal const float RatioOfInnerRadius = 30F;
                internal const float RatioOuterRadius = 37.5F;
            }

            internal struct MediumRoundedRectangle
            {
                internal const float RatioOfThickness = 1.5F;
                internal const float RatioOfInnerRadius = 15F;
                internal const float RatioOuterRadius = 39F;
            }

            internal struct LongRoundedRectangleStyle
            {
                internal const float RatioOfThickness = 1.5F;
                internal const float RatioOfInnerRadius = 0F;
                internal const float RatioOuterRadius = 39F;
            }
        }

        internal struct FixedSize
        {
            internal struct ControlBoxButton
            {
                internal const int Default = 24;
                internal const int Minimum = 20;
            }

            internal struct Forms
            {
                internal struct Minimum
                {
                    internal const int TitleBarHeight = 30;
                    internal const int IconHeight = 20;
                    internal const int FormWidth = 150;
                }
            }
        }

        internal struct ExtensionsPropertyNames
        {
            internal struct Forms
            {
                internal enum BorderProperties
                {
                    FormBorderThickness,
                    FormBorderColor
                }

                internal enum TitleBarProperties
                {
                    TitleBarHeight,
                    TitleBarBackColor,
                    IsTitleBarBottomBorder,
                    TitleBarBottomBorderThickness,
                    TitleBarBottomBorderColor,
                    IsControlBox,
                    IsMaximizeBox,
                    IsMinimizeBox,
                    ControlBoxMaximumSize,
                    IsShowTitleBarIcon,
                    TitleBarIcon,
                    TitleBarIconSize,
                    TitleBarTextIndent,
                    TitleBarTextFont,
                    TitleBarTextForeColor,
                    TitleBarTextAlignment,
                    IsTitleBarTextEllipsis,
                    BackColorHover,
                    BackColorDown,
                    IsAlphaColorOnDown,
                    Alpha,
                    ForeColor,
                    ForeColorHover,
                    ForeColorDown
                }

                internal enum SplashScreenProperties
                {
                    SplashScreenBackColor,
                    SplashScreenBackgroundOpacity,
                    SplashScreenSpokeColor,
                    RotationSpeedPercent,
                    MaximumSpokeSize
                }
            }

            internal struct ControlBoxButton
            {
                internal enum ColorProperties
                {
                    BackColorHover,
                    BackColorDown,
                    ForeColorHover,
                    ForeColorDown,
                    IsAlphaColorOnDown,
                    Alpha,
                }
            }

            internal enum SpokeProperties
            {
                SpokeCount,
                SpokeColor,
                IsFixedSpokeSize,
                FixedSpokeSize,
                SpokeStartPosition,
                MultipleExtraSpokeCount
            }
        }
    }
}
