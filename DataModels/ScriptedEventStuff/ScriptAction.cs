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
        Teleport = 4
    }
    public class ScriptAction
    {
        public ScriptActionType Type { get; set; }
        public int TileX { get; set; }
        public int TileY { get; set; }

        /// <summary>
        /// Stage end is MUTUALLY EXCLUSIVE with Zone Start and Zone End
        /// </summary>
        public string StageEnd { get; set; }
        public string ZoneStart { get; set; }
        public string ZoneEnd { get; set; }


        public int PauseForSeconds { get; set; }

        public int Speed { get; set; }

        public void ValidateScript()
        {
            if (TileX > 0 || TileY > 0)
            {
                if (!string.IsNullOrEmpty(ZoneStart))
                    throw new Exception($"May not specify zone start if TileX or TileY has a non-zero value");
                if (!string.IsNullOrEmpty(ZoneEnd))
                    throw new Exception($"May not specify zone end if TileX or TileY has a non-zero value");

                if (string.IsNullOrEmpty(StageEnd))
                    throw new Exception($"MUST specify stage end if providing tilex and tiley");

            }
        }
    }
}
