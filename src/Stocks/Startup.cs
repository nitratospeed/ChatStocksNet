using GreenPipes;
using MassTransit;
using MBProducer.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Stocks.Consumers;
using Stocks.Options;
using Stocks.Services;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MBProducer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IStockService, StockService>();

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

            services.Configure<StockOptions>(Configuration.GetSection(StockOptions.Stock));

            var rabbitMQOptions = new RabbitMQOptions();
            Configuration.GetSection(RabbitMQOptions.RabbitMQ).Bind(rabbitMQOptions);

            services.AddMassTransit(x =>
            {
                x.AddConsumer<GetStockValueConsumer>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri(rabbitMQOptions.Hostname), h =>
                    {
                        h.Username(rabbitMQOptions.User);
                        h.Password(rabbitMQOptions.Pass);
                    });
                    cfg.ReceiveEndpoint(rabbitMQOptions.Queue, ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));
                        ep.ConfigureConsumer<GetStockValueConsumer>(provider);
                    });
                }));
            });

            services.AddMassTransitHostedService();

            services.AddControllers();

            services.Configure<RabbitMQOptions>(Configuration.GetSection(RabbitMQOptions.RabbitMQ));

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Stocks API"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"v1/swagger.json", "v1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
