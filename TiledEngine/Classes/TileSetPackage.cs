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
        private Dictionary<int, TmxTilesetTile> _backgroundDictionary;
        public Texture2D BackgroundSpriteSheet { get; private set; }

        private int _backgroundDimension;
        private int _backgroundTileCount;


        private Dictionary<int, TmxTilesetTile> _foregroundDictionary;

        public Texture2D ForegroundSpriteSheet { get; private set; }
        private int _foregroundDimension;
        private int _foregroundTileCount;


        public TileSetPackage(TmxMap tmxMap)
        {
            _backgroundDictionary = tmxMap.Tilesets[0].Tiles;
            _backgroundDimension = (int)tmxMap.Tilesets[0].Columns;
            _backgroundTileCount = (int)tmxMap.Tilesets[0].TileCount;

            if (tmxMap.Tilesets.Count > 1)
            {
                _foregroundDictionary = tmxMap.Tilesets[1].Tiles;
                _foregroundDimension = (int)tmxMap.Tilesets[1].Columns;
                _foregroundTileCount = (int)tmxMap.Tilesets[1].TileCount;

            }
        }

        public void LoadContent(ContentManager content, string exteriorTexturePath, string interiorTexturePath)
        {
            BackgroundSpriteSheet = content.Load<Texture2D>(exteriorTexturePath);
            ForegroundSpriteSheet = content.Load<Texture2D>(interiorTexturePath);

        }
        public bool IsForeground(int tileGID)
        {
            return tileGID > _backgroundTileCount;
        }
        public int OffSetBackgroundGID(int oldGID)
        {
            return oldGID + _backgroundTileCount;
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
             return (GetTmxTileSetTile(gid) != null);

        }
        public TmxTilesetTile GetTmxTileSetTile(int gid)
        {
            int gidToCheck = gid;
            if (gid > _backgroundTileCount)
                gidToCheck = OffSetForegroundGID(gidToCheck);
            if (GetDictionary(gid).ContainsKey(gidToCheck))
                return GetDictionary(gid)[gidToCheck];
            return null;
        }

        private Dictionary<int, TmxTilesetTile> GetDictionary(int gid)
        {
            if (gid < _backgroundTileCount)
                return _backgroundDictionary;
            return _foregroundDictionary;

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
