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
        }

        protected override void CreateBody(Vector2 position)
        {
            Vector2 startingRectangle = new Vector2(2, 34);
            Vector2 anchorPoint = new Vector2(1, 32);
            Vector2 pos = Position;
            if(Direction == Direction.Right)
            {
                startingRectangle = new Vector2(2, 34);
                anchorPoint = new Vector2(startingRectangle.X - 1, startingRectangle.Y - 1);

            }
            else if (Direction == Direction.Left)
            {
                startingRectangle = new Vector2(2, 34);
                pos = new Vector2(pos.X ,pos.Y - startingRectangle.Y);
                anchorPoint = new Vector2(startingRectangle.X - 1,1);

            }
            else if (Direction == Direction.Up)
            {
                startingRectangle = new Vector2(34, 2);
                anchorPoint = new Vector2(startingRectangle.X - 1, startingRectangle.Y - 1);

            }

            MainHullBody = PhysicsManager.CreateRectangularHullBody(BodyType.Dynamic, pos, startingRectangle.X, startingRectangle.Y,
                new List<Category>() { (Category)PhysCat.Tool },
               new List<Category>() { (Category)PhysCat.Item, (Category)PhysCat.NPC }, OnCollides, OnSeparates,
               blocksLight: true, userData: this, mass: 0, isSensor: false, ignoreGravity: true);
            _joint = PhysicsManager.RotateWeld(Holder.MainHullBody.Body, MainHullBody.Body, new Vector2(0,0), anchorPoint, null, null);

        }

        protected override void AlterSpriteRotation(GameTime gameTime)
        {
            if(_joint != null)
            Sprite.Rotation = _joint.JointAngle + (float)Math.PI / 4;
            

        }
        public override void ReleaseTool(Direction direction, Vector2 directionVector, Collidable holder)
        {
            base.ReleaseTool(direction, directionVector, holder);
           // MainHullBody.Body.ApplyAngularImpulse(10f);

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
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
