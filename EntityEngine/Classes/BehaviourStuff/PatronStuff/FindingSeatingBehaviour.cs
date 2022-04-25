using EntityEngine.Classes.CharacterStuff;
using Globals.Classes;
using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes;
using TiledEngine.Classes.TileAddons;
using TiledEngine.Classes.TileAddons.FurnitureStuff;

namespace EntityEngine.Classes.BehaviourStuff.PatronStuff
{
    internal class FindingSeatingBehaviour : Behaviour
    {
        public bool HasLocatedTable { get; private set; }

        private DiningTable _table;
        public FindingSeatingBehaviour(Entity entity, StatusIcon statusIcon, Navigator navigator, TileManager tileManager, float? timerFrequency) : base(entity, statusIcon, navigator, tileManager, timerFrequency)
        {
        }
        public override void Update(GameTime gameTime, ref Vector2 velocity)
        {
            if (!HasLocatedTable)
            {
                _table = GetTileWithAvailableTable();
                //Todo: Create no seating icon
                if (_table == null)
                    StatusIcon.SetStatus(StatusIconType.NoPath);
                else
                    Navigator.SetTarget(_table.Tile.Position);

            }
        }

        /// <summary>
        /// Gets list of all tiles in current map with tables, then gets a list from that of tiles with seating available,
        /// then returns a random tile from that list. Returns null if no table found
        /// </summary>
        private DiningTable GetTileWithAvailableTable()
        {
            List<Tile> tilesWithTables = TileManager.TileLocator.LocateTile("furniture", "diningTable");
            List<DiningTable> availableDiningTables = new List<DiningTable>();
            foreach (Tile tile in tilesWithTables)
            {
                List<ITileAddon> tileTables = tile.GetAddonsByType(typeof(DiningTable));
                foreach (ITileAddon tileAddon in tileTables)
                {
                    if ((tileAddon as DiningTable).SeatingAvailable)
                    {
                        availableDiningTables.Add((DiningTable)tileAddon);
                        break;
                    }
                }
            }
            if (availableDiningTables.Count == 0)
                return null;
            return availableDiningTables[Settings.Random.Next(0, availableDiningTables.Count)];
        }
    }
}
