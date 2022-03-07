
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
using QuakeConsole;
using UIEngine.Classes.SultanInterpreter;

namespace UIEngine.Classes.DebugStuff
{
    internal class CustomConsole
    {




        public List<Command> Commands { get; set; }
        public CustomConsole()
        {


            Commands = new List<Command>()
            {
                new Toggle_Debug(this),
                new Toggle_Shadows(this),
                new Toggle_Night(this),
                new Toggle_Path(this),
                new ExitCommand(this),
                new ListCommand(this),
                new SetResolutionCommand(this),
                new HelpCommand(this), 
                new SetZoomCommand(this),
                new CamLockCommand(this),
            };

        }

      


        
       
       

    }

    
}
