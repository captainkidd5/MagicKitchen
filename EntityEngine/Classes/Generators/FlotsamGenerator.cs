using EntityEngine.ItemStuff;
using Globals.Classes;
using Globals.Classes.Helpers;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes;
using static DataModels.Enums;

namespace EntityEngine.Classes.Generators
{
    public class FlotsamGenerator : EntityGenerator
    {


        public FlotsamGenerator(ItemManager itemManager, TileManager tileManager) : base(itemManager, tileManager)
        {
            SpawnRadius = 600;
            SpawnInterval = 1f;
            Load();
        }

        public override void Update(GameTime gameTime)
        {
            if (Flags.SpawnFlotsam)
            {

                if (SpawnTimer.Run(gameTime))
                {
                    Direction direction = Vector2Helper.GetRandomDirection();
                    Vector2 spawnLocation = GetSpawnLocation(direction);
                    if (TileManager.IsTypeOfTile("water",spawnLocation) || TileManager.IsTypeOfTile("deepWater", spawnLocation))
                    {
                        AddFlotsam(spawnLocation,GetJettisonDirection(spawnLocation));
                    }
                }
            }

        }

        private Vector2 GetJettisonDirection(Vector2 spawnLocation)
        {
            Vector2 directionVector = spawnLocation - Shared.PlayerPosition;
            return directionVector;
        }
        public Vector2 GetSpawnLocation(Direction direction)
        {

            return GetRandomPositionAtEdgeOfScreen(direction);

        }


        public void AddFlotsam(Vector2 position, Vector2 jettisonDirection)
        {
            ItemManager.AddWorldItem(position, ItemFactory.GetRandomFlotsam().Name, 1, WorldItemState.Floating, jettisonDirection);
        }
    }
}
