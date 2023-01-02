using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicKitchen.Classes.ConsoleStuff
{
    internal class CommandList
    {
        private List<Command> _commands;

        public CommandList()
        {
        }

        public void Load()
        {
            _commands = new List<Command>()
            {
                new Toggle_Debug(),
                new Toggle_Shadows(),
                new Toggle_Night(),
                new Toggle_Path(),
                new ExitCommand(),
                new ListCommand(),
                new SetResolutionCommand(),
                new HelpCommand(),
                new SetZoomCommand(),
                new CamLockCommand(),
            };
        }
    }
}
