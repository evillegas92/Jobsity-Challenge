using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CsvHelper;
using StocksChat.Bot.Models;

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
            string stringValue;
            string uri = $"?s={stockId}&f=sd2t2ohlcv&h&e=csv";
            var message = new HttpRequestMessage(HttpMethod.Get, uri);
            var result = await _client.SendAsync(message);
            if (result.IsSuccessStatusCode)
            {
                using (var reader = new StreamReader(await result.Content.ReadAsStreamAsync()))
                using(var csvHelper = new CsvReader(reader))
                {
                    IEnumerable<StockQuote> stockQuotes = csvHelper.GetRecords<StockQuote>();
                    stringValue = stockQuotes.FirstOrDefault()?.Open.ToString("C") ?? string.Empty;
                }
            }
            else
            {
                return $"Error retrieving stock data for {stockId}.";
            }
            message.Dispose();
            result.Dispose();

            return string.IsNullOrWhiteSpace(stringValue) ? $"Error retrieving stock data for {stockId}." : $"{stockId.ToUpper()} quote is {stringValue} per share.";
        }
    }
}