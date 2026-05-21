using Microsoft.EntityFrameworkCore;
using SmartAssetTrackingSystem.Data;
using SmartAssetTrackingSystem.Helpers;
using SmartAssetTrackingSystem.Models;
using SmartAssetTrackingSystem.Repositories;
using SmartAssetTrackingSystem.Services;
using System.Globalization;

public static class Program
{
    static async Task Main(string[] args)
    {
        MyDbContext context = new MyDbContext();

        var repo = new AssetRepository();
        var currencyService = new CurrencyService();

        var assetService = new AssetService(repo);

        //Add Seed Data
        var seeder = new TestDataSeeder(context, currencyService);
        seeder.SeedDataAsync().Wait();

        while (true)
        {
            PrintMenu();
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await InputData(assetService, currencyService);
                    break;
                case "2":
                    await ShowList(assetService, context);
                    Pause();
                    break;
                case "3":
                    await ShowList(assetService, context);
                    await EditAsset(assetService);
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
                    return;
                default:
                    Console.WriteLine("Invalid choice!");
                    Pause();
                    break;
            }
        }
    }

    private static async Task InputData(AssetService assetService, CurrencyService currencyService)
    {
        var rates = await currencyService.GetRatesAsync();
        Console.Clear();

        while (true)
        {
            Console.WriteLine("To enter a new product - follow the steps | To quit - enter: \"Q\" ");

            int officeNumber;

            while (true)
            {
                Console.Write("Enter OfficeLocation (1: USA 2: Sweden 3: Germany): ");

                string inputOffice = Console.ReadLine() ?? "";

                if (inputOffice.Trim().Equals("q", StringComparison.OrdinalIgnoreCase))
                    return; // exits entire method

                if (int.TryParse(inputOffice, out officeNumber) && (officeNumber == 1 || officeNumber == 2 || officeNumber == 3))
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
                Console.Write($"Enter Price (Local Currency): ");
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

            string inputCurrency;

            if (officeNumber == 1)
            {
                asset.OfficeLocation = "USA";
                inputCurrency = "USD";
            }
            else if (officeNumber == 2)
            {
                asset.OfficeLocation = "Sweden";
                inputCurrency = "SEK";
            }
            else
            {
                asset.OfficeLocation = "Germany";
                inputCurrency = "EUR";
            }

            asset.Brand = inputBrand;
            asset.ModelName = inputModelName;
            asset.PurchaseDate = validDate;
            asset.WarrantyExpirationDate = AssetHelper.GetWarrantyExpirationDate(validDate);
            asset.LocalPrice = price;
            asset.PurchasePriceUSD = await currencyService.ConvertAsync(price, inputCurrency, "USD");


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
        Console.Clear();

        var assets = await assetService.GetAssets();

        var computerAssets = context.Assets
                            .OfType<ComputerAsset>()
                            .OrderByDescending(a => a.PurchaseDate)
                            .ToList();

        var mobileAssets = context.Assets
                            .OfType<MobileAsset>()
                            .OrderByDescending(a => a.PurchaseDate)
                            .ToList();

        string header =
            $"{"Id",-8}" +
            $"{"Office",-10}" +
            $"{"Type",-10}" +
            $"{"Brand",-10}" +
            $"{"Model",-20}" +
            $"{"Purchase Date",-15}" +
            $"{"Price (Local)", 20}" +
            $"{"Price (Dollar)", 15}";

        string title = "    Asset list    ";
        int totalWidth = header.Length;

        int left = (totalWidth - title.Length) / 2;

        string titleline =
            new string('=', left) +
            title +
            new string('=', totalWidth - left - title.Length);

        Console.WriteLine(titleline + "\n");

        Console.WriteLine("\nComputers");

        Console.WriteLine(new string('-', header.Length));
        Console.WriteLine(header);
        Console.WriteLine(new string('-', header.Length));

        PrintList(computerAssets);

        Console.WriteLine("\nMobile Devices");
        Console.WriteLine(new string('-', header.Length));
        Console.WriteLine(header);
        Console.WriteLine(new string('-', header.Length));

        PrintList(mobileAssets);

        Console.WriteLine("\n");
        Console.WriteLine(new string('=', header.Length));
    }

    private static void PrintList(IEnumerable<Asset> typeAssets)
    {
        foreach (var asset in typeAssets)
        {
            string inputCurrency;

            if (asset.OfficeLocation == "USA")
                inputCurrency = "USD";
            else if (asset.OfficeLocation == "Sweden")
                inputCurrency = "SEK";
            else
                inputCurrency = "EUR";

            string line =
                $"{asset.Id,-8}" +
                $"{asset.OfficeLocation,-10}" +
                $"{asset.AssetType,-10}" +
                $"{asset.Brand,-10}" +
                $"{asset.ModelName,-20}" +
                $"{asset.PurchaseDate,-15:yyyy-MM-dd}" +
                $"{asset.LocalPrice, 15:N2}" +
                $"{inputCurrency, 5}" +
                $"{asset.PurchasePriceUSD, 15:N2}";

            if (asset.WarrantyExpirationDate.AddMonths(-3) < DateTime.Now)
                Console.ForegroundColor = ConsoleColor.Red;
            else if (asset.WarrantyExpirationDate.AddMonths(-6) < DateTime.Now)
                Console.ForegroundColor = ConsoleColor.Yellow;
            else
                Console.ResetColor();

            Console.WriteLine(line);
            Console.ResetColor();
        }
    }

    private static async Task EditAsset(AssetService assetService)
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

        Console.Write($"New Office ({asset.OfficeLocation}): ");
        string inputOffice = Console.ReadLine() ?? "";

        Console.Write($"New Local Price ({asset.LocalPrice}): ");
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

        if (!string.IsNullOrWhiteSpace(inputOffice))
            asset.OfficeLocation = inputOffice;

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

                asset.LocalPrice = price;
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

        await assetService.UpdateAsset(asset);

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
        Console.WriteLine($"Office: {asset.OfficeLocation}");
        Console.WriteLine($"Purchase Date: {asset.PurchaseDate:yyyy-MM-dd}");
        Console.WriteLine($"Warranty Expiration: {asset.WarrantyExpirationDate:yyyy-MM-dd}");
        Console.WriteLine($"Local Price: {asset.LocalPrice:N2}");
        Console.WriteLine($"Price USD: {asset.PurchasePriceUSD:N2}");
        Console.WriteLine($"Serial Number: {asset.SerialNumber ?? "N/A"}");
        Console.WriteLine($"Employee Username: {asset.EmployeeUsername ?? "N/A"}");
    }

    static void PrintMenu()
    {
        Console.Clear();

        Console.WriteLine("1. Add Asset");
        Console.WriteLine("2. Show all Assets");
        Console.WriteLine("3. Update Asset");
        Console.WriteLine("4. Delete Asset");
        Console.WriteLine("5. Search Asset");
        Console.WriteLine("6. Exit");
    }
    static void Pause()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }


}
