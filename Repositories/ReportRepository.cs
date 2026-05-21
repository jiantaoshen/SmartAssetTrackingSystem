using Microsoft.EntityFrameworkCore;
using SmartAssetTrackingSystem.Data;
using SmartAssetTrackingSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAssetTrackingSystem.Repositories
{
    public class ReportRepository:IReportRepository
    {
        private readonly MyDbContext _context = new();

        public async Task<decimal> GetTotalAssetValuePerOffice(int officeId)
        {
            return await _context.Assets
                .Where(a => a.OfficeId == officeId)
                .SumAsync(a => a.LocalPrice);
        }

        public async Task<int> GetAssetCountPerOffice(int officeId)
        {
            return await _context.Assets.CountAsync(a => a.OfficeId == officeId);
        }

        public async Task<List<Asset>> GetAssetsCloseToExpiration(int months)
        {
            var limitDate = DateTime.Now.AddMonths(months);

            return await _context.Assets
                .AsNoTracking()
                .Include(a => a.Office)
                .Where(a => a.WarrantyExpirationDate <= limitDate)
                .OrderBy(a => a.WarrantyExpirationDate)
                .ToListAsync();
        }

        public async Task<List<Asset>> GetMostExpensiveAssets(int top)
        {
            return await _context.Assets
                .AsNoTracking()
                .Include(a => a.Office)
                .OrderByDescending(a => a.PurchasePriceUSD)
                .Take(top)
                .ToListAsync();
        }
    }
}
