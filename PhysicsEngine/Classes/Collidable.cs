using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes.Gadgets;
using SoundEngine.Classes;
using SpriteEngine.Classes;
using SpriteEngine.Classes.ParticleStuff;
using SpriteEngine.Classes.ShadowStuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using static DataModels.Enums;

namespace PhysicsEngine.Classes
{
    /// <summary>
    /// Inherit from this if your class should be collidable!
    /// </summary>
    public class Collidable : ILightDrawable, IEmitter, ISaveable
    {
        public Vector2 Position { get; protected set; }

        protected int XOffSet { get; set; }
        protected int YOffSet { get; set; }

        //This is the "center" of your sprite, useful for collisions and things like magnetizing sprites
        public Vector2 CenteredPosition { get { return new Vector2(Position.X - XOffSet, Position.Y - YOffSet); } }

        //primary hull, defaults to HullBodies[0]
        public HullBody MainHullBody { get; protected set; }

        private List<HullBody> _hullBodies { get; set; }

        /// <summary>
        /// These modify the position of the Collidable
        /// </summary>
        /// 
        public List<PhysicsGadget> Gadgets { get; protected set; }

        protected SoundModuleManager SoundModuleManager { get; set; }

        private bool _mouseHovering { get; set; }

        protected bool PlayerInClickRange { get; private set; }

        //for when controller is plugged in
        protected bool PlayerInControllerActionRange { get; private set; }
        public bool IsPlayingASound => SoundModuleManager.IsPlayingASound;

        protected HullBody BigSensor { get; set; }
        protected List<Category> BigSensorCollidesWithCategories { get; set; }

        protected List<LightCollidable> LightsCollidable { get; set; }

        public Vector2 EmitPosition
        {
            get
            {
                if (EmitOffSet != null)
                    return EmitOffSet.Value + Position;
                else
                    return Position;
            }
        }
        public Vector2? EmitOffSet { get; set; }

        /// <summary>
        /// Will return if is hovered, regardless of input type
        /// </summary>
        /// <param name="controllerConnected">Physics engine may not reference controls project, must pass in as param</param>
        public bool IsHovered(bool controllerConnected)
        {
            return (controllerConnected && PlayerInControllerActionRange) || (_mouseHovering);
        }

        protected virtual bool WithinRangeOfPlayer(bool controllerConnected)
        {
            if (PlayerInClickRange)
            {
                if (IsHovered(controllerConnected) || PlayerInControllerActionRange)
                    return true;
            }
            return false;
        }
        public Collidable()
        {
            //SetToDefault();
        }

        public void Initialize()
        {
            _hullBodies = new List<HullBody>();
            Gadgets = new List<PhysicsGadget>();
            LightsCollidable = new List<LightCollidable>();
        }

        public virtual void SetToDefault()
        {
            if(_hullBodies != null)
            {
                foreach (HullBody body in _hullBodies)
                {
                    body.DestroyFromPhysicsWorld();
                }
                _hullBodies.Clear();

            }
          
            if (MainHullBody != null)
                MainHullBody.DestroyFromPhysicsWorld();
            MainHullBody = null;
            if (LightsCollidable != null)
            {
                foreach (LightCollidable lightCollidable in LightsCollidable)
                {
                    lightCollidable.SetToDefault();
                }
                LightsCollidable.Clear();
            }

            if (BigSensor != null)
                BigSensor.DestroyFromPhysicsWorld();
        }
        protected void AddPrimaryBody(HullBody body)
        {
            MainHullBody = body;

        }
        protected void AddSecondaryBody(HullBody body)
        {
            _hullBodies.Add(body);
        }
        public void SetPrimaryCollidesWith(List<Category> collisionCategories)
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
            //SoundModuleManager.PlayPackage(soundName);
        }
        public virtual void Update(GameTime gameTime)
        {
            if (Gadgets != null)
                foreach (PhysicsGadget gadget in Gadgets)
                {
                    gadget.Update(gameTime);
                }
            if (_hullBodies != null)

                foreach (HullBody body in _hullBodies)
                {
                    body.Position = Position;
                }

            UpdateLights(gameTime);

            if(SoundModuleManager != null)
            SoundModuleManager.Update(CenteredPosition);


            //Position is changed so that our sprite knows where to draw, and position
            //snaps to where the physics system moved the main hullbody
            if (MainHullBody != null)
                Move(new Vector2(MainHullBody.Position.X, MainHullBody.Position.Y));


        }

        protected virtual void UpdateLights(GameTime gameTime)
        {
            if (LightsCollidable != null)
            {
                for (int i = 0; i < LightsCollidable.Count; i++)
                {
                    LightsCollidable[i].Update(gameTime);
                    LightsCollidable[i].Move(Position);
                }
            }
        }

        protected virtual void CreateBody(Vector2 position)
        {
            Position = position;
        }
        protected virtual bool OnClickBoxCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories == (Category)PhysCat.Cursor)
            {
                if (fixtureA.CollidesWith.HasFlag((Category)PhysCat.Cursor))
                    _mouseHovering = true;


            }
            return true;
        }
        protected virtual void OnClickBoxSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories == ((Category)PhysCat.Cursor))
            {
                _mouseHovering = false;
            }
        }
        protected virtual bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories == ((Category)PhysCat.Cursor) && fixtureA.CollidesWith == ((Category)PhysCat.Cursor))
            {
                _mouseHovering = true;


            }
            if (fixtureB.CollisionCategories == ((Category)PhysCat.PlayerBigSensor))
            {
                PlayerInClickRange = true;
            }
            if (fixtureB.CollisionCategories == ((Category)PhysCat.FrontalSensor))
            {
                PlayerInControllerActionRange = true;
            }
            return true;
        }

        protected virtual void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories == ((Category)PhysCat.Cursor))
            {
                _mouseHovering = false;
            }
            if (fixtureB.CollisionCategories == ((Category)PhysCat.PlayerBigSensor))
            {
                PlayerInClickRange = false;
            }
            if (fixtureB.CollisionCategories == ((Category)PhysCat.FrontalSensor))
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

            if(_hullBodies != null)
            foreach (HullBody body in _hullBodies)
            {
                body.Position = Position;
            }
            if (MainHullBody != null)
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
        public void Jettison(Vector2 directionVector, Vector2? pointAppliedTo, float speed = 1f, bool ignoreGrav = false, float gScaleUp = 4f, float gScaleDown = 15f, float restitution = 1f,
            float linearDamp = 2f, float friction = .15f, float bodyMass = 1f)
        {
            MainHullBody.Body.ApplyLinearImpulse(directionVector * 1000 * speed, pointAppliedTo ?? new Vector2(0, 0));
            MainHullBody.Body.IgnoreGravity = ignoreGrav;

            MainHullBody.Body.SetRestitution(restitution);
            MainHullBody.Body.LinearDamping = linearDamp;
            MainHullBody.Body.SetFriction(0);
            MainHullBody.Body.Mass = bodyMass;
        }

        protected void AddGadget(PhysicsGadget gadget)
        {
            Gadgets.Add(gadget);
        }
        protected void ClearGadgets()
        {
            if (Gadgets == null)
                return;
            for (int i = Gadgets.Count - 1; i >= 0; i--)
            {
                Gadgets[i].Destroy();
                Gadgets.RemoveAt(i);
            }
        }
        protected LightCollidable AddLight(LightType lightType, Vector2 offSet, bool immuneToDrain, bool restoresLumens = true, float scale = 1)
        {
            if (LightsCollidable == null)
                LightsCollidable = new List<LightCollidable>();

            LightCollidable lightCollidable = new LightCollidable(Position, offSet, lightType, restoresLumens, scale, immuneToDrain);
            lightCollidable.CreateBody(Position);
            LightsCollidable.Add(lightCollidable);
            return lightCollidable;
        }
        public void DrawLights(SpriteBatch spriteBatch)
        {
            if (LightsCollidable != null)
            {
                for (int i = 0; i < LightsCollidable.Count; i++)
                {
                    LightsCollidable[i].Draw(spriteBatch);
                }
            }

        }


        public void LoadContent()
        {
            throw new NotImplementedException();
        }

        public void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public void Save(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public void LoadSave(BinaryReader reader)
        {
            throw new NotImplementedException();
        }


    }
}
