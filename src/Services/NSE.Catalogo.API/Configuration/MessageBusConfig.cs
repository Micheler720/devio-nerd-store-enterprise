using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSE.Catalogo.API.Services;
using NSE.Core.Utils;
using NSE.MessageBus;
using System;

namespace NSE.Catalogo.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfig(this IServiceCollection services, IConfiguration configuration)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Endereco do Rabbit: {configuration.GetMessageQueueConnection()}");
            services.AddMessageBus(configuration.GetMessageQueueConnection())
                .AddHostedService<CatalogoIntegrationHandler>();
        }
    }
}
