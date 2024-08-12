using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace NCManagementSystem.Components.Helpers
{
    public class ConfigureBuilder
    {
        #region [ Constructor ]
        public ConfigureBuilder()
        {
        }
        #endregion

        #region [ Member Variables / Fields / Properties / EventHandler ]
        public string FilePath { get; set; } = Application.StartupPath;
        public string FileName { get; set; } = "AppConfigure.xml";
        #endregion

        #region [ Override Events / Events / Methods ]
        public Hashtable LoadConfigure()
        {
            try
            {
                string _sFullFilePath = Path.Combine(FilePath, FileName);

                Hashtable _htConfigure = new Hashtable();

                XmlDocument _XmlDocument = new XmlDocument();
                _XmlDocument.Load(_sFullFilePath);

                XmlNode _Elements = _XmlDocument.DocumentElement;
                foreach (XmlNode _Element in _Elements.ChildNodes)
                {
                    if (_Element.NodeType.Equals(XmlNodeType.Element))
                    {
                        if (_Element.HasChildNodes)
                        {
                            Hashtable _htSubConfigure = new Hashtable();

                            XmlNodeList _SubElements = _Element.ChildNodes;
                            foreach (XmlNode _SubElement in _SubElements)
                            {
                                if (_SubElement.NodeType.Equals(XmlNodeType.Element))
                                {
                                    Dictionary<string, string> _Attribute = new Dictionary<string, string>();

                                    XmlAttributeCollection _XmlAttributes = _SubElement.Attributes;
                                    foreach (XmlAttribute _XmlAttribute in _XmlAttributes)
                                    {
                                        _Attribute.Add(_XmlAttribute.LocalName, _XmlAttribute.Value);
                                    }

                                    _htSubConfigure.Add(_SubElement.LocalName, _Attribute);
                                }
                            }

                            _htConfigure.Add(_Element.LocalName, _htSubConfigure);
                        }
                        else
                        {
                            Dictionary<string, string> _Attribute = new Dictionary<string, string>();

                            XmlAttributeCollection _XmlAttributes = _Element.Attributes;
                            foreach (XmlAttribute _XmlAttribute in _XmlAttributes)
                            {
                                _Attribute.Add(_XmlAttribute.LocalName, _XmlAttribute.Value);
                            }

                            _htConfigure.Add(_Element.LocalName, _Attribute);
                        }
                    }
                }

                return _htConfigure;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveConfigure(Hashtable parameters)
        {
            XmlTextWriter _XmlTextWriter = null;

            try
            {
                if (IsExistsConfigureFile())
                {
                    string _sFullFilePath = Path.Combine(FilePath, FileName);

                    XmlDocument _XmlDocument = new XmlDocument();
                    _XmlDocument.Load(_sFullFilePath);

                    ArrayList _StoredElements = new ArrayList();
                    ArrayList _StoredSubElements = new ArrayList();

                    XmlNode _Elements = _XmlDocument.DocumentElement;
                    foreach (XmlNode _Element in _Elements.ChildNodes)
                    {
                        if (_Element.NodeType.Equals(XmlNodeType.Element))
                        {
                            if (_Element.HasChildNodes)
                            {
                                if (parameters.ContainsKey(_Element.Name))
                                {
                                    _StoredElements.Add(_Element.Name);

                                    Hashtable _htSubElements = (parameters[_Element.Name] as Hashtable);
                                    XmlNodeList _SubElements = _Element.ChildNodes;
                                    foreach (XmlNode _SubElement in _SubElements)
                                    {
                                        if (_SubElement.NodeType.Equals(XmlNodeType.Element))
                                        {
                                            if (_htSubElements.ContainsKey(_SubElement.Name))
                                            {
                                                _StoredSubElements.Add(_SubElement.Name);

                                                foreach (KeyValuePair<string, string> _StoragedAttribute in _htSubElements[_SubElement.Name] as Dictionary<string, string>)
                                                {
                                                    _SubElement.Attributes[_StoragedAttribute.Key].InnerText = _StoragedAttribute.Value;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (parameters.ContainsKey(_Element.Name))
                                {
                                    _StoredElements.Add(_Element.Name);

                                    foreach (KeyValuePair<string, string> _StoragedAttribute in parameters[_Element.Name] as Dictionary<string, string>)
                                    {
                                        _Element.Attributes[_StoragedAttribute.Key].InnerText = _StoragedAttribute.Value;
                                    }
                                }
                            }
                        }
                    }

                    foreach (string _sKey in parameters.Keys)
                    {
                        if (_StoredElements.Contains(_sKey))
                        {
                            XmlNode _Element = _Elements.SelectSingleNode(_sKey);
                            if (_Element.HasChildNodes)
                            {
                                Hashtable _htSubElements = parameters[_Element.Name] as Hashtable;
                                foreach (string _sSubKey in _htSubElements.Keys)
                                {
                                    if (!_StoredSubElements.Contains(_sSubKey))
                                    {
                                        XmlElement _SubElement = _XmlDocument.CreateElement(_sSubKey);
                                        foreach (KeyValuePair<string, string> _StoragedAttribute in _htSubElements[_sSubKey] as Dictionary<string, string>)
                                        {
                                            XmlAttribute _Attribute = _XmlDocument.CreateAttribute(_StoragedAttribute.Key);
                                            _Attribute.Value = _StoragedAttribute.Value;
                                            _SubElement.Attributes.Append(_Attribute);
                                        }
                                        _Element.AppendChild(_SubElement);
                                    }
                                }
                            }
                        }
                        else
                        {
                            XmlElement _Element = _XmlDocument.CreateElement(_sKey);
                            if (parameters[_sKey] is Hashtable _htSubElements)
                            {
                                foreach (string _sSubKey in _htSubElements.Keys)
                                {
                                    XmlElement _SubElement = _XmlDocument.CreateElement(_sSubKey);
                                    foreach (KeyValuePair<string, string> _StoragedAttribute in _htSubElements[_sSubKey] as Dictionary<string, string>)
                                    {
                                        XmlAttribute _Attribute = _XmlDocument.CreateAttribute(_StoragedAttribute.Key);
                                        _Attribute.Value = _StoragedAttribute.Value;
                                        _SubElement.Attributes.Append(_Attribute);
                                    }
                                    _Element.AppendChild(_SubElement);
                                }
                                _Elements.AppendChild(_Element);
                            }
                            else
                            {
                                foreach (KeyValuePair<string, string> _StoragedAttribute in parameters[_sKey] as Dictionary<string, string>)
                                {
                                    XmlAttribute _Attribute = _XmlDocument.CreateAttribute(_StoragedAttribute.Key);
                                    _Attribute.Value = _StoragedAttribute.Value;
                                    _Element.Attributes.Append(_Attribute);
                                }
                                _Elements.AppendChild(_Element);
                            }
                        }
                    }

                    _XmlDocument.Save(Path.Combine(FilePath, FileName));
                }
                else
                {
                    bool _IsExistsDirectory = Directory.Exists(FilePath);
                    if (!_IsExistsDirectory)
                    {
                        Directory.CreateDirectory(FilePath);
                    }

                    _XmlTextWriter = new XmlTextWriter(Path.Combine(FilePath, FileName), Encoding.UTF8)
                    {
                        Formatting = Formatting.Indented
                    };
                    _XmlTextWriter.WriteStartDocument();
                    _XmlTextWriter.WriteStartElement("configuration");
                    foreach (string _sKey in parameters.Keys)
                    {
                        _XmlTextWriter.WriteStartElement(_sKey);
                        if (parameters[_sKey] is Hashtable _htSubElements)
                        {
                            foreach (string _sSubKey in _htSubElements.Keys)
                            {
                                _XmlTextWriter.WriteStartElement(_sSubKey);
                                foreach (KeyValuePair<string, string> _Attribute in _htSubElements[_sSubKey] as Dictionary<string, string>)
                                {
                                    _XmlTextWriter.WriteAttributeString(_Attribute.Key, _Attribute.Value);
                                }
                                _XmlTextWriter.WriteEndElement();
                            }
                        }
                        else
                        {
                            foreach (KeyValuePair<string, string> _Attribute in parameters[_sKey] as Dictionary<string, string>)
                            {
                                _XmlTextWriter.WriteAttributeString(_Attribute.Key, _Attribute.Value);
                            }
                        }
                        _XmlTextWriter.WriteEndElement();
                    }
                    _XmlTextWriter.WriteEndElement();
                    _XmlTextWriter.WriteEndDocument();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_XmlTextWriter != null)
                {
                    _XmlTextWriter.Flush();
                    _XmlTextWriter.Close();
                }
            }
        }

        public bool IsExistsConfigureFile()
        {
            try
            {
                if (!File.Exists(Path.Combine(FilePath, FileName)))
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
