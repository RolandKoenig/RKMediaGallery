using Microsoft.Extensions.DependencyInjection;

namespace RKMediaGallery.Views;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        return services
            .AddTransient<HomeViewModel>();
    }
}