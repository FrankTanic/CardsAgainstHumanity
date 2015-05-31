using System;

namespace CardsAgainstHumanity.Front.Models
{
    public class Game
    {
        public int ID { get; set; }
        public string GameName { get; set; }
        public int GameCode { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}