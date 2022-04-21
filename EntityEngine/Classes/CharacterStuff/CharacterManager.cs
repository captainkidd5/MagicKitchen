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
    public class CharacterManager : EntityContainer
    {
        private readonly QuestManager _questManager;

        internal static Texture2D StatusIconTexture { get; set; }


        public CharacterManager( GraphicsDevice graphics, ContentManager content) : base(graphics, content)
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
                Character charac = (Character)entity;
                charac.Update(gameTime);

            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity entity in Entities)
            {
                Character charac = (Character)entity;
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
                    Character charac = (Character)entity;
                charac.Save(writer);


            }
        }
        public override void LoadSave(BinaryReader reader)
        {
            List<CharacterData> allNpcData = new List<CharacterData>();

            string basePath = content.RootDirectory + "/Entities/Characters";
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


                Character newCharacter = new Character(graphics, content, data);
                Entities.Add(newCharacter);

                allNpcData.Add(data);
            }
            _questManager.LoadQuestData(allQuests);
            if(!Flags.IsNewGame)
            foreach (Entity character in Entities)
            {
                Character charac = (Character)character;
                charac.LoadSave(reader);


            }
        }
    }
}
