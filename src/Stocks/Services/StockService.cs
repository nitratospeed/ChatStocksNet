using CsvHelper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stocks.Options;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Stocks.Services
{
    public class StockService : IStockService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger _logger;
        private readonly StockOptions _stockOptions;

        public StockService(IHttpClientFactory clientFactory, ILogger<StockService> logger, IOptions<StockOptions> stockOptions)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _stockOptions = stockOptions.Value;
        }

        public async Task<string> GetByStockCode(string stock_code)
        {
            try
            {
                var result = string.Empty;
                var symbol = string.Empty;
                var close = string.Empty;

                var resource = Regex.Replace(_stockOptions.URI, @"(stock_code)", stock_code);

                var request = new HttpRequestMessage(HttpMethod.Get, resource);

                var client = _clientFactory.CreateClient("Client");

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();

                    using (var reader = new StreamReader(stream))

                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        csv.Read();
                        var stock = csv.GetRecord<Stock>();
                        symbol = stock.Symbol;
                        close = stock.Close;
                    }

                    result = close == "$N/D" ? $"{symbol} quote doesn't exist" : $"{symbol} quote is ${close} per share";
                }
                else
                {
                    _logger.LogInformation($"GetByStockCode: {response.StatusCode}:{response.ReasonPhrase}");
                    result = "Service error. Try again in a few minutes.";
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetByStockCode: {ex.Message}");
                return "Unknown error. Try again in a few minutes.";
            }
        }

        private class Stock
        {
            public string Symbol { get; set; }
            public string Date { get; set; }
            public string Time { get; set; }
            public string Open { get; set; }
            public string High { get; set; }
            public string Low { get; set; }
            public string Close { get; set; }
            public string Volume { get; set; }
        }
    }
}
