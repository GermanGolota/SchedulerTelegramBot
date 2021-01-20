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

            services.AddConfiguredHangfire(x=>
            {
                x.connectionString = Config.GetConnectionString("Main");
                x.PrepareSchema = true;
            });

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 8443;
            });
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
