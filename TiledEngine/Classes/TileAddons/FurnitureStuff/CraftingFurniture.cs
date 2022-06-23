using DataModels.ItemStuff;
using DataModels.MapStuff;
using ItemEngine.Classes.CraftingStuff;
using ItemEngine.Classes.StorageStuff;
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

        public CraftAction CraftAction{ get; set; }

        public CraftingFurniture(FurnitureData furnitureData, TileObject tile, TileManager tileManager, IntermediateTmxShape intermediateTmxShape, string actionType)
            : base(furnitureData, tile, tileManager, intermediateTmxShape, actionType)
        {
            _progressIndicator = new ProgressBarSprite();
            SubKey = "CraftingFurniture";
        }

        public void Craft()
        {

        }

        protected override void CreateStorageContainer()
        {
            StorageContainer = new CraftingStorageContainer(CraftAction, TotalStorageCapacity, FurnitureData);

        }
        public override void Load()
        {

            _progressIndicator.Load(Tile.Position, Tile.TileData.Layer, new Vector2(8, -16));
            base.Load();

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

           // if (IsBeingOperated)
          //  {
                CraftingStorageContainer container = StorageContainer as CraftingStorageContainer;
                if (!container.FuelMetre.Empty)
                {

                }
                else if(container.FuelMetre.Empty && container.ContainsFuelItem)
                {
                    container.TransferItemIntoFuel();
                }
                container.CraftedItemMetre.Update();
                _progressIndicator.Update(gameTime);
           // }

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
