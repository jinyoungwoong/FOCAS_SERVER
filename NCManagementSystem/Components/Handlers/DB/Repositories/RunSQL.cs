using System;
using System.Collections.Generic;
using Dapper;
using NCManagementSystem.Components.Models;

namespace NCManagementSystem.Components.Handlers.DB.Repositories
{
    public class RunSQL : BaseRepository
    {
        #region [ Constructor ]
        public RunSQL()
        {
        }

        private static readonly object m_SyncLock = new object();
        public static DateTime stDate;
        public static bool isRunning = false;
        private static RunSQL m_Instance = null;
        public static RunSQL GetInstance()
        {
            if (m_Instance == null)
            {
                lock (m_SyncLock)
                {
                    m_Instance = new RunSQL();
                }
            }

            return m_Instance;
        }
        #endregion

        #region [ Override Events / Events / Methods ]

        public void setRunningState(bool x)
        {
            isRunning = x;
        }

        public bool getRunningState()
        {
            return isRunning;
        }

        public void setRunningTime(DateTime x)
        {
            stDate = x;
        }

        public DateTime getRunningTime()
        {
            return stDate;
        }

        public bool CountByRecordForMachineMaster(string code)
        {
            try
            {
                string _sQuery = "SELECT COUNT(*) FROM tncms_machine WHERE machine_cd=@machine_cd";
                return ExecuteScalar<int>(_sQuery, new TNCMS_MACHINE { MACHINE_CD = code }) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ExistsCheckForUserMaster(string code)
        {
            try
            {
                string _sQuery = "SELECT * FROM tncms_user WHERE LoginId=@id";
                return ExecuteScalar<int>(_sQuery, new TNCMS_USER { ID = code }) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TNCMS_MACHINE> SelectAllRecordsForMachineMaster()
        {
            try
            {
                string _sQuery = "SELECT machine_cd, machine_nm, model, ip_addr, port, scomment, emp_cd, reg_date, mdfy_date FROM tncms_machine ORDER BY machine_cd";
                return Query<TNCMS_MACHINE>(_sQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<TNCMS_USER> SelectAllRecordsForUserMaster()
        {
            try
            {
                string _sQuery = "SELECT LoginId AS ID, Password, Name, TRIM(EMAIL) AS EMAIL, TRIM(PHONE) AS PHONE, USE_YN, USER_GBN FROM tncms_user ORDER BY LoginId";
                return Query<TNCMS_USER>(_sQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TNCMS_MACHINE SelectByConditionRecordForMachineMaster(string code)
        {
            try
            {
                string _sQuery = "SELECT machine_cd, machine_nm, model, ip_adr, port, remarks, created_on, modified_on FROM tncms_machine WHERE machine_cd=@machine_cd";
                return QueryFirstOrDefault<TNCMS_MACHINE>(_sQuery, new TNCMS_MACHINE { MACHINE_CD = code });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertRecordForMachineMaster(TNCMS_MACHINE model)
        {
            try
            {
                string _sQuery = "INSERT INTO tncms_machine (machine_cd, machine_nm, model, ip_addr, port, emp_cd, scomment, reg_date, mdfy_date, starttime, finishtime) VALUES (@machine_cd, @machine_nm, @model, @ip_addr, @port, @emp_cd, @scomment, current_timestamp, current_timestamp, @starttime, @finishtime)";
                Execute(_sQuery, model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertRecordForUserMaster(TNCMS_USER model)
        {
            try
            {
                string _sQuery = "INSERT INTO tncms_user (LoginId, Password, Name, EMAIL, PHONE, USE_YN, USER_GBN, REG_DATE) VALUES (@ID, @PASSWORD, @NAME, @EMAIL, @PHONE, @USE_YN, @USER_GBN, NOW())";
                Execute(_sQuery, model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateRecordForMachineMaster(TNCMS_MACHINE model)
        {
            try
            {
                string _sQuery = "UPDATE TNCMS_MACHINE SET machine_nm=@machine_nm, model=@model, ip_addr=@ip_addr, port=@port, emp_cd = @emp_cd, scomment=@scomment, mdfy_date=current_timestamp WHERE machine_cd=@machine_cd";
                Execute(_sQuery, model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateRecordForUserMaster(TNCMS_USER model)
        {
            try
            {
                string _sQuery = "UPDATE tncms_user SET Password=@PASSWORD, Name=@NAME, EMAIL=@EMAIL, PHONE=@PHONE, USE_YN=@USE_YN, USER_GBN=@USER_GBN WHERE LoginId=@ID";
                Execute(_sQuery, model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteRecordOfMachineMaster(string code)
        {
            try
            {
                string _sQuery = "DELETE FROM TNCMS_MACHINE WHERE machine_cd=@machine_cd";
                Execute(_sQuery, new TNCMS_MACHINE { MACHINE_CD = code });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteRecordOfUserMaster(string code)
        {
            try
            {
                string _sQuery = "DELETE FROM tncms_user WHERE LoginId=@ID";
                Execute(_sQuery, new TNCMS_USER { ID = code });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpsertRecord(TNCMS_STATUS model)
        {
            try
            {
                using (var _SqlConn = CreateConnection())
                {
                    _SqlConn.Open();

                    using (var _Transaction = _SqlConn.BeginTransaction())
                    {
                        try
                        {
                            string _sQuery = "SELECT MAX(trnx_seq) FROM tncms_history WHERE machine_cd=@machine_cd";
                            object _TrnxSeq = _SqlConn.ExecuteScalar(_sQuery, model, _Transaction);
                            //Console.WriteLine(_Transaction);
                            if (_TrnxSeq == null || !int.TryParse(_TrnxSeq.ToString(), out int _iTrnxSeq))
                            {
                                _iTrnxSeq = 1;
                            }
                            else
                            {
                                _iTrnxSeq++;
                            }
                            _sQuery = $"INSERT INTO tncms_history (machine_cd, trnx_seq, trnx_time, is_connected, cnc_type, mt_type, series, version, max_axis, axes, addinfo, aut, run, motion, mstb, alarm, prgnum, prgmnum, acts, actf, spload, spindle_data, axis_data, part_count, parts_total, operating_time, cutting_time, cycle_time, created_on, running_time) " +
                                      $"VALUES (@machine_cd, {_iTrnxSeq}, @trnx_time, @is_connected, @cnc_type, @mt_type, @series, @version, @max_axis, @axes, @addinfo, @aut, @run, @motion, @mstb, @alarm, @prgnum, @prgmnum, @acts, @actf, @spload, @spindle_data, @axis_data, @part_count, @parts_total, @operating_time, @cutting_time, @cycle_time, current_timestamp, @running_time)";
                            _SqlConn.Execute(_sQuery, model, _Transaction);

                            _sQuery = "SELECT machine_cd, trnx_time, is_connected, cnc_type, mt_type, series, version, max_axis, axes, addinfo, aut, run, motion, mstb, alarm, prgnum, prgmnum, acts, actf, spload, spindle_data, axis_data, part_count, parts_total, operating_time, cutting_time, cycle_time, modified_on FROM TNCMS_STATUS WHERE machine_cd=@machine_cd";
                            TNCMS_STATUS _StoredNCStatus = QueryFirstOrDefault<TNCMS_STATUS>(_sQuery, new { model.MACHINE_CD });
                            if (_StoredNCStatus == null)
                            {
                                _sQuery = "INSERT INTO TNCMS_STATUS (machine_cd, trnx_time, is_connected, cnc_type, mt_type, series, version, max_axis, axes, addinfo, aut, run, motion, mstb, alarm, prgnum, prgmnum, acts, actf, spload, spindle_data, axis_data, part_count, parts_total, operating_time, cutting_time, cycle_time, modified_on, running_time) " +
                                    "VALUES (@machine_cd, @trnx_time, @is_connected, @cnc_type, @mt_type, @series, @version, @max_axis, @axes, @addinfo, @aut, @run, @motion, @mstb, @alarm, @prgnum, @prgmnum, @acts, @actf, @spload, @spindle_data, @axis_data, @part_count, @parts_total, @operating_time, @cutting_time, @cycle_time, current_timestamp, @running_time)";
                                _SqlConn.Execute(_sQuery, model, _Transaction);
                            }
                            else
                            {
                                _sQuery = "UPDATE TNCMS_STATUS SET trnx_time=@trnx_time, is_connected=@is_connected, cnc_type=@cnc_type, mt_type=@mt_type, series=@series, version=@version, max_axis=@max_axis, axes=@axes, addinfo=@addinfo, aut=@aut, run=@run, motion=@motion, mstb=@mstb, alarm=@alarm, prgnum=@prgnum, prgmnum=@prgmnum, acts=@acts, actf=@actf, spload=@spload, spindle_data=@spindle_data, axis_data=@axis_data, part_count=@part_count, parts_total=@parts_total, operating_time=@operating_time, cutting_time=@cutting_time, cycle_time=@cycle_time, modified_on=current_timestamp WHERE machine_cd=@machine_cd";
                                _SqlConn.Execute(_sQuery, model, _Transaction);
                            }

                            _Transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            _Transaction.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}