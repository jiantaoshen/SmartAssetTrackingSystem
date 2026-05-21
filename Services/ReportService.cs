using SmartAssetTrackingSystem.Repositories;
using SmartAssetTrackingSystem.Models;

namespace SmartAssetTrackingSystem.Services
{
    public class ReportService
    {
        private readonly IReportRepository _repo;

        public ReportService(IReportRepository repo)
        {
            _repo = repo;
        }

        public async Task<decimal> GetTotalAssetValuePerOffice(int officeId)
        {
            return await _repo.GetTotalAssetValuePerOffice(officeId);
        }

        public async Task<int> GetAssetCountPerOffice(int officeId)
        {
            return await _repo.GetAssetCountPerOffice(officeId);
        }

        public async Task<List<Asset>> GetAssetsCloseToExpiration(int months)
        {
            return await _repo.GetAssetsCloseToExpiration(months);
        }

        public async Task<List<Asset>> GetMostExpensiveAssets(int top)
        {
            return await _repo.GetMostExpensiveAssets(top);
        }

    }
}
