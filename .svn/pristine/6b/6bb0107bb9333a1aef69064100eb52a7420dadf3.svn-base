using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NCManagementSystem.Libraries.Controls.DataGridView.Components
{
    public abstract class BaseDataModel
    {
        public BaseDataModel(FwDataGridView owner)
        {
            m_Owner = owner;
        }

        protected FwDataGridView m_Owner;

        protected List<ColumnSchema> m_ColumnsSchema { get; set; }

        protected DataTable m_dtDataSource { get; set; }
        protected DataTable m_dtHandledDataSource { get; set; }
        
        public void SetDataSource(DataTable dataTable)
        {
            try
            {
                m_dtDataSource = dataTable;
                m_dtHandledDataSource = dataTable.Copy();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDataSource()
        {
            return m_dtHandledDataSource;
        }

        public void SetColumnsSchema(List<ColumnSchema> columnsSchema)
        {
            try
            {
                m_ColumnsSchema = new List<ColumnSchema>();
                m_ColumnsSchema.AddRange(columnsSchema);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ColumnSchema> GetColumnsSchema()
        {
            return m_ColumnsSchema;
        }

        public DataTable GetColumnsSchemaToDataSource()
        {
            try
            {
                DataTable _dtDataSource = null;
                if (m_ColumnsSchema == null)
                {
                    return _dtDataSource;
                }
                else
                {
                    _dtDataSource = new DataTable();
                    foreach (ColumnSchema _Column in m_ColumnsSchema)
                    {
                        _dtDataSource.Columns.Add(_Column.DataPropertyName, _Column.DataType);
                    }
                    return _dtDataSource.Clone();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void Reset()
        {
            try
            {
                if (m_dtDataSource != null || m_dtHandledDataSource != null || m_ColumnsSchema != null)
                {
                    m_dtDataSource = null;
                    m_dtHandledDataSource = null;
                    m_ColumnsSchema.Clear();
                    m_ColumnsSchema = null;
                }
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
                if (m_dtDataSource != null || m_dtHandledDataSource != null)
                {
                    m_dtDataSource.Clear();
                    m_dtHandledDataSource.Clear();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RefreshDataSource()
        {
            try
            {
                if (!IsBindingDataSource())
                {
                    return;
                }

                m_dtHandledDataSource = m_dtDataSource.Copy();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsBindingDataSource()
        {
            if (m_dtHandledDataSource == null || m_dtHandledDataSource.Rows == null || m_dtHandledDataSource.Rows.Count <= 0)
            {
                return false;
            }
            return true;
        }

        internal void InsertAt(int rowIdx)
        {
            try
            {
                DataRow _drRow = m_dtHandledDataSource.NewRow();
                foreach (ColumnSchema _Column in GetColumnsSchema())
                {
                    if (_Column.DataType == typeof(string))
                    {
                        _drRow[_Column.DataPropertyName] = "";
                    }
                }
                m_dtHandledDataSource.Rows.InsertAt(_drRow, rowIdx);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        internal void InsertAt(int rowIdx, DataRow row)
        {
            try
            {
                m_dtHandledDataSource.Rows.InsertAt(row, rowIdx);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        internal void RemoveAt(int rowIdx)
        {
            try
            {
                if (rowIdx >= m_dtHandledDataSource.Rows.Count)
                {
                    return;
                }

                m_dtHandledDataSource.Rows.RemoveAt(rowIdx);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }

    public class DataModel : BaseDataModel, ISortHandler
    {
        public DataModel(FwDataGridView owner) : base(owner)
        {
        }

        public void Sort(int columnIdx, string columnName, bool isDesc, Type type)
        {
            try
            {
                if (!IsBindingDataSource())
                {
                    return;
                }

                if (type == typeof(int))
                {
                    if (!isDesc)
                    {
                        m_dtHandledDataSource = m_dtHandledDataSource.AsEnumerable().Select(x => x).OrderBy(x => int.Parse(x[columnName].ToString()), new IntegerComparer()).CopyToDataTable();
                    }
                    else
                    {
                        m_dtHandledDataSource = m_dtHandledDataSource.AsEnumerable().Select(x => x).OrderByDescending(x => int.Parse(x[columnName].ToString()), new IntegerComparer()).CopyToDataTable();
                    }
                }
                else if (type == typeof(double))
                {
                    if (!isDesc)
                    {
                        m_dtHandledDataSource = m_dtHandledDataSource.AsEnumerable().Select(x => x).OrderBy(x => double.Parse(x[columnName].ToString()), new DoubleComparer()).CopyToDataTable();
                    }
                    else
                    {
                        m_dtHandledDataSource = m_dtHandledDataSource.AsEnumerable().Select(x => x).OrderByDescending(x => double.Parse(x[columnName].ToString()), new DoubleComparer()).CopyToDataTable();
                    }
                }
                else if (type == typeof(decimal))
                {
                    if (!isDesc)
                    {
                        m_dtHandledDataSource = m_dtHandledDataSource.AsEnumerable().Select(x => x).OrderBy(x => decimal.Parse(x[columnName].ToString()), new DecimalComparer()).CopyToDataTable();
                    }
                    else
                    {
                        m_dtHandledDataSource = m_dtHandledDataSource.AsEnumerable().Select(x => x).OrderByDescending(x => decimal.Parse(x[columnName].ToString()), new DecimalComparer()).CopyToDataTable();
                    }
                }
                else
                {
                    if (!isDesc)
                    {
                        m_dtHandledDataSource = m_dtHandledDataSource.AsEnumerable().Select(x => x).OrderBy(x => x[columnName].ToString()).CopyToDataTable();
                    }
                    else
                    {
                        m_dtHandledDataSource = m_dtHandledDataSource.AsEnumerable().Select(x => x).OrderByDescending(x => x[columnIdx].ToString()).CopyToDataTable();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public interface ISortHandler
    {
        void Sort(int columnIdx, string columnName, bool isDesc, Type type);
    }

    public class IntegerComparer : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            if (x < y)
            {
                return -1;
            }
            else if (x > y)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }

    public class DoubleComparer : IComparer<double>
    {
        public int Compare(double x, double y)
        {
            if (x < y)
            {
                return -1;
            }
            else if (x > y)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }

    public class DecimalComparer : IComparer<decimal>
    {
        public int Compare(decimal x, decimal y)
        {
            if (x < y)
            {
                return -1;
            }
            else if (x > y)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}