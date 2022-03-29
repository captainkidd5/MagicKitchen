using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Dynamics;

namespace TiledEngine.Classes.TileAddons.Actions
{

    internal class ActionTile : TileBody
    {
        public ActionTile(Tile tile, TileManager tileManager, IntermediateTmxShape intermediateTmxShape, string actionType) : base(tile, tileManager, intermediateTmxShape)
        {
        }

        public override void Load()
        {
            List<Category> categoriesCollidesWith = new List<Category>() { Category.Player, Category.PlayerBigSensor, Category.Cursor };
            List<Category> collisionCategories = new List<Category>() { Category.Solid, Category.ActionTile };
            if (IntermediateTmxShape.TmxObjectType == TiledSharp.TmxObjectType.Basic)
            {
                AddPrimaryBody(PhysicsManager.CreateRectangularHullBody(BodyType.Dynamic, IntermediateTmxShape.HullPosition,
               IntermediateTmxShape.Width, IntermediateTmxShape.Height,
             collisionCategories, categoriesCollidesWith, OnCollides, OnSeparates, blocksLight: IntermediateTmxShape.BlocksLight));
            }
            else if (IntermediateTmxShape.TmxObjectType == TiledSharp.TmxObjectType.Ellipse)
            {
                AddPrimaryBody(PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, IntermediateTmxShape.HullPosition, IntermediateTmxShape.Radius,
                collisionCategories, categoriesCollidesWith, OnCollides, OnSeparates, blocksLight: IntermediateTmxShape.BlocksLight));
            }
            else
            {
                throw new Exception($"{IntermediateTmxShape.TmxObjectType} is not supported.");
            }

        }

        protected override void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnCollides(fixtureA, fixtureB, contact);

            if (fixtureB.CollisionCategories.HasFlag(Category.Cursor))
            {

            }
        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnSeparates(fixtureA, fixtureB, contact);
        }
    }
}
