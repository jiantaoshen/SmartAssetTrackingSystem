using SmartAssetTrackingSystem.Data;
using SmartAssetTrackingSystem.Models;
using SmartAssetTrackingSystem.Repositories;
using SmartAssetTrackingSystem.Services;
using SmartAssetTrackingSystem.Helpers;
using System.Globalization;

public static class Program
{
    static async Task Main(string[] args)
    {
        MyDbContext context = new MyDbContext();

        var repo = new AssetRepository();
        var currencyService = new CurrencyService();

        var assetService = new AssetService(repo);


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
                    await ShowList(assetService);
                    Pause();
                    break;
                case "3":
                    Pause();
                    break;
                case "4":
                    Pause();
                    break;
                case "5":
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

    public static async Task InputData(AssetService assetService, CurrencyService currencyService)
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

    public static async Task ShowList(AssetService assetService)
    {
        Console.Clear();

        var assets = await assetService.GetAssets();

        string header =
            $"{"Office",-15}" +
            $"{"Type",-15}" +
            $"{"Brand",-15}" +
            $"{"Model",-15}" +
            $"{"Purchase Date",-15}" +
            $"{"Price (Local)",15}" +
            $"{"Currency",15}" +
            $"{"Price (Dollar)",15}";

        string title = "    Asset list    ";
        int totalWidth = header.Length;

        int left = (totalWidth - title.Length) / 2;

        string titleline =
            new string('=', left) +
            title +
            new string('=', totalWidth - left - title.Length);

        Console.WriteLine(titleline + "\n");
        Console.WriteLine("\n" + header);
        Console.WriteLine(new string('-', header.Length));

        foreach (var asset in assets)
        {
            string inputCurrency;
            if (asset.OfficeLocation == "USA")
            {
                inputCurrency = "USD";
            }
            else if (asset.OfficeLocation == "Sweden")
            {
                inputCurrency = "SEK";
            }
            else
            {
                inputCurrency = "EUR";
            }

            string line =
                $"{asset.OfficeLocation,-15}" +
                $"{asset.AssetType,-15}" +
                $"{asset.Brand,-15}" +
                $"{asset.ModelName,-15}" +
                $"{asset.PurchaseDate,-15:yyyy-MM-dd}" +
                $"{asset.LocalPrice, 15:N2}" +
                $"{inputCurrency ?? "N/A", 15}" +
                $"{asset.PurchasePriceUSD, 15:N2}";

            // Optional coloring logic (based on type or rules)
            if (asset.WarrantyExpirationDate.AddMonths(-3) < DateTime.Now)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (asset.WarrantyExpirationDate.AddMonths(-6) < DateTime.Now)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else
            {
                Console.ResetColor();
            }

            Console.WriteLine(line);
            Console.ResetColor();
        }

        Console.WriteLine("\n");
        Console.WriteLine(new string('=', header.Length));
    }


    static void PrintMenu()
    {
        Console.Clear();

        Console.WriteLine("Menu");
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
