using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ORMComparison.DataContracts;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using DapperExtensions;

namespace ORMComparison
{
    class DapperAccessor : IAccessor
    {
        private string _ConnectionString = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;

        public Athlete FindAthlete(long athleteId)
        {
            using (var conn = new SqlConnection(_ConnectionString))
            {
                return conn.Query<Athlete>(
                    @"select *
                     from Athletes
                     where Id = @Id",
                     new { Id = athleteId }).FirstOrDefault();
            }
        }

        public Athlete SaveAthlete(Athlete athlete)
        {
            using (var conn = new SqlConnection(_ConnectionString))
            {
                conn.Open();

                if (athlete.Id == 0)
                {
                    athlete.Id = conn.Insert(athlete);
                }
                else
                {
                    conn.Update(athlete);
                }

                return athlete;
            }
        }

        public Athlete[] FindByPosition(string position)
        {
            using (var conn = new SqlConnection(_ConnectionString))
            {
                return conn.Query<Athlete>(
                        @"select *
                          from Athletes
                          where Position = @Position",
                          new { Position = position }).ToArray();
            }
        }

        public TeamAthletes FindTeamWithAthletes(long teamId)
        {
            using (var conn = new SqlConnection(_ConnectionString))
            {
                var team = conn.Query<Team>(
                    @"select *
                      from Teams
                      where Id = @Id",
                      new { Id = teamId }
                    ).FirstOrDefault();
                var athletes = conn.Query<Athlete>(
                    @"select Athletes.* 
                      from Athletes
                      join Athlete_Team on Athletes.Id = Athlete_Team.AthleteId
                      where Athlete_Team.TeamId = @TeamId",
                      new { TeamId = teamId }).ToArray();

                return new TeamAthletes
                {
                    Team = team,
                    Athletes = athletes,
                };
            }
        }
    }
}
