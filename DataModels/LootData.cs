using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public class LootData : IWeightable
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public int Weight { get; set; }
    }
}
