using DataModels;
using EntityEngine.Classes.CharacterStuff;
using EntityEngine.Classes.StageStuff;
using Globals.Classes;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes;

namespace EntityEngine.Classes.NPCStuff.Props
{
    internal class Train : NPC
    {
        /// <summary>
        /// Min number of passengers train will unload at each stop
        /// </summary>
        public int UnloadMin { get; set; } = 2;

        /// <summary>
        /// Max number of passengers train will unload at each stop
        /// </summary>
        public int UnloadMax { get; set; } = 5;

        private SimpleTimer _unloadTimer;
        //rate at which passengers unload in seconds
        private readonly float _unloadSpeed = .5f;

        private int _currentPassengerCount;

        public Train(string stageName, StageManager stageManager, GraphicsDevice graphics, ContentManager content) : 
            base(stageName, stageManager, graphics, content)
        {
            _unloadTimer = new SimpleTimer(_unloadSpeed);
        }

        public override void LoadContent(EntityContainer container, Vector2? startPos, string name, bool standardAnimator = true)
        {
            base.LoadContent(container, startPos, name, standardAnimator);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        /// <summary>
        /// Unloads passengers at rate, returns true if all passengers are unloaded
        /// </summary>
        /// <returns></returns>
       public bool UnloadPassengers(GameTime gameTime)
        {
            if (_currentPassengerCount == 0)
                _currentPassengerCount = GetPassengersRandom();

            if(_unloadTimer.Run(gameTime))
            {
                _currentPassengerCount--;
                NPCContainer.CreateNPC("patron", new Vector2(
                    Position.X + Settings.Random.Next(0,20),
                    Position.Y + Settings.Random.Next(40, 60) * -1), false);
            }
            if (_currentPassengerCount == 0)
                return true;

            return false;

        }

        private int GetPassengersRandom()
        {
            return Settings.Random.Next(UnloadMin, UnloadMax);
        }
        public override void Save(BinaryWriter writer)
        {
            base.Save(writer);
            writer.Write(UnloadMin);
            writer.Write(UnloadMax);
        }

        public override void LoadSave(BinaryReader reader)
        {
            base.LoadSave(reader);
            UnloadMin = reader.ReadInt32();
            UnloadMax = reader.ReadInt32();
        }
    }
}
