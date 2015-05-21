namespace CardsAgainstHumanity.WebApi.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CardsAgainstHumanity.WebApi.Models.CardsAgainstHumanityDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(CardsAgainstHumanity.WebApi.Models.CardsAgainstHumanityDbContext context)
        {
            context.Card.AddOrUpdate(
                new Card { Black = 0, Description = "Being on fire." },
                new Card { Black = 0, Description = "Racism." },
                new Card { Black = 0, Description = "Old-people smell." },
                new Card { Black = 0, Description = "A micropenis." },
                new Card { Black = 0, Description = "Women in yoghurt commercials." },
                new Card { Black = 0, Description = "Classist undertones." },
                new Card { Black = 0, Description = "Not giving a shit about the Third World." },
                new Card { Black = 0, Description = "Inserting a mason jar into my anus." },
                new Card { Black = 0, Description = "Court-ordered rehab." },
                new Card { Black = 0, Description = "A windmill full of corpses." },
                new Card { Black = 0, Description = "The Gays" },
                new Card { Black = 0, Description = "An oversized lollipop." },
                new Card { Black = 0, Description = "African children." },
                new Card { Black = 0, Description = "An asymmetric boob job." },
                new Card { Black = 0, Description = "Bingeing and purging." },
                new Card { Black = 0, Description = "The hardworking Mexican." },
                new Card { Black = 0, Description = "And oedipus complex." },
                new Card { Black = 0, Description = "A tiny horse." },
                new Card { Black = 0, Description = "Boogers." },
                new Card { Black = 0, Description = "Penis envy" },
                new Card { Black = 1, Description = "How did i lose my virginity?" },
                new Card { Black = 1, Description = "Why can't i sleep at night?" },
                new Card { Black = 1, Description = "What's that smell?" },
                new Card { Black = 1, Description = "I got 99 problems but __________ ain't one." },
                new Card { Black = 1, Description = "Maybe she's born with it. Maybe it's __________." },
                new Card { Black = 1, Description = "What's the next happy meal toy?" },
                new Card { Black = 1, Description = "Here is the church, here is the steeple, open the doors and there is __________." },
                new Card { Black = 1, Description = "It's a pity that kids these days are all getting involved with _________." });
        }
    }
}
