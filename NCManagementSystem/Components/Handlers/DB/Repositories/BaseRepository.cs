﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;

namespace NCManagementSystem.Components.Handlers.DB.Repositories
{
    public enum DBTypes
    {
        MSSQL,
        PostgreSQL,
        MYSQL
    }

    public class BaseRepository
    {
        #region [ Constructor ]
        /// <summary>
        /// 생성자 입니다.
        /// </summary>
        public BaseRepository()
        {
        }
        #endregion

        #region [ Member Variables / Fields / Properties / EventHandler ]
        /// <summary>
        /// 데이터베이스 종류 (MSSQL, PostgreSQL...)
        /// </summary>
        public DBTypes DBType { get; set; } = DBTypes.MYSQL;

        /// <summary>
        /// 데이터소스
        /// </summary>
        public string DataSource { get; set; } = string.Empty;

        /// <summary>
        /// 데이터베이스 명
        /// </summary>
        public string DataBaseName { get; set; } = string.Empty;

        /// <summary>
        /// 사용자
        /// </summary>
        public string User { get; set; } = string.Empty;

        /// <summary>
        /// 비밀번호
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 연결 타임아웃 (초)
        /// </summary>
        public int ConnectionTimeout { get; set; } = 2;
        #endregion

        #region [ Override Events / Events / Methods ]
        /// <summary>
        /// 속성 설정
        /// </summary>
        /// <param name="type">데이터베이스 종류</param>
        /// <param name="dataSource">데이터소스</param>
        /// <param name="databaseName">데이터베이스명</param>
        /// <param name="user">사용자</param>
        /// <param name="password">비밀번호</param>
        /// <param name="connectionTimeout">연결 타임아웃(초)</param>
        public void SetConnectionProperties(DBTypes type, string dataSource, string databaseName, string user, string password, int connectionTimeout = 2)
        {
            try
            {
                DBType = type;
                DataSource = dataSource;
                DataBaseName = databaseName;
                User = user;
                Password = password;
                ConnectionTimeout = connectionTimeout;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 연결 생성
        /// </summary>
        /// <returns></returns>
        protected IDbConnection CreateConnection()
        {
            try
            {
                switch (DBType)
                {
                    case DBTypes.PostgreSQL:
                    case DBTypes.MYSQL:
                        {
                            string _sConnectionString = $"host={DataSource};port=3306; database={DataBaseName}; User Id={User}; Password={Password};";
                            return new MySqlConnection(_sConnectionString);
                        }

                    case DBTypes.MSSQL:
                    default:
                        {
                            string _sConnectionString = $"Data Source={DataSource};Initial Catalog={DataBaseName};Persist Security Info=True;User ID={User};Password={Password};Connect Timeout={ConnectionTimeout}";
                            return new SqlConnection(_sConnectionString);
                        }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 쿼리를 실행하여 T 형식의 데이터를 반환합니다.
        /// </summary>
        /// <typeparam name="T">반환할 결과 유형입니다.</typeparam>
        /// <param name="sql">쿼리에 대해 실행할 SQL입니다.</param>
        /// <param name="param">전달할 매개변수(있는 경우).</param>
        /// <param name="transaction">사용할 트랜잭션(있는 경우).</param>
        /// <param name="buffered">결과를 메모리에 버퍼링할지 여부입니다.</param>
        /// <param name="commandTimeout">명령 시간 초과(초)입니다.</param>
        /// <param name="commandType">실행할 명령의 유형입니다.</param>
        /// <returns>제공된 유형의 데이터 시퀀스입니다. 기본 유형(int, string 등)이 쿼리되면 가정된 첫 번째 열의 데이터가 가정되고, 그렇지 않으면 행당 인스턴스가 생성되고 직접적인 열 이름===구성원 이름 매핑이 가정됩니다(대소문자 구분 안 함).</returns>
        protected List<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            try
            {
                using (var _SqlConnection = CreateConnection())
                {
                    return _SqlConnection.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 단일 행 쿼리를 실행하여 T 형식의 데이터를 반환합니다.
        /// </summary>
        /// <typeparam name="T">반환할 결과 유형입니다.</typeparam>
        /// <param name="sql">쿼리에 대해 실행할 SQL입니다.</param>
        /// <param name="param">전달할 매개변수(있는 경우).</param>
        /// <param name="transaction">사용할 트랜잭션(있는 경우).</param>
        /// <param name="commandTimeout">명령 시간 초과(초)입니다.</param>
        /// <param name="commandType">실행할 명령의 유형입니다.</param>
        /// <returns>제공된 유형의 데이터 시퀀스입니다. 기본 유형(int, string 등)이 쿼리되면 가정된 첫 번째 열의 데이터가 가정되고, 그렇지 않으면 행당 인스턴스가 생성되고 직접적인 열 이름===구성원 이름 매핑이 가정됩니다(대소문자 구분 안 함).</returns>
        protected T QueryFirst<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            try
            {
                using (var _SqlConnection = CreateConnection())
                {
                    return _SqlConnection.QueryFirst<T>(sql, param, transaction, commandTimeout, commandType);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 단일 행 쿼리를 실행하여 T 형식의 데이터를 반환합니다.
        /// </summary>
        /// <typeparam name="T">반환할 결과 유형입니다.</typeparam>
        /// <param name="sql">쿼리에 대해 실행할 SQL입니다.</param>
        /// <param name="param">전달할 매개변수(있는 경우).</param>
        /// <param name="transaction">사용할 트랜잭션(있는 경우).</param>
        /// <param name="commandTimeout">명령 시간 초과(초)입니다.</param>
        /// <param name="commandType">실행할 명령의 유형입니다.</param>
        /// <returns>제공된 유형의 데이터 시퀀스입니다. 기본 유형(int, string 등)이 쿼리되면 가정된 첫 번째 열의 데이터가 가정되고, 그렇지 않으면 행당 인스턴스가 생성되고 직접적인 열 이름===구성원 이름 매핑이 가정됩니다(대소문자 구분 안 함).</returns>
        protected T QueryFirstOrDefault<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            try
            {
                using (var _SqlConnection = CreateConnection())
                {
                    return _SqlConnection.QueryFirstOrDefault<T>(sql, param, transaction, commandTimeout, commandType);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 단일 행 쿼리를 실행하여 T 형식의 데이터를 반환합니다.
        /// </summary>
        /// <typeparam name="T">반환할 결과 유형입니다.</typeparam>
        /// <param name="sql">쿼리에 대해 실행할 SQL입니다.</param>
        /// <param name="param">전달할 매개변수(있는 경우).</param>
        /// <param name="transaction">사용할 트랜잭션(있는 경우).</param>
        /// <param name="commandTimeout">명령 시간 초과(초)입니다.</param>
        /// <param name="commandType">실행할 명령의 유형입니다.</param>
        /// <returns>제공된 유형의 데이터 시퀀스입니다. 기본 유형(int, string 등)이 쿼리되면 가정된 첫 번째 열의 데이터가 가정되고, 그렇지 않으면 행당 인스턴스가 생성되고 직접적인 열 이름===구성원 이름 매핑이 가정됩니다(대소문자 구분 안 함).</returns>
        protected T QuerySingle<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            try
            {
                using (var _SqlConnection = CreateConnection())
                {
                    return _SqlConnection.QuerySingle<T>(sql, param, transaction, commandTimeout, commandType);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 단일 행 쿼리를 실행하여 T 형식의 데이터를 반환합니다.
        /// </summary>
        /// <typeparam name="T">반환할 결과 유형입니다.</typeparam>
        /// <param name="sql">쿼리에 대해 실행할 SQL입니다.</param>
        /// <param name="param">전달할 매개변수(있는 경우).</param>
        /// <param name="transaction">사용할 트랜잭션(있는 경우).</param>
        /// <param name="commandTimeout">명령 시간 초과(초)입니다.</param>
        /// <param name="commandType">실행할 명령의 유형입니다.</param>
        /// <returns>제공된 유형의 데이터 시퀀스입니다. 기본 유형(int, string 등)이 쿼리되면 가정된 첫 번째 열의 데이터가 가정되고, 그렇지 않으면 행당 인스턴스가 생성되고 직접적인 열 이름===구성원 이름 매핑이 가정됩니다(대소문자 구분 안 함).</returns>
        protected T QuerySingleOrDefault<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            try
            {
                using (var _SqlConnection = CreateConnection())
                {
                    return _SqlConnection.QuerySingleOrDefault<T>(sql, param, transaction, commandTimeout, commandType);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 매개변수화된 SQL을 실행합니다.
        /// </summary>
        /// <param name="sql">이 쿼리에 대해 실행할 SQL입니다.</param>
        /// <param name="param">이 쿼리에 사용할 매개변수입니다.</param>
        /// <param name="transaction">이 쿼리에 사용할 트랜잭션입니다.</param>
        /// <param name="commandTimeout">명령 실행 시간 초과 전의 시간(초)입니다.</param>
        /// <param name="commandType">저장 프로시저 이거나 배치.</param>
        /// <returns>영향을 받는 행 수입니다.</returns>
        protected int Execute(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            try
            {
                using (var _SqlConnection = CreateConnection())
                {
                    return _SqlConnection.Execute(sql, param, transaction, commandTimeout, commandType);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 단일 값을 선택하는 매개변수화된 SQL을 실행합니다.
        /// </summary>
        /// <typeparam name="T">반환할 유형입니다.</typeparam>
        /// <param name="sql">실행할 SQL입니다.</param>
        /// <param name="param">이 명령에 사용할 매개변수입니다.</param>
        /// <param name="transaction">이 명령에 사용할 트랜잭션입니다.</param>
        /// <param name="commandTimeout">명령 실행 시간 초과 전의 시간(초)입니다.</param>
        /// <param name="commandType">저장 프로시저 이거나 배치.</param>
        /// <returns>T로 반환된 첫 번째 셀.</returns>
        protected T ExecuteScalar<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            try
            {
                using (var _SqlConnection = CreateConnection())
                {
                    return _SqlConnection.ExecuteScalar<T>(sql, param, transaction, commandTimeout, commandType);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [ DB State Monitoring ]
        public event DBStateMonitoringExceptionEventHandler OnDBStateMonitoringException;
        public event DBStateChangedEventHandler OnDBStateChanged;

        /// <summary>
        /// 명령 실행 시간 초과 전의 시간(초)입니다.
        /// </summary>
        public int MonitoringCommandTimeout { get; set; } = 1;

        /// <summary>
        /// DB 상태 모니터링 간격(밀리초) 입니다.
        /// </summary>
        public int MonitoringInterval { get; set; } = 500;

        /// <summary>
        /// DB 연결 연부 입니다.
        /// </summary>
        public bool IsConnected { get; set; } = false;

        private CancellationTokenSource m_CancellationTokenSource = null;
        private CancellationToken m_CancellationToken;
        private bool m_IsRunning = false;

        /// <summary>
        /// DB 상태 모니터링 시작
        /// </summary>
        public async void StartDBStateMonitoring()
        {
            try
            {
                m_CancellationTokenSource = new CancellationTokenSource();
                m_CancellationToken = m_CancellationTokenSource.Token;

                await Task.Run(async () =>
                {
                    m_IsRunning = true;
                    while (m_IsRunning)
                    {
                        if (m_CancellationToken.IsCancellationRequested) // * 작업(Task) 취소 여부
                        {
                            break;
                        }

                        bool _IsConnected = GetConnected(); // * 데이터베이스 연결 여부
                        if (!IsConnected.Equals(_IsConnected))
                        {
                            IsConnected = _IsConnected;
                            OnDBStateChanged?.Invoke(); // * 상태 변경 이벤트
                        }

                        await Task.Delay(MonitoringInterval, m_CancellationToken); // * 연결여부 모니터링 간격
                    }
                }).ContinueWith(x =>
                {
                    if (x.IsFaulted)
                    {
                        throw x.Exception.InnerException;
                    }
                }, m_CancellationToken);
            }
            catch (OperationCanceledException)
            {
                IsConnected = false;
                OnDBStateChanged?.Invoke(); // * 상태 변경 이벤트
            }
            catch (Exception ex)
            {
                OnDBStateMonitoringException?.Invoke(this, ex);
            }
        }

        /// <summary>
        /// DB 상태 모니터링 중지
        /// </summary>
        public void StopDBStateMonitoring()
        {
            try
            {
                m_IsRunning = false;

                if (m_CancellationTokenSource != null)
                {
                    m_CancellationTokenSource.Cancel();
                }
            }
            catch (Exception ex)
            {
                OnDBStateMonitoringException?.Invoke(this, ex);
            }
        }

        /// <summary>
        /// DB 연결 상태 가져오기
        /// </summary>
        /// <returns></returns>
        public bool GetConnected()
        {
            try
            {
                return GetConnected(MonitoringCommandTimeout);
                /*
                using (var _SqlConnection = CreateConnection())
                {
                    IDataReader _DataReader = _SqlConnection.ExecuteReader("SELECT 1", null, null, MonitoringCommandTimeout);
                    if (_DataReader.Read())
                    {
                        return true;
                    }
                }
                return false;
                */
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// DB 연결 상태 가져오기
        /// </summary>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public bool GetConnected(int commandTimeout)
        {
            try
            {
                using (var _SqlConnection = CreateConnection())
                {
                    IDataReader _DataReader = _SqlConnection.ExecuteReader("SELECT 1", null, null, commandTimeout);
                    if (_DataReader.Read())
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
