using EntityEngine.Classes.CharacterStuff;
using Globals.Classes;
using Globals.Classes.Console;
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
using System.Threading.Tasks;
using TiledEngine.Classes;

namespace EntityEngine.Classes.NPCStuff
{
    public class NPCManager : ISaveable
    {
        public StageNPCContainer CurrentContainer { get; set; }
        public PersistentManager PersistentManager { get; set; }
        public NPCManager(GraphicsDevice graphics, ContentManager content)
        {
            PersistentManager = new PersistentManager(graphics, content);
        }
        public void LoadContent()
        {

            CommandConsole.RegisterCommand("add_npc", "adds npc to current stage", AddNPCCommand);
            CommandConsole.RegisterCommand("train", "forces train into current stage", AddTrainCommand);

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
            CurrentContainer.CreateNPC(args[0], Controls.CursorWorldPosition);
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
        }
    }
}
