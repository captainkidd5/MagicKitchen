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
using tainicom.Aether.Physics2D.Dynamics.Joints;

namespace EntityEngine.Classes.ToolStuff
{
    internal class Sword : Tool
    {
        private SimpleTimer _swingDurationtimer;
        private float _swingDuration = 2f;
        private RevoluteJoint _joint;
        public Sword(Item item) : base(item)
        {
            RequiresCharge = false;
            SourceRectangle = Item.GetItemSourceRectangle(item.Id);
            _swingDurationtimer = new SimpleTimer(_swingDuration);

        }
        protected override void LoadSprite()
        {
            base.LoadSprite();
        }

        protected override void CreateBody(Vector2 position)
        {
            MainHullBody = PhysicsManager.CreateRectangularHullBody(BodyType.Dynamic, Position, 2f,16f,
                new List<Category>() { (Category)PhysCat.Tool },
               new List<Category>() { (Category)PhysCat.Item, (Category)PhysCat.SolidHigh }, OnCollides, OnSeparates,
               blocksLight: true, userData: this, mass: 1, isSensor: true, ignoreGravity: true);
            _joint = PhysicsManager.RotateWeld(Holder.MainHullBody.Body, MainHullBody.Body, Vector2.Zero,new Vector2(1,15), null, null);

        }

        protected override void AlterSpriteRotation(GameTime gameTime)
        {
            Sprite.Rotation = _joint.JointAngle;

        }
        public override void ReleaseTool(Vector2 directionVector, Collidable holder)
        {
            base.ReleaseTool(directionVector, holder);
            MainHullBody.Body.ApplyAngularImpulse(10f);

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Move(Holder.CenteredPosition);
            if (_swingDurationtimer.Run(gameTime))
                Unload();
        }
        public override void Unload()
        {
            PhysicsManager.VelcroWorld.RemoveAsync(_joint);
            base.Unload();
        }
    }
}
