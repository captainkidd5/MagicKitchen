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
    public class DiningTable : Furniture
    {

        public int TotalSeatingCapacity { get; private set; } = 1;
        public int OccupiedSeatCount { get; internal set; }
        public bool SeatingAvailable => OccupiedSeatCount < TotalSeatingCapacity;


        public DiningTable(Tile tile, TileManager tileManager,
            IntermediateTmxShape intermediateTmxShape,string actionType) :
            base(tile, tileManager, intermediateTmxShape, actionType)
        {
            SubKey = "diningTable";
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (PlayerInClickRange && MouseHovering && Controls.IsClicked)
            {
                if (MayPlaceItem)
                {
                    AddItem(UI.Cursor.HeldItem.Id);
                    UI.Cursor.HeldItemCount--;
                    if (UI.Cursor.HeldItemCount == 0)
                        UI.Cursor.HeldItem = null;
                }
              
            }
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
    }
}
