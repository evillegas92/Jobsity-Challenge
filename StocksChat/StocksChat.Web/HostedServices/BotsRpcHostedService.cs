using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using StocksChat.Web.Hubs;

namespace StocksChat.Web.HostedServices
{
    public class BotsRpcHostedService : IHostedService
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public BotsRpcHostedService(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void SendToHub(string user, string message)
        {
            _hubContext.Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}