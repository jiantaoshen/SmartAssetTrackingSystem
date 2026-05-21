using SmartAssetTrackingSystem.Helpers;
using SmartAssetTrackingSystem.Models;
using SmartAssetTrackingSystem.Repositories;
using SmartAssetTrackingSystem.Services;
using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;

namespace SmartAssetTrackingSystem.Reports
{
    public class ReportsGeneration
    {
        public static void GenerateOfficeReport(IEnumerable<Asset> assets, string officeName, decimal totalValue)
        {
            string header =
                $"{"Id",-8}" +
                $"{"Office",-10}" +
                $"{"Type",-10}" +
                $"{"Brand",-10}" +
                $"{"Model",-20}" +
                $"{"Purchase Date",-15}" +
                $"{"Price (Dollar)",15}" +
                $"{"Price (Local)",20}";

            string title = $"    {officeName}    ";
            int totalWidth = header.Length;

            int left = (totalWidth - title.Length) / 2;

            string titleline =
                new string('=', left) +
                title +
                new string('=', totalWidth - left - title.Length);

            Console.WriteLine(titleline + "\n");

            Console.WriteLine(header);
            Console.WriteLine(new string('-', header.Length));

            AssetHelper.PrintAssets(assets);

            Console.WriteLine($"\nTotal Office Value: {totalValue:F2} {assets.First().Office.Currency}");

            Console.WriteLine(new string('=', header.Length));
        }
    }
}
