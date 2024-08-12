using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace NCManagementSystem.Components.License
{
    public class LicenseHandler
    {
        #region [ Constructor ]
        public LicenseHandler()
        {
        }

        private static readonly object m_SyncLock = new object();

        private static LicenseHandler m_Instance = null;
        public static LicenseHandler GetInstance()
        {
            if (m_Instance == null)
            {
                lock (m_SyncLock)
                {
                    m_Instance = new LicenseHandler();
                }
            }
            return m_Instance;
        }
        #endregion

        #region [ Member Variables / Fields / Properties / EventHandler ]
        #endregion

        #region [ Override Events / Events / Methods ]
        public bool ActivateLicense()
        {
            try
            {
                string _sLicenseKey = Properties.Settings.Default.LICENSE_KEY;
                if (string.IsNullOrEmpty(_sLicenseKey))
                {
                    //라이센스 없어도 그냥 실행
                    return true;

                    using (FormLicense00 _LicenseDialog = new FormLicense00())
                    {
                        DialogResult _DialogResult = _LicenseDialog.ShowDialog();
                        if (_DialogResult.Equals(DialogResult.OK))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {

                    /// 2022-10-05 수정 고광진
                    /// License가 Activate되어 있는경우 추가로 확인하는 루틴 삭제.
                    /// -> License Manager에 프로그램 실행마다 이벤트 발생 안되게, 확인하는 과정삭제
                    /// -> VerificationLicense <- Method 에서 POST 방식으로 License Check 하는 과정 수행
                    /// --> 초기 등록된 댓수를 활용, 이후 라이센스 체크 X
                    return true;

                    /// 2022-10-05 수정 고광진
                    /// 위의 Commnet 참조.
                    /// 
                    //string _sResult = VerificationLicense(_sLicenseKey);
                    //JObject _JObject = JObject.Parse(_sResult);
                    //if (_JObject.Count > 0)
                    //{
                    //    bool _IsSuccess = bool.Parse(_JObject["success"].ToString());
                    //    string _sReturnMessage = _JObject["message"].ToString();
                    //    if (_IsSuccess)
                    //    {
                    //        return true;
                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show(_sReturnMessage, MessageBoxIcon.Warning.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    //        using (FormLicense00 _LicenseDialog = new FormLicense00())
                    //        {
                    //            DialogResult _DialogResult = _LicenseDialog.ShowDialog();
                    //            if (_DialogResult.Equals(DialogResult.OK))
                    //            {
                    //                return true;
                    //            }
                    //            else
                    //            {
                    //                return false;
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    return false;
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string VerificationLicense(string license)
        {
            try
            {
                //#0. 파라미터 셋팅
                string _sLicenseKey = license;
                string _sMacAddress = DeviceInfo.GetMacAddress();
                string _sExternalIPAdr = DeviceInfo.GetExternalIPAddress();
                string _sInternalIPAdr = DeviceInfo.GetInternalIPAddress(";");
                string _sSerial = DeviceInfo.GetAllSerialNoList()[0];
                string _sUser = Environment.UserName;
                string _sDevice = Environment.MachineName;
                //#1. 전송값 [타입:Post , 파라미터: #0. 파라미터 셋팅, UTF8인코딩]
                string _sRequestParams = $"id={_sLicenseKey}&macId={_sMacAddress}&externalIP={_sExternalIPAdr}&internalIP={_sInternalIPAdr}&serial=&{_sSerial}pcUserName={_sUser}&pcDeviceName={_sDevice}";
                Encoding _Encoding = Encoding.UTF8;
                byte[] _RequestParamsByte = _Encoding.GetBytes(_sRequestParams);
                string _sUrl = "http://license.ksolution.kr/Register/";
                HttpWebRequest _HttpWebRequest = (HttpWebRequest)WebRequest.Create(_sUrl);
                _HttpWebRequest.Method = "POST";
                _HttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                _HttpWebRequest.ContentLength = _RequestParamsByte.Length;
                Stream _RequestStream = _HttpWebRequest.GetRequestStream();
                _RequestStream.Write(_RequestParamsByte, 0, _RequestParamsByte.Length);
                _RequestStream.Close();
                HttpWebResponse _HttpWebResponse = (HttpWebResponse)_HttpWebRequest.GetResponse();
                Stream _ResponseStream = _HttpWebResponse.GetResponseStream();
                StreamReader _StreamReader = new StreamReader(_ResponseStream, Encoding.UTF8, true);
                //#2. 결과값
                //string _sResult = _StreamReader.ReadToEnd();
                return _StreamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool VerificationPermission(string key)
        {
            try
            {
                string _sConvertedToHash = CreateMD5Hash(key);
                if (string.Compare(ConstsDefiner.PermissionKey, _sConvertedToHash, false).Equals(0))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string CreateMD5Hash(string data)
        {
            try
            {
                System.Security.Cryptography.MD5 _MD5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                _MD5.ComputeHash(Encoding.ASCII.GetBytes(data));
                byte[] _HashBytes = _MD5.Hash;
                StringBuilder _sbHash = new StringBuilder();
                for (int i = 0; i < _HashBytes.Length; i++)
                {
                    _sbHash.Append(_HashBytes[i].ToString("x2"));
                }
                return _sbHash.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
