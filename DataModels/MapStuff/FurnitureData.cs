using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.MapStuff
{
    public enum FurnitureType
    {
        None = 0,
        Furniture = 1,
        StorableFurniture = 2,
        DiningTable = 3,

    }

    public class FurnitureData
    {

        public FurnitureType FurnitureType { get; set; }

        //Only fill these in for Furniture type of storage
        public int StorageRows { get; set; }
        public int StorageColumns { get; set; }


    }
}
