using Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.PostgreSql;
using WebAPI.Extensions;
using WebAPI.Jobs;
using Microsoft.AspNetCore.Http;
using WebAPI.Commands;
using System;
using WebAPI.Services;

namespace WebAPI
{
    public class Startup
    {
        public IConfiguration Config { get; }

        public Startup(IConfiguration config)
        {
            Config = config;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = GetConnectionString(Config);
            services.AddDbContext<SchedulesContext>(options =>
            {
                options.UseNpgsql(connectionString, b=>b.MigrationsAssembly(nameof(Infrastructure)));
            });

            services.AddHttpClient();

            services.AddRepositories();

            services.AddScoped<IUpdateManager, UpdateManager>();
            services.AddScoped<IJobManager, JobManager>();

            services.AddTelegramClient();
            //Signifies that commands are in the same assembly as startup
            services.AddTelegramCommands(typeof(Startup).Assembly);

            services.AddControllers();

            services.AddConfiguredHangfire(x=>
            {
                x.ConnectionString = connectionString;
                x.PrepareSchema = true;
            });

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 8443;
            });

            string useLongPulling = Config["UseLongPulling"] ?? "false";
            if (useLongPulling == "true")
            {
                services.AddHostedService<LongPullingTelegramService>();
            }
        }

        private string GetConnectionString(IConfiguration config)
        {
            var port = config["DBPort"] ?? "5432";
            var user = config["DBUser"] ?? "postgres";
            var password = config["Password"] ?? "password";
            var dbHost = config["DBHost"] ?? "localhost";
            var initialDb = config["DBName"] ?? "scheduledb";
            string connString = $"Host={dbHost};Port={port};" +
                $"Database={initialDb};Username={user};Password={password}";
            return connString;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

            app.UseHangfireServer();

            //could be moved into dev mode only
            app.UseHangfireDashboard();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
