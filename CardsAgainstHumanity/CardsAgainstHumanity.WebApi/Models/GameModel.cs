using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CardsAgainstHumanity.WebApi.Models
{
    public class Game
    {
        public int ID { get; set; }
        public string GameName { get; set; }
        public int GameCode { get; set; }
        public DateTimeOffset Created { get; set; }
        public virtual ICollection<Player> Players { get; set; }
        public virtual ICollection<Card> Cards { get; set; }
        public virtual ICollection<UsedCard> UsedCards { get; set; }
        public virtual ICollection<GameCardStash> Stash { get; set; }
    }
}