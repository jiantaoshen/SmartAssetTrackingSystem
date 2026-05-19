using SmartAssetTrackingSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAssetTrackingSystem.Repositories
{

    public class AssetRepository : IAssetRepository
    {
        private readonly List<Asset> _assets = new();

        public void Add(Asset asset)
        {
            _assets.Add(asset);
        }

        public List<Asset> GetAll()
        {
            return _assets;
        }
    }
}
