using DataModels.MapStuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;

namespace TiledEngine.Classes.TileAddons.FurnitureStuff
{
    internal class KitchenAppliance : Furniture
    {
        private ProgressIndicator _progressIndicator;

        protected bool IsBeingOperated { get; set; } = true;
        public KitchenAppliance(FurnitureData furnitureData, Tile tile, TileManager tileManager, IntermediateTmxShape intermediateTmxShape, string actionType)
            : base(furnitureData, tile, tileManager, intermediateTmxShape, actionType)
        {
            _progressIndicator = new ProgressIndicator();
            SubKey = "KitchenAppliance";
        }
        public override void Load()
        {
            _progressIndicator.Load(.25f, Tile.Position, Tile.Layer);
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
