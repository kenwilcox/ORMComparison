using ORMComparison.DataContracts;
using ORMComparison.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMComparison
{
    public class EFAccessor : IAccessor
    {
        public Athlete SaveAthlete(Athlete athlete)
        {
            using (var db = new Context())
            {
                athlete = db.Set<Athlete>().Add(athlete);
                db.SaveChanges();
            }
            return athlete;
        }

        public Athlete FindAthlete(long athleteId)
        {
            using (var db = new Context())
            {
                return db.Set<Athlete>().Find(athleteId);
            }
        }

        public Athlete[] FindByPosition(string position)
        {
            using (var db = new Context())
            {
                return db.Set<Athlete>().Where(athlete => athlete.Position.Equals(position, StringComparison.OrdinalIgnoreCase)).ToArray();
            }
        }

        public TeamAthletes FindTeamWithAthletes(long teamId)
        {
            using (var db = new Context())
            {
                var team = db.Set<Team>().Find(teamId);
                var teamathletes = (
                    from athletes in db.Set<Athlete>()
                    join at in db.Set<AthleteTeam>() on athletes.Id equals at.AthleteId
                    where at.TeamId == teamId
                    select athletes
                    ).ToArray();
                return new TeamAthletes
                {
                    Team = team,
                    Athletes = teamathletes,
                };
            }
        }
    }
}
