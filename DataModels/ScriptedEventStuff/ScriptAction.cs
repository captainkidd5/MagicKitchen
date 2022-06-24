using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.ScriptedEventStuff
{
    public enum ScriptActionType
    {
        None = 0,
        Move = 1,
        Pause = 2,
        Unload = 3,
    }
    public class ScriptAction
    {
        public ScriptActionType Type { get; set; }
        public int TileX { get; set; }
        public int TileY { get; set; }

        public string ZoneStart { get; set; }
        public string ZoneEnd { get; set; }


        public int PauseForSeconds { get; set; }

        public int Speed { get; set; }
    }
}
