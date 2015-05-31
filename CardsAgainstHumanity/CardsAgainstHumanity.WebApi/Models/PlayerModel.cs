using System;
using System.Collections.Generic;

namespace CardsAgainstHumanity.WebApi.Models
{
    public class Player
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public ICollection<Connection> Connections { get; set; }
        public virtual ICollection<Game> Games { get; set; }
    }
}