using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiledEngine.Classes.TilePlacementStuff.TilingStuff
{
    internal class WangSet
    {
        public Dictionary<int,List<WangTile>> Set { get; set; }

        public WangSet()
        {
            Set = new Dictionary<int, List<WangTile>>();

            for(int i =0; i < 16; i++)
            {
                Set.Add(i, new List<WangTile>());
            }
        }
        public bool ContainsValue(int gid)
        {
            return Set.Any(x => x.Value.Any(y => y.GID == gid));
        }

        public int GetWeightedvalue(int key)
        {
            return Set[key][0].GID;
        }
        /// <summary>
        /// Dirt, grass, water layerout etc
        /// </summary>
        /// <param name="centralGID"></param>
        public void FillBackTiling(int centralGID)
        {
            Set[0].Add(new WangTile(100, centralGID - 98));
            Set[1].Add(new WangTile(100, centralGID + 3));

            Set[2].Add(new WangTile(100, centralGID + 102));
            Set[3].Add(new WangTile(100, centralGID + 101));

            Set[4].Add(new WangTile(100, centralGID + 2));
            Set[5].Add(new WangTile(100, centralGID + 99));
            Set[6].Add(new WangTile(100, centralGID - 96));
            Set[7].Add(new WangTile(100, centralGID + 100));
            Set[8].Add(new WangTile(100, centralGID + 103));
            Set[9].Add(new WangTile(100, centralGID - 97));
            Set[10].Add(new WangTile(100, centralGID - 99));
            Set[11].Add(new WangTile(100, centralGID+1));
            Set[12].Add(new WangTile(100, centralGID - 101));
            Set[13].Add(new WangTile(100, centralGID - 1));
            Set[14].Add(new WangTile(100, centralGID - 100));
            Set[15].Add(new WangTile(100, centralGID ));
        }

        public void FillTall(int centralGID)
        {
            Set[0].Add(new WangTile(100, centralGID + 8));
            Set[1].Add(new WangTile(100, centralGID + 6));

            Set[2].Add(new WangTile(100, centralGID + 10));
            Set[3].Add(new WangTile(100, centralGID + 402));

            Set[4].Add(new WangTile(100, centralGID + 10));
            Set[5].Add(new WangTile(100, centralGID + 398));
            Set[6].Add(new WangTile(100, centralGID + 4));
            Set[7].Add(new WangTile(100, centralGID + 400));
            Set[8].Add(new WangTile(100, centralGID + 6));
            Set[9].Add(new WangTile(100, centralGID + 6));
            Set[10].Add(new WangTile(100, centralGID - 396));
            Set[11].Add(new WangTile(100, centralGID + 2));
            Set[12].Add(new WangTile(100, centralGID - 400));
            Set[13].Add(new WangTile(100, centralGID - 2));
            Set[14].Add(new WangTile(100, centralGID - 398));
            Set[15].Add(new WangTile(100, centralGID));
        }

        public void FillFence(int centralGID)
        {
            Set[0].Add(new WangTile(100, centralGID));
            Set[1].Add(new WangTile(100, centralGID -200));

            Set[2].Add(new WangTile(100, centralGID -1));
            Set[3].Add(new WangTile(100, centralGID + -201));

            Set[4].Add(new WangTile(100, centralGID -3));
            Set[5].Add(new WangTile(100, centralGID -203));
            Set[6].Add(new WangTile(100, centralGID -2));
            Set[7].Add(new WangTile(100, centralGID -202));
            Set[8].Add(new WangTile(100, centralGID));
            Set[9].Add(new WangTile(100, centralGID ));
            Set[10].Add(new WangTile(100, centralGID - 1));
            Set[11].Add(new WangTile(100, centralGID -201));
            Set[12].Add(new WangTile(100, centralGID - 3));
            Set[13].Add(new WangTile(100, centralGID - 203));
            Set[14].Add(new WangTile(100, centralGID - 2));
            Set[15].Add(new WangTile(100, centralGID -202));
        }
        public void FillFoliage(int centralGID)
        {
            Set[0].Add(new WangTile(100, centralGID -96));
            Set[1].Add(new WangTile(100, centralGID + 6));

            Set[2].Add(new WangTile(100, centralGID + 202));
            Set[3].Add(new WangTile(100, centralGID + 5));

            Set[4].Add(new WangTile(100, centralGID + 199));
            Set[5].Add(new WangTile(100, centralGID -192));
            Set[6].Add(new WangTile(100, centralGID + 400));
            Set[7].Add(new WangTile(100, centralGID + 306));
            Set[8].Add(new WangTile(100, centralGID -94));
            Set[9].Add(new WangTile(100, centralGID - 98));
            Set[10].Add(new WangTile(100, centralGID - 196));
            Set[11].Add(new WangTile(100, centralGID + 2));
            Set[12].Add(new WangTile(100, centralGID - 101));
            Set[13].Add(new WangTile(100, centralGID - 1));
            Set[14].Add(new WangTile(100, centralGID - 100));
            Set[15].Add(new WangTile(100, centralGID));
        }
    }
}
