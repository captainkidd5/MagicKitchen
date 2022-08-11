using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public class StageData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public int InsertionX { get; set; }
        public int InsertionY { get; set; }
        public string AmbientSoundPackageName { get; set; }
    }
}
