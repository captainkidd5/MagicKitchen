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
using static DataModels.Enums;

namespace EntityEngine.Classes.ToolStuff
{
    internal class Sword : Tool
    {
        private SimpleTimer _swingDurationtimer;
        private float _swingDuration = .25f;
        private RevoluteJoint _joint;
        public Sword(Item item) : base(item)
        {
            RequiresCharge = false;
            SourceRectangle = Item.GetItemSourceRectangle(item.Id);
            _swingDurationtimer = new SimpleTimer(_swingDuration);
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
            if(Direction == Direction.Right)
            {
                rotation = 0f;

            }

            else if (Direction == Direction.Left)
            {
                             rotation = MathHelper.Pi *2;

                counterClockWise = true;
            }

            else if (Direction == Direction.Up)
            {
                rotation = MathHelper.Pi + MathHelper.PiOver2;

            }

            else if (Direction == Direction.Down)
            {
                rotation = MathHelper.PiOver2;

            }

            MainHullBody = PhysicsManager.CreateRectangularHullBody(BodyType.Dynamic, Position, startingRectangle.X, startingRectangle.Y,
                new List<Category>() { (Category)PhysCat.Tool },
               new List<Category>() { (Category)PhysCat.Item, (Category)PhysCat.NPC }, OnCollides, OnSeparates,
               blocksLight: true, userData: this, mass: .1f, isSensor: false, ignoreGravity: true, rotation: rotation);
            _joint = PhysicsManager.RotateWeld(Holder.MainHullBody.Body, MainHullBody.Body, new Vector2(0,0), anchorPoint, null, null,counterClockWise);
            //_joint.CollideConnected = false;
        }

        protected override void AlterSpriteRotation(GameTime gameTime)
        {
            if (_joint != null)
                Sprite.Rotation = MainHullBody.Body.Rotation + (float)Math.PI / 4; ;
            

        }
        public override void ReleaseTool(Direction direction, Vector2 directionVector, Collidable holder)
        {
            base.ReleaseTool(direction, directionVector, holder);
           // MainHullBody.Body.ApplyAngularImpulse(10f);

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
            if(_joint != null)
            PhysicsManager.VelcroWorld.RemoveAsync(_joint);
            base.Unload();
        }
    }
}
