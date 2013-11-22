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
}