using DataModels;
using SoundEngine.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataModels.Enums;

namespace ItemEngine.Classes.StorageStuff
{
    public delegate void EquipmentChanged(BodyParts? bodyParts, EquipmentType equipmentType, int yIndex);
    public class EquipmentSlot : StorageSlot
    {
        public event EquipmentChanged EquipmentChanged;
        public EquipmentType AllowedEquipmentType { get; set; }
        public EquipmentSlot(EquipmentType allowedEquipmentType)
        {
            AllowedEquipmentType = allowedEquipmentType;
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
                if (item.EquipmentSlot != AllowedEquipmentType)
                    return false;
            return base.Add(itemName);
        }
        protected override bool MayPlaceItem(ushort itemIdToTryToPlace)
        {
            Item item = ItemFactory.GetItem(itemIdToTryToPlace);
            if (item != null)
                if (item.EquipmentSlot != AllowedEquipmentType)
                    return false;
            return base.MayPlaceItem(itemIdToTryToPlace);
        }
        protected override void OnItemChanged()
        {
            if(Item == null)
            {
                EquipmentChanged?.Invoke(Enums.EquipmentTypeToBodyPart(AllowedEquipmentType), EquipmentType.None, 0);

            }
            else
            {
                EquipmentChanged?.Invoke(Enums.EquipmentTypeToBodyPart(AllowedEquipmentType), AllowedEquipmentType, Item.EquipmentYIndex);
                SoundFactory.PlayEffectPackage("EquipGear");
            }

            base.OnItemChanged();
        }

  
    }
}
