using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes.Gadgets;
using SoundEngine.Classes;
using SpriteEngine.Classes;
using SpriteEngine.Classes.ShadowStuff;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace PhysicsEngine.Classes
{
    /// <summary>
    /// Inherit from this if your class should be collidable!
    /// </summary>
    public class Collidable : ILightDrawable
    {
        public Vector2 Position { get; protected set; }

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
        protected List<PhysicsGadget> Gadgets { get; private set; }

        protected SoundModuleManager SoundModuleManager { get; set; }

        protected bool MouseHovering { get; private set; }

        protected bool PlayerInClickRange { get; private set; }

        //for when controller is plugged in
        protected bool PlayerInControllerActionRange { get; private set; }
        public bool IsPlayingASound => SoundModuleManager.IsPlayingASound;

        protected HullBody BigSensor { get; set; }
        protected List<Category> BigSensorCollidesWithCategories { get; set; }

        protected List<LightCollidable> LightsCollidable { get; set; }

        /// <summary>
        /// Will return if is hovered, regardless of input type
        /// </summary>
        /// <param name="controllerConnected">Physics engine may not reference controls project, must pass in as param</param>
        protected bool IsHovered(bool controllerConnected)
        {
            return (controllerConnected && PlayerInControllerActionRange) || (MouseHovering && PlayerInControllerActionRange);
        }
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
        public void SetPrimaryCollidesWith( List<Category> collisionCategories)
        {

            MainHullBody.Body.SetCollidesWith(PhysicsManager.GetCat(collisionCategories));

        }
        public void SetCollidesWith(Body body, List<Category> collisionCategories)
        {
     
                body.SetCollidesWith(PhysicsManager.GetCat(collisionCategories));
            
        }

        protected void SetCategories(Body body, List<Category> collisionCategories)
        {

            body.SetCollisionCategories(PhysicsManager.GetCat(collisionCategories));

        }

        public void PlayPackage(string soundName)
        {
            SoundModuleManager.PlayPackage(soundName);
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

            if(LightsCollidable != null)
            {
                for (int i = 0; i < LightsCollidable.Count; i++)
                {
                    LightsCollidable[i].Update(gameTime);
                    LightsCollidable[i].Move(Position);
                }
            }
           
            SoundModuleManager.Update(CenteredPosition);


            //Position is changed so that our sprite knows where to draw, and position
            //snaps to where the physics system moved the main hullbody
            if(MainHullBody != null)
            Move(new Vector2(MainHullBody.Position.X, MainHullBody.Position.Y));


        }
        protected virtual void CreateBody(Vector2 position)
        {
            Position = position;
        }

        protected virtual bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories.HasFlag((Category)PhysCat.Cursor) && fixtureA.CollidesWith.HasFlag((Category)PhysCat.Cursor))
            {
                MouseHovering = true;
                
            }
           if(fixtureB.CollisionCategories.HasFlag((Category)PhysCat.PlayerBigSensor))
            {
                PlayerInClickRange = true;
            }
            if (fixtureB.CollisionCategories.HasFlag((Category)PhysCat.FrontalSensor))
            {
                PlayerInControllerActionRange = true;
            }
            return true;
        }

        protected virtual void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories.HasFlag((Category)PhysCat.Cursor))
            {
                MouseHovering = false;
            }
            if (fixtureB.CollisionCategories.HasFlag((Category)PhysCat.PlayerBigSensor))
            {
                PlayerInClickRange = false;
            }
            if (fixtureB.CollisionCategories.HasFlag((Category)PhysCat.FrontalSensor))
            {
                PlayerInControllerActionRange = false;
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
        public void Jettison(Vector2 directionVector, Vector2? pointAppliedTo, float speed = 1f, bool ignoreGrav = false, float gScaleUp = 4f,float gScaleDown = 15f, float restitution = 1f,
            float linearDamp = 2f, float friction = .15f, float bodyMass = 1f)
        {
            MainHullBody.Body.ApplyLinearImpulse(directionVector * 1000 * speed, pointAppliedTo ?? new Vector2(0,0));
            MainHullBody.Body.IgnoreGravity = ignoreGrav;

            MainHullBody.Body.SetRestitution(restitution);
            MainHullBody.Body.LinearDamping = linearDamp;
            MainHullBody.Body.SetFriction(0);
            MainHullBody.Body.Mass = bodyMass;
        }
        public virtual void CleanUp()
        {
            foreach(HullBody body in HullBodies)
            {
                body.Destroy();
            }
            if(MainHullBody != null)
                MainHullBody.Destroy();
            MainHullBody = null;
            HullBodies.Clear();
            if(LightsCollidable != null)
            {
                foreach(LightCollidable lightCollidable in LightsCollidable)
                {
                    lightCollidable.CleanUp();
                }
                LightsCollidable.Clear();
                LightsCollidable = null;
            }
        }
        protected void AddGadget(PhysicsGadget gadget)
        {
            Gadgets.Add(gadget);
        }
        protected void ClearGadgets()
        {
            for(int i = Gadgets.Count - 1; i >= 0; i--)
            {
                Gadgets[i].Destroy();
                Gadgets.RemoveAt(i);
            }
        }
        protected void AddLight(LightType lightType,Vector2 offSet,bool restoresLumens = true, float scale = 1)
        {
            if (LightsCollidable == null)
                LightsCollidable = new List<LightCollidable>();

            LightCollidable lightCollidable = new LightCollidable(Position, offSet, lightType, restoresLumens, scale);
            lightCollidable.CreateBody(Position);
            LightsCollidable.Add(lightCollidable);
        }
        public void DrawLights(SpriteBatch spriteBatch)
        {
            if(LightsCollidable != null)
            {
                for (int i = 0; i < LightsCollidable.Count; i++)
                {
                    LightsCollidable[i].Draw(spriteBatch);
                }
            }
            
        }
    }
}
