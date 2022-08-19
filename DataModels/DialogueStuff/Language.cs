using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.DialogueStuff
{
    public class Language
    {
        public string Name { get; set; }

        public Dictionary<char, Point> CharPoints { get; set;- }
    }
}
