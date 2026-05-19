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
            _repo.Add(asset);
        }

        public List<Asset> GetAllAssets()
        {
            return _repo.GetAll();
        }

        public List<Asset> GetAssetsSorted()
        {
            return _repo.GetAll()
                .OrderBy(a => a.OfficeLocation)
                .ThenBy(a => a.AssetType)
                .ThenBy(a => a.PurchaseDate)
                .ToList();
        }
    }
}