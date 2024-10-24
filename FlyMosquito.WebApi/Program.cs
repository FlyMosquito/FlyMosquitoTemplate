#region using
using FlyMosquito.WebApi.Extensions;
#endregion

namespace FlyMosquito.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.ConfigureApplicationServices(builder.Configuration);

            var app = builder.Build();

            app.ConfigureMiddlewareService();

            app.Run();
        }
    }
}
