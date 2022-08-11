﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.DialogueStuff
{
    public class Dialogue
    {
        public string Name { get; set; }
        public Dictionary<int, DSnippet> DialogueText { get; set; }
        public string FlavorText { get; set; }
        public Dictionary<string, DialogueOption> Options {get;set;}
    }
}
