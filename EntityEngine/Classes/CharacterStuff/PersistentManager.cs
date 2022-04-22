using DataModels;
using DataModels.QuestStuff;
using EntityEngine.Classes.CharacterStuff.QuestStuff;
using EntityEngine.Classes.NPCStuff;
using Globals.Classes;
using InputEngine.Classes.Input;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TiledEngine.Classes;
using static EntityEngine.Classes.CharacterStuff.Scheduler;
using static Globals.Classes.Settings;

namespace EntityEngine.Classes.CharacterStuff
{
    public class PersistentManager : EntityContainer
    {
        private readonly QuestManager _questManager;

        internal static Texture2D StatusIconTexture { get; set; }


        public PersistentManager( GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            _questManager = new QuestManager(graphics,content);
        }

  
        public override void LoadContent()
        {
           
            StatusIconTexture = content.Load<Texture2D>("entities/npc/characters/statusicons");
            base.LoadContent();
        }


        public override void Update(GameTime gameTime)
        {
            foreach (Entity entity in Entities)
            {
                NPC charac = (NPC)entity;
                charac.Update(gameTime);

            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity entity in Entities)
            {
                NPC charac = (NPC)entity;
                if (charac.IsInStage)
                {
                    charac.Draw(spriteBatch);

                }

                if (Flags.DebugVelcro)
                    charac.DrawDebug(spriteBatch);

            }

        }

        public void AssignCharactersToStages(string newStageName, TileManager tileManager, ItemManager itemManager)
        {
            foreach (Character character in Entities)
            {
                if(character.CurrentStageName == newStageName)
                {
                     character.SwitchStage(newStageName, tileManager, itemManager);

                }
            }
        }

        public void SwitchStage(string newStageName, TileManager tileManager, ItemManager itemManager)
        {
            foreach (Character character in Entities)
            {
                character.PlayerSwitchedStage(newStageName, false);
                
            }
        }

        public override void Save(BinaryWriter writer)
        {
            //Test if new game because characters are initially loaded in after save/load logic, therefore the entity list is not populated
            //before first load and therefore not saved
            if(!Flags.IsNewGame)
                foreach (Entity entity in Entities)
                {
                    NPC charac = (NPC)entity;
                charac.Save(writer);


            }
        }
        public override void LoadSave(BinaryReader reader)
        {
           
            if (!Flags.IsNewGame)
                foreach (Entity character in Entities)
                {
                    Character charac = (Character)character;
                    charac.LoadSave(reader);


                }
        }
    }
}
