using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NCManagementSystem.Libraries.Controls.DataGridView.Renderers
{
    public interface IHeaderRenderer
    {
        void DrawColumnHeader(Graphics g, int columnIndex, Rectangle rectangle);
        void DrawRowHeader(Graphics g, int rowIndex, Rectangle rectangle, DataGridViewElementStates elementStates);
        void DrawNC(Graphics g, Rectangle rectangle);
    }

    internal class HeaderRenderer : IHeaderRenderer
    {
        public HeaderRenderer(FwDataGridView owner)
        {
            m_Owner = owner;
        }

        private FwDataGridView m_Owner;

        public void DrawColumnHeader(Graphics g, int columnIdx, Rectangle bounds)
        {
            using (LinearGradientBrush _BackColor = new LinearGradientBrush(new Point(bounds.Left, bounds.Top - 1), new Point(bounds.Left, bounds.Bottom), m_Owner.HeaderProperties.ColumnFirstBackColor, m_Owner.HeaderProperties.ColumnLastBackColor))
            {
                g.FillRectangle(_BackColor, bounds);
            }

            using (SolidBrush _ForeColor = new SolidBrush(m_Owner.HeaderProperties.ColumnForeColor))
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                g.DrawString(m_Owner.Columns[columnIdx].HeaderText, m_Owner.HeaderProperties.Font, _ForeColor, bounds, new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center, FormatFlags = StringFormatFlags.NoWrap });
            }

            using (Pen _Border = new Pen(m_Owner.HeaderProperties.BorderColor))
            {
                if (m_Owner.RowHeadersVisible)
                {
                    g.DrawRectangle(_Border, new Rectangle((bounds.Left - 1), bounds.Top, bounds.Width, (bounds.Height - 1)));
                }
                else
                {
                    if (columnIdx.Equals(0))
                    {
                        g.DrawRectangle(_Border, new Rectangle(bounds.Left, bounds.Top, (bounds.Width - 1), (bounds.Height - 1)));
                    }
                    else
                    {
                        g.DrawRectangle(_Border, new Rectangle((bounds.Left - 1), bounds.Top, bounds.Width, (bounds.Height - 1)));
                    }
                }
            }
        }

        public void DrawRowHeader(Graphics g, int rowIdx, Rectangle bounds, DataGridViewElementStates elementStates)
        {
            using (LinearGradientBrush _BackColor = new LinearGradientBrush(new Point(bounds.Left, bounds.Top - 1), new Point(bounds.Left, bounds.Bottom), (elementStates & DataGridViewElementStates.Selected) != 0 ? m_Owner.HeaderProperties.RowSelectionFirstBackColor : m_Owner.HeaderProperties.RowFirstBackColor, (elementStates & DataGridViewElementStates.Selected) != 0 ? m_Owner.HeaderProperties.RowSelectionLastBackColor : m_Owner.HeaderProperties.RowLastBackColor))
            {
                g.FillRectangle(_BackColor, bounds);
            }

            if ((elementStates & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
            {
                using (SolidBrush _ForeColor = new SolidBrush(m_Owner.HeaderProperties.RowSelectionForeColor))
                {
                    if (m_Owner.HeaderProperties.IsVisibleSelectedRowNumber)
                    {
                        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                        g.DrawString((rowIdx + 1).ToString(), m_Owner.HeaderProperties.Font, _ForeColor, bounds, new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center, FormatFlags = StringFormatFlags.NoWrap });
                    }
                    else
                    {
                        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                        g.DrawString(">", m_Owner.HeaderProperties.Font, _ForeColor, bounds, new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center, FormatFlags = StringFormatFlags.NoWrap });
                    }
                }
            }
            else
            {
                using (SolidBrush _ForeColor = new SolidBrush(m_Owner.HeaderProperties.RowForeColor))
                {
                    g.DrawString((rowIdx + 1).ToString(), m_Owner.HeaderProperties.Font, _ForeColor, bounds, new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center, FormatFlags = StringFormatFlags.NoWrap });
                }
            }

            using (Pen _Border = new Pen(m_Owner.HeaderProperties.BorderColor))
            {
                if (m_Owner.ColumnHeadersVisible)
                {
                    if (rowIdx.Equals((m_Owner.RowCount - 1)))
                    {
                        g.DrawRectangle(_Border, new Rectangle(bounds.Left, (bounds.Top - 1), (bounds.Width - 1), bounds.Height));
                    }
                    else if (rowIdx.Equals(0))
                    {
                        g.DrawRectangle(_Border, new Rectangle(bounds.Left, (bounds.Top - 1), (bounds.Width - 1), bounds.Height));
                    }
                    else
                    {
                        g.DrawRectangle(_Border, new Rectangle(bounds.Left, (bounds.Top - 1), (bounds.Width - 1), (bounds.Height + 1)));
                    }
                }
                else
                {
                    if (rowIdx.Equals(0))
                    {
                        g.DrawRectangle(_Border, new Rectangle((bounds.Left + 1), (bounds.Top + 1), (bounds.Width - 1), (bounds.Height - 1)));
                    }
                    else
                    {
                        g.DrawRectangle(_Border, new Rectangle((bounds.Left + 1), bounds.Top, (bounds.Width - 1), bounds.Height));
                    }
                }
            }
        }

        public void DrawNC(Graphics g, Rectangle rectangle)
        {
            using (LinearGradientBrush _BackColor = new LinearGradientBrush(rectangle, m_Owner.HeaderProperties.NCFirstBackColor, m_Owner.HeaderProperties.NCLastBackColor, LinearGradientMode.Vertical))
            {
                g.FillRectangle(_BackColor, rectangle);
            }

            using (Pen _Border = new Pen(m_Owner.HeaderProperties.BorderColor))
            {
                if (m_Owner.Rows.Count > 0)
                {
                    g.DrawRectangle(_Border, new Rectangle(rectangle.Left, rectangle.Top, (rectangle.Width - 1), (rectangle.Height - 1)));
                }
                else
                {
                    g.DrawRectangle(_Border, new Rectangle(rectangle.Left, rectangle.Top, (rectangle.Width - 1), (rectangle.Height - 1)));
                }
            }
        }

        //public void DrawNC(Graphics g, Rectangle bounds)
        //{
        //    using (LinearGradientBrush _BackColor = new LinearGradientBrush(bounds, m_Owner.HeaderProperties.NCFirstBackColor, m_Owner.HeaderProperties.NCLastBackColor, LinearGradientMode.Vertical))
        //    {
        //        g.FillRectangle(_BackColor, bounds);
        //    }

        //    using (Pen _Border = new Pen(m_Owner.HeaderProperties.BorderColor))
        //    {
        //        if (m_Owner.Rows.Count > 0)
        //        {
        //            g.DrawRectangle(_Border, new Rectangle(bounds.Left, bounds.Top, (bounds.Width - 1), (bounds.Height - 1)));
        //        }
        //        else
        //        {
        //            g.DrawRectangle(_Border, new Rectangle(bounds.Left, bounds.Top, (bounds.Width - 1), (bounds.Height - 1)));
        //        }
        //    }
        //}
    }
}