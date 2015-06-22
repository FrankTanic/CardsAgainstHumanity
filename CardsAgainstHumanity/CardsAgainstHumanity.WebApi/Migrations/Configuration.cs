namespace CardsAgainstHumanity.WebApi.Models
{
    using System;
    using System.Collections.Generic;
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

            var cards = new List<Card>
            {
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
                new Card { Black = 0, Description = "The Gays." },
                new Card { Black = 0, Description = "An oversized lollipop." },
                new Card { Black = 0, Description = "African children." },
                new Card { Black = 0, Description = "An asymmetric boob job." },
                new Card { Black = 0, Description = "Bingeing and purging." },
                new Card { Black = 0, Description = "The hardworking Mexican." },
                new Card { Black = 0, Description = "And oedipus complex." },
                new Card { Black = 0, Description = "A tiny horse." },
                new Card { Black = 0, Description = "Boogers." },
                new Card { Black = 0, Description = "Penis envy." },
                new Card { Black = 0, Description = "A stray pube." },
                new Card { Black = 0, Description = "Heartwarming orphans." },
                new Card { Black = 0, Description = "My relationship status." },
                new Card { Black = 0, Description = "Peeing a little bit." },
                new Card { Black = 0, Description = "Repression." },
                new Card { Black = 0, Description = "A ball of earwax, semen and toenail clippings." },
                new Card { Black = 0, Description = "The devil himself" },
                new Card { Black = 0, Description = "The World of Warcraft." },
                new Card { Black = 0, Description = "MechaHitler." },
                new Card { Black = 0, Description = "Pictures of boobs." },
                new Card { Black = 0, Description = "Pedophiles." },
                new Card { Black = 0, Description = "The Pope." },
                new Card { Black = 0, Description = "Flying sex snakes." },
                new Card { Black = 0, Description = "Civilian Casualties." },
                new Card { Black = 0, Description = "Sexy pillow fights." },
                new Card { Black = 0, Description = "The female orgasm." },
                new Card { Black = 0, Description = "Bitches." },
                new Card { Black = 0, Description = "Auschwitz." },
                new Card { Black = 0, Description = "Finger painting." },
                new Card { Black = 0, Description = "The Jews." },
                new Card { Black = 0, Description = "The blood of Christ." },
                new Card { Black = 0, Description = "Dead parents." },
                new Card { Black = 0, Description = "Natalie Portman." },
                new Card { Black = 0, Description = "Surprise sex!" },
                new Card { Black = 0, Description = "Amputees" },
                new Card { Black = 1, Description = "How did i lose my virginity?" },
                new Card { Black = 1, Description = "Why can't i sleep at night?" },
                new Card { Black = 1, Description = "What's that smell?" },
                new Card { Black = 1, Description = "I got 99 problems but __________ ain't one." },
                new Card { Black = 1, Description = "Maybe she's born with it. Maybe it's __________." },
                new Card { Black = 1, Description = "What's the next happy meal toy?" },
                new Card { Black = 1, Description = "Here is the church, here is the steeple, open the doors and there is __________." },
                new Card { Black = 1, Description = "It's a pity that kids these days are all getting involved with _________." },
                new Card { Black = 1, Description = "Today on Maury: 'Help my son is _______ !'" },
                new Card { Black = 1, Description = "Alternative medicine is now embracing the curative powers of _________." },
                new Card { Black = 1, Description = "What ended my last relationship?" },
                new Card { Black = 1, Description = "MTV's new reality show features eight washed up celebrities living with __________." },
                new Card { Black = 1, Description = "I drink to forget ________." },
                new Card { Black = 1, Description = "I'm sorry, professor, but i couldn't complete my homework because of ________." },
                new Card { Black = 1, Description = "What is Batman's guilty pleasure." },
                new Card { Black = 1, Description = "What's a girl's best friend?" },
                new Card { Black = 1, Description = "TSA guidlines now prohibit _________ on airplanes." },
                new Card { Black = 1, Description = "__________. That's how i want to die." }
            };

            cards.ForEach(s => context.Card.AddOrUpdate(c => c.ID, s));
            context.SaveChanges();



            var games = new List<Game>
            {
                new Game { ID = 1, GameName = "Hello Game", Created = DateTimeOffset.UtcNow, Cards = new List<Card>()}
            };

            games.ForEach(s => context.Game.AddOrUpdate(p => p.ID, s));
            context.SaveChanges();

            var game = context.Game.FirstOrDefault();
            var crds = game.Cards.FirstOrDefault();

            if(crds == null)
            {
                foreach (var card in cards)
                {
                    game.Cards.Add(card);
                }
            }

            context.SaveChanges();


        }

    }
}
