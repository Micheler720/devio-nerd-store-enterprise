using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSE.Core.Utils;
using NSE.MessageBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Bff.Compras.Configurations
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(
                    this IServiceCollection services,
                    IConfiguration configuration)
        {
        }
    }
}
