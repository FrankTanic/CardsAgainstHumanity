using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CardsAgainstHumanity.Front.Controllers
{
    public class GameController : Controller
    {
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(int? id, string username)
        {
            if(id == null || username == null)
            {
                return View();
            }



            return RedirectToAction("Match");
        }


        public ActionResult Match()
        {
            return View();
        }
    }
}