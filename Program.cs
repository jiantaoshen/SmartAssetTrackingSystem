using SmartAssetTrackingSystem.Data;
using SmartAssetTrackingSystem.Models;
using SmartAssetTrackingSystem.Repositories;
using SmartAssetTrackingSystem.Services;
using SmartAssetTrackingSystem.Helpers;
using System.Collections;
using System.Globalization;

public static class Program
{
    static async Task Main(string[] args)
    {
        MyDbContext context = new MyDbContext();

        var repo = new AssetRepository();
        var currencyService = new CurrencyService();

        var addAsset = new AssetService(repo);

        await Run(addAsset, currencyService);
    }

    public static async Task Run(AssetService addAsset, CurrencyService currencyService)
    {
        var rates = await currencyService.GetRatesAsync();

        while (true)
        {
            Console.WriteLine("To enter a new product - follow the steps | To quit - enter: \"Q\" ");

            Console.Write("Enter OfficeLocation (1: USA 2: Sweden 3: Germany): ");

            string inputOffice = Console.ReadLine() ?? "";
            int officeNumber;

            if (inputOffice.Trim().ToLower() == "q") break;

            while (true)
            {
                if (int.TryParse(inputOffice, out officeNumber) && (officeNumber == 1 || officeNumber == 2 || officeNumber == 3))
                    break;

                Console.WriteLine("Invalid input. Try again.");

                Console.Write("Enter OfficeLocation (1: USA 2: Sweden 3: Germany): ");

                inputOffice = Console.ReadLine() ?? "";
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
                addAsset.AddAsset(asset);
                Console.WriteLine("Computer Asset added successfully!\n");
            }
            else
            {
                addAsset.AddAsset(asset);
                Console.WriteLine("Mobile Asset added successfully!\n");
            }
        }
    }


    /*public static async Task ShowList(ListAssets listAssets)
    {
        Console.Clear();

        var lines = await listAssets.ExecuteAsync();

        string header =
            $"{"Office",-15}" +
            $"{"Type",-15}" +
            $"{"Brand",-15}" +
            $"{"Model",-15}" +
            $"{"Purchase Date",-15}" +
            $"{"Price (Local)",-15}" +
            $"{"Currency",-15}" +
            $"{"Price (Dollar)",-15}";

        Console.WriteLine("\n" + header);
        Console.WriteLine(new string('-', header.Length));

        foreach (var line in lines)
        {
            if (line.StartsWith("[RED]"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(line.Replace("[RED] ", ""));
            }
            else if (line.StartsWith("[YELLOW]"))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(line.Replace("[YELLOW] ", ""));
            }
            else
            {
                Console.ResetColor();
                Console.WriteLine(line);
            }
        }

        Console.ResetColor();
    }
    */
}
