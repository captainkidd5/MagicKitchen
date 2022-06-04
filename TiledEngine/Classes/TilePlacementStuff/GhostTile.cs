using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;
using TiledSharp;

namespace TiledEngine.Classes.TilePlacementStuff
{
    internal class GhostTile
    {
        public int GID { get; set; } = -1;

        public bool ContainsItem => GID >= 0;
        public int X { get; set; }
        public int Y { get; set; }

        private Sprite _sprite;
        private readonly TileManager _tileManager;


        public GhostTile(TileManager tileManager)
        {
            _tileManager = tileManager;
        }

        public void LoadNewTile(int gid, bool isForeGround)
        {
           GID = gid;
            int gidForSprite = GID;
            if(isForeGround)
                gidForSprite = _tileManager.TileSetPackage.OffSetBackgroundGID(gidForSprite);
            int tileSetDimension = _tileManager.TileSetPackage.GetDimension(gidForSprite);

            Rectangle sourceRectangle = TileUtility.GetTileSourceRectangle(
                gidForSprite, _tileManager.TileSetPackage, tileSetDimension);

            TmxTilesetTile tmxTileSetTile= _tileManager.TileSetPackage.GetTmxTileSetTile(gidForSprite);
            if (tmxTileSetTile.Properties.ContainsKey("newSource"))
            {
                Rectangle propertySourceRectangle = TileObjectHelper.GetSourceRectangleFromTileProperty(tmxTileSetTile.Properties["newSource"]);

                sourceRectangle = TileRectangleHelper.AdjustSourceRectangle(sourceRectangle, propertySourceRectangle);
            }
            _sprite = SpriteFactory.CreateWorldSprite(
                Vector2.Zero, sourceRectangle, _tileManager.TileSetPackage.ForegroundSpriteSheet,
                Color.White,customLayer: .99f);
        }

        public void ResetTileIfNotEmpty()
        {
            _sprite = null;
            GID = -1;
        }
        public void Update(GameTime gameTime, Vector2 position)
        {
            if(_sprite != null)
            _sprite.Update(gameTime,new Vector2( _tileManager.MouseOverTile.Position.X ,
                _tileManager.MouseOverTile.Position.Y - _sprite.Height / 2));

            if(ContainsItem)
            {

            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_sprite != null)
                _sprite.Draw(spriteBatch);
        }
    }
}
