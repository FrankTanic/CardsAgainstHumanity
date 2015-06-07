using System;
using System.Collections.Generic;

namespace CardsAgainstHumanity.WebApi.Models
{
    public class Card
    {
        public int ID { get; set; }
        public int Black { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Game> Games { get; set; }
    }
}