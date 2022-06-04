using InputEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Classes;

namespace TiledEngine.Classes.TilePlacementStuff
{
    internal class TilePlacementManager
    {
        private GhostTile _ghostTile;

        public TilePlacementManager(TileManager tileManager)
        {
            _ghostTile = new GhostTile(tileManager);
        }

        public void Update(GameTime gameTime)
        {
            if (UI.PlayerCurrentSelectedItem != null && UI.PlayerCurrentSelectedItem.PlaceableItem)
            {
                if (_ghostTile.GID != UI.PlayerCurrentSelectedItem.PlacedItemGID)
                {
                    _ghostTile.LoadNewTile(UI.PlayerCurrentSelectedItem.PlacedItemGID, UI.PlayerCurrentSelectedItem.PlacedItemIsForeground);
                }
            }
            else
                _ghostTile.ResetTileIfNotEmpty();

           _ghostTile.Update(gameTime, Controls.MouseWorldPosition);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _ghostTile.Draw(spriteBatch);
        }
    }
}
