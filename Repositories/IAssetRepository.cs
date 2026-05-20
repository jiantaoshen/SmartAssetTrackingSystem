using SmartAssetTrackingSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAssetTrackingSystem.Repositories
{
    public interface IAssetRepository
    {
        void AddAsset(Asset asset);

        Task<List<Asset>> GetAssets(int PageNumber, int PageSize);
    }
}
