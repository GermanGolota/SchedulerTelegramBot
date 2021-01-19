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
        public static IServiceCollection AddConfiguredHangfire(this IServiceCollection services, Action<HangfireSetupOptions> setOptions)
        {
            HangfireSetupOptions options = new HangfireSetupOptions();
            setOptions(options);
            string connString = options.connectionString;
            bool prepareSchema = options.PrepareSchema;
            var sqlOptions = new PostgreSqlStorageOptions();
            sqlOptions.PrepareSchemaIfNecessary = prepareSchema;

            services.AddHangfire(config =>
            {
                config.UsePostgreSqlStorage(connString, sqlOptions);
            });
            JobStorage.Current = new PostgreSqlStorage(connString, sqlOptions);
            return services;
        }
        public class HangfireSetupOptions
        {
            public bool PrepareSchema { get; set; }
            public string connectionString { get; set; }
        }
    }
}
