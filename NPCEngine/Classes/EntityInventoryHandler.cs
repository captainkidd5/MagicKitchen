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
        private StorageContainer _storageContainer;

        public EntityInventoryHandler(int capacity)
        {
            _storageContainer = new StorageContainer(capacity);
        }
        public virtual int GiveItem(Item item, int amountToGive)
        {
            if (item.Unique)
            {
                for (int i = 0; i < amountToGive; i++)
                {
                    if (!_storageContainer.FillUniqueItem(item))
                        return amountToGive - i;
                }
                //Entity's inventory could hold all of given amount.
                return 0;
            }
            else
                return _storageContainer.FillStackableItem(item.Id, amountToGive);
        }

        public virtual int GiveItem(int itemId, int amountToGive)
        {
            return _storageContainer.FillStackableItem(itemId, amountToGive);
        }

        public virtual Item TakeItem(int itemId, int amountToTake, bool dropInFrontOfEntity = false)
        {
            Item item = _storageContainer.RemoveStackableItem(itemId, amountToTake);

            //if (dropInFrontOfEntity)
            //    item.Drop(Position);

            return item;
        }
    }
}
