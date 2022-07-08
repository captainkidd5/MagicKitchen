using DataModels;
using Globals.Classes;
using Globals.Classes.Chance;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityEngine.Classes.NPCStuff
{
    internal class MobSpawner
    {
        private ushort _spawnRate;
        private byte MaxNPCSpawnValue = 5;
        public float TotalNPCSpawnValue { get; set; }
        public void Update(GameTime gameTime)
        {
            if (TotalNPCSpawnValue < MaxNPCSpawnValue)
            {

                if (Settings.Random.Next(0, _spawnRate) < 2)
                {
                    NPCData spawnedNPC = GetWeightedSpawn();
                }
            }

        }

        private NPCData GetWeightedSpawn()
        {
            return ChanceHelper.GetWheelSelection(EntityFactory.WeightedNPCData, Settings.Random) as NPCData;
        }
    }
}
