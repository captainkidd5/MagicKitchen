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

        private Dictionary<string, StageData> _stageData;
        public void Load(Dictionary<string, StageData> allStageData)
        {
            _stageData = allStageData;
        }

        public Vector2 GetRandomStageLocation(string stageName)
        {

        }


    }
}
