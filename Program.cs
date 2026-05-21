using Microsoft.EntityFrameworkCore;
using SmartAssetTrackingSystem.Data;
using SmartAssetTrackingSystem.Helpers;
using SmartAssetTrackingSystem.Models;
using SmartAssetTrackingSystem.Reports;
using SmartAssetTrackingSystem.Repositories;
using SmartAssetTrackingSystem.Services;
using System.Globalization;
using System.Reflection.PortableExecutable;

public static class Program
{
    static async Task Main(string[] args)
    {
        MyDbContext context = new MyDbContext();

        var repo = new AssetRepository();
        var reportRepo = new ReportRepository();
        var currencyService = new CurrencyService();

        var assetService = new AssetService(repo);
        var reportService = new ReportService(reportRepo);

        //Add Seed Data
        var seeder = new TestDataSeeder(context, currencyService);
        seeder.SeedDataAsync().Wait();

        Office office;

        while (true)
        {
            PrintMenu();
            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    await InputData(context, assetService, currencyService);
                    break;
                case "2":
                    await ShowList(assetService, context);
                    Pause();
                    break;
                case "3":
                    await ShowList(assetService, context);
                    await EditAsset(context, assetService);
                    Pause();
                    break;
                case "4":
                    await ShowList(assetService, context);
                    await RemoveAsset(assetService);
                    Pause();
                    break;
                case "5":
                    await SearchAssetById(assetService);
                    Pause();
                    break;
                case "6":
                    int officeNumber;
                    Console.Write("Enter OfficeLocation (1: STOCKHOLM OFFICE 2: NEW YORK OFFICE 3: BERLIN OFFICE 4: ANKARA OFFICE): ");
                    string inputOffice = Console.ReadLine() ?? "";

                    if (int.TryParse(inputOffice, out officeNumber) && (officeNumber >= 1 && officeNumber <= 4))
                    {
                        ClearConsole();

                        office = officeNumber switch
                        {
                            1 => context.Offices.First(o => o.Country == "Sweden"),
                            2 => context.Offices.First(o => o.Country == "USA"),
                            3 => context.Offices.First(o => o.Country == "Germany"),
                            4 => context.Offices.First(o => o.Country == "Turkey"),
                            _ => throw new InvalidOperationException()
                        };

                        var assets = await assetService.GetAssetsByOffice(office.Id);
                        decimal totalValue = await reportService.GetTotalAssetValuePerOffice(office.Id);
                        ReportsGeneration.GenerateOfficeReport(assets, office.OfficeName, totalValue);
                        Pause();
                        break;
                    }

                    Console.WriteLine("Invalid input. Try again.");
                    Pause();
                    break;
                 case "7":
                    ClearConsole();
                    string header =
                        $"{"Id",-8}" +
                        $"{"Office",-10}" +
                        $"{"Type",-10}" +
                        $"{"Brand",-10}" +
                        $"{"Model",-20}" +
                        $"{"Purchase Date",-15}" +
                        $"{"Price (Dollar)",15}" +
                        $"{"Price (Local)",20}";

                    string title = "    Report    ";
                    int totalWidth = header.Length;

                    int left = (totalWidth - title.Length) / 2;

                    string titleline =
                        new string('=', left) +
                        title +
                        new string('=', totalWidth - left - title.Length);

                    Console.WriteLine(titleline + "\n");

                    Console.WriteLine("Office Asset Counts");
                    Console.WriteLine(new string('-', header.Length));

                    office = context.Offices.First(o => o.Country == "Sweden");
                    int swedenCount = await reportService.GetAssetCountPerOffice(office.Id);
                    Console.WriteLine($"Asset Count for {office.OfficeName}: {swedenCount}");

                    office = context.Offices.First(o => o.Country == "USA");
                    int usaCount = await reportService.GetAssetCountPerOffice(office.Id);
                    Console.WriteLine($"Asset Count for {office.OfficeName}: {usaCount}");

                    office = context.Offices.First(o => o.Country == "Germany");
                    int germanyCount = await reportService.GetAssetCountPerOffice(office.Id);
                    Console.WriteLine($"Asset Count for {office.OfficeName}: {germanyCount}");

                    office = context.Offices.First(o => o.Country == "Turkey");
                    int turkeyCount = await reportService.GetAssetCountPerOffice(office.Id);
                    Console.WriteLine($"Asset Count for {office.OfficeName}: {turkeyCount}");

                    Console.WriteLine("\nAssets Near Expiration");
                    Console.WriteLine(new string('-', header.Length));
                    
                    var expiringAssets = await reportService.GetAssetsCloseToExpiration(3);
                    AssetHelper.PrintAssets(expiringAssets);

                    Console.WriteLine("\nMost Expensive Assets (Top 5)");
                    Console.WriteLine(new string('-', header.Length));
                    var expensiveAssets = await reportService.GetMostExpensiveAssets(5);
                    AssetHelper.PrintAssets(expensiveAssets);

                    Console.WriteLine(new string('=', header.Length));

                    Pause();
                    break;
                 case "8":
                    return; 
                default:
                    Console.WriteLine("Invalid choice!");
                    Pause();
                    break;
            }
        }
    }

    private static async Task InputData(MyDbContext context, AssetService assetService, CurrencyService currencyService)
    {
        var rates = await currencyService.GetRatesAsync();
        ClearConsole();

        while (true)
        {
            Console.WriteLine("To enter a new product - follow the steps | To quit - enter: \"Q\" ");

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

            DateTime validDate;

            while (true)
            {
                Console.Write("Enter a Purchase date (yyyy-mm-dd): ");
                string inputDate = Console.ReadLine() ?? "";

                bool isValid = DateTime.TryParseExact(
                    inputDate,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out validDate);

                if (isValid) break;

                Console.WriteLine("Invalid date format. Please try again.");
            }

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

            Asset asset;

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

            var swedenOffice = context.Offices.First(o => o.Country == "Sweden");
            var usaOffice = context.Offices.First(o => o.Country == "USA");
            var germanyOffice = context.Offices.First(o => o.Country == "Germany");
            var turkeyOffice = context.Offices.First(o => o.Country == "Turkey");

            string currency;
            if (officeNumber == 1)
            {
                asset.OfficeId = swedenOffice.Id;
                currency = swedenOffice.Currency;
            }
            else if (officeNumber == 2)
            {
                asset.OfficeId = usaOffice.Id;
                currency = usaOffice.Currency;
            }
            else if (officeNumber == 3)
            {
                asset.OfficeId = germanyOffice.Id;
                currency = germanyOffice.Currency;
            }
            else
            {
                asset.OfficeId = turkeyOffice.Id;
                currency = turkeyOffice.Currency;
            }

            asset.Brand = inputBrand;
            asset.ModelName = inputModelName;
            asset.PurchaseDate = validDate;
            asset.WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(validDate);
            asset.PurchasePriceUSD = price;
            asset.LocalPrice = await currencyService.ConvertAsync(price, "USD", currency);


            if (typeNumber == 1 || typeNumber == 2)
            {
                assetService.AddAsset(asset);
                Console.WriteLine("Computer Asset added successfully!\n");
            }
            else
            {
                assetService.AddAsset(asset);
                Console.WriteLine("Mobile Asset added successfully!\n");
            }
        }
    }

    private static async Task ShowList(AssetService assetService, MyDbContext context)
    {
        ClearConsole();

        var computerAssets = await context.Assets
                            .AsNoTracking()
                            .Include(a => a.Office)
                            .OfType<ComputerAsset>()
                            .OrderByDescending(a => a.PurchaseDate)
                            .ToListAsync();

        var mobileAssets = await context.Assets
                            .AsNoTracking()
                            .Include(a => a.Office)
                            .OfType<MobileAsset>()
                            .OrderByDescending(a => a.PurchaseDate)
                            .ToListAsync();

        string header =
            $"{"Id",-8}" +
            $"{"Office",-10}" +
            $"{"Type",-10}" +
            $"{"Brand",-10}" +
            $"{"Model",-20}" +
            $"{"Purchase Date",-15}" +
            $"{"Price (Dollar)",15}" +
            $"{"Price (Local)", 20}";

        string title = "    Asset list    ";
        int totalWidth = header.Length;

        int left = (totalWidth - title.Length) / 2;

        string titleline =
            new string('=', left) +
            title +
            new string('=', totalWidth - left - title.Length);

        Console.WriteLine(titleline + "\n");

        Console.WriteLine("Computers");

        Console.WriteLine(new string('-', header.Length));
        Console.WriteLine(header);
        Console.WriteLine(new string('-', header.Length));

        AssetHelper.PrintAssets(computerAssets);

        Console.WriteLine("\nMobile Devices");
        Console.WriteLine(new string('-', header.Length));
        Console.WriteLine(header);
        Console.WriteLine(new string('-', header.Length));

        AssetHelper.PrintAssets(mobileAssets);

        Console.WriteLine("\n");
        Console.WriteLine(new string('=', header.Length));
    }

    private static async Task EditAsset(MyDbContext context, AssetService assetService)
    {
        Console.Write("Enter Asset ID to edit: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        var asset = await assetService.GetAssetById(id);

        if (asset == null)
        {
            Console.WriteLine("Asset not found.");
            return;
        }

        Console.Write($"New Brand ({asset.Brand}): ");
        string inputBrand = Console.ReadLine() ?? "";

        Console.Write($"New Model ({asset.ModelName}): ");
        string inputModel = Console.ReadLine() ?? "";

        int officeNumber;
        string currency = "USD";
        while (true)
        {
            Console.Write("Enter OfficeLocation (1: Sweden Office 2: USA Office 3: Germany Office 4.Turkey Office): ");

            string inputOffice = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(inputOffice))
                break; // keep current office if user presses Enter


            if (int.TryParse(inputOffice, out officeNumber) && (officeNumber >= 1 && officeNumber <= 4))
            {
                var swedenOffice = context.Offices.First(o => o.Country == "Sweden");
                var usaOffice = context.Offices.First(o => o.Country == "USA");
                var germanyOffice = context.Offices.First(o => o.Country == "Germany");
                var turkeyOffice = context.Offices.First(o => o.Country == "Turkey");
                
                if (officeNumber == 1)
                {
                    asset.OfficeId = swedenOffice.Id;
                    currency = swedenOffice.Currency;
                }
                else if (officeNumber == 2)
                {
                    asset.OfficeId = usaOffice.Id;
                    currency = usaOffice.Currency;
                }
                else if (officeNumber == 3)
                {
                    asset.OfficeId = germanyOffice.Id;
                    currency = germanyOffice.Currency;
                }
                else
                {
                    asset.OfficeId = turkeyOffice.Id;
                    currency = turkeyOffice.Currency;
                }
                break;
            }

            Console.WriteLine("Invalid input. Try again.");
        }

        Console.Write($"New Price in USD({asset.PurchasePriceUSD}): ");
        string inputPrice = Console.ReadLine() ?? "";

        Console.Write($"New Purchase Date ({asset.PurchaseDate:yyyy-MM-dd}): ");
        string inputDate = Console.ReadLine() ?? "";

        Console.Write($"New Serial Number ({asset.SerialNumber}): ");
        string inputSerial = Console.ReadLine() ?? "";

        Console.Write($"New Employee Username ({asset.EmployeeUsername}): ");
        string inputEmployee = Console.ReadLine() ?? "";

        // Update only if user entered something
        if (!string.IsNullOrWhiteSpace(inputBrand))
            asset.Brand = inputBrand;

        if (!string.IsNullOrWhiteSpace(inputModel))
            asset.ModelName = inputModel;

        if (!string.IsNullOrWhiteSpace(inputSerial))
            asset.SerialNumber = inputSerial;

        if (!string.IsNullOrWhiteSpace(inputEmployee))
            asset.EmployeeUsername = inputEmployee;

        // Price validation
        if (!string.IsNullOrWhiteSpace(inputPrice))
        {
            if (decimal.TryParse(inputPrice, out decimal price))
            {
                if (price <= 0)
                {
                    Console.WriteLine("Price must be greater than 0.");
                    return;
                }

                asset.PurchasePriceUSD = price;
                asset.LocalPrice = await new CurrencyService().ConvertAsync(price, "USD", currency);
            }
            else
            {
                Console.WriteLine("Invalid price.");
                return;
            }
        }

        // Date validation
        if (!string.IsNullOrWhiteSpace(inputDate))
        {
            if (DateTime.TryParse(inputDate, out DateTime purchaseDate))
            {
                asset.PurchaseDate = purchaseDate;

                // Auto-update warranty
                asset.WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(purchaseDate);
            }
            else
            {
                Console.WriteLine("Invalid date.");
                return;
            }
        }

        assetService.UpdateAsset(asset);

        Console.WriteLine("Asset updated successfully!");
    }

    private static async Task RemoveAsset(AssetService assetService)
    {
        Console.Write("Enter Asset ID to remove: ");

        string inputId = Console.ReadLine() ?? "";

        if (!int.TryParse(inputId, out int id))
        {
            Console.WriteLine("Invalid Asset ID.");
            return;
        }

        var asset = await assetService.GetAssetById(id);

        if (asset == null)
        {
            Console.WriteLine("Asset not found.");
            return;
        }

        await assetService.RemoveAsset(id);

        Console.WriteLine("Asset removed successfully!");
    }

    private static async Task SearchAssetById(AssetService assetService)
    {
        Console.Write("Enter Asset ID: ");

        string inputId = Console.ReadLine() ?? "";

        if (!int.TryParse(inputId, out int id))
        {
            Console.WriteLine("Invalid Asset ID.");
            return;
        }

        var asset = await assetService.GetAssetById(id);

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
        Console.WriteLine($"Serial Number: {asset.SerialNumber ?? "N/A"}");
        Console.WriteLine($"Employee Username: {asset.EmployeeUsername ?? "N/A"}");
    }

    static void PrintMenu()
    {
        ClearConsole();

        Console.WriteLine("1. Add Asset");
        Console.WriteLine("2. Show all Assets");
        Console.WriteLine("3. Update Asset");
        Console.WriteLine("4. Delete Asset");
        Console.WriteLine("5. Search Asset");
        Console.WriteLine("6. Get Office Report");
        Console.WriteLine("7. Get Company Report");
        Console.WriteLine("8. Exit");
    }
    static void Pause()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    static void ClearConsole()
    {
        Console.Clear();
    }


}
