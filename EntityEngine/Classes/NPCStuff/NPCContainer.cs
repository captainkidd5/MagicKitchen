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
using System.Linq;
using System.Text;
using System.Text.Json;
using TiledEngine.Classes;
using static EntityEngine.Classes.CharacterStuff.Scheduler;
using static Globals.Classes.Settings;

namespace EntityEngine.Classes.CharacterStuff
{
    internal class NPCContainer : EntityContainer
    {
        private readonly QuestManager _questManager;


        internal Dictionary<string, NPCData> NPCData;
        public NPCContainer(EntityManager entityManager, GraphicsDevice graphics, ContentManager content) : base(entityManager, graphics, content)
        {
            _questManager = new QuestManager(graphics, content);
            Extension = "NPC";
        }

        internal override void PlayerSwitchedStage(string stageTo)
        {
            base.PlayerSwitchedStage(stageTo);
        }
        internal override void LoadContent(string stageName, TileManager tileManager, ItemManager itemManager)
        {

            base.LoadContent(stageName, tileManager, itemManager);
        }


        internal override void Update(GameTime gameTime)
        {
            foreach (KeyValuePair<string, Entity> character in Entities)
            {
                Character charac = (Character)character.Value;
                charac.Update(gameTime);

            }

        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            foreach (KeyValuePair<string, Entity> character in Entities)
            {
                Character charac = (Character)character.Value;
                if (charac.IsInStage)
                {
                    charac.Draw(spriteBatch);

                }

                if (Flags.DebugVelcro)
                    charac.DrawDebug(spriteBatch);

            }

        }

        internal void AssignCharactersToStages()
        {

        }

        internal override void SwitchStage(string newStage)
        {
            foreach (KeyValuePair<string, Entity> character in Entities)
            {
                Character charac = (Character)character.Value;
                charac.PlayerSwitchedStage(newStage, false);


            }
        }

        public override void Save(BinaryWriter writer)
        {
            //Test if new game because characters are initially loaded in after save/load logic, therefore the entity list is not populated
            //before first load and therefore not saved
            if (!Flags.IsNewGame)
                foreach (KeyValuePair<string, Entity> character in Entities)
                {
                    Character charac = (Character)character.Value;
                    charac.Save(writer);


                }
        }
        public override void LoadSave(BinaryReader reader)
        {

            string basePath = content.RootDirectory + FileLocation;



            string jsonString = File.ReadAllText(basePath + "NPCData.json");
            NPCData = JsonSerializer.Deserialize<List<NPCData>>(jsonString).ToDictionary(x => x.Name);



            if (!Flags.IsNewGame)
                foreach (KeyValuePair<string, Entity> character in Entities)
                {
                    Character charac = (Character)character.Value;
                    charac.LoadSave(reader);


                }
        }

        public NPC CreateNPC(string name, Vector2 position)
        {
            return new NPC(graphics, content, NPCData[name]);
        }
    }
}
