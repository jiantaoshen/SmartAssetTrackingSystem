using SmartAssetTrackingSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAssetTrackingSystem.Repositories
{
    public interface IAssetRepository
    {
        Task AddAsset(Asset asset);
        Task<List<Asset>> GetAssets(int PageNumber, int PageSize);
        Task UpdateAsset(Asset asset);
        Task RemoveAsset(int id);
        Task<Asset?> GetAssetById(int id);
        Task<List<Asset>> GetAssetsByOffice(int officeId);
        Task<List<ComputerAsset>> GetComputerAssetsAsync();
        Task<List<MobileAsset>> GetMobileAssetsAsync();

    }
}
