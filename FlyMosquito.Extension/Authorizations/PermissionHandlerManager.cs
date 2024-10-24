#region using
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using FlyMosquito.Common;
using FlyMosquito.Service.Basic.IBaseService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
#endregion

namespace FlyMosquito.Extension.Authorizations
{
    public class PermissionHandlerManager : AuthorizationHandler<PermissionRequirement>
    {
        #region 功能基础信息
        private readonly IRoleApiAuthMappingService RoleApiAuthMappingService;
        public IAuthenticationSchemeProvider Schemes { get; set; }
        public PermissionRequirement Requirement { get; set; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schemes"></param>
        /// <param name="roleApiAuthMappingService"></param>
        public PermissionHandlerManager(IAuthenticationSchemeProvider schemes, IRoleApiAuthMappingService roleApiAuthMappingService)
        {
            Schemes = schemes;
            RoleApiAuthMappingService = roleApiAuthMappingService;
        }

        /// <summary>
        /// 权限验证逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var httpContext = (context.Resource as DefaultHttpContext)?.HttpContext;
            if (httpContext == null)
            {
                LoggerHelper.Error("无法获取 HttpContext");
                context.Fail();
                return;
            }

            var requestUrl = httpContext.Request.Path.Value.ToUpper();
            var requestMethod = httpContext.Request.Method.ToUpper();

            // Step 1: 检查请求是否已经被拦截处理
            if (await HandleRequestAsync(httpContext))
            {
                context.Fail();
                return;
            }

            // Step 2: 加载系统角色-API权限映射
            var permissions = await LoadPermissionsAsync(requirement);
            if (!permissions.Any())
            {
                LoggerHelper.Warn("没有任何权限数据");
                context.Fail();
                return;
            }

            // Step 3: 获取用户角色
            var userRoles = GetUserRoles(context.User);
            if (!userRoles.Any())
            {
                LoggerHelper.Warn("用户没有任何角色，权限验证失败");
                context.Fail();
                return;
            }

            // Step 4: 验证用户是否有权限访问当前请求的 URL 和请求方法
            if (!HasPermission(userRoles, requestUrl, requestMethod, requirement.Permissions))
            {
                LoggerHelper.Warn("用户没有访问该 API 的权限");
                context.Fail();
                return;
            }

            // Step 5: 验证用户登录状态和 Token 有效期
            if (!await AuthenticateUserAsync(httpContext))
            {
                context.Fail();
                return;
            }

            context.Succeed(requirement);
        }

        /// <summary>
        /// 检查请求是否已经被拦截处理
        /// </summary>
        private async Task<bool> HandleRequestAsync(HttpContext httpContext)
        {
            var handlers = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            foreach (var scheme in await Schemes.GetRequestHandlerSchemesAsync())
            {
                var handler = await handlers.GetHandlerAsync(httpContext, scheme.Name) as IAuthenticationRequestHandler;
                if (handler != null && await handler.HandleRequestAsync())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 加载系统权限
        /// </summary>
        private async Task<IEnumerable<Permission>> LoadPermissionsAsync(PermissionRequirement requirement)
        {
            var roleApiAuthMappings = await RoleApiAuthMappingService.GetRoleApiAuthMappingAsync();

            // 构建权限列表
            var permissions = new List<Permission>();

            foreach (var mapping in roleApiAuthMappings)
            {
                // 对每个角色，获取其对应的 API 权限
                foreach (var apiAuth in mapping.ApiAuths)
                {
                    permissions.Add(new Permission
                    {
                        RoleId = mapping.RoleId,
                        RoleName = mapping.RoleName,
                        Controller = apiAuth.Controller.ToUpper(),
                        RoutePath = apiAuth.RoutePath.ToUpper(),
                        Action = apiAuth.Action.ToUpper() // 假设 ApiAuthDto 中有 Method 字段
                    });
                }
            }

            // 设置权限到要求中
            requirement.Permissions = permissions;

            return permissions;
        }

        /// <summary>
        /// 获取用户角色
        /// </summary>
        private List<string> GetUserRoles(ClaimsPrincipal user)
        {
            return user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        }

        /// <summary>
        /// 验证用户是否有权限访问指定 URL
        /// </summary>
        private bool HasPermission(IEnumerable<string> userRoles, string requestUrl, string requestMethod, IEnumerable<Permission> permissions)
        {
            return permissions.Any(p => userRoles.Contains(p.RoleName) && p.RoutePath == requestUrl && (string.IsNullOrEmpty(p.Action) || p.Action == requestMethod));
        }

        /// <summary>
        /// 验证用户登录状态和 Token 有效性
        /// </summary>
        private async Task<bool> AuthenticateUserAsync(HttpContext httpContext)
        {
            var defaultAuthenticateScheme = await Schemes.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenticateScheme != null)
            {
                var result = await httpContext.AuthenticateAsync(defaultAuthenticateScheme.Name);
                if (result?.Principal != null)
                {
                    httpContext.User = result.Principal;
                    var expirationClaim = httpContext.User.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);
                    if (expirationClaim != null)
                    {
                        var expirationTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expirationClaim.Value)).UtcDateTime;//JWT 中的 exp 是 Unix 时间戳，需要将其转换为 DateTime
                        if (expirationTime >= DateTime.UtcNow)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
