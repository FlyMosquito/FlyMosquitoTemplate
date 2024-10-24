#region using
using FlyMosquito.Domain;
#endregion

namespace FlyMosquito.Service.AuditLog.IAuditLogService
{
    public interface IWebApiLogService
    {
        void CreateWebApiLog(WebApiLog WebApiLog);
    }
}
