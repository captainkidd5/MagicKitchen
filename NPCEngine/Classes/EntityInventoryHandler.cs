using ItemEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityEngine.Classes
{
    internal class EntityInventoryHandler
    {
        public StorageContainer StorageContainer { get; private set; }

        public EntityInventoryHandler(int capacity)
        {
            StorageContainer = new StorageContainer(capacity);
        }
        public virtual int GiveItem(Item item, int amountToGive)
        {
            if (item.Unique)
            {
                for (int i = 0; i < amountToGive; i++)
                {
                    if (!StorageContainer.FillUniqueItem(item))
                        return amountToGive - i;
                }
                //Entity's inventory could hold all of given amount.
                return 0;
            }
            else
                return StorageContainer.FillStackableItem(item.Id, amountToGive);
        }

        public virtual int GiveItem(int itemId, int amountToGive)
        {
            return StorageContainer.FillStackableItem(itemId, amountToGive);
        }

        public virtual Item TakeItem(int itemId, int amountToTake, bool dropInFrontOfEntity = false)
        {
            Item item = StorageContainer.RemoveStackableItem(itemId, amountToTake);

            //if (dropInFrontOfEntity)
            //    item.Drop(Position);

            return item;
        }
    }
}
