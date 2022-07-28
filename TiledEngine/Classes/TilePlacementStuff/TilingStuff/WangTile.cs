using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiledEngine.Classes.TilePlacementStuff.TilingStuff
{
    internal class WangTile : IWeightable
    {
        public byte Weight { get; set; } = 100;

        public int GID { get; set; }

        public WangTile(byte weight, int gID)
        {
            Weight = weight;
            GID = gID;
        }
    }
}
