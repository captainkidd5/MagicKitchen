﻿using DataModels;
using DataModels.NPCStuff;
using DataModels.QuestStuff;
using EntityEngine.Classes.HumanoidStuff;
using EntityEngine.Classes.NPCStuff;
using EntityEngine.Classes.NPCStuff.Props;
using EntityEngine.Classes.StageStuff;
using EntityEngine.ItemStuff;
using Globals.Classes;
using Globals.Classes.Console;
using InputEngine.Classes;
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
using static Globals.Classes.Settings;

namespace EntityEngine.Classes.CharacterStuff
{
    public class NPCContainer : EntityContainer, ICommandRegisterable
    {

        private NPCSpawner _mobSpawner;

        public NPCContainer( GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
        }

        public override void Initialize(StageManager stageManager)
        {
            base.Initialize(stageManager);
            _mobSpawner = new NPCSpawner();

        }
        public void RegisterCommands()
        {
            CommandConsole.RegisterCommand("add_npc", "adds npc to current stage", AddNPCCommand);
            CommandConsole.RegisterCommand("train", "forces train into current stage", AddTrainCommand);
        }
        private void AddNPCCommand(string[] args)
        {
            CreateNPC(args[0], Controls.MouseWorldPosition, true);
        }
        private void AddTrainCommand(string[] args)
        {
            AddTrain();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            foreach (NPC entity in Entities)
            {
                entity.Initialize(StageManager, null, entity.Name, false);
            }
          //  _mobSpawner.Load(StageManager);
            RegisterCommands();
        }

        internal void AddTrain()
        {
            Train train = new Train(graphics, content);
            train.Initialize("TestIsland", StageManager);
            train.Initialize(StageManager, null, "train");
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
                if (Flags.SpawnCharactersOnNewGame)
                {

                    foreach (NPCData npcData in EntityFactory.NPCData.Values)
                    {
                        if (npcData.ImmediatelySpawn)
                        {

                            NPC npc = (NPC)System.Reflection.Assembly.GetExecutingAssembly()
                                .CreateInstance(npcData.ObjectType, true, System.Reflection.BindingFlags.CreateInstance,
                                null, new object[] { graphics, content }, null, null);


                            npc.Initialize(StageManager, null, npcData.Name, npc.GetType() != typeof(HumanoidEntity));
                            Entities.Add(npc);
                        }
                    }
                }

            }

            else
            {

                int count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    string savedType = reader.ReadString();
                    //TODO: Give npcdata a starting stage string
                    NPC npc = (NPC)System.Reflection.Assembly.GetExecutingAssembly()
                        .CreateInstance(savedType, true, System.Reflection.BindingFlags.CreateInstance,
                        null, new object[] { graphics, content }, null, null);
                    npc.LoadSave(reader);
                    npc.Initialize(StageManager, null, npc.Name, npc.GetType() != typeof(HumanoidEntity));
                    Entities.Add(npc);
                }
            }

        }



        public virtual void CreateNPC(string name, Vector2 position, bool standardAnimator, string stageName = null)
        {

            NPC npc = (NPC)System.Reflection.Assembly.GetExecutingAssembly()
                    .CreateInstance(EntityFactory.NPCData[name].ObjectType, true, System.Reflection.BindingFlags.CreateInstance,
                    null, new object[] { graphics, content }, null, null);
            npc.Initialize(StageManager, position, name, EntityFactory.NPCData[name].NPCType == NPCType.Enemy);
            EntitiesToAdd.Add(npc);
        }

     

        public override void Update(GameTime gameTime)
        {
            float totalSpawnVal = 0;
            int totalBoarCount = 0;
            for (int i = Entities.Count - 1; i >= 0; i--)
            {
                NPC entity = Entities[i] as NPC;
                entity.Update(gameTime);
                totalSpawnVal += (float)entity.NPCData.SpawnSlotValue / 100;
                if (entity.FlaggedForRemoval)
                {
                    entity.SetToDefault();
                    Entities.RemoveAt(i);

                }
                if (entity.NPCData != null && entity.NPCData.Name.ToLower() == "boar")
                    totalBoarCount++;
            }
            _mobSpawner.TotalNPCSpawnValue = totalSpawnVal;

            foreach (Entity entity in EntitiesToAdd)
                Entities.Add(entity);

            EntitiesToAdd.Clear();
            _mobSpawner.Update(gameTime);
        }
    }
}
