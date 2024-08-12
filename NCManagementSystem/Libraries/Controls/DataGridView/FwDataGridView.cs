using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NCManagementSystem.Libraries.Controls.DataGridView.Components;
using NCManagementSystem.Libraries.Controls.DataGridView.Renderers;

namespace NCManagementSystem.Libraries.Controls.DataGridView
{
    [ToolboxBitmap(typeof(System.Windows.Forms.DataGridView)), ToolboxItemFilter("System.Windows.Forms")]
    public partial class FwDataGridView : System.Windows.Forms.DataGridView
    {
        #region [ Constructor ]
        public FwDataGridView()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.EnableNotifyMessage, true);
            UpdateStyles();

            InitializeComponent();

            Initialize();
        }
        #endregion

        #region [ Member Variables / Fields / Properties ]
        private readonly int m_iFixedSpacingWidth = 20; // 컬럼 고정 너비
        private readonly int m_iSortOrFilterBounds = 20; // 정렬, 필터 클릭 영역

        private bool m_IsRowClicked = false; // 바인딩 후 행 클릭 했엇는지...

        private IHeaderRenderer m_HeaderRenderer;
        private void SetHeaderRenderer(IHeaderRenderer render)
        {
            m_HeaderRenderer = render;
        }

        private BaseDataModel m_DataModel;
        private void SetDataModel(BaseDataModel dataModel)
        {
            m_DataModel = dataModel;
        }
        public BaseDataModel GetDataModel()
        {
            return m_DataModel;
        }

        private Color m_BorderColor = Color.Black;
        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All)]
        [DefaultValue(typeof(Color), "Black")]
        public Color BorderColor
        {
            get { return m_BorderColor; }
            set
            {
                m_BorderColor = value;
                Invalidate();
            }
        }

        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All)]
        [DefaultValue(false)]
        public bool IsSortable { get; set; } = false;

        private int m_iRowHeight = 20;
        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All)]
        [DefaultValue(20)]
        public int RowHeight
        {
            get { return m_iRowHeight; }
            set
            {
                m_iRowHeight = value;
                RowTemplate.Height = m_iRowHeight;
                Invalidate();
            }
        }

        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All)]
        [DefaultValue(false)]
        public bool IsAutoSizeColumn { get; set; } = false; // 데이터 길이로 컬럼 사이즈 자동 조절

        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All)]
        [DefaultValue(false)]
        public bool IsSelectRowAfterDataBinding { get; set; } = false;  // 데이터 바인딩 후 첫행 선택

        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All)]
        [DefaultValue(true)]
        public bool IsEndEditWithEnter { get; set; } = true; // Enter키로 편집 종료

        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public HeaderProperties HeaderProperties { get; set; }

        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public CellProperties CellProperties { get; set; }
        #endregion

        #region [ Override Events / Events / Methods ]
        private void Initialize()
        {
            try
            {
                HeaderProperties = new HeaderProperties();
                HeaderProperties.PropertyChanged += HeaderProperties_PropertyChanged;

                CellProperties = new CellProperties();
                CellProperties.PropertyChanged += CellProperties_PropertyChanged;
                
                SetHeaderRenderer(new HeaderRenderer(this));

                SetDataModel(new DataModel(this));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void HeaderProperties_PropertyChanged(object sender, ExtensionsPropertyEventArgs e)
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

        private void CellProperties_PropertyChanged(object sender, ExtensionsPropertyEventArgs e)
        {
            try
            {
                string _sPropertyName = e.PropertyName;
                if (Enum.TryParse(_sPropertyName, out DataGridViewConstsDefiner.ExtensionsPropertyNames.CellProperties _PropertyName))
                {
                    switch (_PropertyName)
                    {
                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.CellProperties.CellFont:
                            {
                                DefaultCellStyle.Font = CellProperties.Font;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.CellProperties.CellSelectionBackColor:
                            {
                                DefaultCellStyle.SelectionBackColor = CellProperties.SelectionBackColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.CellProperties.CellSelectionForeColor:
                            {
                                DefaultCellStyle.SelectionForeColor = CellProperties.SelectionForeColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.CellProperties.IsCheckBoxCellInSquares:
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

        protected override bool ProcessDialogKey(Keys keyData)
        {
            try
            {
                if (!IsEndEditWithEnter && keyData.Equals(Keys.Enter))
                {
                    EndEdit();
                    return true;
                }
                return base.ProcessDialogKey(keyData);
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
                base.OnPaint(e);

                if (BorderStyle.Equals(BorderStyle.FixedSingle))
                {
                    using (Pen _Border = new Pen(m_BorderColor))
                    {
                        Rectangle _ClientRectangle = new Rectangle(ClientRectangle.Left, ClientRectangle.Top, (ClientRectangle.Width - 1), (ClientRectangle.Height - 1));
                        e.Graphics.DrawRectangle(_Border, _ClientRectangle);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (DrawHeaders(e.Graphics, e.RowIndex, e.ColumnIndex, e.CellBounds, e.State))
                {
                    e.Handled = true;
                    Invalidate();
                    return;
                }

                if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
                {
                    var _Column = GetColumn(e.ColumnIndex);
                    if (_Column.IsCheckBoxCell && _Column.ColumnBaseSchemaRenderer != null)
                    {
                        string _sValue = e.Value == null ? "" : e.Value.ToString();

                        Rectangle _CellBounds = new Rectangle(e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Width, e.CellBounds.Height);
                        var _ColumnRenderer = m_DataModel.GetColumnsSchema().Cast<ColumnSchema>().Where(x => e.ColumnIndex == x.ColumnIndex).Select(x => x.ColumnBaseSchemaRenderer).SingleOrDefault();
                        _ColumnRenderer.Draw(e.Graphics, e.ColumnIndex, e.RowIndex, _CellBounds, _sValue, e.State);
                        e.Handled = true;
                        return;
                    }
                }

                base.OnCellPainting(e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool DrawHeaders(Graphics g, int rowIdx, int columnIdx, Rectangle bounds, DataGridViewElementStates elementStates)
        {
            try
            {
                if (columnIdx == -1 && rowIdx == -1)
                {
                    m_HeaderRenderer.DrawNC(g, bounds);
                    return true;
                }
                if (columnIdx >= 0 && rowIdx == -1)
                {
                    m_HeaderRenderer.DrawColumnHeader(g, columnIdx, bounds);
                    return true;
                }
                if (columnIdx == -1 && rowIdx >= 0)
                {
                    m_HeaderRenderer.DrawRowHeader(g, rowIdx, bounds, elementStates);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetColumns(List<ColumnSchema> columnsSchema)
        {
            try
            {
                m_DataModel.SetColumnsSchema(columnsSchema);

                CreateColumns();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CreateColumns()
        {
            try
            {
                if (Columns != null && Columns.Count > 0)
                {
                    return;
                }

                int _iColumnIdx = -1;

                if (m_DataModel.GetColumnsSchema() == null)
                {
                    ColumnSchemaFactory _ColumnSchemaFactory = new ColumnSchemaFactory(this);
                    m_DataModel.GetDataSource().Columns.Cast<DataColumn>().ToList().ForEach(x =>
                    {
                        _iColumnIdx = Columns.Add(x.ColumnName, x.Caption);
                        Columns[_iColumnIdx].ReadOnly = true;
                        Columns[_iColumnIdx].Visible = true;
                        if (IsAutoSizeColumn)
                        {
                            using (var _Graphics = CreateGraphics())
                            {
                                var _ColumnWidth = _Graphics.MeasureString(x.ColumnName, DefaultCellStyle.Font);
                                if (_ColumnWidth.Width > Columns[_iColumnIdx].Width)
                                {
                                    Columns[_iColumnIdx].Width = ((int)_ColumnWidth.Width + m_iFixedSpacingWidth);
                                }
                            }
                        }
                        _ColumnSchemaFactory.Create(x.Caption, x.ColumnName, typeof(string), true, true);
                    });
                    m_DataModel.SetColumnsSchema(_ColumnSchemaFactory.ToList());
                }
                else
                {
                    m_DataModel.GetColumnsSchema().ForEach(x =>
                    {
                        _iColumnIdx = Columns.Add(x.DataPropertyName, x.ColumnName);
                        Columns[_iColumnIdx].ReadOnly = x.IsReadOnly;
                        Columns[_iColumnIdx].Visible = x.IsVisible;
                        if (IsAutoSizeColumn)
                        {
                            using (var _Graphics = CreateGraphics())
                            {
                                var _ColumnWidth = _Graphics.MeasureString(x.ColumnName, DefaultCellStyle.Font);
                                if (_ColumnWidth.Width > Columns[_iColumnIdx].Width)
                                {
                                    Columns[_iColumnIdx].Width = ((int)_ColumnWidth.Width + m_iFixedSpacingWidth);
                                }
                            }
                        }
                        else
                        {
                            Columns[_iColumnIdx].Width = x.ColumnWidth;
                        }
                        switch (x.ColumnAlignment)
                        {
                            case DataGridViewConstsDefiner.ColumnAlignment.center:
                                {
                                    Columns[_iColumnIdx].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                }
                                break;

                            case DataGridViewConstsDefiner.ColumnAlignment.right:
                                {
                                    Columns[_iColumnIdx].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                }
                                break;

                            case DataGridViewConstsDefiner.ColumnAlignment.left:
                            default:
                                {
                                    Columns[_iColumnIdx].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                                }
                                break;
                        };
                        if (x.IsCheckBoxCell)
                        {
                            x.ColumnBaseSchemaRenderer = new ColumnSchemaCheckBoxRenderer(this, x.ColumnAlignment, CellProperties.IsCheckBoxCellInSquares);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ColumnSchema> GetColumns()
        {
            return m_DataModel.GetColumnsSchema();
        }

        public ColumnSchema GetColumn(int columnIdx)
        {
            try
            {
                foreach (ColumnSchema _Column in GetColumns())
                {
                    if (_Column.ColumnIndex.Equals(columnIdx))
                    {
                        return _Column;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ColumnSchema GetColumn(string dataPropertyName)
        {
            try
            {
                foreach (ColumnSchema _Column in GetColumns())
                {
                    if (string.Compare(_Column.DataPropertyName, dataPropertyName, false).Equals(0))
                    {
                        return _Column;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetDataSource(DataTable dataTable)
        {
            try
            {
                m_IsRowClicked = false;

                m_DataModel.SetDataSource(dataTable);

                CreateColumns();

                DisplayDataSource();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        private void DisplayDataSource()
        {
            try
            {
                if (m_DataModel.GetDataSource() == null)
                {
                    return;
                }

                if (m_DataModel.GetDataSource().Rows.Count <= 0)
                {
                    if (AllowUserToAddRows)
                    {
                        RowCount = 1;
                    }
                    else
                    {
                        RowCount = 0;
                    }
                    return;
                }

                if (AllowUserToAddRows)
                {
                    RowCount = (m_DataModel.GetDataSource().Rows.Count + 1);
                }
                else
                {
                    RowCount = m_DataModel.GetDataSource().Rows.Count;
                }

                if (IsAutoSizeColumn)
                {
                    using (var _Graphics = CreateGraphics())
                    {
                        for (int _iColumnIdx = 0; _iColumnIdx < m_DataModel.GetDataSource().Columns.Count; _iColumnIdx++)
                        {
                            var _Column = GetColumn(m_DataModel.GetDataSource().Columns[_iColumnIdx].ColumnName);
                            if (_Column == null)
                            {
                                continue;
                            }
                            if (!_Column.IsVisible)
                            {
                                continue;
                            }

                            string[] _StringCollection = m_DataModel.GetDataSource().AsEnumerable().Where(r => r.Field<object>(_iColumnIdx) != null).Select(r => r.Field<object>(_iColumnIdx).ToString()).ToArray();
                            if (_StringCollection.Length <= 0)
                            {
                                continue;
                            }
                            _StringCollection = _StringCollection.OrderBy((x) => x.Length).ToArray();
                            string _sLongestString = _StringCollection.Last(); // 해당 컬럼 중 최대 길이의 문자열
                            Font _FontBold = new Font(DefaultCellStyle.Font, FontStyle.Bold); // 굵은 폰트
                            var _ColumnWidth = _Graphics.MeasureString(_sLongestString, _FontBold); // 굵은 폰트로 길이 측정
                            if ((_ColumnWidth.Width + m_iFixedSpacingWidth) > Columns[_Column.ColumnIndex].HeaderCell.Size.Width)
                            {
                                Columns[_Column.ColumnIndex].Width = ((int)_ColumnWidth.Width + m_iFixedSpacingWidth);
                            }
                            else
                            {
                                Columns[_Column.ColumnIndex].Width = Columns[_Column.ColumnIndex].HeaderCell.Size.Width;
                            }
                        }
                    }
                }

                if (RowCount > 0)
                {
                    foreach (DataGridViewRow _dgvrRow in Rows)
                    {
                        _dgvrRow.Height = RowHeight;
                    }
                }

                if (IsSelectRowAfterDataBinding)
                {
                    if (CurrentRow != null && CurrentRow.Index >= 0)
                    {
                        OnCellClick(new DataGridViewCellEventArgs(0, CurrentRow.Index));
                    }
                }
                else
                {
                    ClearRowSelection();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnCellValuePushed(DataGridViewCellValueEventArgs e)
        {
            try
            {
                if (e.RowIndex >= RowCount)
                {
                    return;
                }

                if (e.ColumnIndex >= m_DataModel.GetColumnsSchema().Count)
                {
                    return;
                }

                if (m_DataModel.GetDataSource() == null)
                {
                    DataTable _dtDataSource = m_DataModel.GetColumnsSchemaToDataSource();
                    m_DataModel.SetDataSource(_dtDataSource);
                    m_DataModel.InsertAt(e.RowIndex);
                }
                else
                {
                    if (e.RowIndex == m_DataModel.GetDataSource().Rows.Count)
                    {
                        m_DataModel.InsertAt(e.RowIndex);
                    }
                }

                var _Column = m_DataModel.GetColumnsSchema().Where(x => x.ColumnIndex == e.ColumnIndex).Select(s => s.DataPropertyName).SingleOrDefault();
                if (_Column == null || _Column == string.Empty || _Column.Count() <= 0)
                {
                    return;
                }

                if (m_DataModel.GetDataSource().Columns.Contains(_Column))
                {
                    m_DataModel.GetDataSource().Rows[e.RowIndex][_Column] = e.Value;
                }

                base.OnCellValuePushed(e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnCellValueNeeded(DataGridViewCellValueEventArgs e)
        {
            try
            {
                if (e.RowIndex >= RowCount)
                {
                    return;
                }

                if (m_DataModel.GetDataSource() == null || e.RowIndex >= m_DataModel.GetDataSource().Rows.Count || e.ColumnIndex >= m_DataModel.GetColumnsSchema().Count)
                {
                    return;
                }

                var _Column = m_DataModel.GetColumnsSchema().Where(x => x.ColumnIndex == e.ColumnIndex).Select(s => s.DataPropertyName).SingleOrDefault();
                if (_Column == null || _Column == string.Empty || _Column.Count() <= 0)
                {
                    return;
                }

                if (m_DataModel.GetDataSource().Columns.Contains(_Column))
                {
                    e.Value = m_DataModel.GetDataSource().Rows[e.RowIndex][_Column];
                }

                base.OnCellValueNeeded(e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (Rows[e.RowIndex].Selected)
                {
                    // 선택 행 굵은 폰트
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                }

                base.OnCellFormatting(e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnCellClick(DataGridViewCellEventArgs e)
        {
            try
            {
                base.OnCellClick(e);

                if (e.RowIndex >= 0)
                {
                    // 최초 클릭 여부
                    m_IsRowClicked = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            try
            {
                base.OnGotFocus(e);

                if (!m_IsRowClicked && !IsSelectRowAfterDataBinding)
                {
                    ClearRowSelection();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnColumnHeaderMouseClick(DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (!m_IsRowClicked)
                {
                    ClearSelection();
                }

                if (!IsSortable)
                {
                    return;
                }

                if (e.Location.X > (Columns[e.ColumnIndex].Width - m_iSortOrFilterBounds))
                {
                    if (m_DataModel is ISortHandler)
                    {
                        var _Column = m_DataModel.GetColumnsSchema().Where(x => x.ColumnIndex == e.ColumnIndex).Select(s => s).SingleOrDefault();
                        if (_Column == null)
                        {
                            return;
                        }

                        ISortHandler _SortHandler = (m_DataModel as ISortHandler);

                        Dictionary<string, string> _dicTag = (Dictionary<string, string>)_Column.Tag;
                        if (string.Compare(_dicTag["data_type"].ToString(), "decimal").Equals(0))
                        {
                            _SortHandler.Sort(e.ColumnIndex, _Column.DataPropertyName, e.Button == MouseButtons.Left ? true : false, typeof(decimal));
                        }
                        else
                        {
                            _SortHandler.Sort(e.ColumnIndex, _Column.DataPropertyName, e.Button == MouseButtons.Left ? true : false, _Column.DataType);
                        }

                        /* [?] - Refresh? Invalidate? 뭐가 나을지?
                        Refresh();
                        */
                        Invalidate();
                    }
                }
                else if (e.Button.Equals(MouseButtons.Left) && e.Location.X < m_iSortOrFilterBounds)
                {
                    /* [미구현] - Filter 
                    if (m_DataModel is IFilterHandler)
                    {
                        var _ColumnName = m_DataModel.GetColumnsSchema().Where(x => x.ColumnIndex == e.ColumnIndex).Select(s => s.DataPropertyName).SingleOrDefault();
                        if (_ColumnName == null || _ColumnName == string.Empty)
                        {
                            return;
                        }

                        if (!m_DataModel.IsBindingDataSource())
                        {
                            return;
                        }

                        //string[] _Values = m_DataModel.GetDataSource().AsEnumerable().GroupBy(x => x[_ColumnName].ToString()).Select(s => s.Key).ToArray();
                        //int _iTotalWidth = (from c in Columns.Cast<DataGridViewColumn>().AsEnumerable() where c.Visible && c.Index >= FirstDisplayedScrollingColumnIndex && c.Index < e.ColumnIndex select c.Width).Sum();
                        //FilterSelector _FilterSelector = new FilterSelector()
                        //{
                        //    StartPosition = FormStartPosition.Manual,
                        //    Location = PointToScreen(new Point(_iTotalWidth + e.Location.X, e.Location.Y))
                        //};
                        //_FilterSelector.AddList(_Values);

                        //if (_FilterSelector.ShowDialog().Equals(DialogResult.OK))
                        //{
                        //    ClearRowSelection();

                        //    IFilterHandler _FilterHandler = m_DataModel as IFilterHandler;
                        //    RowCount = _FilterHandler.Filter(e.ColumnIndex, _ColumnName, _FilterSelector.m_SelectFiled);

                        //    Refresh();
                        //}
                    }
                    */
                }

                base.OnColumnHeaderMouseClick(e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnUserDeletingRow(DataGridViewRowCancelEventArgs e)
        {
            try
            {
                if (e.Row.Index >= RowCount)
                {
                    return;
                }

                m_DataModel.RemoveAt(e.Row.Index);

                base.OnUserDeletingRow(e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ClearRowSelection()
        {
            try
            {
                ClearSelection();
                CurrentCell = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Reset()
        {
            try
            {
                if (Columns == null)
                {
                    return;
                }
                Columns.Clear();
                m_DataModel.Reset();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ClearDataSource()
        {
            try
            {
                m_IsRowClicked = false;
                m_DataModel.ClearDataSource();
                DisplayDataSource();
                /* [?] - Refresh? Invalidate? 뭐가 나을지?
                Refresh();
                */
                Invalidate();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertAt(int rowIdx, DataRow row)
        {
            try
            {
                m_DataModel.InsertAt(rowIdx, row);

                if (AllowUserToAddRows)
                {
                    RowCount = (m_DataModel.GetDataSource().Rows.Count + 1);
                }
                else
                {
                    RowCount = m_DataModel.GetDataSource().Rows.Count;
                }

                /* [?] - Refresh? Invalidate? 뭐가 나을지?
                Refresh();
                */
                Invalidate();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RemoveAt(int rowIdx)
        {
            try
            {
                if (AllowUserToAddRows)
                {
                    RowCount = m_DataModel.GetDataSource().Rows.Count;
                }
                else
                {
                    RowCount = (m_DataModel.GetDataSource().Rows.Count - 1);
                }

                m_DataModel.RemoveAt(rowIdx);

                /* [?] - Refresh? Invalidate? 뭐가 나을지?
                Refresh();
                */
                Invalidate();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetColumnAutoResize()
        {
            try
            {
                foreach (ColumnSchema _Column in GetColumns())
                {
                    AutoResizeColumn(_Column.ColumnIndex);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetColumnSizeChange(Dictionary<string, int> columns)
        {
            try
            {
                if (Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, int> _Column in columns)
                    {
                        if (Columns.Contains(_Column.Key))
                        {
                            Columns[_Column.Key].Width = _Column.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetColumnReadOnly(string columnName, bool isReadOnly)
        {
            try
            {
                if (Columns.Count > 0 && Columns.Contains(columnName))
                {
                    Columns[columnName].ReadOnly = isReadOnly;
                    m_DataModel.GetColumnsSchema()[Columns[columnName].Index].IsReadOnly = isReadOnly;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetColumnReadOnly(Dictionary<string, bool> columns)
        {
            try
            {
                if (Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, bool> _Column in columns)
                    {
                        if (Columns.Contains(_Column.Key))
                        {
                            Columns[_Column.Key].ReadOnly = _Column.Value;
                            m_DataModel.GetColumnsSchema()[Columns[_Column.Key].Index].IsReadOnly = _Column.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetColumnFrozen(int columnIdx)
        {
            try
            {
                foreach (DataGridViewColumn _Column in Columns)
                {
                    _Column.Frozen = false;
                }
                Columns[columnIdx].Frozen = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetColumnFrozen(string columnName)
        {
            try
            {
                var _Column = GetColumn(columnName);
                SetColumnFrozen(_Column.ColumnIndex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DataGridViewColumn> GetVisibleColumns()
        {
            try
            {
                List<DataGridViewColumn> _Columns = new List<DataGridViewColumn>();
                foreach (DataGridViewColumn _dgvcColumn in Columns)
                {
                    if (_dgvcColumn.Visible)
                    {
                        _Columns.Add(_dgvcColumn);
                    }
                }
                return _Columns;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetVisibleColumnCount()
        {
            try
            {
                int _iCount = 0;
                List<ColumnSchema> _ColumnsSchema = GetColumns();
                if (_ColumnsSchema != null)
                {
                    foreach (ColumnSchema _Column in _ColumnsSchema)
                    {
                        if (_Column.IsVisible)
                        {
                            _iCount++;
                        }
                    }
                }
                return _iCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /* [?] - 적용 가능 여부?
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams _CreateParams = base.CreateParams;
                _CreateParams.ExStyle = _CreateParams.ExStyle | NativeMethods.WS_EX_COMPOSITED;
                return _CreateParams;
            }
        }
        */
        #endregion
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class HeaderProperties
    {
        public HeaderProperties()
        {
        }

        public override string ToString()
        {
            return string.Empty;
        }

        private Font m_HeaderFont = new Font("맑은 고딕", 9.75F, FontStyle.Bold);
        [DefaultValue(typeof(Font), "맑은 고딕, 9.75pt, style=Bold")]
        public Font Font
        {
            get { return m_HeaderFont; }
            set
            {
                m_HeaderFont = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.HeaderProperties.HeaderFont.ToString());
            }
        }

        private bool m_IsVisibleSelectedRowNumber = false;
        [DefaultValue(false)]
        public bool IsVisibleSelectedRowNumber
        {
            get { return m_IsVisibleSelectedRowNumber; }
            set
            {
                m_IsVisibleSelectedRowNumber = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.HeaderProperties.IsVisibleSelectedRowNumber.ToString());
            }
        }

        private Color m_HeaderBorderColor = SystemColors.ControlDark;
        [DefaultValue(typeof(Color), "ControlDark")]
        public Color BorderColor
        {
            get { return m_HeaderBorderColor; }
            set
            {
                m_HeaderBorderColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.HeaderProperties.HeaderBorderColor.ToString());
            }
        }

        private Color m_ColumnHeaderFirstBackColor = SystemColors.Control;
        //private Color m_ColumnHeaderFirstBackColor = Color.FromArgb(43, 43, 43);
        [DefaultValue(typeof(Color), "Control")]
        public Color ColumnFirstBackColor
        {
            get { return m_ColumnHeaderFirstBackColor; }
            set
            {
                m_ColumnHeaderFirstBackColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.HeaderProperties.ColumnFirstBackColor.ToString());
            }
        }

        private Color m_ColumnHeaderLastBackColor = SystemColors.Control;
        [DefaultValue(typeof(Color), "Control")]
        public Color ColumnLastBackColor
        {
            get { return m_ColumnHeaderLastBackColor; }
            set
            {
                m_ColumnHeaderLastBackColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.HeaderProperties.ColumnLastBackColor.ToString());
            }
        }

        private Color m_ColumnHeaderForeColor = Color.Gainsboro;
        [DefaultValue(typeof(Color), "WindowText")]
        public Color ColumnForeColor
        {
            get { return m_ColumnHeaderForeColor; }
            set
            {
                m_ColumnHeaderForeColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.HeaderProperties.ColumnForeColor.ToString());
            }
        }

        private Color m_RowHeaderFirstBackColor = SystemColors.Control;
        [DefaultValue(typeof(Color), "Control")]
        public Color RowFirstBackColor
        {
            get { return m_RowHeaderFirstBackColor; }
            set
            {
                m_RowHeaderFirstBackColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.HeaderProperties.RowFirstBackColor.ToString());
            }
        }

        private Color m_RowHeaderLastBackColor = SystemColors.Control;
        [DefaultValue(typeof(Color), "Control")]
        public Color RowLastBackColor
        {
            get { return m_RowHeaderLastBackColor; }
            set
            {
                m_RowHeaderLastBackColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.HeaderProperties.RowLastBackColor.ToString());
            }
        }

        private Color m_RowHeaderForeColor = Color.Gainsboro;
        [DefaultValue(typeof(Color), "WindowText")]
        public Color RowForeColor
        {
            get { return m_RowHeaderForeColor; }
            set
            {
                m_RowHeaderForeColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.HeaderProperties.RowForeColor.ToString());
            }
        }

        private Color m_RowHeaderSelectionFirstBackColor = SystemColors.Highlight;
        [DefaultValue(typeof(Color), "Highlight")]
        public Color RowSelectionFirstBackColor
        {
            get { return m_RowHeaderSelectionFirstBackColor; }
            set
            {
                m_RowHeaderSelectionFirstBackColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.HeaderProperties.RowSelectionFirstBackColor.ToString());
            }
        }

        private Color m_RowHeaderSelectionLastBackColor = SystemColors.Highlight;
        [DefaultValue(typeof(Color), "Highlight")]
        public Color RowSelectionLastBackColor
        {
            get { return m_RowHeaderSelectionLastBackColor; }
            set
            {
                m_RowHeaderSelectionLastBackColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.HeaderProperties.RowSelectionLastBackColor.ToString());
            }
        }

        private Color m_RowHeaderSelectionForeColor = SystemColors.HighlightText;
        [DefaultValue(typeof(Color), "HighlightText")]
        public Color RowSelectionForeColor
        {
            get { return m_RowHeaderSelectionForeColor; }
            set
            {
                m_RowHeaderSelectionForeColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.HeaderProperties.RowSelectionForeColor.ToString());
            }
        }

        private Color m_NCHeaderFirstBackColor = SystemColors.Control;
        [DefaultValue(typeof(Color), "Control")]
        public Color NCFirstBackColor
        {
            get { return m_NCHeaderFirstBackColor; }
            set
            {
                m_NCHeaderFirstBackColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.HeaderProperties.NCFirstBackColor.ToString());
            }
        }

        private Color m_NCHeaderLastBackColor = SystemColors.Control;
        [DefaultValue(typeof(Color), "Control")]
        public Color NCLastBackColor
        {
            get { return m_NCHeaderLastBackColor; }
            set
            {
                m_NCHeaderLastBackColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.HeaderProperties.NCLastBackColor.ToString());
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
    public class CellProperties
    {
        public CellProperties()
        {
        }

        public override string ToString()
        {
            return string.Empty;
        }

        private Font m_CellFont = new Font("맑은 고딕", 9F);
        [DefaultValue(typeof(Font), "맑은 고딕, 9pt")]
        public Font Font
        {
            get { return m_CellFont; }
            set
            {
                m_CellFont = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.CellProperties.CellFont.ToString());
            }
        }

        private bool m_IsCheckBoxCellInSquares = true;
        [DefaultValue(true)]
        public bool IsCheckBoxCellInSquares
        {
            get { return m_IsCheckBoxCellInSquares; }
            set
            {
                m_IsCheckBoxCellInSquares = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.CellProperties.IsCheckBoxCellInSquares.ToString());
            }
        }

        private Color m_CellSelectionBackColor = SystemColors.Highlight;
        [DefaultValue(typeof(Color), "Highlight")]
        public Color SelectionBackColor
        {
            get { return m_CellSelectionBackColor; }
            set
            {
                m_CellSelectionBackColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.CellProperties.CellSelectionBackColor.ToString());
            }
        }

        private Color m_CellSelectionForeColor = SystemColors.HighlightText;
        [DefaultValue(typeof(Color), "HighlightText")]
        public Color SelectionForeColor
        {
            get { return m_CellSelectionForeColor; }
            set
            {
                m_CellSelectionForeColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.CellProperties.CellSelectionForeColor.ToString());
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