﻿using System;
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

        public override Task OnConnected()
        {
            var username = Context.QueryString["username"];

            var player = _db.Player
                            .Include(p => p.Games)
                            .SingleOrDefault(p => p.Username == username);

            if (player == null)
            {
                player = new Player()
                {
                    Username = username,
                    Czar = 0,
                    Joined = DateTimeOffset.UtcNow,
                    Connections = new List<Connection>()
                };
                _db.Player.Add(player);
            }
            else
            {
                player = new Player()
                {
                    Username = username,
                    Connections = new List<Connection>()
                };

                if(player.Games != null)
                {
                    foreach (var item in player.Games)
                    {
                        Groups.Add(Context.ConnectionId, item.GameName);
                    }
                }
            }

            player.Connections.Add(new Connection()
            {
                ConnectionID = Context.ConnectionId,
                UserAgent = Context.Headers["User-Agent"],
                Connected = true
            });
            
            _db.SaveChanges();

            return base.OnConnected();
        }

        public async Task JoinRoom(string username, int gameID)
        {
            var game = _db.Game.Find(gameID);

            if(game != null)
            {
                var player = _db.Player.Where(p => p.Username == username).SingleOrDefault();
                _db.Player.Attach(player);

                game.Players.Add(player);
                _db.SaveChanges();

                await Groups.Add(Context.ConnectionId, gameID.ToString());
            }

            await Player(username, gameID);
            Clients.Group(gameID.ToString()).addChatMessage(username + " joined.");
        }

        public async Task Player(string username, int gameID)
        {
            await Clients.Group(gameID.ToString()).addPlayer(username);
        }

        public async Task LeaveGame(string username, int gameID)
        {
            await Clients.Group(gameID.ToString()).addChatMessage(username + " left the game.");
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var username = Context.QueryString["username"];

            Player player = _db.Player.Where(p => p.Username == username).SingleOrDefault();

            if(player != null)
            {
                Connection connection = _db.Connection.Find(Context.ConnectionId);

                _db.Player.Remove(player);

                if(connection != null)
                {
                    _db.Connection.Remove(connection);                    
                }

                _db.SaveChanges();
            }

            LeaveGame(username, 1);

            return base.OnDisconnected(stopCalled);
        }
        
    }
}