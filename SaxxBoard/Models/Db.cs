using System.Data.Entity;
using SaxxBoard.Migrations;

namespace SaxxBoard.Models
{
    public class Db : DbContext
    {
        public Db()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Db, Configuration>());
        }

        public DbSet<DataPoint> DataPoints { get; set; }
    }
}