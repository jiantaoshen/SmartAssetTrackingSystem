using SmartAssetTrackingSystem.Data;
using SmartAssetTrackingSystem.Models;
using Microsoft.EntityFrameworkCore;

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


        // The method uses the Skip and Take LINQ methods to implement pagination. It skips a certain number of records based on the current page number and page size, and then takes the specified number of records for the current page.
        public async Task<List<Asset>> GetAssets(int PageNumber, int PageSize)
        {
            return await _context.Assets.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToListAsync();
        }
    }
}
