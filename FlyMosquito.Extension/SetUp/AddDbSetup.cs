#region using
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FlyMosquito.Core;
#endregion

namespace FlyMosquito.Extension.SetUp
{
    public static class AddDbSetup
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FlyMosquitoDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default")));
            return services;
        }
    }
}
