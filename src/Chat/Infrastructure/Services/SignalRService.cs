using Application.Common.Interfaces;
using Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using SharedContracts;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class SignalRService : Hub, ISignalRService
    {
        private readonly IServiceProvider _serviceProvider;

        public SignalRService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task SendMessage(string user, string message, string room, bool join)
        {
            using var scope = _serviceProvider.CreateScope();

            var baseRepository = scope.ServiceProvider.GetRequiredService<IBaseRepository<Message>>();

            if (join)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, room);

                var history = await baseRepository.GetAllByFilter(x => x.Room == room);

                var historyList = new List<string>();

                foreach (var item in history)
                {
                    historyList.Add($"{item.CreatedAt:yyyy-MM-dd HH:mm:ss} {item.User} says: {item.Text}");
                }

                await Clients.Client(Context.ConnectionId).SendAsync("History", historyList);

                await Clients.Group(room).SendAsync("ReceiveMessage", user, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {user} joined {room}");
            }
            else
            {
                if (message != null)
                {
                    var match = Regex.Match(message, @"(\/stock=)([a-zA-z\.]*)");

                    if (match.Success)
                    {
                        var stockCode = match.Groups[2].ToString();

                        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

                        var contractMessage = new StockCodeContract { StockCode = stockCode, Room = room };

                        await publishEndpoint.Publish(contractMessage);
                    }
                    else
                    {
                        await baseRepository.Insert(new Message { User = user, Text = message, Room = room });

                        await Clients.Group(room).SendAsync("ReceiveMessage", user, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {user} says: {message}");
                    }
                }
            }
        }
    }
}
