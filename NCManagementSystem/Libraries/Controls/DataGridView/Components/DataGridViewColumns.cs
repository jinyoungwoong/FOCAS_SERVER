using System;
using System.Collections.Generic;
using System.Linq;
using NCManagementSystem.Libraries.Controls.DataGridView.Renderers;

namespace NCManagementSystem.Libraries.Controls.DataGridView.Components
{
    public class ColumnSchema : ICloneable
    {
        public int ColumnIndex { get; set; }
        public string ColumnName { get; set; }
        public string DataPropertyName { get; set; }
        public Type DataType { get; set; }
        public object Tag { get; set; }
        public DataGridViewConstsDefiner.ColumnAlignment ColumnAlignment { get; set; }
        public int ColumnWidth { get; set; }
        public bool IsVisible { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsCheckBoxCell { get; set; }
        public string KeyedName { get; set; }

        private ColumnBaseSchemaRenderer m_ColumnBaseSchemaRenderer;
        public ColumnBaseSchemaRenderer ColumnBaseSchemaRenderer
        {
            get { return m_ColumnBaseSchemaRenderer; }
            set
            {
                value.SwitchColumnAlignment(ColumnAlignment);
                m_ColumnBaseSchemaRenderer = value;
            }
        }
        
        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    public class ColumnSchemaFactory : ICloneable
    {
        public ColumnSchemaFactory()
        {
            m_iColumnIndex = -1;
            m_ColumnsSchema = new List<ColumnSchema>();
        }

        public ColumnSchemaFactory(FwDataGridView owner)
        {
            m_iColumnIndex = -1;
            m_ColumnsSchema = new List<ColumnSchema>();
            m_Owner = owner;
        }

        internal readonly int FixedColumnWidth = 100;
        internal readonly string PrefixRelatedPropertyName = "_R";
        public int m_iColumnIndex = -1;
        public List<ColumnSchema> m_ColumnsSchema;
        private FwDataGridView m_Owner;

        public ColumnSchemaFactory Create(string columnName)
        {
            m_ColumnsSchema.Add(CreateSchema(columnName, columnName, false, typeof(string), null, DataGridViewConstsDefiner.ColumnAlignment.left, FixedColumnWidth, true, true));
            return this;
        }

        public ColumnSchemaFactory Create(string columnName, string dataPropertyName)
        {
            m_ColumnsSchema.Add(CreateSchema(columnName, dataPropertyName, false, typeof(string), null, DataGridViewConstsDefiner.ColumnAlignment.left, FixedColumnWidth, true, true));
            return this;
        }

        public ColumnSchemaFactory Create(string columnName, string dataPropertyName, DataGridViewConstsDefiner.ColumnAlignment columnAlignment, bool isReadOnly)
        {
            m_ColumnsSchema.Add(CreateSchema(columnName, dataPropertyName, false, typeof(string), null, columnAlignment, FixedColumnWidth, true, isReadOnly));
            return this;
        }

        public ColumnSchemaFactory Create(string columnName, string dataPropertyName, DataGridViewConstsDefiner.ColumnAlignment columnAlignment, bool isVisible, bool isReadOnly)
        {
            m_ColumnsSchema.Add(CreateSchema(columnName, dataPropertyName, false, typeof(string), null, columnAlignment, FixedColumnWidth, isVisible, isReadOnly));
            return this;
        }

        public ColumnSchemaFactory Create(string columnName, string dataPropertyName, Type dataType)
        {
            m_ColumnsSchema.Add(CreateSchema(columnName, dataPropertyName, false, dataType, null, DataGridViewConstsDefiner.ColumnAlignment.left, FixedColumnWidth, true, true));
            return this;
        }

        public ColumnSchemaFactory Create(string columnName, string dataPropertyName, Type dataType, DataGridViewConstsDefiner.ColumnAlignment columnAlignment)
        {
            m_ColumnsSchema.Add(CreateSchema(columnName, dataPropertyName, false, dataType, null, columnAlignment, FixedColumnWidth, true, true));
            return this;
        }

        public ColumnSchemaFactory Create(string columnName, string dataPropertyName, Type dataType, bool isReadOnly)
        {
            m_ColumnsSchema.Add(CreateSchema(columnName, dataPropertyName, false, dataType, null, DataGridViewConstsDefiner.ColumnAlignment.left, FixedColumnWidth, true, isReadOnly));
            return this;
        }

        public ColumnSchemaFactory Create(string columnName, string dataPropertyName, Type dataType, bool isVisible, bool isReadOnly)
        {
            m_ColumnsSchema.Add(CreateSchema(columnName, dataPropertyName, false, dataType, null, DataGridViewConstsDefiner.ColumnAlignment.left, FixedColumnWidth, isVisible, isReadOnly));
            return this;
        }
        
        public ColumnSchemaFactory Create(string columnName, string dataPropertyName, object tag, DataGridViewConstsDefiner.ColumnAlignment columnAlignment, int columnWidth, bool isVisible)
        {
            m_ColumnsSchema.Add(CreateSchema(columnName, dataPropertyName, false, typeof(string), tag, columnAlignment, columnWidth, isVisible, true));
            return this;
        }

        public ColumnSchemaFactory Create(string columnName, string dataPropertyName, bool isCheckBoxCell, object tag, DataGridViewConstsDefiner.ColumnAlignment columnAlignment, int columnWidth, bool isVisible)
        {
            m_ColumnsSchema.Add(CreateSchema(columnName, dataPropertyName, isCheckBoxCell, typeof(string), tag, columnAlignment, columnWidth, isVisible, true));
            return this;
        }

        public ColumnSchemaFactory Create(string columnName, string dataPropertyName, bool isCheckBoxCell, object tag, DataGridViewConstsDefiner.ColumnAlignment columnAlignment, int columnWidth, bool isVisible, bool isReadOnly)
        {
            m_ColumnsSchema.Add(CreateSchema(columnName, dataPropertyName, isCheckBoxCell, typeof(string), tag, columnAlignment, columnWidth, isVisible, isReadOnly));
            return this;
        }

        public ColumnSchemaFactory Create(string columnName, string dataPropertyName, object tag, DataGridViewConstsDefiner.ColumnAlignment columnAlignment, int columnWidth, bool isVisible, bool isReadOnly)
        {
            m_ColumnsSchema.Add(CreateSchema(columnName, dataPropertyName, false, typeof(string), tag, columnAlignment, columnWidth, isVisible, isReadOnly));
            return this;
        }

        public ColumnSchemaFactory Create(string columnName, Type dataType, bool isVisible)
        {
            m_ColumnsSchema.Add(CreateSchema(columnName, columnName, false, dataType, null, DataGridViewConstsDefiner.ColumnAlignment.left, 100, isVisible, true));
            return this;
        }

        public ColumnSchemaFactory Create(string columnName, string dataPropertyName, DataGridViewConstsDefiner.ColumnAlignment columnAlignment, int columnWidth, bool isVisible, bool isReadOnly)
        {
            m_ColumnsSchema.Add(CreateSchema(columnName, dataPropertyName, false, typeof(string), null, columnAlignment, columnWidth, isVisible, isReadOnly));
            return this;
        }

        private ColumnSchema CreateSchema(string columnName, string dataPropertyName, bool isCheckBoxCell, Type dataType, object tag, DataGridViewConstsDefiner.ColumnAlignment columnAlignment, int columnWidth, bool isVisible, bool isReadOnly)
        {
            ColumnSchema _Column = new ColumnSchema()
            {
                ColumnIndex = ++m_iColumnIndex,
                ColumnName = columnName,
                DataPropertyName = dataPropertyName,
                DataType = dataType,
                Tag = tag,
                ColumnAlignment = columnAlignment,
                ColumnWidth = columnWidth,
                IsVisible = isVisible,
                IsReadOnly = isReadOnly,
                IsCheckBoxCell = isCheckBoxCell
            };
            return _Column;
        }

        public List<ColumnSchema> ToList()
        {
            return new List<ColumnSchema>(m_ColumnsSchema);
        }

        public ColumnSchema GetColumnSchema(string dataPropertyName)
        {
            foreach (ColumnSchema _Column in m_ColumnsSchema)
            {
                if (string.Compare(_Column.DataPropertyName, dataPropertyName, false).Equals(0))
                {
                    return _Column;
                }
            }
            return null;
        }

        public void SetVisibleColumnSchema(Dictionary<string, bool> properties)
        {
            if (properties != null && properties.Count > 0)
            {
                foreach(KeyValuePair<string, bool> _Property in properties)
                {
                    ColumnSchema _ColumnSchema = GetColumnSchema(_Property.Key);
                    if(_ColumnSchema != null)
                    {
                        _ColumnSchema.IsVisible = _Property.Value;
                    }
                }
            }
        }

        public void SetVisibleColumnSchema(Dictionary<string, bool> properties, Dictionary<string, bool> relatedProperties)
        {
            if (properties != null && properties.Count > 0)
            {
                foreach (KeyValuePair<string, bool> _Property in properties)
                {
                    ColumnSchema _ColumnSchema = GetColumnSchema(_Property.Key);
                    if (_ColumnSchema != null)
                    {
                        _ColumnSchema.IsVisible = _Property.Value;
                    }
                }
            }

            if (relatedProperties != null && relatedProperties.Count > 0)
            {
                foreach (KeyValuePair<string, bool> _Property in relatedProperties)
                {
                    ColumnSchema _ColumnSchema = GetColumnSchema(string.Concat(PrefixRelatedPropertyName, _Property.Key));
                    if (_ColumnSchema != null)
                    {
                        _ColumnSchema.IsVisible = _Property.Value;
                    }
                }
            }
        }

        public void SetColumnSchemaLabelRename(Dictionary<string, string> properties)
        {
            if (properties != null && properties.Count > 0)
            {
                foreach (KeyValuePair<string, string> _Property in properties)
                {
                    ColumnSchema _ColumnSchema = GetColumnSchema(_Property.Key);
                    if (_ColumnSchema != null)
                    {
                        _ColumnSchema.ColumnName = _Property.Value;
                    }
                }
            }
        }

        public void SetDateTimePatten(Dictionary<string, string> properties)
        {
            if (properties != null && properties.Count > 0)
            {
                foreach (KeyValuePair<string, string> _Property in properties)
                {
                    ColumnSchema _ColumnSchema = GetColumnSchema(_Property.Key);
                    if (_ColumnSchema != null)
                    {
                        (_ColumnSchema.Tag as Dictionary<string, string>)["pattern"] = _Property.Value;
                    }
                }
            }
        }

        public void SetKeyedName(string dataPropertyName, string keyedName)
        {
            ColumnSchema _ColumnSchema = GetColumnSchema(dataPropertyName);
            if (_ColumnSchema != null)
            {
                _ColumnSchema.KeyedName = keyedName;
            }
        }

        public object Clone()
        {
            ColumnSchemaFactory _CloneObject = new ColumnSchemaFactory();
            var _CloneMemberwise = new List<ColumnSchema>(m_ColumnsSchema.Select(x => (ColumnSchema)x.Clone()));
            _CloneObject.m_ColumnsSchema = _CloneMemberwise;
            return _CloneObject;
        }
    }
}
