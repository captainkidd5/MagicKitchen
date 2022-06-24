using DataModels;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.TilePlacementStuff.TilingStuff;
using TiledSharp;

namespace TiledEngine.Classes
{
    internal class TilesetPackageManager
    {

        public TileSetPackage ExteriorTileSetPackage { get; private set; }


        public TilesetPackageManager()
        {

        }

        public void LoadContent(ContentManager content, TmxMap exteriorMap)
        {
            ExteriorTileSetPackage = new TileSetPackage(exteriorMap);
            ExteriorTileSetPackage.LoadContent(content, "maps/BackgroundMasterSpriteSheet_Spaced", "maps/ForegroundMasterSpriteSheet");


        }


    }
}
