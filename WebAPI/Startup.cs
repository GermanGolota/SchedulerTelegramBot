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
            services.AddDbContext<SchedulesContext>(options =>
            {
                options.UseNpgsql(Config.GetConnectionString("Main"), b=>b.MigrationsAssembly(nameof(Infrastructure)));
            });

            services.AddHttpClient();

            services.AddRepositories();

            services.AddScoped<IJobManager, JobManager>();

            services.AddTelegramClient();
            //Signifies that commands are in the same assembly as startup
            services.AddTelegramCommands(typeof(Startup).Assembly);

            services.AddControllers();

            var options = new PostgreSqlStorageOptions();
            options.PrepareSchemaIfNecessary = true;

            services.AddHangfire(config =>
            {
                config.UsePostgreSqlStorage(Config.GetConnectionString("Main"), options);
            });
            JobStorage.Current = new PostgreSqlStorage(Config.GetConnectionString("Main"), options);
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
