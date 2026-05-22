using SmartAssetTrackingSystem.Data;
using SmartAssetTrackingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace SmartAssetTrackingSystem.Repositories
{

    public class AssetRepository : IAssetRepository
    {
        private readonly MyDbContext _context = new();

        public async Task AddAsset(Asset asset)
        {
            _context.Assets.Add(asset);
            await _context.SaveChangesAsync();
        }


        // The method uses the Skip and Take LINQ methods to implement pagination. It skips a certain number of records based on the current page number and page size, and then takes the specified number of records for the current page.
        public async Task<List<Asset>> GetAssets(int PageNumber, int PageSize)
        {
            return await _context.Assets.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToListAsync();

        }

        public async Task<List<Asset>> GetAssetsByOffice(int officeId)
        {
            return await _context.Assets
                .AsNoTracking()
                .Include(a => a.Office)
                .Where(a => a.OfficeId == officeId)
                .OrderByDescending(a => a.PurchaseDate)
                .ToListAsync();
        }

        public async Task UpdateAsset(Asset asset)
        {
            _context.Assets.Update(asset);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsset(int id)
        {
            var asset = await _context.Assets.FindAsync(id);

            if (asset != null)
            {
                _context.Assets.Remove(asset);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Asset?> GetAssetById(int id)
        {
            return await _context.Assets
                .Include(a => a.Office)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<ComputerAsset>> GetComputerAssetsAsync()
        {
            return await _context.Assets
                .AsNoTracking()
                .Include(a => a.Office)
                .OfType<ComputerAsset>()
                .OrderByDescending(a => a.PurchaseDate)
                .ToListAsync();
        }

        public async Task<List<MobileAsset>> GetMobileAssetsAsync()
        {
            return await _context.Assets
                .AsNoTracking()
                .Include(a => a.Office)
                .OfType<MobileAsset>()
                .OrderByDescending(a => a.PurchaseDate)
                .ToListAsync();
        }
    }
}
