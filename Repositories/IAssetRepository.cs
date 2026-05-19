using SmartAssetTrackingSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAssetTrackingSystem.Repositories
{
    public interface IAssetRepository
    {
        void Add(Asset asset);

        List<Asset> GetAll();
    }
}
