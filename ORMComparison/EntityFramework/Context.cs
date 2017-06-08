using ORMComparison.DataContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMComparison.EntityFramework
{
    public class Context : DbContext
    {
        public DbSet<Athlete> Athletes { get; set; }
        public DbSet<AthleteTeam> AthleteTeam { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AthleteTeam>().ToTable("Athlete_Team").HasKey(table => new { table.AthleteId, table.TeamId });
            base.OnModelCreating(modelBuilder);
        }

        public Context()
        {
            Database.SetInitializer<Context>(null);
        }
    }

    public class SeedDB
    {
        private List<Team> Teams = new List<Team>();
        
        public void Seed()
        {
            using (var db = new Context())
            {
                if (db.Set<Athlete>().Count() == 0)
                {
                    SeedTeams();
                    SeedAthletes();
                }
            }
        }

        private void SeedTeams()
        {
            var teams = new Team[]
            {
                new Team
                {
                    Name = "Football"
                },
                new Team
                {
                    Name= "Baseball"
                },
                new Team
                {
                    Name = "Soccer"
                }
            };

            using (var db = new Context())
            {
                foreach (var team in teams)
                {
                    Teams.Add(db.Set<Team>().Add(team));
                    db.SaveChanges();
                }
            }
        }

        private void SeedAthletes()
        {
            var footballPositions = new string[] { "Quarter Back", "Wide Receiver", "Safety", "Punter" };
            var baseballPositions = new string[] { "First Base", "Right Field", "Catcher", "Pitcher" };
            var soccerPositons = new string[] { "Forward", "Midfield", "Defense", "Keeper" };

            Athlete athlete;
            using (var db = new Context())
            {
                List<Athlete> footballAthletes = new List<Athlete>();
                for (int x = 0; x < 1000; x++)
                {
                    athlete = new Athlete
                    {
                        FirstName = Guid.NewGuid().ToString(),
                        LastName = Guid.NewGuid().ToString(),
                        Position = footballPositions[x % footballPositions.Length]
                    };
                   footballAthletes.Add(db.Athletes.Add(athlete));
                }
                db.SaveChanges();

                var athleteIds = footballAthletes.Select(ath => ath.Id);
                var teamId = Teams.First(team => team.Name == "Football").Id;
                foreach (var id in athleteIds)
                {
                    db.AthleteTeam.Add(new AthleteTeam
                    {
                        TeamId = teamId,
                        AthleteId = id,
                    });
                }
                db.SaveChanges();

                var baseballAthletes = new List<Athlete>();
                for (int x = 0; x < 1000; x++)
                {
                    athlete = new Athlete
                    {
                        FirstName = Guid.NewGuid().ToString(),
                        LastName = Guid.NewGuid().ToString(),
                        Position = baseballPositions[x % baseballPositions.Length]
                    };
                   baseballAthletes.Add(db.Athletes.Add(athlete));
                }
                db.SaveChanges();

                athleteIds = footballAthletes.Select(ath => ath.Id);
                teamId = Teams.First(team => team.Name == "Baseball").Id;
                foreach (var id in athleteIds)
                {
                    db.AthleteTeam.Add(new AthleteTeam
                    {
                        TeamId = teamId,
                        AthleteId = id,
                    });
                }
                db.SaveChanges();

                var soccerAthletes = new List<Athlete>();
                for (int x = 0; x < 1000; x++)
                {
                    athlete = new Athlete
                    {
                        FirstName = Guid.NewGuid().ToString(),
                        LastName = Guid.NewGuid().ToString(),
                        Position = soccerPositons[x % soccerPositons.Length]
                    };
                    soccerAthletes.Add(db.Athletes.Add(athlete));
                }
                db.SaveChanges();

                athleteIds = footballAthletes.Select(ath => ath.Id);
                teamId = Teams.First(team => team.Name == "Soccer").Id;
                foreach (var id in athleteIds)
                {
                    db.AthleteTeam.Add(new AthleteTeam
                    {
                        TeamId = teamId,
                        AthleteId = id,
                    });
                }
                db.SaveChanges();
            }
        }
    }
}