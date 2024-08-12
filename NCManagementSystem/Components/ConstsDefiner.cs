﻿using System;
using NCManagementSystem.Libraries.Controls.DataGridView;
using NCManagementSystem.Libraries.Controls.DataGridView.Components;

namespace NCManagementSystem.Components
{
    public class ConstsDefiner
    {
        public delegate void GlobalExceptionEventHandler(object sender, Exception gex);

        internal const string FocasLogger = "FocasLogger";
        internal const int DisappearDuplicationNotifyInterval = 5000;

        internal const int ControllerWidth = 792;
        internal const int ControllerHeight = 45;
        internal const int ControllerColumnPadding = 10;
        internal const int ControllerRowPadding = 2;
        internal const int LimitedCountOfControllerRows = 16;

        public struct CommunicationSettings
        {
            public const int Timeout = 1;
            public const int RetryCount = 0; // * 재시도횟수('0'일때무한대)
            public const int ReceiveInterval = 500;

            internal const int AutoStartInterval = 500;
        }

        internal struct FixedString
        {
            internal const string PrefixTitle = "T";
            internal const string BlankSymbol = "-";
            internal const string Deployed = " - [Ver.{0}]";
            internal const string NotDeployed = " - [Not Deployed]";
        }

        public enum ControllerStatuses
        {
            Waiting,
            Disconnected,
            Connected
        }

        public struct ControllerHeaders
        {
            public const string ID = "#";
            public const string Code = "CODE";
            public const string IP = "IP ADR.";
            public const string Port = "PORT";
            public const string State = "STATE";
        }

        internal struct ControllerActions
        {
            internal const string Start = "START";
            internal const string Stop = "STOP";
        }

        internal struct ColumnHeaderNames
        {
            public const string MACHINE_CD = "MACHINE CODE";
            public const string MACHINE_NM = "MACHINE NAME";
            public const string MODEL = "MODEL";
            public const string IP_ADR = "IP ADDRESS";
            public const string PORT = "PORT";
            public const string EMP_NM = "EMP_NM";
            public const string REMARKS = "REMARKS";
            public const string CREATED_ON = "CREATED ON";
            public const string MODIFIED_ON = "MODIFIED ON";
        }

        internal struct UserColumnHeaderNames
        {
            public const string ID = "ID";
            public const string PASSWORD = "PASSWORD";
            public const string NAME = "NAME";
            public const string EMAIL = "EMAIL";
            public const string PHONE = "PHONE";
            public const string USE_YN = "USE";
            public const string USER_GBN = "USER_GBN";
        }

        internal enum DataPropertyNames
        {
            MACHINE_CD,
            MACHINE_NM,
            MODEL,
            IP_ADDR,
            PORT,
            SCOMMENT,
            EMP_CD,
            REG_DATE,
            MDFY_DATE
        }

        internal enum UserDataPropertyNames
        {
            ID,
            PASSWORD,
            NAME,
            EMAIL,
            PHONE,
            USE_YN,
            USER_GBN
        }

        public static ColumnSchemaFactory GetColumnsSchema(FwDataGridView gridview)
        {
            try
            {
                ColumnSchemaFactory _Columns = new ColumnSchemaFactory(gridview);
                _Columns.Create(ColumnHeaderNames.MACHINE_CD, DataPropertyNames.MACHINE_CD.ToString(), DataGridViewConstsDefiner.ColumnAlignment.center, true);
                _Columns.Create(ColumnHeaderNames.MACHINE_NM, DataPropertyNames.MACHINE_NM.ToString(), DataGridViewConstsDefiner.ColumnAlignment.center, true);
                _Columns.Create(ColumnHeaderNames.MODEL, DataPropertyNames.MODEL.ToString(), DataGridViewConstsDefiner.ColumnAlignment.center, true);
                _Columns.Create(ColumnHeaderNames.IP_ADR, DataPropertyNames.IP_ADDR.ToString(), DataGridViewConstsDefiner.ColumnAlignment.center, true);
                _Columns.Create(ColumnHeaderNames.PORT, DataPropertyNames.PORT.ToString(), DataGridViewConstsDefiner.ColumnAlignment.center, true);
                _Columns.Create(ColumnHeaderNames.EMP_NM, DataPropertyNames.EMP_CD.ToString(), DataGridViewConstsDefiner.ColumnAlignment.center, true);
                _Columns.Create(ColumnHeaderNames.REMARKS, DataPropertyNames.SCOMMENT.ToString(), DataGridViewConstsDefiner.ColumnAlignment.center, true);
                _Columns.Create(ColumnHeaderNames.CREATED_ON, DataPropertyNames.REG_DATE.ToString(), DataGridViewConstsDefiner.ColumnAlignment.center, true);
                _Columns.Create(ColumnHeaderNames.MODIFIED_ON, DataPropertyNames.MDFY_DATE.ToString(), DataGridViewConstsDefiner.ColumnAlignment.center, true);
                return _Columns;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ColumnSchemaFactory GetUserColumnsSchema(FwDataGridView gridview)
        {
            try
            {
                ColumnSchemaFactory _Columns = new ColumnSchemaFactory(gridview);
                _Columns.Create(UserColumnHeaderNames.ID, UserDataPropertyNames.ID.ToString(), DataGridViewConstsDefiner.ColumnAlignment.center, true);
                _Columns.Create(UserColumnHeaderNames.PASSWORD, UserDataPropertyNames.PASSWORD.ToString(), DataGridViewConstsDefiner.ColumnAlignment.center, true);
                _Columns.Create(UserColumnHeaderNames.NAME, UserDataPropertyNames.NAME.ToString(), DataGridViewConstsDefiner.ColumnAlignment.center, true);
                _Columns.Create(UserColumnHeaderNames.EMAIL, UserDataPropertyNames.EMAIL.ToString(), DataGridViewConstsDefiner.ColumnAlignment.center, true);
                _Columns.Create(UserColumnHeaderNames.PHONE, UserDataPropertyNames.PHONE.ToString(), DataGridViewConstsDefiner.ColumnAlignment.center, true);
                _Columns.Create(UserColumnHeaderNames.USE_YN, UserDataPropertyNames.USE_YN.ToString(), DataGridViewConstsDefiner.ColumnAlignment.center, true);
                _Columns.Create(UserColumnHeaderNames.USER_GBN, UserDataPropertyNames.USER_GBN.ToString(), DataGridViewConstsDefiner.ColumnAlignment.center, true);
                return _Columns;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public const string RegistryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        internal const string PermissionKey = "5607689776769e4fb0cc3eedb5bac919";

        internal struct MessageSet
        {
            internal const string ALREADY_RUN_APPLICATION = "어플리케이션이 이미 실행 중입니다.";
            internal const string NOT_FOUND_CONFIG = "구성 파일을 찾을 수 없습니다. ['{0}']";
            internal const string INVALID_CONFIG = "잘못된 구성 요소입니다.\r\n['{0}' : {1}]";
            internal const string QST_EXIT = "종료하시겠습니까?";
            internal const string EXISTS_CODE = "코드 '{0}' 가(이) 존재합니다.";
            internal const string NOT_EXISTS_CODE = "코드 '{0}' 가(이) 존재하지 않습니다.";
            internal const string CHECK_DATABASE = "데이터베이스 상태를 확인하세요.";
            internal const string CHECK_IPADR = "IP 주소를 확인하세요.";
            internal const string ALREADY_ASSIGNED_MACHINE = "이미 할당된 설비 코드입니다.";
        }
    }
}
