using System.Drawing;
using System.Windows.Forms;
using NCManagementSystem.Components.Helpers;

namespace NCManagementSystem.Libraries.Controls.DataGridView.Renderers
{
    public abstract class ColumnBaseSchemaRenderer
    {
        public ColumnBaseSchemaRenderer(FwDataGridView owner, DataGridViewConstsDefiner.ColumnAlignment columnAlignment)
        {
            m_Owner = owner;
            m_StringFormat = SetColumnAlignment(columnAlignment);
        }

        protected StringFormat m_StringFormat;
        protected FwDataGridView m_Owner;

        private static StringFormat SetColumnAlignment(DataGridViewConstsDefiner.ColumnAlignment columnAlignment)
        {
            StringFormat _StringFormat = new StringFormat()
            {
                LineAlignment = StringAlignment.Center
            };
            if (columnAlignment == DataGridViewConstsDefiner.ColumnAlignment.center)
            {
                _StringFormat.Alignment = StringAlignment.Center;
            }
            else if (columnAlignment == DataGridViewConstsDefiner.ColumnAlignment.left)
            {
                _StringFormat.Alignment = StringAlignment.Near;
            }
            else
            {
                _StringFormat.Alignment = StringAlignment.Far;
            }
            _StringFormat.FormatFlags = StringFormatFlags.NoWrap;
            return _StringFormat;
        }

        public virtual void SwitchColumnAlignment(DataGridViewConstsDefiner.ColumnAlignment columnAlignment)
        {
            m_StringFormat = SetColumnAlignment(columnAlignment);
        }

        protected void PreDraw(Graphics g, int columnIdx, int rowIdx, Rectangle bounds, string text, DataGridViewElementStates elementStates)
        {
            Color _ActionBackColor = m_Owner.DefaultCellStyle.BackColor;
            if ((elementStates & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
            {
                _ActionBackColor = m_Owner.DefaultCellStyle.SelectionBackColor;
                using (SolidBrush _BackColor = new SolidBrush(_ActionBackColor))
                {
                    g.FillRectangle(_BackColor, bounds);
                }
            }
            else
            {
                if ((rowIdx % 2).Equals(1))
                {
                    _ActionBackColor = m_Owner.AlternatingRowsDefaultCellStyle.BackColor;
                }
                else
                {
                    _ActionBackColor = m_Owner.DefaultCellStyle.BackColor;
                }
                using (SolidBrush _BackColor = new SolidBrush(_ActionBackColor))
                {
                    g.FillRectangle(_BackColor, bounds);
                }
            }
        }

        public virtual void Draw(Graphics g, int columnIdx, int rowIdx, Rectangle bounds, string text, DataGridViewElementStates elementStates)
        {
            if (!m_Owner.CellBorderStyle.Equals(DataGridViewCellBorderStyle.None))
            {
                Color _LineColor = m_Owner.GridColor;
                using (Pen _GridLine = new Pen(_LineColor))
                {
                    g.DrawLine(_GridLine, bounds.Left, bounds.Bottom, (bounds.Right - 1), bounds.Bottom);
                    g.DrawLine(_GridLine, bounds.Right, bounds.Top, bounds.Right, bounds.Bottom);
                }
            }
        }
    }

    public class ColumnSchemaCheckBoxRenderer : ColumnBaseSchemaRenderer
    {
        public ColumnSchemaCheckBoxRenderer(FwDataGridView owner, DataGridViewConstsDefiner.ColumnAlignment columnAlignment)
            : base(owner, columnAlignment)
        {
            m_sTrueString = FixedDefaultTrueString;
        }

        public ColumnSchemaCheckBoxRenderer(FwDataGridView owner, DataGridViewConstsDefiner.ColumnAlignment columnAlignment, string trueString)
            : base(owner, columnAlignment)
        {
            m_sTrueString = trueString;
        }

        public ColumnSchemaCheckBoxRenderer(FwDataGridView owner, DataGridViewConstsDefiner.ColumnAlignment columnAlignment, bool isInSquares)
            : base(owner, columnAlignment)
        {
            m_sTrueString = FixedDefaultTrueString;
            m_IsInSquares = isInSquares;
        }

        internal readonly string FixedDefaultTrueString = "1";
        private string m_sTrueString = "";
        private bool m_IsInSquares = true;

        public override void Draw(Graphics g, int columnIdx, int rowIdx, Rectangle bounds, string text, DataGridViewElementStates elementStates)
        {
            Rectangle _Bounds = bounds;
            if (!m_Owner.CellBorderStyle.Equals(DataGridViewCellBorderStyle.None))
            {
                _Bounds = new Rectangle(bounds.X, bounds.Y, (bounds.Width - 1), (bounds.Height - 1));
            }

            PreDraw(g, columnIdx, rowIdx, _Bounds, text, elementStates);

            Point _Point = Point.Add(_Bounds.Location, new Size((_Bounds.Width / 2), (_Bounds.Height / 2)));
            _Point = Point.Subtract(_Point, new Size(5, 5));

            if (text != null && text.Length > 0 && text == m_sTrueString)
            {
                if (m_IsInSquares)
                {
                    CheckBoxRenderer.DrawCheckBox(g, _Point, System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal);
                }
                else
                {
                    Font _FontOfCheckMark = SizeHelper.AutoSizeFont(g, new Font("Wingdings", 9F), _Bounds.Width, _Bounds.Height, System.Convert.ToChar(252).ToString());
                    float _fMargin = 4F;

                    float _fPointXOfText = (_Bounds.X + (_Bounds.Width / 2F));
                    float _fPointYOfText = (_Bounds.Y + (_Bounds.Height / 2F));
                    if ((_Bounds.Height / 2F) > (_fMargin * 2F))
                    {
                        _fPointYOfText = _fPointYOfText + _fMargin;
                    }
                    else
                    {
                        _fPointYOfText = _fPointYOfText + (_fMargin / 2F);
                    }

                    Color _ActionForeColor = m_Owner.DefaultCellStyle.ForeColor;
                    if ((elementStates & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
                    {
                        _ActionForeColor = m_Owner.DefaultCellStyle.SelectionForeColor;
                    }

                    using (SolidBrush _ForeColor = new SolidBrush(_ActionForeColor))
                    {
                        g.DrawString(System.Convert.ToChar(252).ToString(), _FontOfCheckMark, _ForeColor, new PointF(_fPointXOfText, _fPointYOfText), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center } );
                    }
                }
            }
            else
            {
                if (m_IsInSquares)
                {
                    CheckBoxRenderer.DrawCheckBox(g, _Point, System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);
                }
            }

            base.Draw(g, columnIdx, rowIdx, _Bounds, text, elementStates);
        }
    }
}
