using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StocksChat.Bot.Services;

namespace StocksChat.Bot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<IStockQuotesService, StockQuotesService>();
                    services.AddSingleton<IRabbitMqSender, RabbitMqSender>();
                    services.AddHostedService<Worker>();
                });
    }
}
