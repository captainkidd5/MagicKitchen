using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.ItemStuff
{
    public class FlotsamData : IWeightable
    {
        public string Name { get; set; }
        public int Weight { get; set; }
    }
}
