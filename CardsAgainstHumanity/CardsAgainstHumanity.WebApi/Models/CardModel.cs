using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardsAgainstHumanity.WebApi.Migrations
{
    public class Card
    {
        public int ID { get; set; }
        public int Black { get; set; }
        public string Description { get; set; }
    }
}