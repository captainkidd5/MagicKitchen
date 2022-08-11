using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.QuestStuff
{
    public class QuestItemRequirement
    {
        public string ItemName { get; set; }
        public ushort Count { get; set; }

        public string Description { get; set; }
    }
}
