using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiledEngine.Classes.TileAddons
{
    public abstract class LocateableTileAddon : ITileAddon
    {
        private readonly TileLocator _tileLocator;
        protected string Key;
        protected string SubKey;

        public Tile Tile { get; set; }

        public LocateableTileAddon(TileLocator tileLocator, Tile tile)
        {
            _tileLocator = tileLocator;
            Tile = tile;
        }

        public virtual void Load()
        {
            _tileLocator.AddItem(Key, SubKey, Tile);
        }
        public void CleanUp()
        {
            _tileLocator.RemoveItem(Key, SubKey, Tile);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //throw new NotImplementedException();
        }

        public void Interact(bool isPlayer)
        {
            throw new NotImplementedException();
        }

       

        public void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }
    }
}
