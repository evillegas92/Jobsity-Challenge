using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StocksChat.Bot.Services;

namespace StocksChat.Bot
{
    public class Worker : IHostedService, IDisposable
    {
        private readonly ILogger<Worker> _logger;
        private readonly IStockQuotesService _stockQuotesService;
        private readonly IRabbitMqSender _rabbitMqSender;
        private HubConnection _signalRHubConnection;

        public Worker(ILogger<Worker> logger, IStockQuotesService stockQuotesService, IRabbitMqSender rabbitMqSender)
        {
            _logger = logger;
            _stockQuotesService = stockQuotesService;
            _rabbitMqSender = rabbitMqSender;
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
            string command = message.Split('=')[0].Replace("/", string.Empty).ToLower();
            string commandParameter = message.Split('=')[1];

            _logger.LogInformation($"Worker received command: {command}={commandParameter}");

            if (command == "stock")
            {
                //replace with a better way to dynamically spin up service implementations to bot command registrations (i.e. fetch list of registered bots, use reflection to create an instance of that class and call DoWork on it - where DoWork is a method in the interface all bots must implement)
                var result = await _stockQuotesService.GetStockQuote(commandParameter);
                
                //tell RabbitMQ to publish the result
                _rabbitMqSender.SendMessage(result);
                _logger.LogInformation($"Worker sent result: {result} to RabbitMQ");
            }
        }

        /// <summary>
        /// Dispose Timer
        /// </summary>
        public void Dispose()
        {
        }
    }
}