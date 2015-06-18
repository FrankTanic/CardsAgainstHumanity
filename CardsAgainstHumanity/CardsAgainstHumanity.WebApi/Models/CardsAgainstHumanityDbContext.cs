using System;
using System.Data.Entity;
using CardsAgainstHumanity.WebApi.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

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
        public DbSet<GameCardStash> GameCardStash { get; set; }
        public DbSet<Game> Game { get; set; }
        public DbSet<Player> Player { get; set; }
        public DbSet<Connection> Connection { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Game>()
                .HasMany(g => g.Cards).WithMany(c => c.Games)
                .Map(t => t.MapLeftKey("GameID")
                .MapRightKey("CardID")
                .ToTable("GameCard"));

            base.OnModelCreating(modelBuilder);
        }
    }
}