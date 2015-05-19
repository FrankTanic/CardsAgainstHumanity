using System;
using System.Data.Entity;

namespace CardsAgainstHumanity.WebApi.Models
{
    public class CardsAgainstHumanityDbContext : DbContext
    {
        public CardsAgainstHumanityDbContext()
            : base("CardsAgainstHumanityContext")
        {
        }
    }
}