using Microsoft.EntityFrameworkCore;
using SmartAssetTrackingSystem.Data;
using SmartAssetTrackingSystem.Models;

namespace SmartAssetTrackingSystem.Repositories
{
    public class OfficeRepository:IOfficeRepository
    {
        private readonly MyDbContext _context = new();

        public async Task<Office?> GetByCountryAsync(string country)
        {
            return await _context.Offices.FirstOrDefaultAsync(o => o.Country == country);
        }

        public async Task<List<string>> GetCountriesAsync()
        {
            return await _context.Offices
                .Select(o => o.Country)
                .Distinct()
                .ToListAsync();
        }


    }
}
