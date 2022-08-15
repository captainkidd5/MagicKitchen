using DataModels;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiledEngine.Classes.Procedural
{
    internal class IslandPlacer
    {
        public IslandPlacer()
        {

        }
        //each tile should be 128 by 128

        private Dictionary<string, StageData> _stageData;
        private WorldSize _worldSize;
        public void Load(WorldSize worldSize, Dictionary<string, StageData> allStageData)
        {
            _worldSize = worldSize;
            _stageData = allStageData;
        }

        //public Vector2 GetRandomStageLocation(string stageName)
        //{

        //}


    }
}
