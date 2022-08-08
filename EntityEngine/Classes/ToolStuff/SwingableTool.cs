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
    internal class SwingableTool : Tool
    {
        private SimpleTimer _swingDurationtimer;
        protected virtual float SwingDuration { get; set; } = .6f;
        protected virtual RotateSpeed RotateSpeed { get; set; } = RotateSpeed.None;

        private RevoluteJoint _joint;

        public SwingableTool(Item item) : base(item)
        {
            RequiresCharge = false;
            SourceRectangle = Item.GetItemSourceRectangle(item.Id);
            _swingDurationtimer = new SimpleTimer(SwingDuration);
            RequiresCharge = false;
        }

        protected override void LoadSprite()
        {
            base.LoadSprite();
            Sprite.SwapScale(new Vector2(2f, 2f));
            Sprite.Origin = new Vector2(16, 16);
            Sprite.Rotation = MainHullBody.Body.Rotation;

           
        }

        protected override void CreateBody(Vector2 position)
        {
            Vector2 startingRectangle = new Vector2(2, 34);
            Vector2 anchorPoint = new Vector2(1, 32);
            bool counterClockWise = false;
            float rotation = 0;
            YOffSet = Holder.SubmergenceLevel == SubmergenceLevel.Shallow ? 14 : 0;

            RotateSpeed = Holder.SubmergenceLevel == SubmergenceLevel.Shallow ? RotateSpeed - 1 : RotateSpeed;
            if (Direction == Direction.Right)
            {
                rotation = 0f;
                XOffSet = 4;
                YOffSet = -4;

            }

            else if (Direction == Direction.Left)
            {
                rotation = MathHelper.Pi * 2;

                counterClockWise = true;
                XOffSet = -4;
                YOffSet = -4;

            }

            else if (Direction == Direction.Up)
            {
                rotation = MathHelper.Pi + MathHelper.PiOver2;
                YOffSet = -8;
            }

            else if (Direction == Direction.Down)
            {
                rotation = MathHelper.PiOver2;

            }

            MainHullBody = PhysicsManager.CreateRectangularHullBody(BodyType.Dynamic, Position, startingRectangle.X, startingRectangle.Y,
                new List<Category>() { (Category)PhysCat.Tool },
               new List<Category>() { (Category)PhysCat.Item, (Category)PhysCat.NPC }, OnCollides, OnSeparates,
               blocksLight: true, userData: this, mass: .1f, isSensor: true, ignoreGravity: true, rotation: rotation);
            _joint = PhysicsManager.RotateWeld(RotateSpeed, Holder.MainHullBody.Body, MainHullBody.Body, new Vector2(0, 0), anchorPoint, null, null, counterClockWise);
            //_joint.CollideConnected = false;
        }

        protected override void AlterSpriteRotation(GameTime gameTime)
        {
            if (_joint != null)
                Sprite.Rotation = MainHullBody.Body.Rotation + (float)Math.PI / 4; ;


        }
        public override void ReleaseTool(Direction direction, Vector2 directionVector, Entity holder)
        {
            base.ReleaseTool(direction, directionVector, holder);
            holder.Animator.PerformAction(null, direction, ActionType, (float)RotateSpeed);

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Holder.MainHullBody.Body.LinearVelocity = Vector2.Zero;
            Holder.MainHullBody.Body.AngularVelocity = 0f;
            //Move(Holder.CenteredPosition);
            if (_swingDurationtimer.Run(gameTime))
                Unload();
        }
        public override void Unload()
        {
            if (_joint != null)
                PhysicsManager.VelcroWorld.RemoveAsync(_joint);
            base.Unload();
        }
        protected override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
        

            return base.OnCollides(fixtureA, fixtureB, contact);
        }
        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnSeparates(fixtureA, fixtureB, contact);
        }
    }
}
