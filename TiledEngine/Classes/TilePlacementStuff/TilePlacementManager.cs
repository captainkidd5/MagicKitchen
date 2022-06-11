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
                bool foreGroundSpriteSheet = playeritem.LayerToPlace != null && playeritem.LayerToPlace == DataModels.Enums.Layers.foreground;
                if (!_ghostTile.DoesGIDMatch(playeritem.PlacedItemGID, foreGroundSpriteSheet))
                {
                    _ghostTile.LoadNewTile(playeritem.PlacedItemGID, foreGroundSpriteSheet, playeritem, playeritem.LayerToPlace.Value);
                }
            }
            else
                _ghostTile.ResetTileIfNotEmpty();

           if(_ghostTile.Update(gameTime, Controls.MouseWorldPosition, playeritem))
            {
                UI.RemoveCurrentlySelectedItem(1);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _ghostTile.Draw(spriteBatch);
        }
    }
}
