using System;
using System.Data.Entity;
using CardsAgainstHumanity.WebApi.Models;

namespace CardsAgainstHumanity.WebApi.Models
{
    public class CardsAgainstHumanityDbContext : DbContext
    {
        public CardsAgainstHumanityDbContext()
            : base("CardsAgainstHumanityContext")
        {
        }

        public DbSet<Card> Card { get; set; }
        public DbSet<UsedCard> UsedCard { get; set; }
        public DbSet<Game> Game { get; set; }
    }
}