using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.ScriptedEventStuff
{
    public class ScriptAction
    {
        public int TileX { get; set; }
        public int TileY { get; set; }

        public string ZoneStart { get; set; }
        public string ZoneEnd { get; set; }

        public int Speed { get; set; }
    }
}
