using ORMComparison.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ORMComparison.EntityFramework;
using DapperExtensions.Mapper;
namespace ORMComparison
{
    class Program
    {
        static void Main(string[] args)
        {
            var seeder = new SeedDB();
            seeder.Seed();
            DapperExtensions.DapperExtensions.DefaultMapper = typeof(PluralizedAutoClassMapper<>);

            LoadOneAthlete();
            LoadAthletesForPosition();
            LoadTeamWithAthletes();
            InsertAthletes();
            Console.ReadKey();
        }

        private static void LoadOneAthlete()
        {
            var sw = new Stopwatch();
            var random = new Random();
            var id = random.Next(3000);
            var ef = new EFAccessor();
            var dapper = new DapperAccessor();

            sw.Start();
            ef.FindAthlete(id);
            sw.Stop();
            Console.WriteLine("EF find by Id first: {0}", sw.ElapsedMilliseconds);

            id = random.Next(3000);
            sw.Reset();

            sw.Start();
            ef.FindAthlete(id);
            sw.Stop();
            Console.WriteLine("EF find by Id second: {0}", sw.ElapsedMilliseconds);

            id = random.Next(3000);
            sw.Reset();

            sw.Start();
            dapper.FindAthlete(id);
            sw.Stop();
            Console.WriteLine("Dapper find by Id first: {0}", sw.ElapsedMilliseconds);

            id = random.Next(3000);
            sw.Reset();

            sw.Start();
            dapper.FindAthlete(id);
            sw.Stop();
            Console.WriteLine("Dapper find by Id second: {0}", sw.ElapsedMilliseconds);


        }

        private static void LoadAthletesForPosition()
        {
            var sw = new Stopwatch();
            var random = new Random();
            var positions = new string[] { "Punter", "Pitcher", "Keeper", "First Base" };
            var ef = new EFAccessor();
            var dapper = new DapperAccessor();

            sw.Start();
            ef.FindByPosition(positions[0]);
            sw.Stop();
            Console.WriteLine("EF find by Position first: {0}", sw.ElapsedMilliseconds);

            sw.Restart();
            ef.FindByPosition(positions[1]);
            sw.Stop();
            Console.WriteLine("EF find by Position second: {0}", sw.ElapsedMilliseconds);

            sw.Restart();
            var athletes = dapper.FindByPosition(positions[2]);
            sw.Stop();
            Console.WriteLine("Dapper find by Position second: {0}", sw.ElapsedMilliseconds);

            sw.Restart();
            dapper.FindByPosition(positions[3]);
            sw.Stop();
            Console.WriteLine("Dapper find by Position second: {0}", sw.ElapsedMilliseconds);

        }

        private static void LoadTeamWithAthletes()
        {
            var sw = new Stopwatch();
            var ef = new EFAccessor();
            var dapper = new DapperAccessor();

            long[] teamIds;
            using (var db = new Context())
            {
                teamIds = db.Set<Team>().Select(team => team.Id).ToArray();
            }

            sw.Start();
            ef.FindTeamWithAthletes(teamIds[0]);
            sw.Stop();
            Console.WriteLine("EF find Team with Athletes first: {0}", sw.ElapsedMilliseconds);

            sw.Restart();
            ef.FindTeamWithAthletes(teamIds[1]);
            sw.Stop();
            Console.WriteLine("EF find Team with Athletes second: {0}", sw.ElapsedMilliseconds);

            sw.Restart();
            dapper.FindTeamWithAthletes(teamIds[2]);
            sw.Stop();
            Console.WriteLine("Dapper find Team with Athletes first: {0}", sw.ElapsedMilliseconds);

            sw.Restart();
            dapper.FindTeamWithAthletes(teamIds[0]);
            sw.Stop();
            Console.WriteLine("Dapper find Team with Athletes second: {0}", sw.ElapsedMilliseconds);

        }

        private static void InsertAthletes()
        {
            var sw = new Stopwatch();
            var ef = new EFAccessor();
            var dapper = new DapperAccessor();


            var athlete = new Athlete
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                Position = Guid.NewGuid().ToString(),
            };

            sw.Start();
            ef.SaveAthlete(athlete);
            sw.Stop();
            Console.WriteLine("EF save Athlete first: {0}", sw.ElapsedMilliseconds);

            athlete = new Athlete
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                Position = Guid.NewGuid().ToString(),
            };

            sw.Restart();
            ef.SaveAthlete(athlete);
            sw.Stop();
            Console.WriteLine("EF save Athlete second: {0}", sw.ElapsedMilliseconds);

            athlete = new Athlete
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                Position = Guid.NewGuid().ToString(),
            };
            sw.Restart();
            dapper.SaveAthlete(athlete);
            sw.Stop();
            Console.WriteLine("Dapper save Athlete first: {0}", sw.ElapsedMilliseconds);

            athlete = new Athlete
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                Position = Guid.NewGuid().ToString(),
            };
            sw.Restart();
            dapper.SaveAthlete(athlete);
            sw.Stop();
            Console.WriteLine("Dapper save Athlete second: {0}", sw.ElapsedMilliseconds);
        }
    }
}
