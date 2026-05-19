using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAssetTrackingSystem.Helpers
{
    public static class AssetHelper
    {
        public static decimal ConvertUsdToLocalCurrency(decimal usdAmount, decimal exchangeRate)
        {
            return usdAmount * exchangeRate;
        }

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
    }
}
