using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.DialogueStuff
{
    public class DialogueOption
    {
        public string DialogueText { get; set; }
        public string FlavorText { get; set; }
        public List<DialogueRequirement> DialogueRequirements { get; set; }
    }
}
