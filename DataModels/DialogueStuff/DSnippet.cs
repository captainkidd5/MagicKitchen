using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.DialogueStuff
{
    public class DSnippet
    {
        public string DialogueText { get; set; }
        public byte IndexX{ get; set; }
        public byte IndexY { get; set; }

        public byte GoTo { get; set; }

    }
}
