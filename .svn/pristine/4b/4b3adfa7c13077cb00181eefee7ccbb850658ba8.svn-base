using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace NCManagementSystem.Components.Helpers
{
    public class ConvertHelper
    {
        public static string ConvertToDecimal(string value, bool isCommaSeparator, bool isFloatingPoint, int dotPostLength)
        {
            try
            {
                if (!decimal.TryParse(value, out decimal _mValue))
                {
                    _mValue = 0M;
                }

                if (isFloatingPoint)
                {
                    if (isCommaSeparator)
                    {
                        if (_mValue.Equals(0))
                        {
                            string _sFormat = ("0.").PadRight((2 + dotPostLength), '0');
                            return _mValue.ToString(_sFormat);
                        }
                        else
                        {
                            string _sFormat = ("#,0.").PadRight((4 + dotPostLength), '0');
                            return _mValue.ToString(_sFormat);
                        }
                    }
                    else
                    {
                        string _sFormat = ("0.").PadRight((2 + dotPostLength), '0');
                        return _mValue.ToString(_sFormat);
                    }
                }
                else
                {
                    if (_mValue.Equals(0))
                    {
                        string _sFormat = ("0.").PadRight((2 + dotPostLength), '#');
                        return _mValue.ToString(_sFormat);
                    }
                    else
                    {
                        if (isCommaSeparator)
                        {
                            string _sFormat = ("#,0.").PadRight((4 + dotPostLength), '#');
                            return _mValue.ToString(_sFormat);
                        }
                        else
                        {
                            string _sFormat = ("0.").PadRight((2 + dotPostLength), '#');
                            return _mValue.ToString(_sFormat);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable ConvertToDataTable<T>(List<T> items)
        {
            try
            {
                DataTable _dtConverted = new DataTable(typeof(T).Name);
                PropertyInfo[] _PropertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo _PropertyInfo in _PropertyInfos)
                {
                    _dtConverted.Columns.Add(_PropertyInfo.Name);
                }
                foreach (T _Item in items)
                {
                    var _Values = new object[_PropertyInfos.Length];
                    for (int iIdx = 0; iIdx < _PropertyInfos.Length; iIdx++)
                    {
                        _Values[iIdx] = _PropertyInfos[iIdx].GetValue(_Item, null);
                    }
                    _dtConverted.Rows.Add(_Values);
                }
                return _dtConverted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<T> ConvertToList<T>(DataTable dataTable)
        {
            try
            {
                var _Column = dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
                var _Properties = typeof(T).GetProperties();
                return dataTable.AsEnumerable().Select(r =>
                {
                    var _TGeneric = Activator.CreateInstance<T>();
                    foreach (var _Property in _Properties)
                    {
                        if (_Column.Contains(_Property.Name.ToLower()))
                        {
                            if (string.Compare(_Property.PropertyType.Name, "Boolean", false).Equals(0))
                            {
                                continue;
                            }
                            if (string.IsNullOrEmpty(r[_Property.Name].ToString()))
                            {
                                continue;
                            }
                            _Property.SetValue(_TGeneric, r[_Property.Name]);
                        }
                    }
                    return _TGeneric;
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
