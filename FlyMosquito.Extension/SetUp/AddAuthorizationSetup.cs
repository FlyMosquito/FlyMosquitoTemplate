#region using
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using FlyMosquito.Extension.Authorizations;
using System.Text;
#endregion

namespace FlyMosquito.Extension.SetUp
{
    public static class AddAuthorizationSetup
    {
        public static void AddAuthorizations(this IServiceCollection services, IConfiguration configuration)
        {
            var JwtToken = configuration.GetSection("Jwt").Get<FlyMosquito.Domain.JwtToken>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    // 验证密钥
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtToken.SecretKey)),

                    // 验证令牌的颁发者
                    ValidateIssuer = true,
                    ValidIssuer = JwtToken.Issuer,

                    // 验证令牌的受众
                    ValidateAudience = true,
                    ValidAudience = JwtToken.Audience,

                    // 验证令牌的过期时间
                    ValidateLifetime = true,

                    // 允许一定的时钟偏移量
                    ClockSkew = TimeSpan.Zero
                };
                x.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        //此处终止代码
                        context.HandleResponse();
                        var res = "{\"code\":401,\"err\":\"无权限\"}";
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.WriteAsync(res);
                        return Task.FromResult(0);
                    }
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("PermissionPolicy", policy => policy.Requirements.Add(new PermissionRequirement("RequiredPermission")));
            });
        }
    }
}
