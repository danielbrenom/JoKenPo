using JoKenPo.Api.Configurations;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(JoKenPo.Api.Startup))]
namespace JoKenPo.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            ApplicationConfigurator.Instantiate();
            builder.Services.ConfigureContainer(ApplicationConfigurator.Instance.Get());
        }
    }
}