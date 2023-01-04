using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.ScriptedEventStuff
{
    public class ScriptedEvent
    {
        public string Name { get; set; }
        public List<SubScript> Subscripts { get; set; }

        public void ValidateScripts()
        {
            foreach(SubScript subscript in Subscripts)
            {
                subscript.ValidateScrips();
            }
        }
    }
}
