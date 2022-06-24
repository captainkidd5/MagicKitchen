using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.TilePlacementStuff.TilingStuff;
using TiledSharp;

namespace TiledEngine.Classes
{
    public class TileSetPackage
    {
        private Dictionary<int, TmxTilesetTile> _backgroundDictionary;
        public Texture2D BackgroundSpriteSheet { get; private set; }

        private int _backgroundDimension;
        private int _backgroundTileCount;


        private Dictionary<int, TmxTilesetTile> _foregroundDictionary;

        public Texture2D ForegroundSpriteSheet { get; private set; }
        private int _foregroundDimension;
        private int _foregroundTileCount;
        internal WangManager TilingSetManager { get; private set; }


        public TileSetPackage(TmxMap tmxMap)
        {
            var _backGroundSet = tmxMap.Tilesets.FirstOrDefault(x => x.Name.Contains("Back"));
            _backgroundDictionary = _backGroundSet.Tiles;
            _backgroundDimension = (int)_backGroundSet.Columns; 

            _backgroundTileCount = (int)_backGroundSet.TileCount;

            if (tmxMap.Tilesets.Count > 1)
            {
                var foreGroundSet = tmxMap.Tilesets.FirstOrDefault(x => x.Name.Contains("Fore"));
                _foregroundDictionary = foreGroundSet.Tiles;
                _foregroundDimension = (int)foreGroundSet.Columns;
                _foregroundTileCount = (int)foreGroundSet.TileCount;

            }

            TilingSetManager = new WangManager();
        }

        public void LoadContent(ContentManager content, string exteriorTexturePath, string interiorTexturePath)
        {
            BackgroundSpriteSheet = content.Load<Texture2D>(exteriorTexturePath);
            ForegroundSpriteSheet = content.Load<Texture2D>(interiorTexturePath);

            for(int i =0; i< _backgroundTileCount; i++)
            {
                TileUtility.PreliminaryData(this, i);
            }

            for (int i = 0; i < _foregroundTileCount; i++)
            {
                TileUtility.PreliminaryData(this, i);
            }
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
            TmxTilesetTile t;
             GetDictionary(gid).TryGetValue(gidToCheck, out t);
            return t;
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
