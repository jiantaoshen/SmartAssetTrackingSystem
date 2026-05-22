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

        // Determine color based on how close the warranty expiration date is
        public static ConsoleColor GetColor(DateTime warrantyExpirationDate)
        {
            if (DateTime.Now >= warrantyExpirationDate.AddMonths(-3))
                return ConsoleColor.Red;
            if (DateTime.Now >= warrantyExpirationDate.AddMonths(-6))
                return ConsoleColor.Yellow;
            return ConsoleColor.White;
        }

        // Print assets in a tabular format with color coding for warranty status
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

                Console.ForegroundColor = GetColor(asset.WarrantyExpirationDate);
                Console.WriteLine(line);
                Console.ResetColor();
            }

            Console.WriteLine();
        }
    }
}
