using SmartAssetTrackingSystem.Data;
using SmartAssetTrackingSystem.Models;

namespace SmartAssetTrackingSystem.Repositories
{

    public class AssetRepository : IAssetRepository
    {
        private readonly MyDbContext _context = new();

        public void AddAsset(Asset asset)
        {
            _context.Assets.Add(asset);
            _context.SaveChanges();
        }
    }
}
