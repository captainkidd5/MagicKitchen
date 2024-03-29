﻿using Globals.Classes;
using Globals.Classes.Console;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using SoundEngine.Classes;
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
        public int MaxLumens => _player.MaxLumens;
        public int CurrentLumens { get; set; }

        public int LumensLastFrame { get; set; }

        protected HullBody LightSensor { get; set; }

        public bool Illuminated { get; set; }

        private static float _baseLumenRechargeRate = .5f;
        private float _lumenRechargeRate = .15f;
        private SimpleTimer _lumenRechargeTimer;

        private float _lumenDrainRate = .4f;
        private SimpleTimer _lumenDrainTimer;

        private LightCollidable _lightCollidable;
        private List<Fixture> _lightsTouching;
        private Player _player;
        public LumenHandler()
        {
        }

        public void Initialize(HullBody lightSensor)
        {
            LightSensor = lightSensor;

        }
        public void Load(Player player, LightCollidable lightCollidable, List<Fixture> lightsTouching)
        {
            _player = player;
            _lumenRechargeTimer = new SimpleTimer(_lumenRechargeRate);
            _lumenDrainTimer = new SimpleTimer(_lumenDrainRate);

            _lightCollidable = lightCollidable;
            _lightsTouching = lightsTouching;
            CurrentLumens = MaxLumens;
            LumensLastFrame = CurrentLumens;
            CommandConsole.RegisterCommand("drain", "drains x lumens from player", DrainLumensCommand);



        }

        private void DrainLumensCommand(string[] args)
        {
            CurrentLumens -= int.Parse(args[0]);
            if (CurrentLumens < 0)
                CurrentLumens = 0;
        }
        /// <summary>
        /// If player light sensor overlaps with any light, begin recharging. Recharges at interval, until 
        /// current lumens is equal to max lumens
        /// </summary>
        /// <param name="gameTime"></param>
        public void HandleLumens(GameTime gameTime)
        {


       
            LumensLastFrame = CurrentLumens;



            if (!RechargeLumens(gameTime))
            {
                

            }




            Illuminated = false;

            foreach (Fixture fixture in _lightsTouching)
            {
                if ((fixture.Body.Tag as LightCollidable).ImmuneToDrain)
                {
                    return;
                }
            }
            DrainLumens(gameTime);

     

        }

        /// <summary>
        /// Returns true if was able to gain any charge from lights nearby or timer has not run out
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        private bool RechargeLumens(GameTime gameTime)
        {
            if (CurrentLumens < MaxLumens)
            {

                if (_lumenRechargeTimer.Run(gameTime))
                {

                    foreach (Fixture fixture in _lightsTouching)
                    {
                        int siphonedAmt = 0;
                        (fixture.Body.Tag as LightCollidable).PReached -= OnParticleReachedDestination;
                        (fixture.Body.Tag as LightCollidable).PReached += OnParticleReachedDestination;
                        (fixture.Body.Tag as LightCollidable).SiphonLumens(1);
                        //if (siphonedAmt > 0)
                        //{
                        //    CurrentLumens += siphonedAmt;
                        //    SoundFactory.PlayEffectPackage("LightRecharge");
                        //    return true;
                        //}
                    }

                }


            }
            return false;
        }
        public void OnParticleReachedDestination()
        {
            CurrentLumens ++;
            if (CurrentLumens > MaxLumens)
                CurrentLumens = MaxLumens;
            SoundFactory.PlayEffectPackage("LightRecharge");
        }
        private void DrainLumens(GameTime gameTime)
        {


            if (_lumenDrainTimer.Run(gameTime))
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
            writer.Write(_lumenRechargeRate);
        }

        public void LoadSave(BinaryReader reader)
        {
            CurrentLumens = reader.ReadInt32();
            _lumenRechargeRate = reader.ReadSingle();
        }

        public void SetToDefault()
        {
            CurrentLumens = MaxLumens;
            _lumenRechargeRate = _baseLumenRechargeRate;
        }
    }
}
