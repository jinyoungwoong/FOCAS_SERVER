﻿using System;
using System.Deployment.Application;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NCManagementSystem.Components;
using NCManagementSystem.Components.Controllers;
using NCManagementSystem.Components.Handlers.Log;
using NCManagementSystem.Components.Handlers.Publish;
using NCManagementSystem.Components.Handlers.TrayNotify;
using NCManagementSystem.Components.License;

namespace NCManagementSystem
{
    static class Program
    {
        #region [ Main Method ]
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException); // * 처리되지 않은 예외
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ApplicationExit += Application_ApplicationExit;

            //Properties.Settings.Default.Reset(); // * 어플리케이션 설정 리셋

            try
            {
                InitializeHandlers();

                string _sApplicationProductName = Application.ProductName; // * 어플리케이션 제품명
                // * 어플리케이션 중복 방지
                Mutex _MutexPreventDuplication = new Mutex(true, _sApplicationProductName, out bool _IsCreated); // * 뮤텍스(상호배제) 초기 소유권 부여
                if (_IsCreated)
                {
                    bool _IsEntryCompleted = false;
                    bool _IsLicenseActivated = LicenseHandler.GetInstance().ActivateLicense(); // * 라이선스 활성화
                    
                    if (_IsLicenseActivated)
                    {
                        //Properties->Settings.settings
                        // CONTOROLLER_COUNT 가 0인경우 -> 초기등록상태
                        // 2022-09-21 수정 : ActivateLicense과정에서 설비갯수 등록(License 인증과 동시에 설비 대수 입력됨)
                        // 실행 안되는 루틴임.
                        if (Properties.Settings.Default.CONTROLLER_COUNT.Equals(0))
                        {
                            using (FormAppSettings00 _AppSettingsDialog = new FormAppSettings00())
                            {
                                if (_AppSettingsDialog.ShowDialog().Equals(DialogResult.OK))
                                {
                                    _IsEntryCompleted = true;
                                }
                            }
                        }
                        else
                        {
                            _IsEntryCompleted = true;
                        }
                    }
                    if (_IsEntryCompleted)
                    {
                        //PublishHandler.GetInstance().IsDeployed = true;
                        Application.Run(new FormMain00());
                    }
                    else
                    {
                        Application.DoEvents();
                        Application.Exit();
                    }

                    _MutexPreventDuplication.ReleaseMutex();
                }
                else
                {
                    try
                    {
                        // * 트레이 알림 메시지 표시 및 종료
                        TrayNotifyHandler.GetInstance().IsShowNotifier = true;
                        TrayNotifyHandler.GetInstance().OnBalloonTipClicked += Notifier_OnBalloonTipClicked;
                        m_CancellationTokenSource = new CancellationTokenSource();
                        CancellationToken _CancellationToken = m_CancellationTokenSource.Token;
                        Task.Run(() =>
                        {
                            TrayNotifyHandler.GetInstance().ShowBalloonTip(ConstsDefiner.MessageSet.ALREADY_RUN_APPLICATION, ToolTipIcon.Warning);
                            LogHandler.Warn(ConstsDefiner.MessageSet.ALREADY_RUN_APPLICATION);
                            while (!_CancellationToken.IsCancellationRequested) { }
                        }, _CancellationToken).Wait(ConstsDefiner.DisappearDuplicationNotifyInterval, _CancellationToken);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    Application.DoEvents();
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                if (LogHandler.IsInitialized)
                {
                    LogHandler.Fatal(LogHandler.SerializeException(ex));
                }
                MessageBox.Show(ex.Message);
                Application.DoEvents();
                Application.Exit();
            }
        }
        #endregion

        #region [ Member Variables / Fields / Properties / EventHandler ]
        /// <summary>
        /// 취소되도록 System.Threading.CancellationToken에 신호를 보냅니다.
        /// </summary>
        public static CancellationTokenSource m_CancellationTokenSource = null;
        #endregion

        #region [ Override Events / Events / Methods ]
        /// <summary>
        /// 핸들러 초기화
        /// </summary>
        private static void InitializeHandlers()
        {
            try
            {
                // * LogHandler
                LogHandler.Initialize(MethodBase.GetCurrentMethod().DeclaringType);
                // * PublishHandler
                PublishHandler.GetInstance().FileNames = "AppConfigure.xml,Controllers.json"; // * 데이터파일s
                PublishHandler.GetInstance().FilePath = GetDataFilePath();
                PublishHandler.GetInstance().RunTransferDataFile();
                // * TrayNotifyHandler
                string _sTrayNotifyTitle = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title;
                TrayNotifyHandler.GetInstance().Initialize(false, _sTrayNotifyTitle, Properties.Resources.logo);
                TrayNotifyHandler.GetInstance().OnBalloonTipClicked += Notifier_OnBalloonTipClicked;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 데이터 파일 경로 가져오기.
        /// </summary>
        /// <returns></returns>
        public static string GetDataFilePath()
        {
            try
            {
                string _sPath = string.Empty;
                if (ApplicationDeployment.IsNetworkDeployed) // * 게시 여부
                {
                    _sPath = ApplicationDeployment.CurrentDeployment.DataDirectory;
                    PublishHandler.GetInstance().Version = ApplicationDeployment.CurrentDeployment.CurrentVersion;
                    PublishHandler.GetInstance().IsDeployed = true;
                }
                else
                {
                    _sPath = AppDomain.CurrentDomain.BaseDirectory;
                    PublishHandler.GetInstance().Version = Assembly.GetExecutingAssembly().GetName().Version;
                    PublishHandler.GetInstance().IsDeployed = false;
                }
                return _sPath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 풍선 설명을 클릭할 때 발생합니다.
        /// </summary>
        /// <param name="sender">이벤트 소스입니다.</param>
        /// <param name="e">이벤트 데이터가 포함되지 않은 개체입니다.</param>
        private static void Notifier_OnBalloonTipClicked(object sender, EventArgs e)
        {
            try
            {
                if (m_CancellationTokenSource != null)
                {
                    m_CancellationTokenSource.Cancel();
                }
            }
            catch (Exception ex)
            {
                LogHandler.Fatal(LogHandler.SerializeException(ex));
                throw ex;
            }
        }

        /// <summary>
        /// 애플리케이션이 종료되려고 할 때 발생합니다.
        /// </summary>
        /// <param name="sender">이벤트 소스입니다.</param>
        /// <param name="e">이벤트 데이터가 포함되지 않은 개체입니다.</param>
        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            try
            {
                TrayNotifyHandler.GetInstance().OnBalloonTipClicked -= Notifier_OnBalloonTipClicked;
                TrayNotifyHandler.GetInstance().Deinitialize();
            }
            catch (Exception ex)
            {
                LogHandler.Fatal(LogHandler.SerializeException(ex));
            }
        }

        /// <summary>
        /// 애플리케이션 도메인에서 처리되지 않는 예외에 의해 발생합니다.
        /// </summary>
        /// <param name="sender">처리되지 않은 예외 이벤트의 원본입니다.</param>
        /// <param name="e">이벤트 데이터를 포함하는 UnhandledExceptionEventArgs입니다.</param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (LogHandler.IsInitialized)
            {
                LogHandler.Fatal(LogHandler.SerializeException((Exception)e.ExceptionObject));
            }
        }
        #endregion
    }
}