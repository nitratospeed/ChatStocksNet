using Application.Common.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<AppDbContext>(options =>
            //        options.UseSqlServer(
            //            configuration.GetConnectionString("DefaultConnection"),
            //            b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

            //services.AddScoped<IAppDbContext>(provider => provider.GetService<AppDbContext>());
            
            services.AddSingleton<IRabbitMQConsumerService, RabbitMQConsumerService>();
            services.AddSingleton<IRabbitMQProducerService, RabbitMQProducerService>();
            services.AddSingleton<ISignalRService, SignalRService>();
            services.AddSingleton<IStockService, StockService>();

            services.AddHttpClient("Client", c =>
            {
                c.DefaultRequestHeaders.Connection.Add("keep-alive");
                c.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                c.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
                c.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("br"));
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
            });

            return services;
        }
    }
}
