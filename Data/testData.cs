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
            // Seed Offices
            if (!_context.Offices.Any())
            {
                var testOffices = new List<Office>
                {
                    new Office
                    {
                        OfficeName = "STOCKHOLM OFFICE",
                        Country = "Sweden",
                        Currency = "SEK"
                    },

                    new Office
                    {
                        OfficeName = "NEW YORK OFFICE",
                        Country = "USA",
                        Currency = "USD"
                    },

                    new Office
                    {
                        OfficeName = "BERLIN OFFICE",
                        Country = "Germany",
                        Currency = "EUR"
                    },

                    new Office
                    {
                        OfficeName = "ANKARA OFFICE",
                        Country = "Turkey",
                        Currency = "TRY"
                    }
                };

                _context.Offices.AddRange(testOffices);
                await _context.SaveChangesAsync();
            }

            // Load offices from DB
            var swedenOffice = _context.Offices.First(o => o.Country == "Sweden");
            var usaOffice = _context.Offices.First(o => o.Country == "USA");
            var germanyOffice = _context.Offices.First(o => o.Country == "Germany");
            var turkeyOffice = _context.Offices.First(o => o.Country == "Turkey");

            // Seed Assets
            if (!_context.Assets.Any())
            {
                var testAssets = new List<Asset>
                {
                    // ================= USA =================

                    new ComputerAsset
                    {
                        AssetType = "Laptop",
                        Brand = "Lenovo",
                        ModelName = "ThinkPad X1",
                        Office = usaOffice,
                        PurchaseDate = DateTime.Now.AddMonths(-70),
                        PurchasePriceUSD = 1500,
                        LocalPrice = await _currencyService.ConvertAsync(1500, "USD", "USD"),
                        WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-70))
                    },

                    new ComputerAsset
                    {
                        AssetType = "Desktop",
                        Brand = "HP",
                        ModelName = "EliteDesk 800",
                        Office = usaOffice,
                        PurchaseDate = DateTime.Now.AddMonths(-22),
                        PurchasePriceUSD = 1100,
                        LocalPrice = await _currencyService.ConvertAsync(1100, "USD", "USD"),
                        WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-22))
                    },

                    new MobileAsset
                    {
                        AssetType = "Mobile",
                        Brand = "iPhone",
                        ModelName = "14 Pro",
                        Office = usaOffice,
                        PurchaseDate = DateTime.Now.AddMonths(-8),
                        PurchasePriceUSD = 1200,
                        LocalPrice = await _currencyService.ConvertAsync(1200, "USD", "USD"),
                        WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-8))
                    },

                    new MobileAsset
                    {
                        AssetType = "Tablet",
                        Brand = "Samsung",
                        ModelName = "Galaxy Tab S8",
                        Office = usaOffice,
                        PurchaseDate = DateTime.Now.AddMonths(-36),
                        PurchasePriceUSD = 800,
                        LocalPrice = await _currencyService.ConvertAsync(800, "USD", "USD"),
                        WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-36))
                    },

                    new ComputerAsset
                    {
                        AssetType = "Desktop",
                        Brand = "Dell",
                        ModelName = "Inspiron Desktop",
                        Office = usaOffice,
                        PurchaseDate = DateTime.Now.AddMonths(-40),
                        PurchasePriceUSD = 700,
                        LocalPrice = await _currencyService.ConvertAsync(700, "USD", "USD"),
                        WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-40))
                    },

                    new MobileAsset
                    {
                        AssetType = "Tablet",
                        Brand = "Lenovo",
                        ModelName = "Tab P11",
                        Office = usaOffice,
                        PurchaseDate = DateTime.Now.AddMonths(-16),
                        PurchasePriceUSD = 400,
                        LocalPrice = await _currencyService.ConvertAsync(400, "USD", "USD"),
                        WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-16))
                    },

                    // ================= SWEDEN =================

                    new ComputerAsset
                    {
                        AssetType = "Laptop",
                        Brand = "Mac",
                        ModelName = "MacBook Pro M2",
                        Office = swedenOffice,
                        PurchaseDate = DateTime.Now.AddMonths(-35),
                        PurchasePriceUSD = 2500,
                        LocalPrice = await _currencyService.ConvertAsync(2500, "USD", "SEK"),
                        WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-35))
                    },

                    new ComputerAsset
                    {
                        AssetType = "Laptop",
                        Brand = "Lenovo",
                        ModelName = "IdeaPad 5",
                        Office = swedenOffice,
                        PurchaseDate = DateTime.Now.AddMonths(-32),
                        PurchasePriceUSD = 900,
                        LocalPrice = await _currencyService.ConvertAsync(900, "USD", "SEK"),
                        WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-32))
                    },

                    new MobileAsset
                    {
                        AssetType = "Mobile",
                        Brand = "Nokia",
                        ModelName = "G50",
                        Office = swedenOffice,
                        PurchaseDate = DateTime.Now.AddMonths(-20),
                        PurchasePriceUSD = 500,
                        LocalPrice = await _currencyService.ConvertAsync(500, "USD", "SEK"),
                        WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-20))
                    },

                    new MobileAsset
                    {
                        AssetType = "Tablet",
                        Brand = "Apple",
                        ModelName = "iPad Air",
                        Office = swedenOffice,
                        PurchaseDate = DateTime.Now.AddMonths(-15),
                        PurchasePriceUSD = 900,
                        LocalPrice = await _currencyService.ConvertAsync(900, "USD", "SEK"),
                        WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-15))
                    },

                    new MobileAsset
                    {
                        AssetType = "Mobile",
                        Brand = "iPhone",
                        ModelName = "13",
                        Office = swedenOffice,
                        PurchaseDate = DateTime.Now.AddMonths(-24),
                        PurchasePriceUSD = 900,
                        LocalPrice = await _currencyService.ConvertAsync(900, "USD", "SEK"),
                        WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-24))
                    },

                    // ================= GERMANY =================

                    new ComputerAsset
                    {
                        AssetType = "Desktop",
                        Brand = "Dell",
                        ModelName = "OptiPlex 7090",
                        Office = germanyOffice,
                        PurchaseDate = DateTime.Now.AddMonths(-30),
                        PurchasePriceUSD = 1200,
                        LocalPrice = await _currencyService.ConvertAsync(1200, "USD", "EUR"),
                        WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-30))
                    },

                    new MobileAsset
                    {
                        AssetType = "Mobile",
                        Brand = "Samsung",
                        ModelName = "Galaxy S23",
                        Office = germanyOffice,
                        PurchaseDate = DateTime.Now.AddMonths(-12),
                        PurchasePriceUSD = 1000,
                        LocalPrice = await _currencyService.ConvertAsync(1000, "USD", "EUR"),
                        WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-12))
                    },

                    // ================= TURKEY =================

                    new ComputerAsset
                    {
                        AssetType = "Laptop",
                        Brand = "Lenovo",
                        ModelName = "ThinkBook 15",
                        Office = turkeyOffice,
                        PurchaseDate = DateTime.Now.AddMonths(-26),
                        PurchasePriceUSD = 1000,
                        LocalPrice = await _currencyService.ConvertAsync(1000, "USD", "TRY"),
                        WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-26))
                    },

                    new MobileAsset
                    {
                        AssetType = "Mobile",
                        Brand = "Samsung",
                        ModelName = "Galaxy A54",
                        Office = turkeyOffice,
                        PurchaseDate = DateTime.Now.AddMonths(-9),
                        PurchasePriceUSD = 500,
                        LocalPrice = await _currencyService.ConvertAsync(500, "USD", "TRY"),
                        WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(DateTime.Now.AddMonths(-9))
                    }
                };

                _context.Assets.AddRange(testAssets);
                await _context.SaveChangesAsync();
            }
        }
    }
}