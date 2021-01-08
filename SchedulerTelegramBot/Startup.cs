using Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SchedulerTelegramBot.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.PostgreSql;

namespace SchedulerTelegramBot
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
            services.AddDbContext<SchedulesContext>(options =>
            {
                options.UseNpgsql(Config.GetConnectionString("Main"), b=>b.MigrationsAssembly(nameof(Infrastructure)));
            });

            services.AddHttpClient();
            
            services.AddSingleton<ITelegramBotClientFactory, TelegramBotClientFactory>();

            services.AddSingleton<ITelegramClientAdapter, TelegramClientAdapter>();

            services.AddControllers();

            services.AddHangfire(config =>
            {
                config.UsePostgreSqlStorage(Config.GetConnectionString("Hangfire"));
            });
            JobStorage.Current = new PostgreSqlStorage(Config.GetConnectionString("Hangfire"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHangfireServer();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseHangfireDashboard();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
