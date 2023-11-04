using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSE.Core.Utils;
using NSE.MessageBus;
using NSE.Pedidos.API.Serivces;

namespace NSE.Pedidos.API.Configuration
{
    public static class MessageBusConfig
    {
        public static IServiceCollection AddMessageBusConfiguration(
                   this IServiceCollection services,
                   IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnection())
                .AddHostedService<PedidoOrquestradorIntegrationHandler>()
                .AddHostedService<PedidoIntegrationHandler>();
            return services;
        }
    }
}
