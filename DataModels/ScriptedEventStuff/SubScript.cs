using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.ScriptedEventStuff
{
    public class SubScript
    {
        public string CharacterName { get; set; }
        public List<ScriptAction> ScriptActions { get; set; }

        public void ValidateScrips()
        {
            foreach(ScriptAction action in ScriptActions)
            {
                action.ValidateScript();
            }
        }
    }
}
