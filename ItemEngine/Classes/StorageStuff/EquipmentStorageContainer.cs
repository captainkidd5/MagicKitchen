﻿using DataModels.ItemStuff;
using DataModels.MapStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataModels.Enums;

namespace ItemEngine.Classes.StorageStuff
{
    public class EquipmentStorageContainer : StorageContainer
    {

        public EquipmentSlot HelmetEquipmentSlot { get; private set; }
        public EquipmentSlot TorsoEquipmentSlot { get; private set; }

        public EquipmentSlot LegsEquipmentSlot { get; private set; }

        public EquipmentSlot BootsEquipmentSlot { get; private set; }




        public EquipmentStorageContainer(int capacity) : base(capacity)
        {
          
        }
        public void ReduceDurabilityOnEquippedArmor(int? amt = null)
        {
            foreach (StorageSlot slot in Slots)
            {
                if (!slot.Empty)
                {
                    if (slot.Item.RemoveDurability(amt ?? slot.Item.ArmorValue))
                    {
                        slot.Remove(slot.StoredCount);
                    }

                }
            }
        }
        public int GetArmorValue()
        {

            return (int)GetStatValue(StatType.Armor);
        }
        public int GetMaxLumens()
        {

            return (int)GetStatValue(StatType.MaxLumens);
        }
        public float GetStatValue(StatType statType)
        {
            float totalValue = 0;
            foreach (StorageSlot slot in Slots)
            {
                if (!slot.Empty)
                    totalValue += slot.Item.StatTypeToValue(statType);
            }
            return totalValue;
        }
        protected override void AddSlots()
        {
            Slots = new List<StorageSlot>();
            HelmetEquipmentSlot = new EquipmentSlot(EquipmentType.Helmet);
            TorsoEquipmentSlot = new EquipmentSlot(EquipmentType.Torso);

            LegsEquipmentSlot = new EquipmentSlot(EquipmentType.Legs);
            BootsEquipmentSlot = new EquipmentSlot(EquipmentType.Boots);
            Slots.Add(HelmetEquipmentSlot);
            Slots.Add(new EquipmentSlot(EquipmentType.Trinket));

            Slots.Add(TorsoEquipmentSlot);
            Slots.Add(new EquipmentSlot(EquipmentType.Trinket));

            Slots.Add(LegsEquipmentSlot);
            Slots.Add(new EquipmentSlot(EquipmentType.Trinket));

            Slots.Add(BootsEquipmentSlot);
            Slots.Add(new EquipmentSlot(EquipmentType.Trinket));


      

        }
    }
}
