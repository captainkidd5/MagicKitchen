using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public class TileLootData
    {
        public int TileId { get; set; }
        public List<LootData> Loot { get; set; }
    }
}
