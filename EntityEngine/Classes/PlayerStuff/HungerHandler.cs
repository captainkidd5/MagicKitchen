using DataModels.ItemStuff;
using Globals.Classes;
using Microsoft.Xna.Framework;
using SoundEngine.Classes;
using SpriteEngine.Classes.ParticleStuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityEngine.Classes.PlayerStuff
{
    internal class HungerHandler : ISaveable
    {
        public byte MaxHunger { get; set; } = 100;
        public byte CurrentHunger { get; set; } = 100;



        private static float _baseHungerDrainRate = 2f;
        private float _currentHungerDrainRate = 2f;
        private SimpleTimer _hungerRechargeTimer;

        private Player _player;
        public HungerHandler()
        {
        }
        public void Load(Player player)
        {
            _player = player;
            _hungerRechargeTimer = new SimpleTimer(_currentHungerDrainRate);

        }

        public void HandleHunger(GameTime gameTime)
        {

            DrainHunger(gameTime);


        }


        public void ConsumeFood(byte amt)
        {
            CurrentHunger += amt;
            if(CurrentHunger > MaxHunger)
                CurrentHunger = MaxHunger;
            ParticleManager.AddParticleEmitter(_player, EmitterType.Text,"+" + amt.ToString(), Color.DarkGreen, Color.LightGreen);

            SoundFactory.PlayEffectPackage("FoodBite");
        }
        private void DrainHunger(GameTime gameTime)
        {
   
                if (_hungerRechargeTimer.Run(gameTime))
                {
                    if (CurrentHunger > 0)
                    {

                        CurrentHunger--;
                    }
                    else
                    {
                        _player.TakeDamage(null,  5);

                    }
                }
    

        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(CurrentHunger);
            writer.Write(MaxHunger);
        }

        public void LoadSave(BinaryReader reader)
        {
            CurrentHunger = reader.ReadByte();
            MaxHunger = reader.ReadByte();
        }

        public void CleanUp()
        {
            throw new NotImplementedException();
        }

        public void SetToDefault()
        {
            CurrentHunger = MaxHunger;
            _currentHungerDrainRate = _baseHungerDrainRate;
        }
    }
}
