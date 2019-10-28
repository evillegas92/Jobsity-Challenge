using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace StocksChat.Bot.Services
{
    public interface IRabbitMqSender
    {
        void SendMessage(object payload);
    }

    public class RabbitMqSender : IRabbitMqSender
    {
        public void SendMessage(object payload)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                string message = JsonConvert.SerializeObject(payload);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                    routingKey: "hello",
                    basicProperties: null,
                    body: body);
            }

        }
    }
}