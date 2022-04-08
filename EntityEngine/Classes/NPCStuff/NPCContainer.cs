using DataModels;
using DataModels.QuestStuff;
using EntityEngine.Classes.CharacterStuff.QuestStuff;
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
    internal class NPCContainer : EntityContainer
    {
        private readonly QuestManager _questManager;

        internal static Texture2D StatusIconTexture { get; set; }


        public NPCContainer(EntityManager entityManager, GraphicsDevice graphics, ContentManager content) : base(entityManager, graphics, content)
        {
            _questManager = new QuestManager(graphics,content);
            Extension = "NPC";
        }

        internal override void PlayerSwitchedStage(string stageTo)
        {
            base.PlayerSwitchedStage(stageTo);
        }
        internal override void LoadContent(string stageName, TileManager tileManager, ItemManager itemManager)
        {
           
            StatusIconTexture = content.Load<Texture2D>("entities/npc/characters/statusicons");
            base.LoadContent(stageName,tileManager,itemManager);
        }


        internal override void Update(GameTime gameTime)
        {
            foreach(KeyValuePair<string, Entity> character in Entities)
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
            if(!Flags.IsNewGame)
            foreach (KeyValuePair<string, Entity> character in Entities)
            {
                Character charac = (Character)character.Value;
                charac.Save(writer);


            }
        }
        public override void LoadSave(BinaryReader reader)
        {
            List<CharacterData> allNpcData = new List<CharacterData>();

            string basePath = content.RootDirectory + FileLocation;
            string[] directories = Directory.GetDirectories(basePath);
            List<Quest> allQuests = new List<Quest>();
            foreach (string directory in directories)
            {
                string npcName = directory.Split("Characters\\")[1];

                string characterSubDirectory = directory + "/";
                CharacterData data = CharacterData.GetCharacterData(characterSubDirectory, npcName);

                List<Quest> npcQuests = Quest.GetQuests(characterSubDirectory);
                allQuests.AddRange(npcQuests);
                data.Quests = npcQuests;

                foreach (Schedule sch in data.Schedules)
                    sch.ConvertTimeString();

                data.Schedules.Sort(0, data.Schedules.Count, new ScheduleTimeComparer());
                Character newCharacter = new Character(graphics, content, data);
                Entities.Add(newCharacter.Name, newCharacter);

                allNpcData.Add(data);
            }
            _questManager.LoadQuestData(allQuests);
            if(!Flags.IsNewGame)
            foreach (KeyValuePair<string, Entity> character in Entities)
            {
                Character charac = (Character)character.Value;
                charac.LoadSave(reader);


            }
        }
    }
}
