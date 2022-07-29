using Globals.Classes.Time;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
using SpriteEngine.Classes.ShadowStuff;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;
using TiledEngine.Classes.Helpers;


namespace TiledEngine.Classes.TileAddons.LightStuff
{
    internal class LightBody : TileBody
    {
        private Point _pointOffset;
        private float _lightRadius;
        private bool _immuneToDrain;
        private LightType _lightType;
        private LightCollidable _light;

        public LightBody(TileObject tile, IntermediateTmxShape intermediateTmxShape,
            string lightPropertyString) : base(tile, intermediateTmxShape)
        {

            _pointOffset = GetOffSet(lightPropertyString);
            _lightRadius = GetRadius(lightPropertyString);
            _lightType = GetLightType(lightPropertyString);
            _immuneToDrain = GetIfImmune(lightPropertyString);
        }
        public override void Load()
        {
            CreateBody(Tile.Position + new Vector2(_pointOffset.X, _pointOffset.Y));
            _light = AddLight(_lightType, new Vector2(_pointOffset.X, _pointOffset.Y),_immuneToDrain, true, _lightRadius);
            TileLightDataDTO? tileLightDto = Tile.TileManager.TileLightManager.GetLightFromTile(Tile);
            if (tileLightDto != null)
            {
                _light.SetCurrentLumens(tileLightDto.Value.CurrentCharge);
                _light.TimeSinceFull = tileLightDto.Value.TimeCreated;
            }
            else
            {
                //else we load in the saved tile light data and adjust the current lumens based on the time elapsed from the save value
                tileLightDto = new TileLightDataDTO(Tile.TileData.GetKey(), Clock.TotalTime, _light.CurrentLumens);
                Tile.TileManager.TileLightManager.AddNewItem(tileLightDto.Value);
                //to do, recharge current lumens based on time elapsed since unloaded
                if(_light.CurrentLumens != _light.MaxLumens)
                {
                    float timeElapsedSinceUnloaded = Clock.TotalTime - tileLightDto.Value.TimeCreated;
                    byte lumens = (byte)(LightCollidable.S_RechargeRate * timeElapsedSinceUnloaded);
                    if(lumens > _light.MaxLumens)
                        lumens = _light.MaxLumens;

                    _light.SetCurrentLumens(lumens);
                }
            }

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var addons = Tile.GetAddonsByType(typeof(TileProgressBar));

            if (addons.Count == 0)
                throw new Exception($"Tile light should not exist without tile progress bar");
            TileProgressBar progressBar = addons[0] as TileProgressBar;
            progressBar.ManualSetCurrentAmountAndUpdate(_light.CurrentLumens, _light.MaxLumens);

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            _light.DrawEmitter(spriteBatch);
        }
        protected override void CreateBody(Vector2 position)
        {
            AddPrimaryBody(PhysicsManager.CreateCircularHullBody(BodyType.Static, position, 4f,
                new List<Category>() { (Category)PhysCat.LightSource }, new List<Category>() { (Category)PhysCat.PlayerBigSensor }, null, null,
              isSensor: true));
        }

        /// <summary>
        /// For use with the "lightSource" tile property
        /// </summary>
        private Point GetOffSet(string lightString)
        {

            return new Point(int.Parse(lightString.Split(',')[1]),
                int.Parse(lightString.Split(',')[2]));

        }

        /// <summary>
        /// For use with the "lightSource" tile property
        /// </summary>
        private float GetRadius(string lightString)
        {

            return float.Parse(lightString.Split(',')[0]);

        }
        /// <summary>
        /// For use with the "lightSource" tile property
        /// </summary>
        private LightType GetLightType(string lightString)
        {

            return (LightType)Enum.Parse(typeof(LightType), lightString.Split(',')[3]);

        }
        /// <summary>
        /// For use with the "lightSource" tile property
        /// </summary>
        private bool GetIfImmune(string lightString)
        {

            return bool.Parse(lightString.Split(',')[4].Split(':')[1]);

        }
        public override void CleanUp()
        {
            //when unloaded make sure tile light manager gets the updated values
            Tile.TileManager.TileLightManager.Update(new TileLightDataDTO(Tile.TileData.GetKey(), _light.TimeSinceFull, _light.CurrentLumens));

            base.CleanUp();
        }
    }
}
