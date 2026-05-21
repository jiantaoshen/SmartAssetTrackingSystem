using SmartAssetTrackingSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAssetTrackingSystem.Repositories
{
    public interface IReportRepository
    {
        Task<decimal> GetTotalAssetValuePerOffice(int officeId);
        Task<int> GetAssetCountPerOffice(int officeId);
        Task<List<Asset>> GetAssetsCloseToExpiration(int months);

        Task<List<Asset>> GetMostExpensiveAssets(int top);
    }
}
