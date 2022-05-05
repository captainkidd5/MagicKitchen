using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.MapStuff
{
    public enum FurnitureType
    {
        None = 0,
        Storage = 1,
        DiningTable = 2,

    }

    public class FurnitureData
    {

        public FurnitureType FurnitureType { get; set; }

        //Only fill these in for Furniture type of storage
        public int StorageRows { get; set; }
        public int StorageColumns { get; set; }


    }
}
