﻿using DataModels.ItemStuff;
using DataModels.MapStuff;
using ItemEngine.Classes.CraftingStuff;
using ItemEngine.Classes.StorageStuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.ParticleStuff;
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

        public CraftingFurniture(FurnitureData furnitureData, TileObject tile, IntermediateTmxShape intermediateTmxShape, string actionType)
            : base(furnitureData, tile, intermediateTmxShape, actionType)
        {
            _progressIndicator = new ProgressBarSprite();
            SubKey = "CraftingFurniture";
        }

        public void Craft()
        {

        }
        
        protected override void CreateStorageContainer()
        {
            StorageContainer = new CraftingStorageContainer(CraftAction, TotalStorageCapacity, FurnitureData, CraftingDoneAction);
        }

        protected virtual void PlayFinishedSoundEffect()
        {
            PlayPackage("PressureRelease");

        }
        protected virtual void CraftingDoneAction()
        {
            PlayFinishedSoundEffect();
            ParticleManager.AddParticleEmitter(this, EmitterType.Smoke, customLayer: Tile.DrawLayer);
          //  ParticleManager.AddParticleEmitter(this, EmitterType.Text, amt.ToString());
        }
        public override void Load()
        {

            _progressIndicator.Load(Tile.Position, (byte)Tile.TileData.Layer, new Vector2(8, -16));
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
                _progressIndicator.Update(gameTime, Tile.Position);
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
