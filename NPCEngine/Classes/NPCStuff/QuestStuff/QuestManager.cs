using DataModels.QuestStuff;
using Globals.Classes;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EntityEngine.Classes.NPCStuff.QuestStuff
{
    public class QuestManager : Component
    {
       

        private static List<Quest> AllQuests { get; set; }
        public QuestManager(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
        }
        public void LoadQuestData(List<Quest> allQuests)
        {
            AllQuests = allQuests;
        }

        /// <summary>
        /// Gets all possible quests for a given character which could produce dialogue.
        /// This basically excludes:
        /// 1). Completed Quests
        /// 2). Quests which the player does not have all the requirements
        /// 3). TODO
        /// </summary>
        /// <param name="characterSpecificQuests">quests originating with this character</param>
        /// <returns></returns>
        public List<Quest> GetPossibleQuests(List<Quest> characterSpecificQuests)
        {
            List<Quest> possibleQuests = characterSpecificQuests.Where(
                x => !x.Completed).ToList();


            for(int i = possibleQuests.Count; i > 0; i--)
            {
                if (!CheckAllPreReqs(possibleQuests[i]))
                {
                    possibleQuests.RemoveAt(i);
                }
            }
            return possibleQuests;
        }

        /// <summary>
        /// Returns true if all pre reqs are satisfied.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        private bool CheckAllPreReqs(Quest quest)
        {
            foreach(PreRequisite req in quest.PreRequisites)
            {
                if (!CheckPreRequisite(req))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns true if all pre reqs are fulfilled
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private bool CheckPreRequisite(PreRequisite req)
        {
            return CheckIfQuestsDone(req);
        }

        /// <summary>
        /// Returns true if all required quests are completed.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private bool CheckIfQuestsDone(PreRequisite req)
        {
            if (req.RequiredQuestNames != null)
            {
                foreach (string questname in req.RequiredQuestNames)
                {
                    Quest questInQuestion = AllQuests.FirstOrDefault(x => x.QuestName == questname);
                    if (questInQuestion == null)
                        throw new Exception($"Quest with questname {questname} does not exist!");
                    if (!questInQuestion.Completed)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }
}
