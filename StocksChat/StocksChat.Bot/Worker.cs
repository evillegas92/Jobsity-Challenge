using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace StocksChat.Bot
{
    public class Worker : IHostedService, IDisposable
    {
        private readonly ILogger<Worker> _logger;
        private HubConnection _signalRHubConnection;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            StartSignalRConnection();
            _logger.LogInformation("SignalRConnection initiated.");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker stopping at: {time}", DateTimeOffset.Now);
            return Task.CompletedTask;
        }

        private async void StartSignalRConnection()
        {
            _signalRHubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44313/chatHub")
                .Build();
           
            _signalRHubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
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
            if (!message.StartsWith("/") || !message.Contains("="))
                return;
            string command = message.Split('=')[0].Replace("/", string.Empty);
            string commandParameter = message.Split('=')[1];
            //ready to queue message to RabbitMQ
            _logger.LogInformation($"Worker received command: {command}={commandParameter}");
        }

        /// <summary>
        /// Dispose Timer
        /// </summary>
        public void Dispose()
        {
        }
    }
}