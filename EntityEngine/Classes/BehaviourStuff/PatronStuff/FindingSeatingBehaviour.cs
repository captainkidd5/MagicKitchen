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
using TiledEngine.Classes.Helpers;
using TiledEngine.Classes.TileAddons;
using TiledEngine.Classes.TileAddons.FurnitureStuff;
using TiledEngine.Classes.TileAddons.FurnitureStuff.SeatingFurniture;
using static DataModels.Enums;

namespace EntityEngine.Classes.BehaviourStuff.PatronStuff
{
    internal class FindingSeatingBehaviour : Behaviour
    {
        public bool HasLocatedTable { get; private set; }

        private DiningTable _table;
        private readonly PatronBehaviour _patronBehaviour;

        public FindingSeatingBehaviour(PatronBehaviour patronBehaviour, Entity entity, StatusIcon statusIcon, Navigator navigator, TileManager tileManager, float? timerFrequency) : base(entity, statusIcon, navigator, tileManager, timerFrequency)
        {
            _patronBehaviour = patronBehaviour;
        }
        public override void Update(GameTime gameTime, ref Vector2 velocity)
        {


            if (!HasLocatedTable && SimpleTimer.Run(gameTime))
            {
                _table = GetTileWithAvailableTable();
                //Todo: Create no seating icon
                if (_table == null)
                {
                    StatusIcon.SetStatus(StatusIconType.NoPath);

                }
                else
                {
                    List<Point> clearPoints = TileLocationHelper.GetSorroundingClearTilesAsPoints(
                        TileManager.PathGrid,
                        _table.Tile);

                    //table has no adjacent clear spots
                    if (clearPoints.Count < 1)
                    {
                        StatusIcon.SetStatus(StatusIconType.NoTable);
                    }
                    else
                    {

                        Point chosenPoint = clearPoints[Settings.Random.Next(0, clearPoints.Count)];

                        if (Navigator.FindPathTo(
                            Entity.Position, Vector2Helper.GetWorldPositionFromTileIndex(
                                chosenPoint.X,
                                chosenPoint.Y)))



                        {
                            Navigator.SetTarget(_table.Tile.Position);
                            HasLocatedTable = true;
                        }
                        else
                        {
                            StatusIcon.SetStatus(StatusIconType.NoTable);

                        }
                    }


                }

            }
            else if (HasLocatedTable && Navigator.HasActivePath)
            {
                if (Navigator.FollowPath(gameTime, Entity.Position, ref velocity))
                {
                    Direction direction = Vector2Helper.GetDirectionOfEntityInRelationToEntity(
                        Entity.Position, _table.Tile.CentralPosition);
                    Entity.FaceDirection(direction);
                    Direction tableDirection = Vector2Helper.GetOppositeDirection(direction);
                    if (_table.SitDown(tableDirection))
                    {
                        Entity.Halt();

                        _patronBehaviour.ChangePatronStateToOrdering(_table, tableDirection);
                    }
                    else
                    {
                        //Else someone else got to the table before you, too bad!
                        HasLocatedTable = false;
                        StatusIcon.SetStatus(StatusIconType.NoTable);

                        Entity.Halt();
                    }
                    
                }

            }
        }

        /// <summary>
        /// Gets list of all tiles in current map with tables, then gets a list from that of tiles with seating available,
        /// then returns a random tile from that list. Returns null if no table found
        /// </summary>
        private DiningTable GetTileWithAvailableTable()
        {
            
            
            
            
            
            
            List<Tile> tilesWithTables = TileManager.TileLocator.LocateTile("furniture", "DiningTable");
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
