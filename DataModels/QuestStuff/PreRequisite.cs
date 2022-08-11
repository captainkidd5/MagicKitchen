using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.QuestStuff
{
    public class PreRequisite
    {
        /// <summary>
        /// Names of all quests needed to be complete before can start quest assigned to
        /// </summary>
        public List<string> RequiredQuestNames { get; set; }

        public List<QuestItemRequirement> ItemRequirements { get; set; }

        //TODO: Add more things like lvl reqs
    }
}
