using Infrastructure.DTOs;
using Infrastructure.Parsers;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.Extensions
{
    public static class RepositoriesExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IModelConverter, ModelConverter>();
            services.AddSingleton<ICroneVerifier, CroneVerifier>();

            services.AddScoped<IChatRepo, ChatRepo>();
            services.AddScoped<IScheduleRepo, ScheduleRepo>();

            return services;
        }
    }
}
