using Microsoft.EntityFrameworkCore;
using SmartAssetTrackingSystem.Data;
using SmartAssetTrackingSystem.Models;

namespace SmartAssetTrackingSystem.Repositories
{
    public interface IOfficeRepository
    {
        public Task<Office?> GetByCountryAsync(string country);

        Task<List<string>> GetCountriesAsync();
    }
}
