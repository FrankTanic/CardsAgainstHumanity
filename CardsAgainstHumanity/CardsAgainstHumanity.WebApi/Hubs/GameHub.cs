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
            var userCookie = HttpContext.Current.Request.Cookies["user"];
            var username = userCookie.Value;

            var player = _db.Player
                            .Include(p => p.Games)
                            .SingleOrDefault(p => p.Username == username);

            var playerCount = _db.Player.Count();

            if (player == null)
            {
                if (playerCount == 0)
                {
                    player = new Player()
                    {
                        Username = username,
                        Czar = 1,
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
                        Czar = 0,
                        Joined = DateTimeOffset.UtcNow,
                        Connections = new List<Connection>()
                    };
                    _db.Player.Add(player);
                }
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

        public async Task JoinRoom(int gameID)
        {
            var game = _db.Game.Find(gameID);

            var userCookie = HttpContext.Current.Request.Cookies["user"];
            var username = userCookie.Value;

            if(game != null)
            {
                var player = _db.Player.Where(p => p.Username == username).SingleOrDefault();
                _db.Player.Attach(player);

                game.Players.Add(player);
                _db.SaveChanges();

                await Groups.Add(Context.ConnectionId, gameID.ToString());

                await Player(player.ID, player.Czar, username, gameID);
            }

            Clients.Group(gameID.ToString()).addChatMessage(username + " joined.");
        }

        public async Task Player(int playerID, int playerCzar, string username, int gameID)
        {
            await Clients.Group(gameID.ToString()).addPlayer(playerID, playerCzar, username);
        }

        public async Task PlayCard(int cardID, int gameID)
        {
            var userCookie = HttpContext.Current.Request.Cookies["user"];
            var username = userCookie.Value;

            var game = _db.Game.Where(g => g.ID == 1).Single();
            var card = _db.Card.Where(c => c.ID == cardID).Single();

            var playerCount = game.Players.Count();
            var usedCardCount = game.UsedCards.Count();

            var cardDescription = card.Description;

            game.Stash.Add(new GameCardStash()
                {   
                    ConnectionID = Context.ConnectionId,
                    Card = card
                });

            _db.SaveChanges();

                await Clients.Group(gameID.ToString()).playWhiteCard(cardID, cardDescription);

                checkIfShowWhiteCards(gameID, username);
        }

        public async Task checkIfShowWhiteCards(int gameID, string username)
        {
            var game = _db.Game.Where(g => g.ID == gameID).Single();

            var playerCount = game.Players.Count();
            var usedCardCount = game.Stash.Where(s => s.Game.ID == gameID).Count();

            if(playerCount == usedCardCount)
            {
                await Clients.Group(gameID.ToString()).showWhiteCards();
            }
        }

        public async Task RemoveCardFromDeck(int cardID, int gameID)
        {
            await Clients.Group(gameID.ToString()).removeCard(cardID);
        }

        // The Next round method retrieve a new black card and empty the stash of the game
        public async Task NextRound(int cardID, int gameID)
        {
            var czar = _db.Player.Where(p => p.Czar == 1).Single();
            czar.Czar = 0;
            _db.SaveChanges();

            var newczar = _db.Player.OrderBy(i => i.ID).FirstOrDefault(i => i.ID > czar.ID);
            if (newczar == null)
            {
                var czarReset = _db.Player.OrderBy(i => i.ID).First();
                czarReset.Czar = 1;
                _db.SaveChanges();
            }
            else
            {
                newczar.Czar = 1;
                _db.SaveChanges();
            }

            var game = _db.Game.Where(g => g.ID == gameID)
                                .Single();

            var usedCards = _db.Card.ToList();
            var unUsedCards = _db.UsedCard.ToList();

            //Get the old black card and mark it as no longer used.//

            //Net _db.UsedCard.where veranderd naar usedCards.where//
            var oldBlackCard = usedCards.Where(c => unUsedCards.Any(uc => uc.Card.ID == c.ID && uc.IsUsed == true && c.Black == 1)).Take(1).First();
            var oldBlackCardID = unUsedCards.Where(c => usedCards.Any(uc => c.Card.ID == uc.ID && c.IsUsed == true && uc.Black == 1)).Take(1).First();

            var RememberCard = oldBlackCardID.Card;

            _db.UsedCard.Remove(oldBlackCardID);

                _db.UsedCard.Add (new UsedCard()
                {
                    Card = RememberCard,
                    Game = game,
                    Username = null,
                    IsUsed = false
                });

            _db.SaveChanges();

            var usedCardsNew = _db.Card.ToList();
            var unUsedCardsNew = _db.UsedCard.ToList();

            var card = usedCardsNew.Where(c => unUsedCardsNew.Any(uc => uc.Card.ID != c.ID && uc.IsUsed != true && c.Black == 1)).OrderBy(c => Guid.NewGuid()).Take(1).First();

            

            //Add the new black card and mark it as being used.//
                    _db.UsedCard.Add(new UsedCard()
                            {
                                 Card = card,
                                 Game = game,
                                 Username = null,
                                 IsUsed = true
                            });

            _db.SaveChanges();
            

            

            var gameStash = game.Stash.ToList();

            if (gameStash != null)
            {
                foreach (var stash in gameStash)
                {
                    _db.GameCardStash.Remove(stash);
                }

                _db.SaveChanges();
            }

            var czarID = _db.Player.Where(b => b.Czar == 1).Single();

            await Clients.Group(gameID.ToString()).nextBlackCard(card.Description, card.ID, czarID.ID); 
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