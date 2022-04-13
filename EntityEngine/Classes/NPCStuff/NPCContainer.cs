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
            StageName = StageName;

            base.LoadContent(stageName, tileManager, itemManager);


            Train train = new Train(graphics, content);
            train.LoadContent(itemManager);
            train.SwitchStage(StageName, TileManager, ItemManager);
            Entities.Add(train);

        }

        internal override void Update(GameTime gameTime)
        {
            foreach (Entity n in Entities)
            {
                NPC npc = (NPC)n;
                npc.Update(gameTime);

            }

        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            foreach ( Entity n in Entities)
            {
                NPC npc = (NPC)n;
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


            writer.Write(Entities.Count);
                foreach (Entity n in Entities)
                {
                n.Save(writer);


                }
        }
        public override void LoadSave(BinaryReader reader)
        {

         int count = reader.ReadInt32();
            for(int i =0; i < count; i++)
            {
                NPC npc = new NPC(graphics,content);
                npc.LoadSave(reader);
                npc.LoadContent(ItemManager,null,null);
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
