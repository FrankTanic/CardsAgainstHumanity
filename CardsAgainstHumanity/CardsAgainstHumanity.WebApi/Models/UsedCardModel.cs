using System;
using CardsAgainstHumanity.WebApi.Models;

namespace CardsAgainstHumanity.WebApi.Models
{
    public class UsedCard
    {
        public int ID { get; set; }
        public bool IsUsed { get; set; }
        public virtual Card Card { get; set; }
        public virtual Game Game { get; set; }
    }
}