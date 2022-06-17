using Globals.Classes;
using Globals.Classes.Console;
using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataModels.Enums;

namespace ItemEngine.Classes
{
    public class FlotsamGenerator
    {
        private readonly ItemManager _itemManager;
        private SimpleTimer _spawnNewItemTimer;

        private readonly int _spawnRadius = 600;

        public FlotsamGenerator(ItemManager itemManager)
        {
            _itemManager = itemManager;
            _spawnNewItemTimer = new SimpleTimer(1f);
        }



        public Vector2? Update(GameTime gameTime)
        {
            if (Flags.SpawnFloatingItems)
            {

                if (_spawnNewItemTimer.Run(gameTime))
                {
                   return GetSpawnLocation();
                }
            }

            return null;
        }
        public Vector2 GetSpawnLocation()
        {

            Direction direction = Vector2Helper.GetRandomDirection();
            Rectangle screenRectangle = Settings.GetVisibleRectangle();
            switch (direction)
            {
                case Direction.None:
                    throw new Exception($"Must specify direction");
                case Direction.Up:
                    return new Vector2(screenRectangle.X +Settings.Random.Next(0, screenRectangle.Width), screenRectangle.Y);
                case Direction.Down:
                    return new Vector2(screenRectangle.X + Settings.Random.Next(0, screenRectangle.Width), screenRectangle.Y + screenRectangle.Height);

                case Direction.Left:
                    return new Vector2(screenRectangle.X, screenRectangle.Y + Settings.Random.Next(0, screenRectangle.Height));

                case Direction.Right:
                    return new Vector2(screenRectangle.X + screenRectangle.Width, screenRectangle.Y + Settings.Random.Next(0, screenRectangle.Height));



            }
            throw new Exception($"Must specify direction");

            Vector2 location = new Vector2(
                Settings.Random.Next((int)Shared.PlayerPosition.X - _spawnRadius, (int)Shared.PlayerPosition.X + _spawnRadius),
                   Settings.Random.Next((int)Shared.PlayerPosition.Y - _spawnRadius, (int)Shared.PlayerPosition.Y + _spawnRadius));
            return location;
        }
        public void AddFlotsam(Vector2 position)
        {
            _itemManager.AddWorldItem(position, "Dirt", 1, WorldItemState.Floating, Vector2Helper.GetTossDirectionFromDirectionFacing(Direction.Down));
        }
    }
}
