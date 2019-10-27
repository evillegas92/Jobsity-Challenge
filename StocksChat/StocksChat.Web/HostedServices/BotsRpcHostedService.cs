using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using StocksChat.Web.Hubs;

namespace StocksChat.Web.HostedServices
{
    public class BotsRpcHostedService : IHostedService
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private HubConnection _signalRHubConnection;

        public BotsRpcHostedService(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            StartSignalRConnection();
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

        private async void StartSignalRConnection()
        {
            _signalRHubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44313/chatHub")
                .Build();
           
            _signalRHubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                System.Diagnostics.Debugger.Break();
                this.ProcessCommandMessage(user, message);
            });

            _signalRHubConnection.Closed += async (error) =>
            {
                await Task.Delay(10000);
                await _signalRHubConnection.StartAsync();
            };

            await _signalRHubConnection.StartAsync();
        }

        

        private async void ProcessCommandMessage(string user, string message)
        {
            if (!message.StartsWith("/"))
                return;
            string command = message.Split('=')[0].Replace("/", string.Empty);
            string commandParameter = message.Split('=')[1];
            //ready to queue message to RabbitMQ
        }
    }
}