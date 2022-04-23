using DataModels;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace TiledEngine.Classes
{
    internal class TilesetPackageManager
    {

        public TileSetPackage ExteriorTileSetPackage { get; private set; }

        public TileSetPackage InteriorTileSetPackage { get; private set; }

        public TilesetPackageManager()
        {

        }

        public void LoadContent(ContentManager content, TmxMap exteriorMap, TmxMap interiorMap)
        {
            ExteriorTileSetPackage = new TileSetPackage(exteriorMap);
            ExteriorTileSetPackage.LoadContent(content, "maps/BackgroundMasterSpriteSheet_Spaced", "maps/ForegroundMasterSpriteSheet");

            InteriorTileSetPackage = new TileSetPackage(interiorMap);
            InteriorTileSetPackage.LoadContent(content, "maps/InteriorBackground_Spaced", "maps/InteriorForeground");
        }

        public TileSetPackage GetPackageFromMapType(MapType mapType)
        {
            if (mapType == MapType.Exterior)
                return ExteriorTileSetPackage;


            if (mapType == MapType.Interior)
                return InteriorTileSetPackage;

            throw new Exception($"No sprite sheet associated with map type {mapType.ToString()}");
        }
    }
}
