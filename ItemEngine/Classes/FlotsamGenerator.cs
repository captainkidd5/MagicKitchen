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

        private readonly int _spawnRadius = 30;

        public FlotsamGenerator(ItemManager itemManager)
        {
            _itemManager = itemManager;
            _spawnNewItemTimer = new SimpleTimer(1f);
        }



        public void Update(GameTime gameTime)
        {
            if (Flags.SpawnFloatingItems)
            {

                if (_spawnNewItemTimer.Run(gameTime))
                {
                    AddFlotsam();
                }
            }

        }
        public Vector2 GetSpawnLocation()
        {
            Vector2 location = new Vector2(
                Settings.Random.Next((int)Shared.PlayerPosition.X - _spawnRadius, (int)Shared.PlayerPosition.X + _spawnRadius),
                   Settings.Random.Next((int)Shared.PlayerPosition.Y - _spawnRadius, (int)Shared.PlayerPosition.Y + _spawnRadius));
            return location;
        }
        private void AddFlotsam()
        {
            _itemManager.AddWorldItem(GetSpawnLocation(), "Dirt", 1, WorldItemState.Floating, Vector2Helper.GetTossDirectionFromDirectionFacing(Direction.Down));
        }
    }
}
