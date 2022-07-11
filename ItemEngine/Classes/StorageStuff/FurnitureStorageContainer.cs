using DataModels.MapStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemEngine.Classes.StorageStuff
{
    public class FurnitureStorageContainer : StorageContainer
    {
        public FurnitureData? FurnitureData { get; private set; }

        public FurnitureStorageContainer(int capacity, FurnitureData furnitureData = null) : base(capacity)
        {
            FurnitureData = furnitureData;

        }
    }
}
