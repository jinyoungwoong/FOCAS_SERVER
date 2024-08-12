using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace NCManagementSystem.Components.Helpers
{
    public class SizeHelper
    {
        public static Font AutoSizeFont(Graphics g, Font font, float width, float height, string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text) || width <= 0 || height <= 0)
                {
                    return font;
                }

                Font _Font;
                float _fFactor, _fFactorX, _fFactorY;
                Graphics _Graphics = g;
                SizeF _MeasureSize = _Graphics.MeasureString(text, font);
                _fFactorX = (width / _MeasureSize.Width);
                _fFactorY = (height / _MeasureSize.Height);
                if (_fFactorX > _fFactorY)
                {
                    _fFactor = _fFactorY;
                }
                else
                {
                    _fFactor = _fFactorX;
                }
                _Font = new Font(font.Name, (font.SizeInPoints * _fFactor), font.Style);
                return _Font;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            try
            {
                Rectangle _ImageBounds = new Rectangle(0, 0, width, height);

                Bitmap _Bitmap = new Bitmap(width, height);
                _Bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                using (Graphics _Graphics = Graphics.FromImage(_Bitmap))
                {
                    _Graphics.CompositingMode = CompositingMode.SourceCopy;
                    _Graphics.CompositingQuality = CompositingQuality.HighQuality;
                    _Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    _Graphics.SmoothingMode = SmoothingMode.HighQuality;
                    _Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    using (ImageAttributes _ImageAttributes = new ImageAttributes())
                    {
                        _ImageAttributes.SetWrapMode(WrapMode.TileFlipXY);
                        _Graphics.DrawImage(image, _ImageBounds, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, _ImageAttributes);
                    }
                }
                return _Bitmap;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
