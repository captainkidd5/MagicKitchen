using Microsoft.Xna.Framework;
using Penumbra;
using PhysicsEngine.Classes.Gadgets;
using SoundEngine.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Collision.Handlers;
using VelcroPhysics.Dynamics;

namespace PhysicsEngine.Classes
{
    /// <summary>
    /// Inherit from this if your class should be collidable!
    /// </summary>
    public class Collidable
    {
        public Vector2 Position { get; private set; }

        protected int XOffSet { get; set; } 
        protected int YOffSet { get; set; } 

        //This is the "center" of your sprite, useful for collisions and things like magnetizing sprites
        public Vector2 CenteredPosition { get { return new Vector2(Position.X - XOffSet, Position.Y - YOffSet); } }

        //primary hull, defaults to HullBodies[0]
        public HullBody MainHullBody { get; protected set; }

        private List<HullBody> HullBodies { get; set; }

        /// <summary>
        /// These modify the position of the Collidable
        /// </summary>
        /// 
        public List<PhysicsGadget> Gadgets { get; set; }

        protected SoundModuleManager SoundModuleManager { get; set; }

        protected bool MouseHovering { get; private set; }

        protected bool PlayerInClickRange { get; private set; }

        public bool IsPlayingASound => SoundModuleManager.IsPlayingASound;
        public Collidable()
        {
            HullBodies = new List<HullBody>();
            Gadgets = new List<PhysicsGadget>();
            SoundModuleManager = new SoundModuleManager();
        }

        protected void AddPrimaryBody(HullBody body)
        {
            MainHullBody = body;

        }
        protected void AddSecondaryBody(HullBody body)
        {
            HullBodies.Add(body);
        }

        protected void SetCollidesWith(Body body, List<Category> collisionCategories)
        {
            foreach (var category in collisionCategories)
            {
                body.SetCollidesWith(category);
            }
        }

        public void PlaySound(string soundName)
        {
            SoundModuleManager.PlayDestructableSound(soundName);
        }
        public virtual void Update(GameTime gameTime)
        {
            foreach (PhysicsGadget gadget in Gadgets)
            {
                gadget.Update(gameTime);
            }
            foreach (HullBody body in HullBodies)
            {
                body.Position = Position;
            }
            SoundModuleManager.Update(CenteredPosition);

           // MainHullBody.Body.LinearVelocity = Velocity * Speed * (float)gameTime.ElapsedGameTime.Milliseconds;

            //Position is changed so that our sprite knows where to draw, and position
            //snaps to where the physics system moved the main hullbody
            Move(new Vector2(MainHullBody.Position.X, MainHullBody.Position.Y));


        }
        protected virtual void CreateBody(Vector2 position)
        {
            Position = position;
        }

        protected virtual void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories.HasFlag(Category.Cursor) && fixtureA.CollidesWith.HasFlag(Category.Cursor))
            {
                MouseHovering = true;
                
            }
           if(fixtureB.CollisionCategories.HasFlag(Category.PlayerBigSensor))
            {
                PlayerInClickRange = true;
            }
        }

        protected virtual void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories.HasFlag(Category.Cursor))
            {
                MouseHovering = false;
            }
            if (fixtureB.CollisionCategories.HasFlag(Category.PlayerBigSensor))
            {
                PlayerInClickRange = false;
            }
        }

        /// <summary>
        /// Moves away from position
        /// </summary>

        public virtual void Avoid(GameTime gameTime, Vector2 newPosition, float speed)
        {
            Vector2 dir = Position - newPosition;
            dir.Normalize();
            Position += dir * speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds * .05f;

            foreach (HullBody body in HullBodies)
            {
                body.Position = Position;
            }
        }

        /// <summary>
        /// Use this instead of setting position so that all associated bodies have their positions updated as well
        /// </summary>
        /// <param name="newPosition"></param>
        public virtual void Move(Vector2 newPosition)
        {
            Position = newPosition;

            foreach (HullBody body in HullBodies)
            {
                body.Position = Position;
            }
            if(MainHullBody != null)
             MainHullBody.Position = Position;
        }


        protected virtual bool IsStopped()
        {
            return MainHullBody.Body.LinearVelocity.X < .2 && MainHullBody.Body.LinearVelocity.Y < .2;
        }

        /// <summary>
        /// Jetison's body in given direction
        /// </summary>
        /// <param name="directionVector"></param>
        public void Jettison(Vector2 directionVector, Vector2? pointAppliedTo, bool ignoreGrav = false, float gScaleUp = 4f,float gScaleDown = 15f, float restitution = .2f,
            float linearDamp = .75f, float friction = .75f, float bodyMass = 1f)
        {
            MainHullBody.Body.ApplyLinearImpulse(directionVector * 500, pointAppliedTo ?? new Vector2(0,0));
            MainHullBody.Body.IgnoreGravity = ignoreGrav;
            if (directionVector.X == 0)
                MainHullBody.Body.GravityScale = gScaleUp;
            else
                MainHullBody.Body.GravityScale = gScaleDown;
            MainHullBody.Body.Restitution = restitution;
            MainHullBody.Body.LinearDamping = linearDamp;
            MainHullBody.Body.Friction = friction;
            MainHullBody.Body.Mass = bodyMass;
        }
        public virtual void Unload()
        {
            foreach(HullBody body in HullBodies)
            {
                body.Destroy();
            }
            if(MainHullBody != null)
                MainHullBody.Destroy();
            HullBodies.Clear();

        }

        protected void ClearGadgets()
        {
            for(int i = Gadgets.Count - 1; i >= 0; i--)
            {
                Gadgets[i].Destroy();
                Gadgets.RemoveAt(i);
            }
        }
    }
}
