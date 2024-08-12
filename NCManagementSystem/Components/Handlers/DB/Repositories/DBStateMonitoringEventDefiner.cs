using System;

namespace NCManagementSystem.Components.Handlers.DB.Repositories
{
    public delegate void DBStateMonitoringExceptionEventHandler(object sender, Exception gex);
    public delegate void DBStateChangedEventHandler();
}
