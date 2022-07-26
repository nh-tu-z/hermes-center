using System.Net.WebSockets;
using System.Text;
using HermesCenter.Logger;

namespace HermesCenter.BackgroundServices
{
    /// <summary>
    /// Ref for backgroud services: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-6.0&tabs=visual-studio 
    /// This implementation hosts service that activates a scoped service. The scope service ca use dependency injection (DI)
    /// 
    /// WebSocket Serive uses WebSocket protocol to listen a configured socket and receive messege. 
    /// The message will be processed and executed for specific perpose. i.e. TODO - add information about primitive project
    /// </summary>
    public class WebSocketService : BackgroundService
    {
        private readonly ILogManager _logManager;

        public WebSocketService(IServiceProvider serviceProvider /* TOption for configuring websocket */)
        {
            var scope = serviceProvider.CreateScope().ServiceProvider;
            _logManager = scope.GetRequiredService<ILogManager>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logManager.Information("Background Websocket Start!", "HermesCenter");
            while (!stoppingToken.IsCancellationRequested)
            {
                using var socket = new ClientWebSocket();
                try
                {
                    await socket.ConnectAsync(new Uri("wss://demo.piesocket.com/v3/channel_1?api_key=VCXCEuvhGcBDP7XhiJJUDvR1e1D3eiVjgZ9VRiaV&notify_self"), stoppingToken);
                    await Receive(socket, stoppingToken);
                    _logManager.Information($"WebSocket state: {socket.State}", "HermesCenter");
                }
                catch(Exception ex)
                {
                    _logManager.Error(ex.Message, "HermesCenter");
                }
            }
        }

        private async Task Receive(ClientWebSocket socket, CancellationToken stoppingToken)
        {
            var buffer = new ArraySegment<byte>(new byte[2048]);
            while (!stoppingToken.IsCancellationRequested)
            {
                WebSocketReceiveResult result;
                using var ms = new MemoryStream();
                do
                {
                    result = await socket.ReceiveAsync(buffer, stoppingToken);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                } while (!result.EndOfMessage);

                if (result.MessageType == WebSocketMessageType.Close)
                    break;

                ms.Seek(0, SeekOrigin.Begin);
                using var reader = new StreamReader(ms, Encoding.UTF8);
                {
                    var json = reader.ReadToEndAsync().Result;
                    _logManager.Information($"WebSocket Message: {json}", "HermesCenter");

                    // TODO - implement to decode json message
                }
            }
        }
    }
}
