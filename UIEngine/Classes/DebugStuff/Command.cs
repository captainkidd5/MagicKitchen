using DataModels;
using EntityEngine.Classes.PlayerStuff;
using Globals.Classes;
using Globals.Classes.Console;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace UIEngine.Classes.DebugStuff
{
    abstract class Command
    {
        protected string returnInfo;

        protected string Keyword;
        protected CmdArg[] Args;
        protected CustomConsole console;
        public string Description { get; protected set; }

        internal Command(CustomConsole console)
        {
            this.console = console;
        }

        /// <summary>
        /// Returns true if this is the correct command to use with the arguments
        /// </summary>
        public bool IsCommand(string[] args)
        {
            if (args[0] == Keyword)
                return true;

            return false;
        }

        protected bool CorrectArgAmount(string[] userInput)
        {
            if (Args.Length != userInput.Length)
                return false;
            return true;
        }
        public virtual void ExecuteCommand(string[] userInput)
        {

        }

        public void Clear()
        {
            returnInfo = string.Empty;
        }

        public string GetInfo()
        {
            string returnInfo = GetType().Name + ": " + Description + "\n";
            int argNum = 0;
            foreach (var arg in Args)
            {
                returnInfo += argNum.ToString() + ": " + arg.Description + "\n";
                argNum++;
            }
            return returnInfo;
        }
    }

    internal class CmdArg
    {
        private readonly Type type;
        private readonly int index;
        public string Description { get; }

        internal CmdArg(Type type, int index, string description = "no description set")
        {
            this.type = type;
            this.index = index;
            Description = description;
        }

        string GetErrorMsg()
        {
            return ($"Argument {index} requires type {type.ToString()}");
        }
        /// <summary>
        /// Returns correct type if argument is valid, otherwise returns null
        /// </summary>
        /// <param name="enteredVal">User input</param>
        internal object Validate(string enteredVal, ref string errMsg)
        {

            if (type == typeof(int))
            {
                int result = 0;
                if (int.TryParse(enteredVal, out result))
                    return result;
                else
                {
                    errMsg += GetErrorMsg();
                    return null;

                }
            }
            else if (type == typeof(float))
            {
                float result = 0;
                if (float.TryParse(enteredVal, out result))
                    return result;
                else
                {
                    errMsg += GetErrorMsg();
                    return null;

                }
            }
            else if (type == typeof(double))
            {
                double result = 0;
                if (double.TryParse(enteredVal, out result))
                    return result;
                else
                {
                    errMsg += GetErrorMsg();
                    return null;

                }
            }
            else if (type == typeof(string))
            {
                return enteredVal;
            }
            else
            {
                errMsg += $"Index {index} had no recognized type";
                return null;
            }

        }
    }

    internal class TeleportCommand : Command
    {
        public TeleportCommand(CustomConsole console) : base(console)
        {
            Keyword = "tp";
            Args = new CmdArg[2] { new CmdArg(typeof(float), 0, "X pos"), new CmdArg(typeof(float), 1, "Y pos") };
            Description = "Teleports player to specified position";

            CommandConsole.RegisterCommand(Keyword, Description, ExecuteCommand);
        }
        public override void ExecuteCommand(string[] userInput)
        {
            base.ExecuteCommand(userInput);

            var x = Args[0].Validate(userInput[0], ref returnInfo);

            if (x == null)
            {
                CommandConsole.Append(returnInfo);
                return;

            }

            var y = Args[0].Validate(userInput[1], ref returnInfo);

            if (y == null)
            {
                CommandConsole.Append(returnInfo);
                return;
            }


            PlayerManager.Player1.Move(new Vector2((float)x, (float)y));

            CommandConsole.Append($"Teleported player to {x},{y}");

        }
    }

    internal class GetPositionCommand : Command
    {
        public GetPositionCommand(CustomConsole console) : base(console)
        {
            Keyword = "pos";
            Args = new CmdArg[1] { new CmdArg(typeof(float), 0, "X pos")};
            Description = "Gets player's position";

            CommandConsole.RegisterCommand(Keyword, Description, ExecuteCommand);
        }
        public override void ExecuteCommand(string[] userInput)
        {
            base.ExecuteCommand(userInput);


                CommandConsole.Append(PlayerManager.Player1.Position.ToString());
                return;




        }
    }

    internal class Toggle_Debug : Command
    {
        public Toggle_Debug(CustomConsole console) : base(console)
        {
            Keyword = "toggle_debug";
            Args = new CmdArg[0] { };
            Description = "Switches on or off debug";
            CommandConsole.RegisterCommand(Keyword, Description, ExecuteCommand);


        }
        public override void ExecuteCommand(string[] userInput)
        {
            if (!CorrectArgAmount(userInput))
            {
                CommandConsole.Append($"Error: Expected {Args.Length} arguments but received {userInput.Length}.");
                return;
            }


            Flags.DebugVelcro = !Flags.DebugVelcro;
            Flags.DisplayMousePosition = !Flags.DisplayMousePosition;
            CommandConsole.Append($"Toggled debug to {Flags.DebugVelcro.ToString()}");


        }
    }
    internal class Toggle_Night : Command
    {
        public Toggle_Night(CustomConsole console) : base(console)
        {
            Keyword = "t_night";
            Args = new CmdArg[0] { };
            Description = "Switches on or off night time";
            CommandConsole.RegisterCommand(Keyword, Description, ExecuteCommand);


        }
        public override void ExecuteCommand(string[] userInput)
        {
            if (!CorrectArgAmount(userInput))
            {
                CommandConsole.Append($"Error: Expected {Args.Length} arguments but received {userInput.Length}.");
                return;
            }


            Flags.IsNightTime = !Flags.IsNightTime;
            CommandConsole.Append($"Toggled night to {Flags.IsNightTime.ToString()}");


        }
    }
    internal class Toggle_Shadows : Command
    {
        public Toggle_Shadows(CustomConsole console) : base(console)
        {
            Keyword = "toggle_shadows";
            Args = new CmdArg[0] { };
            Description = "Switches on or off lighting system";
            CommandConsole.RegisterCommand(Keyword, Description, ExecuteCommand);


        }
        public override void ExecuteCommand(string[] userInput)
        {
            if (!CorrectArgAmount(userInput))
            {
                CommandConsole.Append($"Error: Expected {Args.Length} arguments but received {userInput.Length}.");
                return;
            }



            Flags.EnableShadows = !Flags.EnableShadows;
            CommandConsole.Append($"Toggled shadows to {Flags.EnableShadows.ToString()}");



        }
    }
    internal class Toggle_Path : Command
    {
        public Toggle_Path(CustomConsole console) : base(console)
        {
            Keyword = "toggle_path";
            Args = new CmdArg[0] { };
            Description = "Switches on or off pathgrid";
            CommandConsole.RegisterCommand(Keyword, Description, ExecuteCommand);


        }
        public override void ExecuteCommand(string[] userInput)
        {




            Flags.DebugGrid = !Flags.DebugGrid;
            CommandConsole.Append($"Toggled weight to {Flags.DebugGrid.ToString()}");



        }
    }

    internal class ExitCommand : Command
    {
        public ExitCommand(CustomConsole console) : base(console)
        {
            Keyword = "exit";
            Args = new CmdArg[0] { };
            Description = "Immedietaly exits the game";

            CommandConsole.RegisterCommand(Keyword, Description, ExecuteCommand);

        }
        public override void ExecuteCommand(string[] userInput)
        {
            base.ExecuteCommand(userInput);
            if (!CorrectArgAmount(userInput))
            {
                CommandConsole.Append($"Error: Expected {Args.Length + 1} arguments but received {userInput.Length}.");
                return;
            }
            //TODO: game.Exit();

            CommandConsole.Append($"Exited");
        }
    }

    internal class GiveCommand : Command
    {
        public GiveCommand(CustomConsole console) : base(console)
        {
            Description = "Gives entity x amount of item";

            Keyword = "give";
            Args = new CmdArg[3] {
                new CmdArg(typeof(string), 0, "Entity to give"),
                new CmdArg(typeof(int), 1, "Id of thing"),
                new CmdArg(typeof(int), 2, "Amount to give") };
            CommandConsole.RegisterCommand(Keyword, Description, ExecuteCommand);

        }
        public override void ExecuteCommand(string[] userInput)
        {
            base.ExecuteCommand(userInput);
            if (!CorrectArgAmount(userInput))
            {
                CommandConsole.Append($"Error: Expected {Args.Length + 1} arguments but received {userInput.Length}.");
                return;
            }
            //TODO have this depend on input
            Player player = PlayerManager.Player1;
            var entityToGive = Args[0].Validate(userInput[0], ref returnInfo);

            if (entityToGive == null)
            {
                CommandConsole.Append($"Invalid entity {userInput[0]}");
                return;
            }

            var id = Args[1].Validate(userInput[1], ref returnInfo);

            if (id == null)
            {
                CommandConsole.Append($"Invalid item id: {userInput[1]}");
                return;

            }

            if (!ItemFactory.DoesItemExist((int)id))
            {
                CommandConsole.Append($"Item with id {id} does not exist");
                return;

            }

            var countToGive = Args[2].Validate(userInput[2], ref returnInfo);

            if (countToGive == null)
            {
                CommandConsole.Append($"Invalid count: {userInput[2]}");
                return;

            }

           // int amountLeftOver = player.GiveItem((int)id, (int)countToGive);

           // CommandConsole.Append($"Gave player {player.Name} {(int)countToGive - amountLeftOver} of {id}");

        }
    }

    internal class ListCommand : Command
    {
        public ListCommand(CustomConsole console) : base(console)
        {
            Description = "Lists all of x type";

            Keyword = "list_items";
            Args = new CmdArg[1] {
                new CmdArg(typeof(string), 0, "Thing to list out"),
            };
            CommandConsole.RegisterCommand(Keyword, Description, ExecuteCommand);

        }
        public override void ExecuteCommand(string[] userInput)
        {
            base.ExecuteCommand(userInput);
            if (!CorrectArgAmount(userInput))
            {
                CommandConsole.Append($"Error: Expected {Args.Length + 1} arguments but received {userInput.Length}.");
                return;
            }

            var thingToList = Args[0].Validate(userInput[0], ref returnInfo);
            string returnString = string.Empty;
            switch (thingToList)
            {
                case "items":
                    foreach (KeyValuePair<int, ItemData> pair in ItemFactory.ItemDictionary)
                    {
                        returnString += $"{pair.Value.Id}: {pair.Value.Name} \n";
                    }
                    CommandConsole.Append(returnString);
                    return;
                default:
                    CommandConsole.Append("Second value invalid.");
                    return;
            }


        }
    }

    internal class SetResolutionCommand : Command
    {
        public SetResolutionCommand(CustomConsole console) : base(console)
        {
            Description = "Sets resolution to specified preset resolution";

            Keyword = "sr";
            Args = new CmdArg[1] {
                new CmdArg(typeof(string), 0, "resolution index"),
            };
            CommandConsole.RegisterCommand(Keyword, Description, ExecuteCommand);


        }
        public override void ExecuteCommand(string[] userInput)
        {
            base.ExecuteCommand(userInput);
            if (!CorrectArgAmount(userInput))
            {
                CommandConsole.Append($"Error: Expected {Args.Length + 1} arguments but received {userInput.Length}.");

                return;
            }
            //TODO have this depend on input
            var resolutionRequest = Args[0].Validate(userInput[0], ref returnInfo);

            if (resolutionRequest == null)
            {
                CommandConsole.Append($"Invalid resolution request {userInput[0]}");
                return;
            }

            string res = (string)resolutionRequest;
            int resX = 640;
            int resY = 480;


            switch (res)
            {
                case "full":
                    Settings.ToggleFullscreen();
                    CommandConsole.Append($"Toggled fullscreen");
                    return;

                case "1":
                    resX = 640;
                    resY = 480;
                    break;
                case "2":
                    resX = 1280;
                    resY = 720;
                    break;
                case "3":
                    resX = 1980;
                    resY = 1080;
                    break;
            }
            Settings.SetResolution(resX, resY);
            CommandConsole.Append($"Set resolution to {resX} by {resY}");

        }
    }

    internal class SetZoomCommand : Command
    {
        public SetZoomCommand(CustomConsole console) : base(console)
        {
            Description = "Sets camera zoom to given float";

            Keyword = "sz";
            Args = new CmdArg[1] {
                new CmdArg(typeof(string), 0, "zoom level"),
            };
            CommandConsole.RegisterCommand(Keyword, Description, ExecuteCommand);

        }
        public override void ExecuteCommand(string[] userInput)
        {
            base.ExecuteCommand(userInput);
            if (!CorrectArgAmount(userInput))
            {
                CommandConsole.Append($"Error: Expected {Args.Length + 1} arguments but received {userInput.Length}.");
                return;
            }
            //TODO have this depend on input
            var resolutionRequest = Args[0].Validate(userInput[0], ref returnInfo);

            if (resolutionRequest == null)
            {
                CommandConsole.Append($"Invalid resolution request {userInput[0]}");
                return;
            }

            string res = (string)resolutionRequest;

            Settings.CameraZoom = float.Parse(res);

            CommandConsole.Append($"Set zoom to {res}.");

        }
    }

    internal class CamLockCommand : Command
    {
        public CamLockCommand(CustomConsole console) : base(console)
        {
            Description = "Prevents camera from moving outside of map bounds";

            Keyword = "camlock";
            Args = new CmdArg[0] {
            };
            CommandConsole.RegisterCommand(Keyword, Description, ExecuteCommand);

        }
        public override void ExecuteCommand(string[] userInput)
        {
            base.ExecuteCommand(userInput);
            if (!CorrectArgAmount(userInput))
            {
                CommandConsole.Append($"Error: Expected {Args.Length + 1} arguments but received {userInput.Length}.");
                return;
            }


            Settings.Camera.LockBounds = !Settings.Camera.LockBounds;

            CommandConsole.Append($"Toggled camera lock to {Settings.Camera.LockBounds.ToString()}.");

        }
    }
    internal class HelpCommand : Command
    {
        public HelpCommand(CustomConsole console) : base(console)
        {
            Description = "Shows the list of all commands";

            Keyword = "help";
            Args = new CmdArg[0] {
            };
            CommandConsole.RegisterCommand(Keyword, Description, ExecuteCommand);

        }
        public override void ExecuteCommand(string[] userInput)
        {
            base.ExecuteCommand(userInput);
            if (!CorrectArgAmount(userInput))
            {
                CommandConsole.Append($"Error: Expected {Args.Length + 1} arguments but received {userInput.Length}.");
                return;

            }

            console.Help();
            CommandConsole.Append("Requested help");

        }
    }
}
