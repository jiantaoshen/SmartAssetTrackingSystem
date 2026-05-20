using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAssetTrackingSystem.Models
{
    public abstract class Asset
    {
        public int Id { get; set; }

        public string AssetType { get; set; } = string.Empty;

        public string Brand { get; set; } = string.Empty;

        public string ModelName { get; set; } = string.Empty;

        public DateTime PurchaseDate { get; set; }

        public decimal PurchasePriceUSD { get; set; }

        public decimal LocalPrice { get; set; }

        public string OfficeLocation { get; set; } = string.Empty;

        public string? SerialNumber { get; set; }

        public string? EmployeeUsername { get; set; }

        public DateTime WarrantyExpirationDate { get; set; }
    }

    public class ComputerAsset : Asset
    {
        
    }

    public class MobileAsset : Asset
    {

    }

}
