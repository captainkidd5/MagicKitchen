using DataModels;
using DataModels.QuestStuff;
using EntityEngine.Classes.CharacterStuff.QuestStuff;
using EntityEngine.Classes.NPCStuff;
using EntityEngine.Classes.NPCStuff.Props;
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
using System.Text.Json.Serialization;
using TiledEngine.Classes;
using static EntityEngine.Classes.CharacterStuff.Scheduler;
using static Globals.Classes.Settings;

namespace EntityEngine.Classes.CharacterStuff
{
    internal class NPCContainer : EntityContainer
    {
        public string StageName { get; private set; }

        public NPCContainer( EntityManager entityManager, GraphicsDevice graphics, ContentManager content) : base(entityManager, graphics, content)
        {
            Extension = "NPC";
        }

        internal override void PlayerSwitchedStage(string stageTo)
        {
            base.PlayerSwitchedStage(stageTo);
        }
        internal override void LoadContent(string stageName, TileManager tileManager, ItemManager itemManager)
        {
            StageName = stageName;

            base.LoadContent(stageName, tileManager, itemManager);


        }

        internal void AddTrain()
        {
            Train train = new Train(graphics, content);
            train.LoadContent(ItemManager, null, null);
            train.SwitchStage(StageName, TileManager, ItemManager);
            Entities.Add(train);
        }

        internal override void Update(GameTime gameTime)
        {
            foreach (Entity n in Entities)
            {
                Entity npc = (Entity)n;
                npc.Update(gameTime);

            }

        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            foreach ( Entity n in Entities)
            {
                Entity npc = (Entity)n;
                npc.Draw(spriteBatch);
                if (Flags.DebugVelcro)
                    npc.DrawDebug(spriteBatch);

            }
          

        }


        internal override void SwitchStage(string newStage)
        {
            Console.WriteLine("test");
        }

        public override void Save(BinaryWriter writer)
        {

            writer.Write(StageName ?? String.Empty);
            writer.Write(Entities.Count);
                foreach (Entity n in Entities)
                {
                writer.Write(n.GetType().ToString());

                n.Save(writer);


                }
        }
        public override void LoadSave(BinaryReader reader)
        {
            StageName = reader.ReadString();
         int count = reader.ReadInt32();
            for(int i =0; i < count; i++)
            {
                string savedType = reader.ReadString();

                NPC npc = (NPC)System.Reflection.Assembly.GetExecutingAssembly()
                    .CreateInstance(savedType, true, System.Reflection.BindingFlags.CreateInstance,
                    null, new object[] { graphics, content },null,null);
                npc.LoadSave(reader);
                npc.LoadContent(ItemManager,null,null);
                npc.SwitchStage(StageName, TileManager, ItemManager);
                Entities.Add(npc);
            }

        }

       

        public void CreateNPC(string name, Vector2 position)
        {
            NPC npc = new NPC(graphics, content);
            npc.LoadContent(ItemManager, position, name);
            npc.SwitchStage(StageName, TileManager, ItemManager);
            Entities.Add(npc);
        }

        internal static Texture2D GetTextureFromNPCType(NPCType npcType)
        {
            if (npcType == NPCType.Enemy)
                return EntityFactory.NPCSheet;
            else if (npcType == NPCType.Prop)
                return EntityFactory.Props_1;
            else
                throw new Exception($"Invalid npc type {npcType.ToString()}");
        }
    }
}
