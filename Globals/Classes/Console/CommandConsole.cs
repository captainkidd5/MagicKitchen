using QuakeConsole;
using System;
using System.Collections.Generic;
using System.Text;
using UIEngine.Classes.SultanInterpreter;

namespace Globals.Classes.Console
{
    /// <summary>
    /// Wrapper for Disco Sultan's Quake console
    /// </summary>
    public static class CommandConsole
    {
        private static ConsoleComponent s_consoleComponent;
        private static ManualInterpreter s_manualInterpreter;

        public static void Toggle() => s_consoleComponent.ToggleOpenClose();

        public static bool IsActive => s_consoleComponent.IsVisible;
        public static Dictionary<string, string> Commands { get; private set; }
        public static void Load(ConsoleComponent consoleComponent)
        {
            s_consoleComponent = consoleComponent;
            s_manualInterpreter = new ManualInterpreter();
            s_consoleComponent.Interpreter = s_manualInterpreter;
            Commands = new Dictionary<string, string>();
            RegisterCommand("toggle", "toggles a number of different flags", ToggleAction);
            RegisterCommand("list", "lists all commands", ListCommandsAction);

            s_consoleComponent.Output.Append("test output");
        }

        /// <summary>
        /// Use case: Open quake console, "toggle debug"
        /// </summary>
        /// <param name="commands"></param>
        private static void ToggleAction(string[] commands)
        {
            switch (commands[0])
            {

                case "debug_ui":
                Flags.DisplayMousePosition = !Flags.DisplayMousePosition;
                    break;
                case "t_selector":
                    Flags.ShowTileSelector = !Flags.ShowTileSelector;
                    break;


                case "fps":
                    Flags.DisplayFPS = !Flags.DisplayFPS;
                    break;
                case "flotsam":
                    Flags.SpawnFlotsam = !Flags.SpawnFlotsam;
                    break;
                case "play_area":
                    Flags.DisplayPlayAreaCollisions = !Flags.DisplayPlayAreaCollisions;
                    break;
            }
        }

        private static void ListCommandsAction(string[] commands)
        {
            foreach (KeyValuePair<string, string> command in Commands)
            {
                s_consoleComponent.Output.Append($"{command.Key}, {command.Value}");
            }
        }

        public static void Append(string output)
        {
            s_consoleComponent.Output.Append(output);
        }
        public static void RegisterCommand(string commandName, string description, Action<string[]> command)
        {
            if (!Commands.ContainsKey(commandName))
            {
                Commands.Add(commandName, ":---------------" + description);
                s_manualInterpreter.RegisterCommand(commandName, command);


            }




        }
    }
}
