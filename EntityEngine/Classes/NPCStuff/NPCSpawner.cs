using DataModels.NPCStuff;
using EntityEngine.Classes.CharacterStuff;
using EntityEngine.Classes.StageStuff;
using Globals.Classes;
using Globals.Classes.Chance;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes;

namespace EntityEngine.Classes.NPCStuff
{
    internal class NPCSpawner
    {
        private ushort _spawnRate;
        private byte MaxNPCSpawnValue = 5;

        public float TotalNPCSpawnValue { get; set; }

        private StageManager _stageManager;
        public void Load(StageManager stageManager)
        {
            _stageManager = stageManager;
        }
        public void Update(GameTime gameTime)
        {
            if (!Flags.AllowNPCSpawning)
                return;
            if (TotalNPCSpawnValue < MaxNPCSpawnValue)
            {

                if (Settings.Random.Next(0, _spawnRate) < 2)
                {
                    NPCData spawnedNPC = GetWeightedSpawn();
                    string requiredTileType = spawnedNPC.AlwaysSubmerged ? "water" : "land";
                    Vector2? emptyTile = _stageManager.CurrentStage.TileManager.TileLocationHelper.RandomClearPositionWithinRange(Settings.Random, tileType: requiredTileType);
                    if (emptyTile != null)
                    {
                        _stageManager.CurrentStage.NPCContainer.CreateNPC(spawnedNPC.Name, emptyTile.Value, true);
                        //TotalNPCSpawnValue += (float)spawnedNPC.SpawnSlotValue / 100;

                    }
                }
            }

        }

        private NPCData GetWeightedSpawn()
        {
            return ChanceHelper.GetWheelSelection(EntityFactory.WeightedNPCData, Settings.Random) as NPCData;
        }
    }
}
