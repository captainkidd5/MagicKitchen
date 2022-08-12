using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DataModels.QuestStuff
{
    public class QuestStep
    {
        [JsonIgnore]

        public bool Completed { get; set; }
        public string StepName{ get; set; }
        
        /// <summary>
        /// Name of the NPC to talk to to get
        /// </summary>
        public string AcquiredFrom { get; set; }
        /// <summary>
        /// Name of the NPC to talk to once conditions are met
        /// </summary>
        public string TurnInto { get; set; }
        /// <summary>
        /// List of prerequisites to complete this STEP in the overall quest
        /// </summary>
        /// <summary>
        /// What the NPC will say to Player at start of sub quest
        /// </summary>
        public string StartText { get; set; }


        /// <summary>
        /// What the NPC says when you talk to them about the quest step when it is not yet completed
        /// </summary>
        public string HelpText { get; set; }
        /// <summary>
        /// What the NPC will say to Player at start of end of sub quest
        /// </summary>
        public string EndText { get; set; }


        public List<PreRequisite> PreRequisites { get; set; }

        public QuestReward QuestReward { get; set; }
    }
}
