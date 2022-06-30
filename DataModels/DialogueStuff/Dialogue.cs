using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.DialogueStuff
{
    public class Dialogue
    {
        public string Name { get; set; }
        public string DialogueText { get; set; }
        public string FlavorText { get; set; }
        public Dictionary<string, DialogueOption> Options {get;set;}
    }
}
