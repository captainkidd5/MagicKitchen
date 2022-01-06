using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public enum MapType
    {
        Exterior = 1,
        Interior = 2,
    }
    public class StageData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public MapType MapType { get; set; }

        public string AmbientSoundPackageName { get; set; }
    }
}
