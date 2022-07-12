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

        private EquipmentSlot _helmetEquipmentSlot;
        private EquipmentSlot _torsoEquipmentSlot;
        private EquipmentSlot _legsEquipmentSlot;
        private EquipmentSlot _bootsEquipmentSlot;




        public EquipmentStorageContainer(int capacity) : base(capacity)
        {
          
        }

        protected override void AddSlots()
        {
            Slots = new List<StorageSlot>();
            _helmetEquipmentSlot = new EquipmentSlot(EquipmentType.Helmet);
            _torsoEquipmentSlot = new EquipmentSlot(EquipmentType.Torso);

            _legsEquipmentSlot = new EquipmentSlot(EquipmentType.Legs);
            _bootsEquipmentSlot = new EquipmentSlot(EquipmentType.Boots);
            Slots.Add(_helmetEquipmentSlot);
            Slots.Add(_torsoEquipmentSlot);
            Slots.Add(_legsEquipmentSlot);
            Slots.Add(_bootsEquipmentSlot);

            for (int i = 0; i < 4; i++)
            {
                Slots.Add(new EquipmentSlot(EquipmentType.Trinket));
            }

        }
    }
}
