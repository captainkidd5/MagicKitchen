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

        //For example a dining room table with configuration
        //
        //-v-
        //vxv
        //-v-
        //
        //Would have storage indices of 0,1,3,4 where the middle isn't visible. This is of course up to
        //player discretion and only affects graphics
        public List<VisibleStorageIndex> VisibleStorageIndicies { get; set; }


    }

    public class VisibleStorageIndex
    {
        public int Index { get; set; }
        public int XOffSet { get; set; }
        public int YOffSet { get; set; }
    }
}
