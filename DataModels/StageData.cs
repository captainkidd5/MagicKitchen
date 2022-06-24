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

        public string AmbientSoundPackageName { get; set; }
    }
}
