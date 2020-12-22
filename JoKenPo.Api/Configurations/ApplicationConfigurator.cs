using System;
using JoKenPo.Domain.Interfaces;
using JoKenPo.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace JoKenPo.Api.Configurations
{
    public class ApplicationConfigurator
    {
        public static ApplicationConfigurator Instance { get; protected set; }
        protected internal ServiceProvider Container;

        public static void Instantiate()
        {
            if (Instance != null)
                return;
            Instance = new ApplicationConfigurator();
        }
        protected internal static void Initialize(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMemoryCache()
                             .AddTransient<ICache, Cache>()
                             .AddTransient<ISessionService, SessionService>()
                             .AddTransient<IPlayerService, PlayerService>()
                             .AddTransient<ITurnService, TurnService>();
        }
        
        public  object GetService(Type tipo)
        {
            return Container.GetService(tipo);
        }
        public TType GetService<TType>()
        {
            return Container.GetService<TType>();
        }
    }
    
    public static class ApplicationConfiguratorExtension
    {
        public static IServiceCollection ConfigureContainer(this IServiceCollection services, ApplicationConfigurator configurator)
        {
            ApplicationConfigurator.Initialize(services);
            configurator.Container = services.BuildServiceProvider();
            return services;
        }

        public static ApplicationConfigurator Get(this ApplicationConfigurator applicationConfigurator)
        {
            return applicationConfigurator;
        }
    }
}