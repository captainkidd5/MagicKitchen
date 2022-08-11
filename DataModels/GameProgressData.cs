using DataModels.QuestStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public class GameProgressData
    {
        public string Path { get; set; }
        public Dictionary<ushort, bool> DiscoveredItems { get; set; }

        public Dictionary<ushort, bool> DiscoveredRecipes { get; set; }

        public Dictionary<string, Quest> QuestProgress { get; set; }
        public GameProgressData()
        {
            DiscoveredItems = new Dictionary<ushort, bool>();
            DiscoveredRecipes = new Dictionary<ushort, bool>();
            QuestProgress = new Dictionary<string, Quest>();
        }

        public bool IsItemDiscovered(ushort id) => DiscoveredItems.ContainsKey(id);

        public void DiscoverNewItem(ushort id) 
        {
            DiscoveredItems.Add(id, true);

        } 

        public void StartNewQuest(Quest quest)
        {
            if (QuestProgress.ContainsKey(quest.Name))
                throw new Exception($"Quest with name {quest.Name} has already been started");

            QuestProgress.Add(quest.Name, quest);
        }

        public void CompleteQuestStep(Quest quest, int index)
        {
            QuestProgress[quest.Name].Steps[index].Completed = true;
        }
    }
}
