using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CardsAgainstHumanity.Front.Models;

namespace CardsAgainstHumanity.Front.Controllers
{
    public class GameController : Controller
    {
        private HttpClient _httpClient;

        public GameController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:6408/"),
            };
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Player model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var userCookie = new HttpCookie("user", model.UserName);
            HttpContext.Response.SetCookie(userCookie);

            return RedirectToAction("Match", "Game", new { id = 1 });
        }

        public async Task<ActionResult> Match(int? id)
        {
            if (id != null)
            {

                HttpCookie userCookie = HttpContext.Request.Cookies.Get("user");
                var username = userCookie.Value;

                var response = await _httpClient.GetAsync("api/Game/GetGame/" + id + "?username=" + username);

                if (response.IsSuccessStatusCode)
                {
                    Game game = await response.Content.ReadAsAsync<Game>();

                    var blackCard = game.Cards.Where(c => c.Black == 1).Take(1).First();

                    ViewBag.BlackCard = blackCard.Description;
                    ViewBag.BlackCardID = blackCard.ID;

                    var whiteCards = game.UsedCards.Where(u => u.Username == username && u.Game.ID == id).Take(10).ToList();
                    ViewBag.WhiteCards = whiteCards;

                    var stash = game.Stash.Where(s => s.Game.ID == game.ID);
                    ViewBag.Stash = stash;

                    return View(game);
                }

                return View();
            }
            else
            {
                return HttpNotFound();
            }
        }
    }
}