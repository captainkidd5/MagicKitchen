using DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityEngine.Classes.NPCStuff.DialogueStuff
{
    public static class DialogueInterpreter
    {
        public static string GetSpeech(Dialogue dialogue)
        {
            string response = "unable to find text";

            if(!dialogue.SpeechTree.TryGetValue("default", out response))
            {
                return "No default value!";
            }

            return response;
        }
    }
}
