using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;

namespace TiledEngine.Classes.TilePlacementStuff
{
    internal class GhostTile
    {
        public int GID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        private Sprite _sprite;
        private readonly TileManager _tileManager;

        public GhostTile(TileManager tileManager)
        {
            _tileManager = tileManager;
        }

        public void LoadNewTile(int gid)
        {
           GID = gid;
            int tileSetDimension = _tileManager.TileSetPackage.GetDimension(GID);

            Rectangle sourceRectangle = TileUtility.GetTileSourceRectangle(
                GID, _tileManager.TileSetPackage, tileSetDimension);

            _sprite = SpriteFactory.CreateWorldSprite(Vector2.Zero, sourceRectangle, _tileManager.TileSetPackage.GetTexture(GID), Color.White);
        }

        public void Update(GameTime gameTime, Vector2 position)
        {
            _sprite.Update(gameTime, position);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _sprite.Draw(spriteBatch);
        }
    }
}
