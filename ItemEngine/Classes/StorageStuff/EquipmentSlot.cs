using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataModels.Enums;

namespace ItemEngine.Classes.StorageStuff
{
    public class EquipmentSlot : StorageSlot
    {
        public EquipmentType AllowedEquipment { get; set; }
        public EquipmentSlot()
        {
        }
        /// <summary>
        /// Must match this slots equipment type to add
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public override bool Add(string itemName)
        {
            Item item = ItemFactory.GetItem(itemName);
            if (item != null)
                if (item.EquipmentSlot != AllowedEquipment)
                    return false;
            return base.Add(itemName);
        }
    }
}
