using InputEngine.Classes;
using ItemEngine.Classes;
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
        private readonly TileManager _tileManager;

        public TilePlacementManager(TileManager tileManager)
        {
            _ghostTile = new GhostTile(tileManager);
            _tileManager = tileManager;
        }

        public void Update(GameTime gameTime)
        {
            Item playeritem = UI.PlayerCurrentSelectedItem;
            if (playeritem != null && playeritem.PlaceableItem)
            {
                if (!_ghostTile.DoesGIDMatch(playeritem.PlacedItemGID, playeritem.PlacedItemIsForeground))
                {
                    _ghostTile.LoadNewTile(playeritem.PlacedItemGID, playeritem.PlacedItemIsForeground, playeritem);
                }
            }
            else
                _ghostTile.ResetTileIfNotEmpty();

           _ghostTile.Update(gameTime, Controls.MouseWorldPosition, playeritem);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _ghostTile.Draw(spriteBatch);
        }
    }
}
