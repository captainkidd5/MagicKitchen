using EntityEngine.Classes.CharacterStuff;
using Globals.Classes.Console;
using InputEngine.Classes.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityEngine.Classes.NPCStuff
{
    public class NPCManager
    {
        public NPCContainer CurrentContainer { get; set; }
        public void LoadContent()
        {

            CommandConsole.RegisterCommand("add_npc", "adds npc to current stage", AddNPCCommand);
            CommandConsole.RegisterCommand("train", "forces train into current stage", AddTrainCommand);


        }
    
        private void AddNPCCommand(string[] args)
        {
            CurrentContainer.CreateNPC(args[0], Controls.CursorWorldPosition);
        }
        private void AddTrainCommand(string[] args)
        {
            CurrentContainer.AddTrain();
        }


    }
}
