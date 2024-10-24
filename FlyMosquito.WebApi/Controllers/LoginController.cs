#region using
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FlyMosquito.DataTransferObjec;
using FlyMosquito.Domain;
using FlyMosquito.Extension.SetUp;
using FlyMosquito.Service.AuditLog.IAuditLogService;
using FlyMosquito.Service.Basic.IBaseService;

#endregion

namespace FlyMosquito.WebApi.Controllers
{
    /// <summary>
    ///  用户登录
    /// </summary>
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService UserService;
        private readonly ILoginLogService LoginLogService;
        private readonly LoginLogSetup LoginLogSetup;

        /// <summary>
        /// 用户登录控制器构造函数
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="loginLogService"></param>
        /// <param name="loginLogSetup"></param>
        public LoginController(IUserService userService, ILoginLogService loginLogService, LoginLogSetup loginLogSetup)
        {
            this.UserService = userService;
            this.LoginLogService = loginLogService;
            this.LoginLogSetup = loginLogSetup;
        }

        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="Uid">账号</param>
        /// <param name="PassWord">密码</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login/LoginUserAsync")]
        [AllowAnonymous]
        public async Task<ApiResult<UserInfoDto>> LoginUserAsync(string Uid, string PassWord)
        {
            if (string.IsNullOrEmpty(Uid) || string.IsNullOrEmpty(PassWord))
            {
                return ApiResult<UserInfoDto>.Fail("数据异常");
            }
            var Result = await UserService.LoginAsync(Uid, PassWord);
            if (Result.Data != null)
            {
                await LoginLogService.CreateUserToken(new LoginLog() { Token = Result.Data.Token, LoginTime = DateTime.Now, LoginAccount = Result.Data.Uid, UserName = Result.Data.UserName });//记录登录信息
            }
            return Result;
        }

        /// <summary>
        /// 用户登出
        /// </summary>
        [HttpGet]
        [Route("Login/LogOutUserAsync")]
        public async Task<ApiResult<object>> LogOutUserAsync()
        {
            var LoginLog = LoginLogSetup.GetUserTokenAsync().Result;//获取当前用户的登录日志(验证登录信息是否还在）
            await LoginLogService.DeleteUserToken(LoginLog);
            return ApiResult<object>.Success();
        }
    }
}
