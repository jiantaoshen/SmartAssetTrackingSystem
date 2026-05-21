using SmartAssetTrackingSystem.Models;
using SmartAssetTrackingSystem.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace SmartAssetTrackingSystem.Services
{
    public class AssetService
    {
        private readonly IAssetRepository _repo;

        public AssetService(IAssetRepository repo)
        {
            _repo = repo;
        }

        public void AddAsset(Asset asset)
        {
            _repo.AddAsset(asset);
        }

        public async Task<List<Asset>> GetAssets()
        {
            // Pragination is hardcoded for now, but we can easily modify this method to accept page number and page size as parameters in the future.
            return await _repo.GetAssets(1, 20);
        }

        public async Task<Asset?> GetAssetById(int id)
        {
            var assets = await _repo.GetAssets(1, 20);
            return assets.FirstOrDefault(a => a.Id == id);
        }

        public async Task UpdateAsset(Asset asset)
        {
            await _repo.UpdateAsset(asset);
        }

        public async Task RemoveAsset(int id)
        {
            await _repo.RemoveAsset(id);
        }


    }
}