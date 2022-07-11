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
        public EquipmentStorageContainer(int capacity) : base(capacity)
        {
            _helmetEquipmentSlot = new EquipmentSlot(EquipmentType.Helmet);
            _torsoEquipmentSlot = new EquipmentSlot(EquipmentType.Torso);
        }
    }
}
