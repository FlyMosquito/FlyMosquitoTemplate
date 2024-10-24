#region using
#endregion

namespace FlyMosquito.WebApi.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void ConfigureMiddlewareService(this WebApplication app)
        {
            //配置中间件管道
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseCors("any");
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();//认证
            app.UseAuthorization();//授权
            app.MapControllers();
        }
    }
}
