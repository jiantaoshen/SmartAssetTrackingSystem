using Microsoft.EntityFrameworkCore;
using SmartAssetTrackingSystem.Data;
using SmartAssetTrackingSystem.Helpers;
using SmartAssetTrackingSystem.Models;
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
        var officeRepo = new OfficeRepository();
        var currencyService = new CurrencyService();
        var assetService = new AssetService(repo, officeRepo, currencyService, reportRepo);

        //Add Seed Data
        var seeder = new TestDataSeeder(context, currencyService);
        seeder.SeedDataAsync().Wait();

        Office office;

        while (true)
        {
            PrintHelper.PrintMenu();
            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    assetService.AddAsset().Wait();
                    break;
                case "2":
                    assetService.GetAssets().Wait();
                    PrintHelper.Pause();
                    break;
                case "3":
                    assetService.GetAssets().Wait();
                    assetService.UpdateAsset().Wait();
                    PrintHelper.Pause();
                    break;
                case "4":
                    assetService.GetAssets().Wait();
                    assetService.RemoveAsset().Wait();
                    PrintHelper.Pause();
                    break;
                case "5":
                    assetService.GetAssetById().Wait(); 
                    PrintHelper.Pause();
                    break;
                case "6":
                    assetService.GetAssetsByOffice().Wait();
                    PrintHelper.Pause();
                    break;
                 case "7":
                    assetService.GetFullReport().Wait();
                    PrintHelper.Pause();
                    break;
                 case "8":
                    Console.WriteLine("Exiting...");
                    return; 
                default:
                    Console.WriteLine("Invalid choice!");
                    PrintHelper.Pause();
                    break;
            }
        }
    }
}
