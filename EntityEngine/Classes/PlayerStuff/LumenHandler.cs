using Globals.Classes;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Collision.Shapes;
using tainicom.Aether.Physics2D.Dynamics;

namespace EntityEngine.Classes.PlayerStuff
{
    internal class LumenHandler : ISaveable
    {
        public int MaxLumens { get; set; } = 100;
        public int CurrentLumens { get; set; } = 100;

        public int LumensLastFrame { get; set; } = 100;

        protected HullBody LightSensor { get; set; }

        public bool Illuminated { get; set; }

        private static float _baseLumenRechargeRate = .5f;
        private float _lumenRechargeRate = .5f;
        private SimpleTimer _lumenRechargeTimer;
        private LightCollidable _lightCollidable;
        private List<Fixture> _lightsTouching;
        private Player _player;
        public LumenHandler(HullBody lightSensor)
        {
            LightSensor = lightSensor;
        }
        public void Load(Player player, LightCollidable lightCollidable, List<Fixture> lightsTouching)
        {
            _player = player;
            _lumenRechargeTimer = new SimpleTimer(_lumenRechargeRate);
            _lightCollidable = lightCollidable;
            _lightsTouching = lightsTouching;
            ResizeLightBody();


        }
        /// <summary>
        /// If player light sensor overlaps with any light, begin recharging. Recharges at interval, until 
        /// current lumens is equal to max lumens
        /// </summary>
        /// <param name="gameTime"></param>
        public void HandleLumens(GameTime gameTime)
        {
            if (LumensLastFrame != CurrentLumens)
            {
                ResizeLightBody();
            }
            LumensLastFrame = CurrentLumens;

            if (Illuminated)
            {
                RechargeLumens(gameTime);

            }
            else
            {
                DrainLumens(gameTime);

            }

            Illuminated = false;

        }
        private void ResizeLightBody()
        {
            float newVal = (float)CurrentLumens / (float)MaxLumens * 10;
            _lightCollidable.ResizeLight(new Vector2(newVal, newVal));

        }
        private void RechargeLumens(GameTime gameTime)
        {
            if (CurrentLumens < MaxLumens)
            {

                if (_lumenRechargeTimer.Run(gameTime))
                {
                    foreach (Fixture fixture in _lightsTouching)
                    {
                        int siphonedAmt = 0;
                        siphonedAmt += (fixture.Body.Tag as LightCollidable).SiphonLumens(1);
                        if (siphonedAmt > 0)
                        {
                            CurrentLumens += siphonedAmt;
                            break;
                        }
                    }
                }
            }

        }

        private void DrainLumens(GameTime gameTime)
        {


            if (_lumenRechargeTimer.Run(gameTime))
            {
                if (CurrentLumens > 0)
                {
                    CurrentLumens--;
                }
                else
                {
                    _player.TakeDamage(null, 5);
                }
            }


        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(CurrentLumens);
            writer.Write(MaxLumens);
            writer.Write(_lumenRechargeRate);
        }

        public void LoadSave(BinaryReader reader)
        {
            CurrentLumens = reader.ReadInt32();
            MaxLumens = reader.ReadInt32();
            _lumenRechargeRate = reader.ReadSingle();
        }

        public void CleanUp()
        {
            throw new NotImplementedException();
        }

        public void SetToDefault()
        {
            CurrentLumens = MaxLumens;
            _lumenRechargeRate = _baseLumenRechargeRate;
        }
    }
}
