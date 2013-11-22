using ORMComparison.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace ORMComparison
{
    class Program
    {
        static void Main(string[] args)
        {
            var athlete = new Athlete
            {
                FirstName = "Cole",
                LastName = "Easterday",
                Position = "Developer",
            };

            var db = new EFAccessor();
            var sw = new Stopwatch();
            sw.Start();
            db.SaveAthlete(athlete);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            for (int x = 0; x < 10; x++)
            {
                athlete.Id = 0;
                sw.Reset();
                sw.Start();
                db.SaveAthlete(athlete);
                sw.Stop();
                Console.WriteLine(sw.ElapsedMilliseconds);
            }
            Console.ReadKey();

        }
    }
}
