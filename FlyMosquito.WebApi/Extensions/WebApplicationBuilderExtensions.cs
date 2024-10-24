#region using
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using FlyMosquito.Common;
using FlyMosquito.Domain;
using FlyMosquito.Extension.Authorizations;
using FlyMosquito.Extension.Filter;
using FlyMosquito.Extension.SetUp;
#endregion

namespace FlyMosquito.WebApi.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static void ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //基础服务
            services.AddHttpContextAccessor();
            services.AddMemoryCache();
            services.AddSingleton<MemoryCacheHelper>();
            services.AddSingleton(new AppsettHelper(configuration));
            services.Configure<JwtToken>(configuration.GetSection("Jwt"));
            services.AddSingleton<RedisHelper>();

            //授权
            services.AddAuthorizations(configuration);

            //框架服务
            services.AddCors(c => c.AddPolicy("any", p => p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
            services.AddDbContext(configuration);
            services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy));
                options.Filters.Add<GlobalActionFilter>();
                options.Filters.Add<GlobalExceptionFilter>();
            });

            //自定义服务
            services.AddTransient<IAuthorizationHandler, PermissionHandlerManager>();
            services.AddScoped<LoginLogSetup>();
            services.AddSingleton<ApiRouteInfoHelper>();
            services.RepositoryRegister();
            services.ServicesRegister();

            //AutoMapper
            services.AddAutoMapper(typeof(AutoMapperSetup));

            //Swagger
            services.AddSwagger();
        }
    }
}

