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
        public ActionResult Create(int? id)
        {
            if(id == null)
            {
                return View();
            }

            return RedirectToAction("Match", "Game", new { id = id });
        }


        public async Task<ActionResult> Match(int? id)
        {
            var response = await _httpClient.GetAsync("api/Game/GetGame/" + id + "");

            if (response.IsSuccessStatusCode)
            {
                Game game = await response.Content.ReadAsAsync<Game>();

                return View(game);
            }

            return View();
        }
    }
}