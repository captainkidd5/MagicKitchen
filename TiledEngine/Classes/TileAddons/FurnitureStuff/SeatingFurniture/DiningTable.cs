using DataModels.MapStuff;
using InputEngine.Classes.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoundEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;
using UIEngine.Classes;

using static DataModels.Enums;

namespace TiledEngine.Classes.TileAddons.FurnitureStuff
{
    public class DiningTable : StorableFurniture
    {
        private static readonly int s_centerOfTableIndex = 2;
        public Dictionary<Direction, bool> Seats { get; set; }
        public int TotalSeatingCapacity { get; private set; } = 4;
        public int OccupiedSeatCount { get; internal set; }
        public bool SeatingAvailable => OccupiedSeatCount < TotalSeatingCapacity;


        public DiningTable(FurnitureData furnitureData, TileObject tile, TileManager tileManager,
            IntermediateTmxShape intermediateTmxShape,string actionType) :
            base(furnitureData,tile, tileManager, intermediateTmxShape, actionType)
        {
        }
        protected override void AddPlacedItems(FurnitureData furnitureData, TileObject tile)
        {
            Seats = new Dictionary<Direction, bool>();
            for(int i =0; i < TotalSeatingCapacity; i++)
            {
                Seats.Add((Direction)(i + 1), false);
            }
            TotalStorageCapacity = 5;
            for (int i = 0; i < (TotalStorageCapacity); i++)
            {
                PlacedItems.Add(new PlacedOnItem(i, tile));
            }
        }

        /// <summary>
        /// Usually called when a patron is done eating and pays for the food
        /// </summary>
        /// <param name="amount"></param>
        public void AddMoneyToCentralSlot(int amount)
        {
            UnlockPlaceLock(s_centerOfTableIndex);
            AddItemAtIndex(s_centerOfTableIndex, "Coin", amount);
            SetPlaceLock(s_centerOfTableIndex);
            SoundFactory.PlaySoundEffect("CoinJingle");
        }
        /// <summary>
        /// Gets the placed item corresponding to where the patron is seated. For example a patron seated at the left side of the table
        /// will get the item placed on the left side of the table
        /// </summary>
        /// <param name="directionSeatedAt">The side of the table the patron is at</param>
        public PlacedOnItem GetPlacedItemFromSeatedDirection(Direction directionSeatedAt)
        {
            return PlacedItems[GetIndexFromSeating(directionSeatedAt)];
        }
        private int GetIndexFromSeating(Direction directionSeated)
        {
            switch (directionSeated)
            {
                case Direction.None:
                    throw new Exception($"Invalid seating location");
                case Direction.Up:
                    return 0;
                case Direction.Down:
                    return 4;

                case Direction.Left:
                    return 1;

                case Direction.Right:
                    return 3;

            }
            throw new Exception($"Invalid seating location");
        }
        public void DeleteItemFromTable(Direction direction)
        {
            RemoveItemAtIndex(GetIndexFromSeating(direction), 1);
        }
        public bool SitDown(Direction direction)
        {
            if(SeatingAvailable)
            {
                if(!Seats[direction])
                {
                    OccupiedSeatCount++;
                    Seats[direction] = true;
                    return true;
                }
                
            }
            return false;
        }

        public override void Load()
        {
            base.Load();
            //Lock table center slot so player cannot place items there
            SetPlaceLock(s_centerOfTableIndex);
        }
    }
}
