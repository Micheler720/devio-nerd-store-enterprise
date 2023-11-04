using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSE.Core.Messages.Integration;
using NSE.MessageBus;
using NSE.Pedidos.API.Application.Queries;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.Pedidos.API.Serivces
{
    public class PedidoOrquestradorIntegrationHandler : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PedidoOrquestradorIntegrationHandler> _logger;
        private readonly IMessageBus _bus;
        private Timer _timer;

        public PedidoOrquestradorIntegrationHandler(
            ILogger<PedidoOrquestradorIntegrationHandler> logger, 
            IServiceProvider serviceProvider,
            IMessageBus bus)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _bus = bus;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Serviço de pedidos iniciado");
            _timer = new Timer(ProcessarPedidos, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));
            return Task.CompletedTask;
        }

        private async void ProcessarPedidos(object state)
        {
            _logger.LogInformation("Processando Pedidos.");

            PedidoAutorizadoIntegrationEvent pedidoAutorizado;

            using (var scope = _serviceProvider.CreateScope())
            {
                var pedidoQueries = scope.ServiceProvider.GetRequiredService<IPedidoQueries>();
                var pedido = await pedidoQueries.ObterPedidosAutorizados();

                if (pedido == null) return;

                pedidoAutorizado = new PedidoAutorizadoIntegrationEvent(
                    pedido.ClienteId,
                    pedido.Id,
                    pedido.PedidoItems.ToDictionary(p => p.ProdutoId, p => p.Quantidade));               

            }

            await _bus.PublishAsync(pedidoAutorizado);

            _logger.LogInformation($"Pedido ID: {pedidoAutorizado.PedidoId} foi encaminhado para baixa no estoque.");

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Serviço de pedido finalizado.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }

        
    }
}
