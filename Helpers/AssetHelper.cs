using SmartAssetTrackingSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAssetTrackingSystem.Helpers
{
    public static class AssetHelper
    {
        // Assuming a standard warranty period of 3 years (36 months)
        public static DateTime GetWarrantyExpirationDate(DateTime purchaseDate, int warrantyPeriodMonths = 36)
        {
            return purchaseDate.AddMonths(warrantyPeriodMonths);
        }

        public static string GetColor(DateTime warrantyExpirationDate)
        {
            if (DateTime.Now >= warrantyExpirationDate.AddMonths(-3))
                return "RED";
            if (DateTime.Now >= warrantyExpirationDate.AddMonths(-6))
                return "YELLOW";
            return "NORMAL";
        }

        public static void PrintAssets(IEnumerable<Asset> typeAssets)
        {
            foreach (var asset in typeAssets)
            {
                string line =
                    $"{asset.Id,-8}" +
                    $"{asset.Office.Country,-10}" +
                    $"{asset.AssetType,-10}" +
                    $"{asset.Brand,-10}" +
                    $"{asset.ModelName,-20}" +
                    $"{asset.PurchaseDate,-15:yyyy-MM-dd}" +
                    $"{asset.PurchasePriceUSD,15:N2}" +
                    $"{asset.LocalPrice,15:N2}" +
                    $"{asset.Office.Currency,5}";

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
    }
}
