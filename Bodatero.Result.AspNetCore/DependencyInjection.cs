using Microsoft.Extensions.DependencyInjection;

namespace Bodatero.Result.AspNetCore
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddResult(
            this IServiceCollection serviceCollection, Action<ResultServiceConfig> configAction)
        {
            serviceCollection.AddSingleton(new ResultService(configAction));

            return serviceCollection;
        }

        public static IServiceCollection AddResult(
            this IServiceCollection serviceCollection, ResultServiceConfig config)
        {
            serviceCollection.AddSingleton(new ResultService(config));

            return serviceCollection;
        }

        public static IServiceCollection AddResult(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(new ResultService());

            return serviceCollection;
        }
    }
}
