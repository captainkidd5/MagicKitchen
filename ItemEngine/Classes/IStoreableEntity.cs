using System;
using System.Collections.Generic;
using System.Text;

namespace ItemEngine.Classes
{
    public interface IStoreableEntity
    {
         StorageContainer StorageContainer { get; set; }
        int GiveItem(Item item, int amountToGive);

        int GiveItem(int itemId, int amountToGive);

        Item TakeItem(int itemId, int amountToTake, bool dropInFrontOfEntity = false);
    }
}
