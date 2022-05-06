using DataModels.MapStuff;
using InputEngine.Classes.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;
using UIEngine.Classes;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Dynamics;
using static DataModels.Enums;

namespace TiledEngine.Classes.TileAddons.FurnitureStuff
{
    public class DiningTable : StorableFurniture
    {
        public Dictionary<Direction, bool> Seats { get; set; }
        public int TotalSeatingCapacity { get; private set; } = 4;
        public int OccupiedSeatCount { get; internal set; }
        public bool SeatingAvailable => OccupiedSeatCount < TotalSeatingCapacity;


        public DiningTable(FurnitureData furnitureData, Tile tile, TileManager tileManager,
            IntermediateTmxShape intermediateTmxShape,string actionType) :
            base(furnitureData,tile, tileManager, intermediateTmxShape, actionType)
        {
        }
        protected override void AddPlacedItems(FurnitureData furnitureData, Tile tile)
        {
            Seats = new Dictionary<Direction, bool>();
            for(int i =0; i < TotalSeatingCapacity; i++)
            {
                Seats.Add((Direction)(i + 1), false);
            }
            TotalStorageCapacity = 5;
            for (int i = 0; i < (TotalStorageCapacity); i++)
            {
                PlacedItems.Add(new PlacedItem(i, tile));
            }
        }

        /// <summary>
        /// Gets the placed item corresponding to where the patron is seated. For example a patron seated at the left side of the table
        /// will get the item placed on the left side of the table
        /// </summary>
        /// <param name="directionSeatedAt">The side of the table the patron is at</param>
        public PlacedItem GetPlacedItemFromSeatedDirection(Direction directionSeatedAt)
        {
            switch (directionSeatedAt)
            {
                case Direction.None:
                    throw new Exception($"Invalid seating location");
                case Direction.Up:
                    return PlacedItems[0];
                case Direction.Down:
                    return PlacedItems[4];

                case Direction.Left:
                    return PlacedItems[1];

                case Direction.Right:
                    return PlacedItems[3];

            }
            return PlacedItems[(int)directionSeatedAt];
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
           
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

        protected override void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnCollides(fixtureA, fixtureB, contact);
        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnSeparates(fixtureA, fixtureB, contact);
        }



        public override void Load()
        {
            base.Load();
        }
    }
}
