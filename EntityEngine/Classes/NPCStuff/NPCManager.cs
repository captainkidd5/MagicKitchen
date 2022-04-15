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
        private Dictionary<string, NPCContainer> _npcContainerDictionary = new Dictionary<string, NPCContainer>();

        private NPCContainer _currentContainer;
        public void LoadContent()
        {

            _npcContainerDictionary = new Dictionary<string, NPCContainer>();
            CommandConsole.RegisterCommand("add_npc", "adds npc to current stage", AddNPCCommand);
            CommandConsole.RegisterCommand("train", "forces train into current stage", AddTrainCommand);


        }
        public void ChangeContainer(string key)
        {
            _currentContainer = _npcContainerDictionary[key];
        }
        public void AddNewContainer(string stageName, NPCContainer npcContainer)
        {
            _npcContainerDictionary.Add(stageName, npcContainer);
        }

        private void AddNPCCommand(string[] args)
        {
            _currentContainer.CreateNPC(args[0], Controls.CursorWorldPosition);
        }
        private void AddTrainCommand(string[] args)
        {
            _currentContainer.AddTrain();
        }

        public void CleanUp()
        {
            _npcContainerDictionary.Clear();
            _currentContainer = null;
        }
    }
}
