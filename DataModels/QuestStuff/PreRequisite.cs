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


        public bool Satisfied(Dictionary<string, int> itemsHeld, List<Quest> completedQuests)
        {
            if(ItemRequirements != null)
            {
                foreach (QuestItemRequirement itemRequirement in ItemRequirements)
                {
                    int itemCountHeld = 0;
                    if (itemsHeld.TryGetValue(itemRequirement.ItemName, out itemCountHeld))
                    {
                        //Player did not have all required items
                        if (itemCountHeld < itemRequirement.Count)
                            return false;
                    }
                    else
                        return false;
                }
            }

            if(RequiredQuestNames != null)
                foreach(Quest requiredQuest in completedQuests)
                {
                    if (!completedQuests.Contains(requiredQuest))
                        return false;
                }

            return true;
           
        }
    }
}
