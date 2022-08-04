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

        public GameProgressData()
        {
            DiscoveredItems = new Dictionary<ushort, bool>();
            DiscoveredRecipes = new Dictionary<ushort, bool>();
        }

        public bool IsItemDiscovered(ushort id) => DiscoveredItems.ContainsKey(id);

        public void DiscoverNewItem(ushort id) 
        {
            DiscoveredItems.Add(id, true);

        } 
    }
}
