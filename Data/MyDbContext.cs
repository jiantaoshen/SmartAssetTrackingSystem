using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;


namespace SmartAssetTrackingSystem.Data
{
    internal class MyDbContext : DbContext
    {
        //Make sure to change the connectionstring to your own database or create a new database with the name "Db_Assets" in your localdb.
        string connectionString = "Server=(localdb)\\mssqllocaldb;Database=Db_Assets;Trusted_Connection=True;";
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // We tell the app to use the connectionstring.
            optionsBuilder.UseSqlServer(connectionString);
        }

        //public DbSet<Rocket> Rockets { get; set; }

        protected override void OnModelCreating(ModelBuilder ModelBuilder)
        {

        }
    }
}
