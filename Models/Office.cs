using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAssetTrackingSystem.Models
{
    public class Office
    {
        public int Id { get; set; }

        public string OfficeName { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string Currency { get; set; } = string.Empty;

        public ICollection<Asset> Assets { get; set; } = new List<Asset>();
    }
}
