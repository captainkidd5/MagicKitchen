using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemEngine.Classes.StorageStuff
{
    public class FuelStorageSlot : StorageSlot
    {
        public FuelStorageSlot()
        {
          //  AddBlackListedItem("Mana");
        }

        protected override bool MayPlaceItem(ushort itemIdToTryToPlace)
        {
            if (ItemFactory.GetItemData(itemIdToTryToPlace).FuelValue <= 0)
                return false;

            return base.MayPlaceItem(itemIdToTryToPlace);
        }
    }
}
