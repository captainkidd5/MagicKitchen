using Globals.Classes;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes;

namespace EntityEngine.Classes.Generators
{
    public abstract class EntityGenerator
    {
        protected ItemManager ItemManager { get; set; }
        protected TileManager TileManager { get; set; }
        protected SimpleTimer SpawnTimer { get; set; }
        protected float SpawnInterval { get; set; }

        protected int SpawnRadius { get; set; }
        public EntityGenerator(ItemManager itemManager, TileManager tileManager)
        {
            ItemManager = itemManager;
            TileManager = tileManager;

        }

        public virtual void Load()
        {
            SpawnTimer = new SimpleTimer(SpawnInterval);

        }

        public virtual void Update(GameTime gameTime)
        {

        }

    }
}
