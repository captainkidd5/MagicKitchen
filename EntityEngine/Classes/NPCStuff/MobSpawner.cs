using DataModels;
using EntityEngine.Classes.CharacterStuff;
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
    internal class MobSpawner
    {
        private ushort _spawnRate;
        private byte MaxNPCSpawnValue = 5;
        private TileManager _tileManager;
        private NPCContainer _npcContainer;
        public float TotalNPCSpawnValue { get; set; }

        public void Load(NPCContainer npcContainer, TileManager tileManager)
        {
            _npcContainer = npcContainer;
            _tileManager = tileManager;
        }
        public void Update(GameTime gameTime)
        {
            if (TotalNPCSpawnValue < MaxNPCSpawnValue)
            {

                if (Settings.Random.Next(0, _spawnRate) < 2)
                {
                    NPCData spawnedNPC = GetWeightedSpawn();
                    string requiredTileType = spawnedNPC.AlwaysSubmerged ? "water" : "land";
                    Vector2? emptyTile = _tileManager.TileLocationHelper.RandomClearPositionWithinRange(Settings.Random,tileType: requiredTileType);
                    if (emptyTile != null)
                    {
                        _npcContainer.CreateNPC(spawnedNPC.Name, emptyTile.Value, true);
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
