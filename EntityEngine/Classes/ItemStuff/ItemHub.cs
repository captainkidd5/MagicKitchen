using EntityEngine.ItemStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityEngine.Classes.ItemStuff
{
    public static class ItemHub
    {
        public static Dictionary<string, ItemManager> ItemManagers { get; private set; }

        public static void Initialize()
        {
            ItemManagers = new Dictionary<string, ItemManager>();
        }
        public static void LoadItemManager(string stageName, ItemManager itemManager)
        {
            ItemManagers.Add(stageName, itemManager);
        }

        public static void Clear()
        {
            ItemManagers.Clear();   
        }
    }
}
