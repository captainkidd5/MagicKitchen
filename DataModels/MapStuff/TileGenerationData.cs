using System;
using System.Collections.Generic;
using System.Text;
using static DataModels.Enums;

namespace DataModels.MapStuff
{
    public class TileGenerationData
    {
        public ushort GID { get; set; }
        public Layers LayersToPlace { get; set; }
        public List<string> AllowedTilingSets { get; set; }

        //Only complete water tile, for example, none of its tiling
        public bool OnlyCentralTilingTile { get; set; }

        public ushort Tries { get; set; }
        public byte MinDistance { get; set; }
        public byte MaxDistance { get; set; }
        public byte MaxCluster { get; set; }
        public byte OddsAdditionalSpawn { get; set; }
    }
}
