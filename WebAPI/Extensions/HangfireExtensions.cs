using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Extensions
{
    public static class HangfireExtensions
    {
        public static IServiceCollection AddConfiguredHangfire(this IServiceCollection services, IConfiguration Config)
        {
            var options = new PostgreSqlStorageOptions();
            options.PrepareSchemaIfNecessary = true;

            services.AddHangfire(config =>
            {
                config.UsePostgreSqlStorage(Config.GetConnectionString("Main"), options);
            });
            JobStorage.Current = new PostgreSqlStorage(Config.GetConnectionString("Main"), options);

            return services;
        }
    }
}
