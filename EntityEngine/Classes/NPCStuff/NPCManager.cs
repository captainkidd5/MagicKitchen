using EntityEngine.Classes.CharacterStuff;
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
using System.Threading.Tasks;
using TiledEngine.Classes;

namespace EntityEngine.Classes.NPCStuff
{
    public class NPCManager : ISaveable, ICommandRegisterable
    {
        public StageNPCContainer CurrentContainer { get; set; }
        public PersistentManager PersistentManager { get; set; }

        public Dictionary<string, PathGrid> StageGrids { get; set; }
        public NPCManager(GraphicsDevice graphics, ContentManager content)
        {
            PersistentManager = new PersistentManager(this, graphics, content);
            StageGrids = new Dictionary<string, PathGrid>();
        }

        public void RegisterCommands()
        {
            CommandConsole.RegisterCommand("add_npc", "adds npc to current stage", AddNPCCommand);
            CommandConsole.RegisterCommand("train", "forces train into current stage", AddTrainCommand);
        }
        public void LoadContent()
        {

         

            PersistentManager.LoadContent();



        }
        public void AssignCharactersToStages(string newStageName, TileManager tileManager, ItemManager itemManager)
            => PersistentManager.AssignCharactersToStages(newStageName, tileManager, itemManager);
        
            public void SwitchStage(string newStageName, TileManager tileManager, ItemManager itemManager)
        {
            PersistentManager.SwitchStage(newStageName, tileManager, itemManager);
        }
        public void Update(GameTime gameTime)
        {
            PersistentManager?.Update(gameTime);
        }
        private void AddNPCCommand(string[] args)
        {
            CurrentContainer.CreateNPC(args[0], Controls.MouseWorldPosition, true);
        }
        private void AddTrainCommand(string[] args)
        {
            CurrentContainer.AddTrain();
        }

        public void Save(BinaryWriter writer)
        {
            PersistentManager.Save(writer);
        }

        public void LoadSave(BinaryReader reader)
        {
            PersistentManager.LoadSave(reader);
        }

        public void CleanUp()
        {
            PersistentManager?.CleanUp();
            StageGrids.Clear();
        }
    }
}
