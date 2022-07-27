using Globals.Classes.Helpers;
using InputEngine.Classes;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoundEngine.Classes;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;
using TiledEngine.Classes.TileAddons;
using TiledSharp;
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace TiledEngine.Classes.TilePlacementStuff
{
    internal class GhostTile
    {
        public TileObject CurrentTile { get; private set; }
        public int GID { get; set; } = -1;

        public int X { get; set; }
        public int Y { get; set; }
        private Layers _layer;
        private Sprite _sprite;
        private readonly TileManager _tileManager;

        private bool _mayPlace;
        private Vector2 _position;

        private Rectangle _obstructedArea;
        public GhostTile(TileManager tileManager)
        {
            _tileManager = tileManager;
        }

        internal bool DoesGIDMatch(int gid, bool isForegroundTileset)
        {
            if(isForegroundTileset)
            {
                return _tileManager.TileSetPackage.OffSetBackgroundGID(gid) == GID - 1;
            }
            return GID -1 == gid;
        }
        public void LoadNewTile(int gid, bool isForegroundTileSet, Item item, Layers layerToPlace)
        {
            GID = gid + 1;
            _layer = layerToPlace;
            if (isForegroundTileSet)
                GID = _tileManager.TileSetPackage.OffSetBackgroundGID(gid+ 1) ;
            if (CurrentTile != null)
                CurrentTile.Unload();
            TileData newTileData = new TileData((ushort)GID, 0, 0, _layer);
            CurrentTile = new TileObject(_tileManager, newTileData,false, true);
            //TileUtility.AssignProperties(_tileManager, CurrentTile,CurrentTile.TileData,true);


            _sprite = SpriteFactory.CreateWorldSprite(
                Vector2.Zero, CurrentTile.SourceRectangle,  _tileManager.TileSetPackage.GetTexture(GID),
                Color.White, customLayer: .99f);

            //Update once so that changing to valid placed item will immediately show if can place under cursor
            //without actually having to move cursor
            UpdateMayPlace(item);
        }

        public void ResetTileIfNotEmpty()
        {
            _sprite = null;
            GID = -1;
            if (CurrentTile != null)
                CurrentTile.Unload();
            CurrentTile = null;
        }
        /// <summary>
        /// Returns true if item was successfully placed
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="position"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Update(GameTime gameTime, Vector2 position, Item item)
        {

            if (GID >= 0)
            {
               // if (Controls.HasCursorTileIndexChanged)
               // {
                    UpdateMayPlace(item);
                //}
                _sprite.Update(gameTime, _position);

                if (Controls.IsClickedWorld)
                {
                    if (_mayPlace)
                    {

                        if (!Controls.ClickActionTriggeredThisFrame)
                        {
                            TileData? tileData = _tileManager.GetTileFromWorldPosition(
                                _tileManager.MouseOverTile.Position, _layer);

                            //_tileManager.TileObjects[tileData.Value.GetKey()].Unload();
                            SoundFactory.PlayEffectPackage(item.PlacementSound);
                            _tileManager.SwitchGID((ushort)(GID - 1), tileData.Value);
                            return true;
                        }
                    }

                }

            }
            return false;
        }

        private void UpdateMayPlace(Item placedItem)
        {
            _obstructedArea = CurrentTile.GetTotalHitBoxRectangle(_position);


            int ySubtractionAmt;
            if (_obstructedArea.Height < 1)
            {
                ySubtractionAmt = 0;

            }
            else
            {
                ySubtractionAmt = _sprite.Height - _obstructedArea.Height;

            }

            _position = new Vector2(_tileManager.MouseOverTile.Position.X,
                _tileManager.MouseOverTile.Position.Y - ySubtractionAmt - 16 + _tileManager.MouseOverTile.SourceRectangle.Height);

            Rectangle rect = RectangleHelper.RectFromPosition(
              new Vector2(_position.X, _position.Y + ySubtractionAmt), _obstructedArea.Width, _obstructedArea.Height);

                _mayPlace = CurrentTile.TileManager.TileLocationHelper.MayPlaceTile(placedItem,rect,_layer);

          
            if (_mayPlace)
                _sprite.UpdateColor(Color.Green);
            else
                _sprite.UpdateColor(Color.Red);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (GID >= 0)
                _sprite.Draw(spriteBatch);
        }
    }
}
