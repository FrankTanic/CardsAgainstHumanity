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

        public async Task PlayCard(int cardID, int gameID)
        {
            var game = _db.Game.Where(g => g.ID == 1).Single();
            var card = _db.Card.Where(c => c.ID == cardID).Single();

            game.Stash.Add(new GameCardStash()
                {   
                    ConnectionID = Context.ConnectionId,
                    Card = card
                });

            _db.SaveChanges();

            await Clients.Group(gameID.ToString()).playWhiteCard(cardID);
        }

        public async Task RemoveCardFromDeck(int cardID, int gameID)
        {
            await Clients.Group(gameID.ToString()).removeCard(cardID);
        }

        // The Next round method retrieve a new black card and empty the stash of the game
        public async Task NextRound(int cardID, int gameID)
        {
            var card = _db.Card.Where(c => c.ID != cardID && c.Black == 1)
                                .OrderBy(c => Guid.NewGuid())
                                .Take(1)
                                .First();

            var game = _db.Game.Where(g => g.ID == gameID)
                                .Single();

            var gameStash = game.Stash.ToList();

            if (gameStash != null)
            {
                foreach (var stash in gameStash)
                {
                    _db.GameCardStash.Remove(stash);
                }

                _db.SaveChanges();
            }

            await Clients.Group(gameID.ToString()).nextBlackCard(card.Description, card.ID);
        }

        public async Task LeaveGame(string username, int gameID)
        {
            await Clients.Group(gameID.ToString()).removePlayer(username);
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