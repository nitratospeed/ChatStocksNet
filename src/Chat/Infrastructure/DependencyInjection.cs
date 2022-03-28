using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Options;
using GreenPipes;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("ChatStocksNetDb"));

            services
                .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                        .AddEntityFrameworkStores<AppDbContext>();

            services.AddSingleton<ISignalRService, SignalRService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IBaseRepository<Message>, BaseRepository<Message>>();

            services.AddSignalR();

            services.Configure<RabbitMQOptions>(configuration.GetSection(RabbitMQOptions.RabbitMQ));

            var rabbitMQOptions = new RabbitMQOptions();
            configuration.GetSection(RabbitMQOptions.RabbitMQ).Bind(rabbitMQOptions);

            services.AddMassTransit(x =>
            {
                x.AddConsumer<NotificationService>();
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
                        ep.ConfigureConsumer<NotificationService>(provider);
                    });
                }));
            });

            services.AddMassTransitHostedService();

            return services;
        }
    }
}
