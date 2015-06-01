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
        public async Task<IHttpActionResult> GetGame(int id)
        {
            Game game = _db.Game.Where(g => g.ID == id).SingleOrDefault();

            return Ok(game);
        }
    }
}
