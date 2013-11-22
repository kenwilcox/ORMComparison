using ORMComparison.DataContracts;
using ORMComparison.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMComparison
{
    public class EFAccessor
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
    }
}
