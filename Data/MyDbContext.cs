using Microsoft.EntityFrameworkCore;
using SmartAssetTrackingSystem.Models;

namespace SmartAssetTrackingSystem.Data
{
    public class MyDbContext : DbContext
    {
        //Make sure to change the connectionstring to your own database or create a new database with the name "Db_Assets" in your localdb.
        string connectionString = "Server=(localdb)\\mssqllocaldb;Database=Db_Assets;Trusted_Connection=True;";
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // We tell the app to use the connectionstring.
            optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<ComputerAsset> ComputerAssets { get; set; }
        public DbSet<MobileAsset> MobileAssets { get; set; }
        public DbSet<Office> Offices { get; set; }

        protected override void OnModelCreating(ModelBuilder ModelBuilder)
        {


        }
    }
}
