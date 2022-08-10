using System;
using System.Collections.Generic;
using System.Text;
using static DataModels.Enums;

namespace DataModels.MapStuff
{
    public class PoissonData
    {
        public ushort GID { get; set; }
        public Layers LayersToPlace { get; set; }
        public byte Tries { get; set; }
        public byte MinDistance { get; set; }
        public byte MaxDistance { get; set; }
    }
}
