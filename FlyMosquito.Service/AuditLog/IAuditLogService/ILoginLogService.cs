

#region using
using FlyMosquito.Domain;
#endregion

namespace FlyMosquito.Service.AuditLog.IAuditLogService
{
    public interface ILoginLogService
    {
        Task<int> CreateUserToken(LoginLog loginLog);
        Task<int> DeleteUserToken(LoginLog LoginLog);
        Task<LoginLog> GetUserToken(string StringToken);
    }
}
