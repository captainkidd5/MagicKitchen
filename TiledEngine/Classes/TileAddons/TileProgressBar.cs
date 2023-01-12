using DataModels;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes.Presets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataModels.Enums;

namespace TiledEngine.Classes.TileAddons
{
    internal class TileProgressBar : ITileAddon
    {
        private ProgressBarSprite _progressBar;
        private Vector2 _offSet => new Vector2(Tile.SourceRectangle.Width / 2, Tile.SourceRectangle.Height / 2 * -1);
        public TileObject Tile { get; set; }

        public TileProgressBar(TileObject tile)
        {
            Tile = tile;
        }

        public void Load()
        {
            _progressBar = new ProgressBarSprite();
            _progressBar.Load(Tile.Position + _offSet, .99f, color: Color.White);
        }
        public void ManualSetCurrentAmountAndUpdate(float currentAmt, float totalAmt) => 
            _progressBar.ManualSetCurrentAmountAndUpdate(currentAmt, totalAmt);
       

        public void Update(GameTime gameTime)
        {
            _progressBar.Update(gameTime, Tile.Position + new Vector2(_offSet.X - _progressBar.Width / 2, _offSet.Y));
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //No need to draw progress bar if tile is charged
            if (!_progressBar.FullyCharged)
                _progressBar.Draw(spriteBatch);
        }

        public Action Interact(ref ActionType? actionType, bool isPlayer, Item heldItem, Vector2 entityPosition, Direction directionEntityFacing)

        {
            throw new NotImplementedException();
        }

        public void SetToDefault()
        {
            //throw new NotImplementedException();
        }
    }
}
