using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StocksChat.Web.Hubs;

namespace StocksChat.Web.HostedServices
{
    public class BotsRpcHostedService : IHostedService
    {
        private readonly IHubContext<ChatHub> _hubContext;

        private readonly IConnection _connection;
        private readonly IModel _channel;

        public BotsRpcHostedService(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
            
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            RegisterRabbitMqListener();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _connection.Close();
            return Task.CompletedTask;
        }

        private void RegisterRabbitMqListener()
        {
            _channel.QueueDeclare(queue: "hello",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);

                SendToHub("Bot", message);
            };

            _channel.BasicConsume(queue: "hello", autoAck: true, consumer: consumer);
        }

        private void SendToHub(string user, string message)
        {
            _hubContext.Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}