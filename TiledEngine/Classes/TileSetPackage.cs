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

        private int _backgroundDimension;
        private int _backgroundTileCount;


        public Dictionary<int, TmxTilesetTile> ForegroundDictionary { get; private set; }

        public Texture2D ForegroundSpriteSheet { get; private set; }
        private int _foregroundDimension;
        private int _foregroundTileCount;


        public TileSetPackage(TmxMap tmxMap)
        {
            BackgroundDictionary = tmxMap.Tilesets[0].Tiles;
            _backgroundDimension = (int)tmxMap.Tilesets[0].Columns;
            _backgroundTileCount = (int)tmxMap.Tilesets[0].TileCount;

            if (tmxMap.Tilesets.Count > 1)
            {
                ForegroundDictionary = tmxMap.Tilesets[1].Tiles;
                _foregroundDimension = (int)tmxMap.Tilesets[1].Columns;
                _foregroundTileCount = (int)tmxMap.Tilesets[1].TileCount;

            }
        }

        public void LoadContent(ContentManager content, string exteriorTexturePath, string interiorTexturePath)
        {
            BackgroundSpriteSheet = content.Load<Texture2D>(exteriorTexturePath);
            ForegroundSpriteSheet = content.Load<Texture2D>(interiorTexturePath);

        }

        public int OffSetForegroundGID(int oldGID)
        {
            return oldGID - _backgroundTileCount;
        }
        public int GetDimension(int gid)
        {
            if (gid < _backgroundTileCount)
                return _backgroundDimension;
            return _foregroundDimension;
        }
        public bool ContainsKey(int gid)
        {
            if (gid > _backgroundTileCount)
                gid = OffSetForegroundGID(gid);

             return (GetProperty(gid) != null);

        }
        public TmxTilesetTile GetProperty(int gid)
        {
            int gidToCheck = gid;
            if (gid > _backgroundTileCount)
                gidToCheck = OffSetForegroundGID(gidToCheck);
            if (GetDictionary(gid).ContainsKey(gidToCheck))
                return GetDictionary(gid)[gidToCheck];
            return null;
        }

        public Dictionary<int, TmxTilesetTile> GetDictionary(int gid)
        {
            if (gid < _backgroundTileCount)
                return BackgroundDictionary;
            return ForegroundDictionary;

        }

        /// <summary>
        /// If Gid is greater than background dictionary it means the tile is from the fore ground
        /// </summary>
        /// <returns></returns>
        public Texture2D GetTexture(int gid)
        {
            if (gid < _backgroundTileCount)
                return BackgroundSpriteSheet;
            return ForegroundSpriteSheet;
        }
    }
}
