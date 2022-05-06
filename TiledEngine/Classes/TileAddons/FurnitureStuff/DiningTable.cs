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

namespace TiledEngine.Classes.TileAddons.FurnitureStuff
{
    public class DiningTable : StorableFurniture
    {

        public int TotalSeatingCapacity { get; private set; } = 1;
        public int OccupiedSeatCount { get; internal set; }
        public bool SeatingAvailable => OccupiedSeatCount < TotalSeatingCapacity;


        public DiningTable(FurnitureData furnitureData, Tile tile, TileManager tileManager,
            IntermediateTmxShape intermediateTmxShape,string actionType) :
            base(furnitureData,tile, tileManager, intermediateTmxShape, actionType)
        {
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
           
        }
        public bool SitDown()
        {
            if(SeatingAvailable)
            {
                OccupiedSeatCount++;
                return true;
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
