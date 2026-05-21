using SmartAssetTrackingSystem.Helpers;
using SmartAssetTrackingSystem.Models;
using SmartAssetTrackingSystem.Services;

namespace SmartAssetTrackingSystem.Data
{
    public class TestDataSeeder
    {
        private readonly MyDbContext _context;
        private readonly CurrencyService _currencyService;

        public TestDataSeeder(MyDbContext context, CurrencyService currencyService)
        {
            _context = context;
            _currencyService = currencyService;
        }

        public async Task SeedDataAsync()
        {
            if (_context.Assets.Any())
                return;

            var testAssets = new List<Asset>
            {
                new ComputerAsset
                {
                    AssetType = "Laptop",
                    Brand = "Lenovo",
                    ModelName = "ThinkPad X1",
                    OfficeLocation = "USA",
                    PurchaseDate = DateTime.Now.AddMonths(-70),
                    LocalPrice = 1500,
                    PurchasePriceUSD = await _currencyService.ConvertAsync(1500, "USD", "USD"),
                    WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-70))
                },

                new ComputerAsset
                {
                    AssetType = "Laptop",
                    Brand = "Mac",
                    ModelName = "MacBook Pro M2",
                    OfficeLocation = "Sweden",
                    PurchaseDate = DateTime.Now.AddMonths(-35),
                    LocalPrice = 25000,
                    PurchasePriceUSD = await _currencyService.ConvertAsync(25000, "SEK", "USD"),
                    WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-35))
                },

                new ComputerAsset
                {
                    AssetType = "Desktop",
                    Brand = "Dell",
                    ModelName = "OptiPlex 7090",
                    OfficeLocation = "Germany",
                    PurchaseDate = DateTime.Now.AddMonths(-30),
                    LocalPrice = 1200,
                    PurchasePriceUSD = await _currencyService.ConvertAsync(1200, "EUR", "USD"),
                    WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-30))
                },

                new ComputerAsset
                {
                    AssetType = "Desktop",
                    Brand = "HP",
                    ModelName = "EliteDesk 800",
                    OfficeLocation = "USA",
                    PurchaseDate = DateTime.Now.AddMonths(-22),
                    LocalPrice = 1100,
                    PurchasePriceUSD = await _currencyService.ConvertAsync(1100, "USD", "USD"),
                    WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-22))
                },

                new ComputerAsset
                {
                    AssetType = "Laptop",
                    Brand = "Lenovo",
                    ModelName = "IdeaPad 5",
                    OfficeLocation = "Sweden",
                    PurchaseDate = DateTime.Now.AddMonths(-32),
                    LocalPrice = 9000,
                    PurchasePriceUSD = await _currencyService.ConvertAsync(9000, "SEK", "USD"),
                    WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-32))
                },

                new MobileAsset
                {
                    AssetType = "Mobile",
                    Brand = "iPhone",
                    ModelName = "14 Pro",
                    OfficeLocation = "USA",
                    PurchaseDate = DateTime.Now.AddMonths(-8),
                    LocalPrice = 1200,
                    PurchasePriceUSD = await _currencyService.ConvertAsync(1200, "USD", "USD"),
                    WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-8))
                },

                new MobileAsset
                {
                    AssetType = "Mobile",
                    Brand = "Samsung",
                    ModelName = "Galaxy S23",
                    OfficeLocation = "Germany",
                    PurchaseDate = DateTime.Now.AddMonths(-12),
                    LocalPrice = 1000,
                    PurchasePriceUSD = await _currencyService.ConvertAsync(1000, "EUR", "USD"),
                    WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-12))
                },

                new MobileAsset
                {
                    AssetType = "Mobile",
                    Brand = "Nokia",
                    ModelName = "G50",
                    OfficeLocation = "Sweden",
                    PurchaseDate = DateTime.Now.AddMonths(-20),
                    LocalPrice = 5000,
                    PurchasePriceUSD = await _currencyService.ConvertAsync(5000, "SEK", "USD"),
                    WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-20))
                },

                new MobileAsset
                {
                    AssetType = "Tablet",
                    Brand = "Samsung",
                    ModelName = "Galaxy Tab S8",
                    OfficeLocation = "USA",
                    PurchaseDate = DateTime.Now.AddMonths(-36),
                    LocalPrice = 800,
                    PurchasePriceUSD = await _currencyService.ConvertAsync(800, "USD", "USD"),
                    WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-36))
                },

                new MobileAsset
                {
                    AssetType = "Tablet",
                    Brand = "Apple",
                    ModelName = "iPad Air",
                    OfficeLocation = "Sweden",
                    PurchaseDate = DateTime.Now.AddMonths(-15),
                    LocalPrice = 9000,
                    PurchasePriceUSD = await _currencyService.ConvertAsync(9000, "SEK", "USD"),
                    WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-15))
                },

                new ComputerAsset
                {
                    AssetType = "Laptop",
                    Brand = "Lenovo",
                    ModelName = "ThinkBook 15",
                    OfficeLocation = "Germany",
                    PurchaseDate = DateTime.Now.AddMonths(-26),
                    LocalPrice = 1000,
                    PurchasePriceUSD = await _currencyService.ConvertAsync(1000, "EUR", "USD"),
                    WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-26))
                },

                new ComputerAsset
                {
                    AssetType = "Desktop",
                    Brand = "Dell",
                    ModelName = "Inspiron Desktop",
                    OfficeLocation = "USA",
                    PurchaseDate = DateTime.Now.AddMonths(-40),
                    LocalPrice = 700,
                    PurchasePriceUSD = await _currencyService.ConvertAsync(700, "USD", "USD"),
                    WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-40))
                },

                new MobileAsset
                {
                    AssetType = "Mobile",
                    Brand = "iPhone",
                    ModelName = "13",
                    OfficeLocation = "Sweden",
                    PurchaseDate = DateTime.Now.AddMonths(-24),
                    LocalPrice = 9000,
                    PurchasePriceUSD = await _currencyService.ConvertAsync(9000, "SEK", "USD"),
                    WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-24))
                },

                new MobileAsset
                {
                    AssetType = "Mobile",
                    Brand = "Samsung",
                    ModelName = "Galaxy A54",
                    OfficeLocation = "Germany",
                    PurchaseDate = DateTime.Now.AddMonths(-9),
                    LocalPrice = 500,
                    PurchasePriceUSD = await _currencyService.ConvertAsync(500, "EUR", "USD"),
                    WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-9))
                },

                new MobileAsset
                {
                    AssetType = "Tablet",
                    Brand = "Lenovo",
                    ModelName = "Tab P11",
                    OfficeLocation = "USA",
                    PurchaseDate = DateTime.Now.AddMonths(-16),
                    LocalPrice = 400,
                    PurchasePriceUSD = await _currencyService.ConvertAsync(400, "USD", "USD"),
                    WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-16))
                }
            };

            _context.Assets.AddRange(testAssets);
            _context.SaveChanges();
        }
    }
}