using Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class StockService : IStockService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger _logger;

        public StockService(IHttpClientFactory clientFactory, ILogger<StockService> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<string> GetByStockCode(string stock_code)
        {
            try
            {
                var result = "";
                var symbol = "";
                var close = "$";

                var resource = $"https://stooq.com/q/l/?s={stock_code}&f=sd2t2ohlcv&h&e=csv";

                var request = new HttpRequestMessage(HttpMethod.Get, resource);

                var client = _clientFactory.CreateClient("Client");

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var csvFile = await response.Content.ReadAsStreamAsync();

                    using var reader = new StreamReader(csvFile);

                    var i = 0;

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        if (i == 1)
                        {
                            symbol = values[0];
                            close += values[6];
                        }
                        i++;
                    }

                    result = close == "$N/D" ? $"{symbol} quote doesn't exist" : $"{symbol} quote is {close} per share";
                }
                else
                {
                    _logger.LogInformation($"{response.StatusCode}:{response.ReasonPhrase}");
                    result = "Service error. Try again in a few minutes.";
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return "Unknown service error. Try again in a few minutes.";
            }
        }
    }
}
