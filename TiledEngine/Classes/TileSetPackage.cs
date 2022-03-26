using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
        public Texture2D BackgroundSpriteSheet { get; private set; }

        public Dictionary<int, TmxTilesetTile> ForegroundDictionary { get; private set; }

        public Texture2D ForegroundSpriteSheet { get; private set; }

        public TileSetPackage(TmxMap tmxMap)
        {
            BackgroundDictionary = tmxMap.Tilesets[0].Tiles;
            if (tmxMap.Tilesets.Count > 0)
                ForegroundDictionary = tmxMap.Tilesets[1].Tiles;
        }

        public void LoadContent(ContentManager content, string exteriorTexturePath, string interiorTexturePath)
        {
            BackgroundSpriteSheet = content.Load<Texture2D>(exteriorTexturePath);
            ForegroundSpriteSheet = content.Load<Texture2D>(interiorTexturePath);

        }

        public TmxTilesetTile GetProperty(int gid)
        {
            if(GetDictionary(gid).ContainsKey(gid))
                return GetDictionary(gid)[gid];
            return null;
        }

        public Dictionary<int, TmxTilesetTile> GetDictionary(int gid)
        {
            if (gid < BackgroundDictionary.Count)
                return BackgroundDictionary;
            return ForegroundDictionary;

        }

        /// <summary>
        /// If Gid is greater than background dictionary it means the tile is from the fore ground
        /// </summary>
        /// <returns></returns>
        public Texture2D GetTexture(int gid)
        {
            if (gid < BackgroundDictionary.Count)
                return BackgroundSpriteSheet;
            return ForegroundSpriteSheet;
        }
    }
}
