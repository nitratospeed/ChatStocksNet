using Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class SignalRService : Hub, ISignalRService
    {
        public async Task SendMessage(string user, string message, string room, bool join)
        {
            if (join)
            {
                await JoinRoom(room);
                await Clients.Group(room).SendAsync("ReceiveMessage", user, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {user} joined {room}");
            }
            else
            {
                await Clients.Group(room).SendAsync("ReceiveMessage", user, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {user} says: {message}");
            }
        }

        public async Task JoinRoom(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task LeaveRoom(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }
    }
}
