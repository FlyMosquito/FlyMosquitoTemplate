#region using
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;
#endregion

namespace FlyMosquito.Extension.SetUp
{
    public static class AddSwaggerSetup
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                var basePath = AppContext.BaseDirectory;//获取当前应用程序的路径
                var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";//获取 XML 注释文件路径
                var xmlPath = Path.Combine(basePath, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }

                // 添加安全定义
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "格式：Bearer {token}",
                    Name = "Authorization", // 默认的参数名
                    In = ParameterLocation.Header,// 放于请求头中
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                // 添加安全要求
                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                   {
                        new OpenApiSecurityScheme{
                             Reference = new OpenApiReference{
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    }, new string[]{}
                   }
                });

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FlyMosquito API", Version = "v1" });
            });
        }
    }
}
