using DiscountWorker.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DiscountWorker.Infrastructure.Network
{
    public class TcpServerService : BackgroundService
    {
        private readonly ILogger<TcpServerService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly int _port = 5000; // Set your desired port

        public TcpServerService(IServiceScopeFactory scopeFactory, ILogger<TcpServerService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, _port);
            listener.Start();
            _logger.LogInformation("TCP server started and listening on port {Port}", _port);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (listener.Pending())
                    {
                        TcpClient client = await listener.AcceptTcpClientAsync(stoppingToken);
                        _ = HandleClientAsync(client, stoppingToken);
                    }
                    else
                    {
                        await Task.Delay(100, stoppingToken); // Prevent tight loop
                    }
                }
            }
            finally
            {
                listener.Stop();
                _logger.LogInformation("TCP server stopped.");
            }
        }

        private async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken)
        {
            using (client)
            {
                var buffer = new byte[1024];
                var stream = client.GetStream();

                while (!cancellationToken.IsCancellationRequested && client.Connected)
                {
                    int bytesRead = 0;
                    try
                    {
                        bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error reading from client stream.");
                        break;
                    }

                    if (bytesRead == 0)
                    {
                        // Client closed connection
                        break;
                    }

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    _logger.LogInformation("Received message: {Message}", message);

                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var processor = scope.ServiceProvider.GetRequiredService<IMessageProcessor>();
                        // TODO: Process or save the message as needed
                        // Optional: respond back to client

                        var res = await processor.ProcessMessage(message);

                        byte[] response = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(res));
                        await stream.WriteAsync(response, 0, response.Length);
                    }
                }
            }
        }
    }
}