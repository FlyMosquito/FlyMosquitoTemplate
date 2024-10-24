#region using
using FlyMosquito.Core;
using FlyMosquito.Domain;
using FlyMosquito.Service.AuditLog.IAuditLogService;
#endregion

namespace FlyMosquito.Service.AuditLog.AuditLogService
{
    public class LoginLogService : ILoginLogService
    {
        private IRepository<LoginLog> LoginLogRepo;

        public LoginLogService(IRepository<LoginLog> loginLogRepo)
        {
            LoginLogRepo = loginLogRepo;
        }

        /// <summary>
        /// 登陆日志
        /// </summary>
        /// <param name="loginLog"></param>
        /// <returns></returns>
        public async Task<int> CreateUserToken(LoginLog loginLog)
        {
            var BoolResult = await LoginLogRepo.InsertAsync(loginLog);
            return BoolResult;
        }

        /// <summary>
        /// 获取用户用户token
        /// </summary>
        /// <param name="StringToken"></param>
        /// <returns></returns>
        public async Task<LoginLog> GetUserToken(string StringToken)
        {
            var LoginLog = await LoginLogRepo.GetAsync(x => x.Token == StringToken);
            return LoginLog;
        }

        /// <summary>
        /// 删除登陆日志
        /// </summary>
        /// <param name="LoginLog"></param>
        /// <returns></returns>
        public async Task<int> DeleteUserToken(LoginLog LoginLog)
        {
            var ListLogin = await LoginLogRepo.GetListAsync(x => x.LoginAccount == LoginLog.LoginAccount);//查询当前用户的登录日志记录
            var IntResult = await LoginLogRepo.DeleteAsync(x => ListLogin.Select(x => x.LoginAccount).Contains(x.LoginAccount));//删除当前用户的登录日志记录
            return IntResult;
        }
    }
}
