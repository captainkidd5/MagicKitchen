using EntityEngine.ItemStuff;
using Globals.Classes;
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

        /// <summary>
        /// Will return a position which is just barely out of view of the viewport, at specified direction
        /// </summary>
        /// <param name="bufferAmt">A small buffer to ensure not partially visible on screen when spawned</param>
        protected Vector2 GetRandomPositionAtEdgeOfScreen(Direction directionOfScreen, int bufferAmt = 64)
        {
            Rectangle screenRectangle = Settings.GetVisibleRectangle();
            switch (directionOfScreen)
            {
                case Direction.None:
                    throw new Exception($"Must specify direction");
                case Direction.Up:
                    return new Vector2(screenRectangle.X + Settings.Random.Next(0, screenRectangle.Width), screenRectangle.Y - bufferAmt);
                case Direction.Down:
                    return new Vector2(screenRectangle.X + Settings.Random.Next(0, screenRectangle.Width), screenRectangle.Y + screenRectangle.Height + bufferAmt);

                case Direction.Left:
                    return new Vector2(screenRectangle.X - bufferAmt, screenRectangle.Y + Settings.Random.Next(0, screenRectangle.Height));

                case Direction.Right:
                    Vector2 spawnLocation = new Vector2(screenRectangle.X + screenRectangle.Width + bufferAmt, screenRectangle.Y + Settings.Random.Next(0, screenRectangle.Height));
                    return spawnLocation;


            }
            throw new Exception($"Must specify direction");
        }
    }
}
