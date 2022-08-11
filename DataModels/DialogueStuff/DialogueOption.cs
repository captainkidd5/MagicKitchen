﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.DialogueStuff
{
    public enum DialogueAction
    {
        None =0,
        OpenShop = 1,
    }
    public class DialogueOption
    {
        public string Title { get; set; }
        public string DialogueText { get; set; }
        public string FlavorText { get; set; }
        public DialogueAction DialogueAction { get; set; }
        public List<DialogueRequirement> DialogueRequirements { get; set; }
    }
}
