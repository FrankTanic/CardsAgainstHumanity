using System;
using System.Collections.Generic;

namespace CardsAgainstHumanity.Front.Models
{
    public class GameCardStash
    {
        public int ID { get; set; }
        public string ConnectionID { get; set; }
        public Card Card { get; set; }
        public Game Game { get; set; }
    }
}