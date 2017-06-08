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
        private static Dictionary<OrmType, List<long>> _results;
        private static Stopwatch _sw = new Stopwatch();

        static void Main(string[] args)
        {
            var seeder = new SeedDB();
            seeder.Seed();
            DapperExtensions.DapperExtensions.DefaultMapper = typeof(PluralizedAutoClassMapper<>);
            _results = new Dictionary<OrmType, List<long>>();
            foreach (var i in Enumerable.Range(1, 5))
            {
                LoadOneAthlete();
                LoadAthletesForPosition();
                LoadTeamWithAthletes();
                InsertAthletes();
            }
            Console.WriteLine("---RESULTS---");
            foreach(var type in Enum.GetValues(typeof(OrmType)))
            {
                Console.WriteLine($"{type}");
                Console.WriteLine($"average time: {_results[(OrmType)type].Average()}ms");
                Console.WriteLine($"slowest time: {_results[(OrmType)type].Max()}ms");
                Console.WriteLine($"fastest time: {_results[(OrmType)type].Min()}ms");
            }
            Console.ReadKey();
        }

        private enum OrmType
        {
            EntityFramework,
            Dapper,
        }        

        private static long Time<T>(Func<long, T> action, long id, OrmType type)
        {
            _sw.Reset();
            _sw.Start();
            T ret = action(id);
            _sw.Stop();

            var time = _sw.ElapsedMilliseconds;
            if (!_results.ContainsKey(type))
            {
                _results[type] = new List<long>();
            }
            _results[type].Add(time);
            return time;
        }

        private static long Time<T>(Func<string, T> action, string id, OrmType type)
        {
            _sw.Reset();
            _sw.Start();
            T ret = action(id);
            _sw.Stop();

            var time = _sw.ElapsedMilliseconds;
            if (!_results.ContainsKey(type))
            {
                _results[type] = new List<long>();
            }
            _results[type].Add(time);
            return time;
        }

        private static long Time(Func<Athlete, Athlete> action, Athlete id, OrmType type)
        {
            _sw.Reset();
            _sw.Start();
            var ret = action(id);
            _sw.Stop();

            var time = _sw.ElapsedMilliseconds;
            if (!_results.ContainsKey(type))
            {
                _results[type] = new List<long>();
            }
            _results[type].Add(time);
            return time;
        }

        private static void LoadOneAthlete()
        {
            var random = new Random();
            var id = random.Next(3000);
            var ef = new EFAccessor();
            var dapper = new DapperAccessor();

            Console.WriteLine("EF find by Id first: {0}", Time(ef.FindAthlete, id, OrmType.EntityFramework));

            id = random.Next(3000);
            Console.WriteLine("EF find by Id second: {0}", Time(ef.FindAthlete, id, OrmType.EntityFramework));

            id = random.Next(3000);
            Console.WriteLine("Dapper find by Id first: {0}", Time(dapper.FindAthlete, id, OrmType.Dapper));

            id = random.Next(3000);
            Console.WriteLine("Dapper find by Id second: {0}", Time(dapper.FindAthlete, id, OrmType.Dapper));
        }

        private static void LoadAthletesForPosition()
        {
            var random = new Random();
            var positions = new string[] { "Punter", "Pitcher", "Keeper", "First Base" };
            var ef = new EFAccessor();
            var dapper = new DapperAccessor();

            Console.WriteLine("EF find by Position first: {0}", Time(ef.FindByPosition, positions[0], OrmType.EntityFramework));
            Console.WriteLine("EF find by Position second: {0}", Time(ef.FindByPosition, positions[1], OrmType.EntityFramework));

            Console.WriteLine("Dapper find by Position second: {0}", Time(dapper.FindByPosition, positions[2], OrmType.Dapper));
            Console.WriteLine("Dapper find by Position second: {0}", Time(dapper.FindByPosition, positions[3], OrmType.Dapper));
        }

        private static void LoadTeamWithAthletes()
        {
            var ef = new EFAccessor();
            var dapper = new DapperAccessor();

            long[] teamIds;
            using (var db = new Context())
            {
                teamIds = db.Set<Team>().Select(team => team.Id).ToArray();
            }

            Console.WriteLine("EF find Team with Athletes first: {0}", Time(ef.FindTeamWithAthletes, teamIds[0], OrmType.EntityFramework));
            Console.WriteLine("EF find Team with Athletes second: {0}", Time(ef.FindTeamWithAthletes, teamIds[1], OrmType.EntityFramework));

            Console.WriteLine("Dapper find Team with Athletes first: {0}", Time(dapper.FindTeamWithAthletes, teamIds[2], OrmType.Dapper));
            Console.WriteLine("Dapper find Team with Athletes second: {0}", Time(dapper.FindTeamWithAthletes, teamIds[0], OrmType.Dapper));

        }

        private static void InsertAthletes()
        {
            var ef = new EFAccessor();
            var dapper = new DapperAccessor();

            Console.WriteLine("EF save Athlete first: {0}", Time(ef.SaveAthlete, NewAthlete(), OrmType.EntityFramework));
            Console.WriteLine("EF save Athlete second: {0}", Time(ef.SaveAthlete, NewAthlete(), OrmType.EntityFramework));

            Console.WriteLine("Dapper save Athlete first: {0}", Time(dapper.SaveAthlete, NewAthlete(), OrmType.Dapper));
            Console.WriteLine("Dapper save Athlete second: {0}", Time(dapper.SaveAthlete, NewAthlete(), OrmType.Dapper));
        }

        private static Athlete NewAthlete()
        {
            return new Athlete
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                Position = Guid.NewGuid().ToString(),
            };
        }
    }
}
