using System;
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
        public DialogueAction DialogueAction { get; set; }
        public string GoTo { get; set; }
        public List<DialogueRequirement> DialogueRequirements { get; set; }
    }
}
