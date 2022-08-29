using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.JsonConverters
{
    public class LetterData
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public int OverrideGap { get; set; }
        public Dictionary<Char, int> KerningPairs { get; set; }
    }
}
