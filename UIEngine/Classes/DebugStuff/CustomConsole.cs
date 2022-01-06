
using UIEngine.Classes.TextStuff;
using Globals.Classes;
using Globals.Classes.Helpers;
using InputEngine.Classes.Input;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.InterfaceStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextEngine.Classes;
using static Globals.Classes.Settings;
using EntityEngine.Classes.PlayerStuff;
using QuakeConsole;
using UIEngine.Classes.SultanInterpreter;

namespace UIEngine.Classes.DebugStuff
{
    internal class CustomConsole : InterfaceSection
    {
        private Action Command { get; set; }
        private readonly TypingBox typingBox;

        private bool EnableText { get; set; }

        private Queue<string> AllPreviousCommands { get; set; }


        private readonly int maxCommands = 99;

        private DebugDetailWindow DDWidnow { get; set; }

        private ConsoleList ConsoleList { get; set; }



        public int testNum { get; 
            set; }

        public List<Command> Commands { get; set; }
        public CustomConsole( GraphicsDevice graphicsDevice, ContentManager content, Vector2? position)
            : base(graphicsDevice, content, position)
        {
            Position = RectangleHelper.PlaceBottomLeftScreen(TypingBox.DefaultHeight);
            typingBox = new TypingBox(graphicsDevice, content, this, Position, null, null, Color.White * .5f);
            typingBox.ExecuteCommand += ProcessCommand;
            DDWidnow = new DebugDetailWindow();
            ConsoleList = new ConsoleList(Position);
            AllPreviousCommands = new Queue<string>();

            Commands = new List<Command>()
            {
                new TeleportCommand(this),
                new Toggle_Debug(this),
                new Toggle_Shadows(this),
                new Toggle_Night(this),
                new Toggle_Path(this),
                new ExitCommand(this),
                new GetPositionCommand(this),
                new GiveCommand(this),
                new ListCommand(this),
                new SetResolutionCommand(this),
                new HelpCommand(this), 
                new SetZoomCommand(this),
                new CamLockCommand(this),
            };

        }

        public void Help()
        {
            foreach(Command cmd in Commands)
            {
                ConsoleList.AddInfo(cmd.GetInfo());
            }
        }


        public override void Update(GameTime gameTime)
        {
            IsActive = Flags.Pause;
            if (IsActive)
            {
                base.Update(gameTime);
                typingBox.Update(gameTime);
                if(DDWidnow.IsActive)
                    DDWidnow.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                base.Draw(spriteBatch);
                typingBox.Draw(spriteBatch);
                ConsoleList.Draw(spriteBatch);


                if (DDWidnow.IsActive)
                    DDWidnow.Draw(spriteBatch);

            }
        }
        private int PreviousCommandIndex { get; set; }

        /// <param name="direction">Must be UP or DOWN.</param>
        /// <returns>Returns a string from the previous command list.</returns>
        public string GetPreviousCommand(Direction direction)
        {
            //PreviousCommandIndex = AllPreviousCommands.Count;
            PreviousCommandIndex = ScrollHelper.GetIndexFromScroll(direction, PreviousCommandIndex, AllPreviousCommands.Count);

            if (PreviousCommandIndex >= AllPreviousCommands.Count)
                return string.Empty;

            return AllPreviousCommands.ElementAt(PreviousCommandIndex);

        }

        public void ProcessCommand(string command)
        {
            string[] commands = command.Split(" ");
            for (int i = 0; i < commands.Length; i++)
            {
                commands[i] = commands[i].ToLower();

            }
            ConsoleList.AddInfo(command);

            if (commands.Length == 0)
                commands = new string[] { command };


            AllPreviousCommands.Enqueue(command);
            if (AllPreviousCommands.Count > maxCommands)
            {
                AllPreviousCommands.Dequeue();
            }
            string result = string.Empty;
            foreach(Command cmd in Commands)
            {
                if (cmd.IsCommand(commands))
                {
                    //result = cmd.ExecuteCommand(commands);
                    cmd.Clear();
                }
            }

            ConsoleList.AddInfo(result);
            return;

            switch (commands[0])
            {
             

                //TOGGLE DISPLAYING MOUSE POSITION ON SCREEN
                case "mouse":
                    switch (commands[1])
                    {
                        case "position":
                            EnableText = !EnableText;
                            break;
                    }
                    break;

           

                    //case "cycle-clothing":
                    //    switch (commands[1])
                    //    {
                    //        case "skin":
                    //            Game1.PlayerManager.LocalPlayer.Wardrobe.CycleSkin(ControlsStuff.Direction.Right);
                    //            break;
                    //        case "shirt":
                    //            Game1.PlayerManager.LocalPlayer.Wardrobe.CycleShirt();
                    //            break;
                    //        case "hair":
                    //            Game1.PlayerManager.LocalPlayer.Wardrobe.CycleHair(ControlsStuff.Direction.Right);
                    //            break;
                    //        case "pants":
                    //            Game1.PlayerManager.LocalPlayer.Wardrobe.CyclePants(ControlsStuff.Direction.Right);
                    //            break;
                    //    }

                    //    break;

                    //case "detail":
                    //    DetailWindow.Toggle();
                    //    break;

                    //case "create-save":
                    //    Game1.SaveManager.CreateNewSave(commands[1]);
                    //    break;
                    //case "load-save":
                    //    Game1.SaveManager.LoadGame(commands[1]);
                    //    break;

            }


        }


        public override void Toggle()
        {
            base.Toggle();
            PreviousCommandIndex = AllPreviousCommands.Count;
        }

    }

    
}
