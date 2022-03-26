using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace TiledEngine.Classes
{
    internal class TileSetPackage
    {
        public Dictionary<int, TmxTilesetTile> BackgroundDictionary { get; private set; }

        public Dictionary<int, TmxTilesetTile> ForegroundDictionary { get; private set; }


        public TileSetPackage(TmxMap tmxMap)
        {
            BackgroundDictionary = tmxMap.Tilesets[0].Tiles;
            if(tmxMap.Tilesets.Count > 0)
                ForegroundDictionary = tmxMap.Tilesets[1].Tiles;
        }

    }
}
