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

namespace EntityEngine.Classes.Flotsam
{
    public class FlotsamGenerator
    {
        private readonly ItemManager _itemManager;
        private readonly TileManager _tileManager;
        private SimpleTimer _spawnNewItemTimer;

        private readonly int _spawnRadius = 600;

        public FlotsamGenerator(ItemManager itemManager, TileManager tileManager)
        {
            _itemManager = itemManager;
            _tileManager = tileManager;
            _spawnNewItemTimer = new SimpleTimer(1f);
        }



        public void Update(GameTime gameTime)
        {
            if (Flags.SpawnFloatingItems)
            {

                if (_spawnNewItemTimer.Run(gameTime))
                {
                    Direction direction = Vector2Helper.GetRandomDirection();
                    Vector2 spawnLocation = GetSpawnLocation(direction);
                    if (_tileManager.IsWatertile(spawnLocation))
                    {
                        AddFlotsam(spawnLocation,GetJettisonDirection(spawnLocation));
                    }
                }
            }

        }

        private Vector2 GetJettisonDirection(Vector2 spawnLocation)
        {
            Vector2 directionVector = spawnLocation - Shared.PlayerPosition;
            //directionVector.Normalize();
            return directionVector;
        }
        public Vector2 GetSpawnLocation(Direction direction)
        {

            Rectangle screenRectangle = Settings.GetVisibleRectangle();
            switch (direction)
            {
                case Direction.None:
                    throw new Exception($"Must specify direction");
                case Direction.Up:
                    return new Vector2(screenRectangle.X + Settings.Random.Next(0, screenRectangle.Width), screenRectangle.Y);
                case Direction.Down:
                    return new Vector2(screenRectangle.X + Settings.Random.Next(0, screenRectangle.Width), screenRectangle.Y + screenRectangle.Height);

                case Direction.Left:
                    return new Vector2(screenRectangle.X, screenRectangle.Y + Settings.Random.Next(0, screenRectangle.Height));

                case Direction.Right:
                    Vector2 spawnLocation = new Vector2(screenRectangle.X + screenRectangle.Width, screenRectangle.Y + Settings.Random.Next(0, screenRectangle.Height));
                    return spawnLocation;


            }
            throw new Exception($"Must specify direction");

        }


        public void AddFlotsam(Vector2 position, Vector2 jettisonDirection)
        {
            _itemManager.AddWorldItem(position, ItemFactory.GetRandomFlotsam().Name, 1, WorldItemState.Floating, jettisonDirection);
        }
    }
}
