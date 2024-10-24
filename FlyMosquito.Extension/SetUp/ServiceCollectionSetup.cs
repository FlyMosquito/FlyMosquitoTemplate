#region using
using Microsoft.Extensions.DependencyInjection;
using FlyMosquito.Core;
using FlyMosquito.Service.AuditLog.AuditLogService;
using FlyMosquito.Service.AuditLog.IAuditLogService;
using FlyMosquito.Service.Basic.BaseService;
using FlyMosquito.Service.Basic.IBaseService;
#endregion

namespace FlyMosquito.Extension.SetUp
{
    public static class ServiceCollectionSetup
    {
        /// <summary>
        /// 注册仓储
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection RepositoryRegister(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            return services;
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ServicesRegister(this IServiceCollection services)
        {
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRoleMappingService, UserRoleMappingService>();
            services.AddTransient<IRoleApiAuthMappingService, RoleApiAuthMappingService>();
            services.AddTransient<IApiAuthService, ApiAuthService>();
            services.AddTransient<IWebApiLogService, WebApiLogService>();
            services.AddTransient<ILoginLogService, LoginLogService>();
            return services;
        }
    }
}
