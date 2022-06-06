using Globals.Classes.Helpers;
using InputEngine.Classes;
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
using static Globals.Classes.Settings;

namespace TiledEngine.Classes.TilePlacementStuff
{
    internal class GhostTile
    {
        public Tile CurrentTile { get; private set; }
        public int GID { get; set; } = -1;

        public int X { get; set; }
        public int Y { get; set; }

        private Sprite _sprite;
        private readonly TileManager _tileManager;

        private bool _mayPlace;
        private Vector2 _position;

        private Rectangle _obstructedArea;
        public GhostTile(TileManager tileManager)
        {
            _tileManager = tileManager;
        }

        public void LoadNewTile(int gid, bool isForeGround)
        {
            GID = gid;
            int gidForSprite = GID;
            if (isForeGround)
                gidForSprite = _tileManager.TileSetPackage.OffSetBackgroundGID(gidForSprite) + 1;


            CurrentTile = new Tile(_tileManager, gidForSprite, Layers.foreground, .99f, 0, 0);
            TileUtility.AssignProperties(CurrentTile, Layers.foreground);


            _sprite = SpriteFactory.CreateWorldSprite(
                Vector2.Zero, CurrentTile.SourceRectangle, _tileManager.TileSetPackage.ForegroundSpriteSheet,
                Color.White, customLayer: .99f);

            //_obstructedArea = 
        }

        public void ResetTileIfNotEmpty()
        {
            _sprite = null;
            GID = -1;
            if (CurrentTile != null)
                CurrentTile.Unload();
            CurrentTile = null;
        }
        public void Update(GameTime gameTime, Vector2 position)
        {



            //todo: check if area is clear
            if (GID >= 0)
            {
                if (Controls.HasCursorTileIndexChanged)
                {
                    _obstructedArea = CurrentTile.GetTotalHitBoxRectangle();
                    _obstructedArea = new Rectangle((int)_position.X,
                           (int)_position.Y, _obstructedArea.Width, _obstructedArea.Height);
                    _position = new Vector2(_tileManager.MouseOverTile.Position.X,
                        _tileManager.MouseOverTile.Position.Y - _sprite.Height + _obstructedArea.Height);
                    _sprite.Update(gameTime, _position);



                    _mayPlace = TileLocationHelper.MayPlaceTile(_tileManager.PathGrid, RectangleHelper.RectangleFromPosition(
                        new Vector2(_position.X, _position.Y + _sprite.Height - _obstructedArea.Height), _obstructedArea.Width, _obstructedArea.Height));
                    if (_mayPlace)
                        _sprite.UpdateColor(Color.Green);
                    else
                        _sprite.UpdateColor(Color.Red);
                }

                if (Controls.IsClickedWorld)
                {
                    if (_mayPlace)
                    {

                        if (!Controls.ClickActionTriggeredThisFrame)
                        {
                            Tile tile = _tileManager.GetTileFromWorldPosition(_tileManager.MouseOverTile.Position, Layers.foreground);
                            TileUtility.SwitchGid(tile, Layers.foreground, _tileManager.TileSetPackage.OffSetBackgroundGID(GID));
                        }
                    }

                }

            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (GID >= 0)
                _sprite.Draw(spriteBatch);
        }
    }
}
