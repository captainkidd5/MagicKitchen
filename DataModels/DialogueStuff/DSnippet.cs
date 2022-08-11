using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.DialogueStuff
{
    public class DSnippet
    {
        public string DialogueText { get; set; }
        public byte PortraitIndexX{ get; set; }
        public byte PortraitIndexY { get; set; }

        public string GoTo { get; set; }

    }
}
