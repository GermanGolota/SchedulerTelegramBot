using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SchedulerTelegramBot.Client;
using System.Threading.Tasks;

namespace SchedulerTelegramBot
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await InitializeTelegramClient(host);

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.UseKestrel();
                    webBuilder.UseStartup<Startup>();
                });
        }
        private static async Task InitializeTelegramClient(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var client = scope.ServiceProvider.GetService<ITelegramClientAdapter>();
                await client.BootUpClient();
            }
        }

    }
}
