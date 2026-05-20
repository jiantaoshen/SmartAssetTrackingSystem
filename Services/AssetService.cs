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


    }
}