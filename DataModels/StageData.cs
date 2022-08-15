using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public enum IslandDistance : byte
    {
        None = 0,
        Inner = 1,
        Mid = 2,
        Outer = 3
    }
    public class StageData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public int InsertionX { get; set; }
        public int InsertionY { get; set; }

        /// <summary>
        /// Will determine where in the map this island will spawn
        /// </summary>
        public IslandDistance IslandDistance { get; set; }
        public string AmbientSoundPackageName { get; set; }
    }
}
