using DataModels.ItemStuff;
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


        public bool Satisfied(Dictionary<string, int> itemsHeld, List<string> completedQuestNames)
        {
            if(ItemRequirements != null)
            {
                foreach (QuestItemRequirement itemRequirement in ItemRequirements)
                {
                    int itemCountHeld = 0;
                    if(itemsHeld.TryGetValue(itemRequirement.ItemName, out itemCountHeld))
                    {
                        //Player did not have all required items
                        if (itemCountHeld < itemRequirement.Count)
                            return false;
                    }
                }
            }

            if(RequiredQuestNames != null)
                foreach(string requiredQuestName in RequiredQuestNames)
                {
                    if (!completedQuestNames.Contains(requiredQuestName))
                        return false;
                }

            return true;
           
        }
    }
}
