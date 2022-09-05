using EntityEngine.ItemStuff;
using Globals.Classes.Helpers;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
using PhysicsEngine.Classes.Gadgets;
using PhysicsEngine.Classes.Shapes;
using SoundEngine.Classes;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using static DataModels.Enums;

namespace EntityEngine.Classes.ToolStuff
{
    
    public class Hook : ProjectileTool
    {
        private bool _isReturning;

        private float _maximumDistanceFromEntity = 140f;

        private Sprite _directionalArrowSprite;

        private WorldItem _hookedItem;
        public Hook(Item item) : base (item)
        {

            SourceRectangle = new Rectangle(16, 0, 16, 16);
            RequiresCharge = true;
        }

        public override void Load( )
        {
            base.Load();
            Sprite.Origin = new Vector2(XOffSet, YOffSet);
        }
        protected override AnimationFrame[] GetAnimationFrames()
        {
            AnimationFrame[] frames = new AnimationFrame[2];
            frames[0] = new AnimationFrame(0, 0, 0, 1f);
            frames[1] = new AnimationFrame(1, 0, 0, 1f);
            return frames;
        }

        private Vector2 _arrowDirectionVector;
        public override void ChargeUpTool(GameTime gameTime, Vector2 aimPosition)
        {
            base.ChargeUpTool(gameTime, aimPosition);
            if (IsCharging)
            {
                Move(Holder.CenteredPosition);

                if (Vector2Helper.IsNormalized(aimPosition))
                {
                    _arrowDirectionVector = aimPosition * -1;
                }
                else
                {
                    _arrowDirectionVector = Holder.CenteredPosition - aimPosition;
                    _arrowDirectionVector.Normalize();
                }
                 
                _directionalArrowSprite.Rotation = Vector2Helper.VectorToDegrees(_arrowDirectionVector) - (float)Math.PI /4;
                _directionalArrowSprite.Update(gameTime, Position);
            }
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
           
            if (!IsCharging && Vector2.Distance(MainHullBody.Position, Holder.CenteredPosition) > _maximumDistanceFromEntity)
            {
                Return();
            }
        }
        public override void ReleaseTool(Direction direction, Vector2 directionVector, Entity holder)
        {
            base.ReleaseTool(direction, _arrowDirectionVector, holder);
            SoundFactory.PlaySoundEffect("HookFire");

            Vector2 newOffSet = new Vector2(BaseOffSet.X, BaseOffSet.Y);
  

            Sprite.Origin = newOffSet;

        }
        public override void BeginCharge(Entity holder)
        {
            base.BeginCharge(holder);
            _directionalArrowSprite = SpriteFactory.CreateWorldSprite(Position, new Rectangle(0, 96, 16, 16), ItemFactory.ItemSpriteSheet);

        }
        protected override void AlterSpriteRotation(GameTime gameTime)
        {
            if (_isReturning)
            {
                Vector2 directionVector = MainHullBody.Position - Holder.CenteredPosition;
                directionVector.Normalize();
                Sprite.Rotation = Vector2Helper.VectorToDegrees(directionVector);

            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (IsCharging)
            {
                _directionalArrowSprite.Draw(spriteBatch);
                LineUtility.DrawLine(null, spriteBatch, Position, Holder.CenteredPosition, Color.White, .99f);

            }
            else
            {
                LineUtility.DrawLine(null, spriteBatch, Position, Holder.CenteredPosition, Color.White, Sprite.LayerDepth);

            }
        }
        private void Return()
        {
            (Sprite as AnimatedSprite).SetTargetFrame(1, true);

            if (Gadgets.FirstOrDefault(x => x.GetType() == typeof(Magnetizer)) == null)
            {
                AddGadget(new Magnetizer(this, Holder,8,8));
                SetCollidesWith(MainHullBody.Body,
               new List<Category>() { (Category)PhysCat.Item, (Category)PhysCat.Player });
                _isReturning = true;

            }
        }

        protected override void LoadSprite()
        {
         
            Sprite = SpriteFactory.CreateWorldAnimatedSprite(Position, SourceRectangle,
                  ItemFactory.ToolSheet, GetAnimationFrames());
            (Sprite as AnimatedSprite).Paused = true;
        }
        protected override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories==((Category)PhysCat.Item))
            {
                Return();
                SoundFactory.PlaySoundEffect("HookGrab");
                _hookedItem = fixtureB.Body.Tag as WorldItem;

                
            }
            if (fixtureB.CollisionCategories==((Category)PhysCat.SolidHigh))
            {
                SoundFactory.PlaySoundEffect("HookMiss");
                Return();

            }
            else if (fixtureB.CollisionCategories==((Category)PhysCat.Player))
            {
                if (_hookedItem != null && _hookedItem.MainHullBody!= null)
                    _hookedItem.SetStandardCollides();
                Item.RemoveDurability(1);
                Unload();
            }
            return base.OnCollides(fixtureA, fixtureB, contact);
        }
       
    }
}
