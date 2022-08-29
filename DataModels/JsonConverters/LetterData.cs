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
        /// <summary>
        /// Default is 8, otherwise gap is given. Usually used for wider characters which need more breathing room
        /// </summary>
        public int OverrideGap { get; set; }
        /// <summary>
        /// How long the text builder will pause for after each character. Default will be provided if none given. Result will divide this by 100.
        /// </summary>
        public float PauseDuration { get; set; }
        public Dictionary<Char, int> KerningPairs { get; set; }
    }
}
