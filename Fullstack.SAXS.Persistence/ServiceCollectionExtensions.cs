using Fullstack.SAXS.Persistence.DbContexts;
using Fullstack.SAXS.Persistence.DbListnres;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fullstack.SAXS.Persistence
{
    public static class ServiceCollectionExtensions
    {
        /*public static void AddDbListner(this IServiceCollection services, IConfiguration config)
        {
            services
                .AddHostedService<PostgresNotifyListener>(
                    option => option.
                );
        }

        public static void AddDb(this IServiceCollection services, IConfiguration config)
        {
            services
                .AddDbContext<PosgresDbContext>(
                    options => options.UseNpgsql(config.GetConnectionString("PostgresConnection"))
                )
                .AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<PosgresDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();
        }*/
    }
}
