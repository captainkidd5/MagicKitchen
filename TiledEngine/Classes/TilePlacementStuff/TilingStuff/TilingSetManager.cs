using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiledEngine.Classes.TilePlacementStuff.TilingStuff
{
    internal class TilingSetManager
    {
        internal Dictionary<string, Dictionary<int, int>> TilingSets { get; private set; }

        public TilingSetManager()
        {
            TilingSets = new Dictionary<string, Dictionary<int, int>>();
        }

       public void AddNewSet(string name, int gid)
        {
            if (TilingSets.ContainsKey(name))
                return;
            TilingSets[name] = FillTilingDictionary(gid);
        }
        private Dictionary<int, int> FillTilingDictionary(int centralGID)
        {
            return new Dictionary<int, int>()
            {
                {0, centralGID -98},{1, centralGID + 3}, {2,  centralGID + 102 },  {3, centralGID + 101}, {4, centralGID + 2}, {5, centralGID + 99},{6,centralGID - 96},
                { 7, centralGID + 100}, {8, centralGID + 103}, {9, centralGID - 97}, {10, centralGID - 99}, {11, centralGID + 1},
                { 12,centralGID - 101}, {13,centralGID - 1}, {14,centralGID - 100}, {15, centralGID}
            };
        }
    }
}
