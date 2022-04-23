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
    public class PersistentManager : StageNPCContainer
    {
        private readonly QuestManager _questManager;

        internal static Texture2D StatusIconTexture { get; set; }


        public PersistentManager(NPCManager manager, GraphicsDevice graphics, ContentManager content) : base(manager,graphics, content)
        {
            _questManager = new QuestManager(graphics,content);
        }

  
        public override void LoadContent()
        {
           
            StatusIconTexture = content.Load<Texture2D>("entities/npc/characters/statusicons");
            base.LoadContent();
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
            foreach (NPC character in Entities)
            {
                if (character.CurrentStageName == newStageName)
                {
                    character.SwitchStage(newStageName, tileManager, itemManager);

                }
            }
        }

        public void SwitchStage(string newStageName, TileManager tileManager, ItemManager itemManager)
        {
            TileManager = tileManager;
            ItemManager = itemManager;
            foreach (NPC character in Entities)
            {
                character.PlayerSwitchedStage(newStageName, false);
                
            }
        }


        public override void LoadSave(BinaryReader reader)
        {
            base.LoadSave(reader);
        }

        public override void CreateNPC( string name, Vector2 position, bool standardAnimator, string stageName)
        {
            NPC npc = (NPC)System.Reflection.Assembly.GetExecutingAssembly()
                    .CreateInstance(EntityFactory.NPCData[name].ObjectType, true, System.Reflection.BindingFlags.CreateInstance,
                    null, new object[] { this, graphics, content }, null, null);
            npc.LoadContent(position, name, standardAnimator);
              npc.SwitchStage(stageName, TileManager, ItemManager);
            EntitiesToAdd.Add(npc);
        }
    }
}
