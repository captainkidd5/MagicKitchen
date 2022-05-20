using DataModels.MapStuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Presets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;

namespace TiledEngine.Classes.TileAddons.FurnitureStuff
{
    internal class CraftingFurniture : StorableFurniture
    {
        private ProgressBarSprite _progressIndicator;

        protected bool IsBeingOperated { get; set; } = false;
        public CraftingFurniture(FurnitureData furnitureData, Tile tile, TileManager tileManager, IntermediateTmxShape intermediateTmxShape, string actionType)
            : base(furnitureData, tile, tileManager, intermediateTmxShape, actionType)
        {
            _progressIndicator = new ProgressBarSprite();
            SubKey = "CraftingFurniture";
        }
        public override void Load()
        {
            _progressIndicator.Load( .25f, Tile.Position, Tile.Layer);
            base.Load();

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsBeingOperated)
            {
                _progressIndicator.Update(gameTime);
            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (IsBeingOperated)
            {
                _progressIndicator.Draw(spriteBatch);
            }
        }
    }
}
