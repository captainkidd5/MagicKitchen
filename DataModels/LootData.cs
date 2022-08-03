using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public class LootData : IWeightable
    {
        public string ItemName { get; set; }
        public bool Guaranteed { get; set; }
        public int QuantityMin { get; set; }
        public int QuantityMax { get; set; }

        public byte Weight { get; set; }
    }
}
