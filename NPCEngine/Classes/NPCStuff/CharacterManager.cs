using DataModels;
using DataModels.QuestStuff;
using EntityEngine.Classes.NPCStuff.QuestStuff;
using Globals.Classes;
using InputEngine.Classes.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static EntityEngine.Classes.NPCStuff.Scheduler;
using static Globals.Classes.Settings;

namespace EntityEngine.Classes.NPCStuff
{
    public class CharacterManager : Component, ISaveable
    {
        private readonly QuestManager _questManager;

        public List<Character> AllCharacters { get; set; }

        internal static Texture2D StatusIconTexture { get; set; }


        public CharacterManager(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            _questManager = new QuestManager(graphics,content);
        }
        public void LoadCharacterData(GraphicsDevice graphics, ContentManager content)
        {
            AllCharacters = new List<Character>();
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
                AllCharacters.Add(newCharacter);

                allNpcData.Add(data);
            }
            _questManager.LoadQuestData(allQuests);
            StatusIconTexture = content.Load<Texture2D>("entities/npc/characters/statusicons");

        }

        public void Update(GameTime gameTime, string stage)
        {
            foreach (Character character in AllCharacters)
            {
               // character.UpdatePath(gameTime);

               // if (character.CurrentStage == stage)
               //{
                    character.Update(gameTime);

                //}
            }
        }

        public void Draw(SpriteBatch spriteBatch, string stage)
        {
            foreach (Character character in AllCharacters)
            {
                if(character.CurrentStageName == stage)
                {
                    character.Draw(spriteBatch);

                }

                if(Flags.DebugVelcro)
                    character.DrawDebug(spriteBatch);
            }
        }

        public void SwitchStage(string newStage)
        {
            foreach (Character character in AllCharacters)
            {
               
                    character.PlayerSwitchedStage(newStage, false);

                
            }
        }

        public void Save(BinaryWriter writer)
        {

        }
        public void Load(BinaryReader reader)
        {

        }

        public void LoadSave(BinaryReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
