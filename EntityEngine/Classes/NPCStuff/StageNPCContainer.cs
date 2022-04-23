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
using PhysicsEngine.Classes.Pathfinding;
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
    public class StageNPCContainer : EntityContainer
    {
        private readonly NPCManager _npcManager;

        public string StageName { get; private set; }

        public StageNPCContainer(NPCManager npcManager, GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            _npcManager = npcManager;
        }


        public void LoadContent(string stageName, TileManager tileManager, ItemManager itemManager)
        {
            StageName = stageName;
            TileManager = tileManager;
            ItemManager = itemManager;

            foreach (NPC entity in Entities)
            {

                entity.LoadContent(null,entity.Name,false);
               entity.SwitchStage(stageName, tileManager, itemManager);
            }


        }

        internal void AddTrain()
        {
            Train train = new Train(this, graphics, content);
            train.LoadContent(null, null);
            train.SwitchStage(StageName, TileManager, ItemManager);
            Entities.Add(train);
        }



        public override void Save(BinaryWriter writer)
        {

            writer.Write(Entities.Count);
                foreach (Entity n in Entities)
                {
                writer.Write(n.GetType().ToString());

                n.Save(writer);


                }
        }
        public override void LoadSave(BinaryReader reader)
        {
            if (Flags.IsNewGame)
            {
                foreach (NPCData npcData in EntityFactory.NPCData.Values)
                {
                    if (npcData.ImmediatelySpawn && this.GetType() == typeof(PersistentManager))
                    {

                        NPC npc = (NPC)System.Reflection.Assembly.GetExecutingAssembly()
                            .CreateInstance(npcData.ObjectType, true, System.Reflection.BindingFlags.CreateInstance,
                            null, new object[] {this, graphics, content }, null, null);

                     
                        npc.LoadContent(null, npcData.Name, npc.GetType() != typeof(HumanoidEntity));
                        Entities.Add(npc);
                    }
                }
            }

            else
            {

         int count = reader.ReadInt32();
            for(int i =0; i < count; i++)
            {
                string savedType = reader.ReadString();

                NPC npc = (NPC)System.Reflection.Assembly.GetExecutingAssembly()
                    .CreateInstance(savedType, true, System.Reflection.BindingFlags.CreateInstance,
                    null, new object[] { this, graphics, content },null,null);
                npc.LoadSave(reader);
                        npc.LoadContent(null, npc.Name, npc.GetType() != typeof(HumanoidEntity));
                Entities.Add(npc);
            }
            }

        }

        public PathGrid GetPathGrid(string stageName)
        {
            return _npcManager.StageGrids[stageName];
        }

        public virtual void CreateNPC( string name, Vector2 position, bool standardAnimator, string stageName = null)
        {
            if (stageName != StageName)
                throw new Exception($"Persistent entities cannot be added non persistent managers");
            NPC npc = new NPC(this, graphics, content);
            npc.LoadContent(position, name, standardAnimator);
            npc.SwitchStage(StageName, TileManager, ItemManager);
            EntitiesToAdd.Add(npc);
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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
