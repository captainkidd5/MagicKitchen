using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiledEngine.Classes.TilePlacementStuff
{
    internal class TilePlacementManager
    {
        private GhostTile _ghostTile;

        public TilePlacementManager()
        {

        }

        public void Update(GameTime gameTime)
        {
            _ghostTile.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _ghostTile.Draw(spriteBatch);
        }
    }
}
