using Globals.Classes;
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
    internal class FloatingItemGenerator
    {
        private readonly ItemManager _itemManager;
        private SimpleTimer _spawnNewItemTimer;


        public FloatingItemGenerator(ItemManager itemManager)
        {
            _itemManager = itemManager;
            _spawnNewItemTimer = new SimpleTimer(1f);
        }

        public void Update(GameTime gameTime)
        {
            if (_spawnNewItemTimer.Run(gameTime))
            {
                AddNewFloatingItem();
            }
        }
        private Vector2 GetSpawnLocation()
        {
            return Vector2Helper.GetWorldPositionFromTileIndex(Settings.Random.Next(112,144), 111);
        }
        private void AddNewFloatingItem()
        {
            _itemManager.AddFloatingItem(GetSpawnLocation(), "Dirt", 1, Vector2Helper.GetTossDirectionFromDirectionFacing(Direction.Down));
        }
    }
}
