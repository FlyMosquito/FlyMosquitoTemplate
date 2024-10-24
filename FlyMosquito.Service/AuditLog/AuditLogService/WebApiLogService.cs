#region using
using FlyMosquito.Core;
using FlyMosquito.Domain;
using FlyMosquito.Service.AuditLog.IAuditLogService;
#endregion

namespace FlyMosquito.Service.AuditLog.AuditLogService
{
    public class WebApiLogService : IWebApiLogService
    {
        private readonly IRepository<WebApiLog> WebApiLogRepo;
        public WebApiLogService(IRepository<WebApiLog> webApiLogRepo)
        {
            WebApiLogRepo = webApiLogRepo;
        }

        public void CreateWebApiLog(WebApiLog WebApiLog)
        {
            WebApiLogRepo.Insert(WebApiLog);
        }
    }
}
