using System;
using System.Threading.Tasks;
using StocksChat.Bot.Services;
using Xunit;

namespace StocksChat.Bot.Test.Services
{
    public class StockQuotesServiceTests
    {
        [Fact]
        public async Task GetStockQuote_Success()
        {
            const string stockId = "aapl.us";
            string error = $"Error retrieving stock data for {stockId}.";

            IStockQuotesService service = new StockQuotesService();
            string response = await service.GetStockQuote(stockId);

            Assert.NotEqual(error, response);
        }
    }
}
