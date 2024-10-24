#region using
using Microsoft.AspNetCore.Http;
using FlyMosquito.Common;
using FlyMosquito.Domain;
using FlyMosquito.Service.AuditLog.IAuditLogService;
using System.Security.Authentication;
#endregion

namespace FlyMosquito.Extension.SetUp
{
    public class LoginLogSetup
    {
        private readonly IHttpContextAccessor HttpContextAccessor;
        private readonly ILoginLogService LoginLogService;
        private readonly MemoryCacheHelper MemoryCacheHelper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="loginLogService"></param>
        /// <param name="memoryCacheHelper"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public LoginLogSetup(IHttpContextAccessor httpContextAccessor, ILoginLogService loginLogService, MemoryCacheHelper memoryCacheHelper)
        {
            HttpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            LoginLogService = loginLogService ?? throw new ArgumentNullException(nameof(loginLogService));
            MemoryCacheHelper = memoryCacheHelper ?? throw new ArgumentNullException(nameof(memoryCacheHelper));
        }

        private HttpContext HttpContext => HttpContextAccessor.HttpContext;

        /// <summary>
        /// 会员中心当前登录会员token记录
        /// </summary>
        public async Task<LoginLog> GetUserTokenAsync()
        {
            if (HttpContext == null)
            {
                throw new AuthenticationException("无法获取当前HTTP上下文。");
            }
            string StringAuthHeader = HttpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(StringAuthHeader) && StringAuthHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                string StringToken = StringAuthHeader.Substring("Bearer ".Length).Trim();
                if (string.IsNullOrEmpty(StringToken))
                {
                    throw new AuthenticationException("Token为空。");
                }
                string StringMemoryCacheKey = $"Token@{StringToken}";
                var MemoryCacheLoginLog = MemoryCacheHelper.GetObject<LoginLog>(StringMemoryCacheKey);
                if (MemoryCacheLoginLog != null)
                {
                    return MemoryCacheLoginLog;
                }
                LoginLog LoginLog = await LoginLogService.GetUserToken(StringToken);
                if (LoginLog == null)
                {
                    throw new AuthenticationException("授权信息失效，请重新登录。");
                }
                MemoryCacheHelper.SetObject(StringMemoryCacheKey, LoginLog, 60);
                return LoginLog;
            }
            else
            {
                throw new AuthenticationException("授权信息无效。");
            }
        }
    }
}
