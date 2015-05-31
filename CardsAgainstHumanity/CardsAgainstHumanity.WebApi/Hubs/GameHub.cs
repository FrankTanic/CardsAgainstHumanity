using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using CardsAgainstHumanity.WebApi.Models;

namespace CardsAgainstHumanity.WebApi.Hubs
{
    public class GameHub : Hub
    {
        private CardsAgainstHumanityDbContext _db = new CardsAgainstHumanityDbContext();

        public async Task JoinRoom(string username, int roomID)
        {
            await Groups.Add(Context.ConnectionId, roomID.ToString());
            Clients.Group(roomID.ToString()).addChatMessage(username + " joined.");
        }

        public Task LeaveRoom(string username, int roomID)
        {
            Clients.Group(roomID.ToString()).addChatMessage(username + "has lefted.");
            return Groups.Remove(Context.ConnectionId, roomID.ToString());
        }
        
    }
}