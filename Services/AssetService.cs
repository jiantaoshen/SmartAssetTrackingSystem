using Microsoft.EntityFrameworkCore;
using SmartAssetTrackingSystem.Helpers;
using SmartAssetTrackingSystem.Models;
using SmartAssetTrackingSystem.Repositories;
using SmartAssetTrackingSystem.Services.External;
using System.Globalization;
using System.Reflection.PortableExecutable;

namespace SmartAssetTrackingSystem.Services
{
    public class AssetService
    {
        private readonly IAssetRepository _repo;
        private readonly IOfficeRepository _officeRepo;
        private readonly IReportRepository _reportRepo;
        private readonly CurrencyService _currencyService;

        // Predefined header for asset listings
        private readonly string header =
                $"{"Id",-8}" +
                $"{"Office",-10}" +
                $"{"Type",-10}" +
                $"{"Brand",-10}" +
                $"{"Model",-20}" +
                $"{"Purchase Date",-15}" +
                $"{"Price (Dollar)",15}" +
                $"{"Price (Local)",20}";

        public AssetService(IAssetRepository repo, IOfficeRepository officeRepo, CurrencyService currencyService, IReportRepository reportRepo)
        {
            _repo = repo;
            _officeRepo = officeRepo;
            _currencyService = currencyService;
            _reportRepo = reportRepo;
        }

        public async Task AddAsset()
        {
            Asset asset;

            //----------------------------------------------------------
            // User input and validation loops for asset creation
            // ---------------------------------------------------------

            while (true)
            {
                Console.WriteLine("To enter a new product - follow the steps | To quit - enter: \"Q\" ");

                // Loop until a valid office location is entered
                int officeNumber;

                while (true)
                {
                    Console.Write("Enter OfficeLocation (1: Sweden Office 2: USA Office 3: Germany Office 4.Turkey Office): ");

                    string inputOffice = Console.ReadLine() ?? "";

                    if (inputOffice.Trim().Equals("q", StringComparison.OrdinalIgnoreCase))
                        return; // exits entire method

                    if (int.TryParse(inputOffice, out officeNumber) && (officeNumber >= 1 && officeNumber <= 4))
                        break;

                    Console.WriteLine("Invalid input. Try again.");
                }

                // Loop until a valid asset type is entered
                int typeNumber;

                while (true)
                {
                    Console.Write("Enter a number (1:Laptop 2:Desktop 3:Mobile): ");
                    string inputType = Console.ReadLine() ?? "";

                    if (int.TryParse(inputType, out typeNumber) && (typeNumber == 1 || typeNumber == 2 || typeNumber == 3))
                        break;

                    Console.WriteLine("Invalid input. Try again.");
                }

                Console.Write("Enter a Brand: ");
                string inputBrand = Console.ReadLine() ?? "Unknown";

                Console.Write("Enter a Model Name: ");
                string inputModelName = Console.ReadLine() ?? "Unknown";

                // Loop until a valid date is entered
                DateTime validDate;

                while (true)
                {
                    Console.Write("Enter a Purchase date (yyyy-mm-dd): ");
                    string inputDate = Console.ReadLine() ?? "";

                    if (DateTime.TryParseExact(inputDate,"yyyy-MM-dd",CultureInfo.InvariantCulture,DateTimeStyles.None, out validDate))
                        break;

                    Console.WriteLine("Invalid date format. Please try again.");
                }


                // Loop until a valid price is entered
                decimal price;

                while (true)
                {
                    Console.Write($"Enter Price (USD): ");
                    string inputPrice = Console.ReadLine() ?? "";

                    if (decimal.TryParse(inputPrice, out price))
                    {
                        price = Math.Round(price, 2);
                        break;
                    }

                    Console.WriteLine("Invalid input of price. Please try again.");
                }

                // ---------------------------------------------------------
                // Asset Creation and setting properties based on user input
                // ---------------------------------------------------------

                // Create asset object based on type selection
                if (typeNumber == 1 || typeNumber == 2)
                {
                    asset = new ComputerAsset();
                    asset.AssetType = typeNumber == 1 ? "Laptop" : "Desktop";
                }
                else
                {
                    asset = new MobileAsset();
                    asset.AssetType = "Mobile";
                }

                // Set office and local price based on office selection
                string country = officeNumber switch
                {
                    1 => "Sweden",
                    2 => "USA",
                    3 => "Germany",
                    4 => "Turkey",
                    _ => throw new ArgumentException("Invalid office number")
                };

                var office = await _officeRepo.GetByCountryAsync(country);

                if (office == null)
                {
                    Console.WriteLine("Invalid office selection.");
                    return;
                }

                asset.OfficeId = office.Id;
                asset.LocalPrice = await _currencyService.ConvertAsync(price, "USD", office.Currency);
                asset.Brand = inputBrand;
                asset.ModelName = inputModelName;
                asset.PurchaseDate = validDate;
                asset.WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(validDate);
                asset.PurchasePriceUSD = price;

                await _repo.AddAsset(asset);

                if (typeNumber == 1 || typeNumber == 2) Console.WriteLine("Computer Asset added successfully!\n");
                else Console.WriteLine("Mobile Asset added successfully!\n");
            }
        }

        public async Task GetAssets()
        {
            var computerAssets = await _repo.GetComputerAssetsAsync();
            var mobileAssets = await _repo.GetMobileAssetsAsync();

            PrintHelper.PrintHeader("Asset List", header.Length);
            PrintHelper.PrintSubHeader("Computers", header.Length);
            PrintHelper.PrintSubHeader(header, header.Length);

            AssetHelper.PrintAssets(computerAssets);

            PrintHelper.PrintSubHeader("Mobile Devices", header.Length);
            PrintHelper.PrintSubHeader(header, header.Length);

            AssetHelper.PrintAssets(mobileAssets);

            PrintHelper.PrintFooter(header.Length);
        }

        public async Task UpdateAsset()
        {
            Console.Write("Enter Asset ID to edit: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            var asset = await _repo.GetAssetById(id);

            if (asset == null)
            {
                Console.WriteLine("Asset not found.");
                return;
            }

            Console.WriteLine("Leave input empty to keep current value.");

            // Loop until a valid office location is entered
            while (true)
            {
                Console.Write($"Enter new office (1: Sweden Office 2: USA Office 3: Germany Office 4.Turkey Office) ({asset.Office.Country} Office): ");

                int officeNumber = 0;
                string inputOffice = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(inputOffice)) break;

                if (int.TryParse(inputOffice, out officeNumber) && (officeNumber >= 1 && officeNumber <= 4))
                {
                    string country = officeNumber switch
                    {
                        1 => "Sweden",
                        2 => "USA",
                        3 => "Germany",
                        4 => "Turkey",
                        _ => throw new ArgumentException("Invalid office number")
                    };

                    var office = await _officeRepo.GetByCountryAsync(country);

                    if (office == null)
                        throw new InvalidOperationException($"{country} office not found");

                    asset.OfficeId = office.Id;
                    break;
                }

                Console.WriteLine("Invalid input. Try again.");
            }

            // Brand update
            Console.Write($"New Brand ({asset.Brand}): ");
            string inputBrand = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(inputBrand)) asset.Brand = inputBrand;

            // Model update
            Console.Write($"New Model ({asset.ModelName}): ");
            string inputModel = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(inputModel)) asset.ModelName = inputModel;

            // Price update with validation
            while (true)
            {
                Console.Write($"New Price in USD({asset.PurchasePriceUSD}): ");
                string inputPrice = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(inputPrice)) break;

                if (decimal.TryParse(inputPrice, out decimal price))
                {
                    asset.PurchasePriceUSD = Math.Round(price, 2);
                    asset.LocalPrice = await _currencyService.ConvertAsync(asset.PurchasePriceUSD, "USD", asset.Office.Currency);
                    break;
                }

                Console.WriteLine("Invalid price.");
            }

            // Purchase date update with validation
            while (true)
            {
                Console.Write($"New Purchase Date ({asset.PurchaseDate:yyyy-MM-dd}): ");
                string inputDate = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(inputDate)) break;

                if (DateTime.TryParseExact(inputDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validDate))
                {
                    asset.PurchaseDate = validDate;
                    asset.WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(validDate);
                    break;
                }

                Console.WriteLine("Invalid date format. Please try again.");
            }

            await _repo.UpdateAsset(asset);
            Console.WriteLine("Asset updated successfully!");
        }

        public async Task RemoveAsset()
        {
            Console.Write("Enter Asset ID to remove: ");

            string inputId = Console.ReadLine() ?? "";

            if (!int.TryParse(inputId, out int id))
            {
                Console.WriteLine("Invalid Asset ID.");
                return;
            }

            var asset = await _repo.GetAssetById(id);

            if (asset == null)
            {
                Console.WriteLine("Asset not found.");
                return;
            }

            await _repo.RemoveAsset(id);
            Console.WriteLine("Asset removed successfully!");
        }

        public async Task GetAssetById()
        {
            Console.Write("Enter Asset ID: ");

            string inputId = Console.ReadLine() ?? "";

            if (!int.TryParse(inputId, out int id))
            {
                Console.WriteLine("Invalid Asset ID.");
                return;
            }

            var asset = await _repo.GetAssetById(id);

            if (asset == null)
            {
                Console.WriteLine("Asset not found.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("===== Asset Details =====");
            Console.WriteLine($"ID: {asset.Id}");
            Console.WriteLine($"Type: {asset.AssetType}");
            Console.WriteLine($"Brand: {asset.Brand}");
            Console.WriteLine($"Model: {asset.ModelName}");
            Console.WriteLine($"Office: {asset.Office.OfficeName}");
            Console.WriteLine($"Purchase Date: {asset.PurchaseDate:yyyy-MM-dd}");
            Console.WriteLine($"Warranty Expiration: {asset.WarrantyExpirationDate:yyyy-MM-dd}");
            Console.WriteLine($"Local Price: {asset.LocalPrice:N2}");
            Console.WriteLine($"Price USD: {asset.PurchasePriceUSD:N2}");
        }

        public async Task GetAssetsByOffice()
        {
            int officeNumber;
            Console.Write("Enter OfficeLocation (1: STOCKHOLM OFFICE 2: NEW YORK OFFICE 3: BERLIN OFFICE 4: ANKARA OFFICE): ");
            string inputOffice = Console.ReadLine() ?? "";


            if (int.TryParse(inputOffice, out officeNumber) && (officeNumber >= 1 && officeNumber <= 4))
            {
                Console.Clear();

                string country = officeNumber switch
                {
                    1 => "Sweden",
                    2 => "USA",
                    3 => "Germany",
                    4 => "Turkey",
                    _ => throw new ArgumentException("Invalid office number")
                };

                var office = await _officeRepo.GetByCountryAsync(country);

                if (office == null)
                {
                    Console.WriteLine("Office not found.");
                    return;
                }

                decimal totalValue = await _reportRepo.GetTotalAssetValuePerOffice(office.Id);

                PrintHelper.PrintHeader(office.OfficeName, header.Length);
                PrintHelper.PrintSubHeader(header, header.Length);

                var assets = await _repo.GetAssetsByOffice(office.Id);

                AssetHelper.PrintAssets(assets);

                Console.WriteLine($"Total Office Value: {totalValue:F2} {assets.First().Office.Currency} \n");

                PrintHelper.PrintFooter(header.Length);
            }
            else Console.WriteLine("Invalid input. Try again.");
        }

        public async Task GetFullReport()
        {
            Console.Clear();

            PrintHelper.PrintHeader("Report", header.Length);
            PrintHelper.PrintSubHeader("Asset Counts Per Office", header.Length);

            var countries = await _officeRepo.GetCountriesAsync();

            foreach (var country in countries)
            {
                var office = await _officeRepo.GetByCountryAsync(country);

                if (office == null)
                {
                    Console.WriteLine($"Office for {country} not found.");
                    continue;
                }

                int count = await _reportRepo.GetAssetCountPerOffice(office.Id);
                Console.WriteLine($"Asset Count for {office.OfficeName}: {count}");
            }


            PrintHelper.PrintSubHeader("\nAssets Near Expiration", header.Length);
            var expiringAssets = await _reportRepo.GetAssetsCloseToExpiration(3);
            AssetHelper.PrintAssets(expiringAssets);

            PrintHelper.PrintSubHeader("\nMost Expensive Assets (Top 5)", header.Length);
            var expensiveAssets = await _reportRepo.GetMostExpensiveAssets(5);
            AssetHelper.PrintAssets(expensiveAssets);

            PrintHelper.PrintFooter(header.Length);
        }
    }
}