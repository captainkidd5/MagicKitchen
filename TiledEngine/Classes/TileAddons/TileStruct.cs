using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiledEngine.Classes.TileAddons
{
    internal struct TileStruct
    {
        public short GID;

        public short X;
        public short Y;
        public byte Layer;

        public TileStruct(short gid,short x, short y, byte layer)
        {
            GID = gid;
            X = x;
            Y = y;
            Layer = layer;
        }
    }
}
