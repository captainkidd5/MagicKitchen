using DataModels;
using Globals.Classes;
using Globals.Classes.Helpers;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using tainicom.Aether.Physics2D.Dynamics.Joints;
using static DataModels.Enums;
using static PhysicsEngine.Classes.PhysicsManager;

namespace EntityEngine.Classes.ToolStuff
{
    internal class Sword : SwingableTool
    {
        protected override float SwingDuration { get; set; } = .4f;
        protected override RotateSpeed RotateSpeed { get; set; } = RotateSpeed.Woah;
        protected override ActionType ActionType { get; set; } = ActionType.Smash;

        public Sword(Item item) : base(item)
        {

        }
  
        public override void ReleaseTool(Direction direction, Vector2 directionVector, Entity holder)
        {
            base.ReleaseTool(direction, directionVector, holder);
            SoundModuleManager.PlayPackage("Slash");
      

        }


        protected override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories.HasFlag((Category)PhysCat.NPC))
            {

                (fixtureB.Body.Tag as Entity).TakeDamage(Holder, Item.DamageValue, 100 *Vector2Helper.GetVectorFromDirection(Direction));
                Item.RemoveDurability();

                SoundModuleManager.PlayPackage("SwordConnect");

            }

            return base.OnCollides(fixtureA, fixtureB, contact);
        }
        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnSeparates(fixtureA, fixtureB, contact);
        }


    }
}
