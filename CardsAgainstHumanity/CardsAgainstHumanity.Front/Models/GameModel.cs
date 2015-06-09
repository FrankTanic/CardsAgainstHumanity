using System;
using System.Collections.Generic;

namespace CardsAgainstHumanity.Front.Models
{
    public class Game
    {
        public int ID { get; set; }
        public string GameName { get; set; }
        public int GameCode { get; set; }
        public DateTimeOffset Created { get; set; }
        public virtual ICollection<Player> Players { get; set; }
        public virtual ICollection<Card> Cards { get; set; }
        public virtual ICollection<GameCardStash> Stash { get; set; }

    }

    public class Player
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public int Czar { get; set; }
    }

    public class Card
    {
        public int ID { get; set; }
        public int Black { get; set; }
        public string Description { get; set; }
    }
}