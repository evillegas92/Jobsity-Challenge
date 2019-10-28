using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StocksChat.Bot.Services
{
    public interface IStockQuotesService
    {
        Task<string> GetStockQuote(string stockId);
    }

    public class StockQuotesService : IStockQuotesService
    {
        private static readonly HttpClient _client = new HttpClient {BaseAddress = new Uri("https://stooq.com/q/l/")};

        public async Task<string> GetStockQuote(string stockId)
        {
            string response = string.Empty;
            string uri = $"?s={stockId}&f=sd2t2ohlcv&h&e=csv";
            var message = new HttpRequestMessage(HttpMethod.Get, uri);
            var result = await _client.SendAsync(message);
            if (result.IsSuccessStatusCode)
            {
                //var reason = await result.Content.ReadAsJsonAsync<ReasonModel>();

                response = "APPL.US quote is $93.42 per share.";
            }
            else
            {
                response = $"Error retrieving stock data for {stockId}.";
            }
            message.Dispose();
            result.Dispose();
            return response;
        }
    }
}