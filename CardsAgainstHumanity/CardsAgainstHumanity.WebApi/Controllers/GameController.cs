using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CardsAgainstHumanity.WebApi.Models;

namespace CardsAgainstHumanity.WebApi.Controllers
{
    [RoutePrefix("api/Game")]
    public class GameController : ApiController
    {
        private CardsAgainstHumanityDbContext _db = new CardsAgainstHumanityDbContext();

        [Route("GetGame/{id}")]
        public async Task<IHttpActionResult> GetGame(int? id, string username)
        {
            if (id != null)
            {
                Game game = _db.Game.Where(g => g.ID == id).Single();

                if (game.UsedCards.Count == 0)
                {
                    var cards = game.Cards.Where(c => c.Black != 1)
                                .OrderBy(c => Guid.NewGuid())
                                .Take(10)
                                .ToList();

                    foreach (var card in cards)
                    {
                        game.UsedCards.Add(new UsedCard()
                        {
                            Card = card,
                            Game = game,
                            Username = username
                        });
                    }

                    _db.SaveChanges();
                }
                else
                {
                    var user = game.UsedCards.Where(u => u.Username == username).FirstOrDefault();

                    if (user == null)
                    {
                        var usedCards = game.UsedCards.ToList();
                        var cards = game.Cards.ToList();

                        var newCards = cards.Where(c => !usedCards.Any(uc => uc.Card.ID == c.ID && uc.Card.Black == 1) && c.Black == 0)
                                            .OrderBy(c => Guid.NewGuid())
                                            .Take(10)
                                            .ToList();

                        foreach (var card in newCards)
                        {
                            game.UsedCards.Add(new UsedCard()
                                {
                                    Card = card,
                                    Game = game,
                                    Username = username
                                });
                        }
                        _db.SaveChanges();
                    }
                }

                return Ok(game);
            }

            return NotFound();
        }
    }
}
