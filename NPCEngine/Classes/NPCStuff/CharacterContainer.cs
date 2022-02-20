using DataModels;
using DataModels.QuestStuff;
using EntityEngine.Classes.NPCStuff.QuestStuff;
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
using static EntityEngine.Classes.NPCStuff.Scheduler;
using static Globals.Classes.Settings;

namespace EntityEngine.Classes.NPCStuff
{
    internal class CharacterContainer : EntityContainer
    {
        private readonly QuestManager _questManager;


        internal static Texture2D StatusIconTexture { get; set; }


        public CharacterContainer(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            _questManager = new QuestManager(graphics,content);
        }
        public override void Load(string stageName, TileManager tileManager, ItemManager itemManager)
        {
            List<NPCData> allNpcData = new List<NPCData>();

            string pathExtension = "/entities/NPC/Characters";
            string basePath = content.RootDirectory + pathExtension;
            string[] directories = Directory.GetDirectories(basePath);
            List<Quest> allQuests = new List<Quest>();
            foreach (string directory in directories)
            {
                string npcName = directory.Split("Characters\\")[1];

                string characterSubDirectory = directory + "/";
                NPCData data = NPCData.GetNPCData(characterSubDirectory, npcName);
                
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
            StatusIconTexture = content.Load<Texture2D>("entities/npc/characters/statusicons");

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

        }
        public override void LoadSave(BinaryReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
