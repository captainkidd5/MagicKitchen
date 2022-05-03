using InputEngine.Classes;
using InputEngine.Classes.Input;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;
using UIEngine.Classes;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Dynamics;

namespace TiledEngine.Classes.TileAddons.Actions
{

    public class ActionTile : TileBody
    {
        public CursorIconType CursorIconType { get; private set; }

        public ActionTile(Tile tile, TileManager tileManager , IntermediateTmxShape intermediateTmxShape, string actionType) : base(tile, tileManager, intermediateTmxShape)
        {
            CursorIconType = Cursor.GetCursorIconTypeFromString(actionType);

        }

        public override void Load()
        {
            List<Category> categoriesCollidesWith = new List<Category>() { Category.Player, Category.PlayerBigSensor, Category.Cursor };
            List<Category> collisionCategories = new List<Category>() { Category.Solid, Category.ActionTile };
            if (IntermediateTmxShape.TmxObjectType == TiledSharp.TmxObjectType.Basic)
            {
                AddPrimaryBody(PhysicsManager.CreateRectangularHullBody(BodyType.Dynamic, IntermediateTmxShape.HullPosition,
               IntermediateTmxShape.Width, IntermediateTmxShape.Height,
             collisionCategories, categoriesCollidesWith, OnCollides, OnSeparates, blocksLight: IntermediateTmxShape.BlocksLight, mass: 0f)); ;
            }
            else if (IntermediateTmxShape.TmxObjectType == TiledSharp.TmxObjectType.Ellipse)
            {
                AddPrimaryBody(PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, IntermediateTmxShape.HullPosition, IntermediateTmxShape.Radius,
                collisionCategories, categoriesCollidesWith, OnCollides, OnSeparates, blocksLight: IntermediateTmxShape.BlocksLight, mass:0f));
            }
            else
            {
                throw new Exception($"{IntermediateTmxShape.TmxObjectType} is not supported.");
            }
            Move(IntermediateTmxShape.HullPosition);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Move(IntermediateTmxShape.HullPosition);
            if (PlayerInClickRange && MouseHovering)
            {
                UI.Cursor.ChangeCursorIcon(CursorIconType);

            }
        }

        protected override void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnCollides(fixtureA, fixtureB, contact);


        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnSeparates(fixtureA, fixtureB, contact);
        }

        public override void Interact(bool isPlayer)
        {
            base.Interact(isPlayer);
            if (isPlayer)
            {
                if (!PlayerInClickRange)
                    return;
            }
        }
    }
}
