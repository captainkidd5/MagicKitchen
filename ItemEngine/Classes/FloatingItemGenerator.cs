using Globals.Classes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemEngine.Classes
{
    internal class FloatingItemGenerator
    {
        private readonly ItemManager _itemManager;
        private SimpleTimer _spawnNewItemTimer;


        public FloatingItemGenerator(ItemManager itemManager)
        {
            _itemManager = itemManager;
        }

        public void Update(GameTime gameTime)
        {
            if (_spawnNewItemTimer.Run(gameTime))
            {

            }
        }

        private void AddNewFloatingItem()
        {
            
        }
    }
}
