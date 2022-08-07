﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Globals.Classes
{
    public static class Flags
    {
        //Will show "developed by" screen if true
        public static bool DisplaySplashScreens = true;
        public static bool ShowTileSelector = true;

        public static bool SpawnCharactersOnNewGame = true;
        public static bool Pause;
        //if true, all buttons will reset their colors to untouched and buttons will not reponse to mouse stuff
        public static bool DisableAllUIUpdates = false;

        public static bool DisablePlayerUIInteractions = false;

        public static bool DisplayPlayAreaCollisions  = false;



        public static bool DisplayMousePosition  = false;
        public static bool DisplayFPS = false;



        public static bool SpawnFlotsam = true;


        public static bool IsNewGame;

        private static bool _firstBootUp = true;


        //Useful for things like registering commands, where commands should only be registered once ber game boot up, and then never again
        public static bool FirstBootUp { get { return _firstBootUp; } set 
            {
                if (value)
                { throw new Exception($"May not set first boot up to true"); }
                _firstBootUp = value; } }

    }
}
