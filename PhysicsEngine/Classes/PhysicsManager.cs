﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Globals.Classes;
using Penumbra;
using System.Collections.Generic;
using Globals.Classes.Console;
using System;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Diagnostics;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Dynamics.Joints;

namespace PhysicsEngine.Classes
{
    public enum PhysCat
    {
        None = 0x0,
        Player = 0x1,
        Solid = 0x2,
        TransparencySensor = 0x4,
        FrontalSensor = 0x8,
        Item = 0x10,
        NPC = 0x20,
        PlayerBigSensor = 0x40,
        Projectile = 0x80,
        ArtificialFloor = 0x100,
        Portal = 0x200,
        Cursor = 0x400,
        NPCBigSensor = 0x800,
        ActionTile = 0x1000,
        LightSource = 0x2000,
        SpecialZone = 0x4000,
        Grass = 0x8000,
        Cat17 = 0x10000,
        Cat18 = 0x20000,
        Cat19 = 0x40000,
        Cat20 = 0x80000,
        Cat21 = 0x100000,
        Cat22 = 0x200000,
        Cat23 = 0x400000,
        Cat24 = 0x800000,
        Cat25 = 0x1000000,
        Cat26 = 0x2000000,
        Cat27 = 0x4000000,
        Cat28 = 0x8000000,
        Cat29 = 0x10000000,
        Cat30 = 0x20000000,
        Cat31 = 0x40000000,
        All = int.MaxValue
    }
    public static class PhysicsManager
    {
        public static PenumbraComponent Penumbra;
        public static World VelcroWorld { get; set; }
        public static DebugView VelcroDebugger { get; set; }

        internal static Random Random;

        public static void Initialize(PenumbraComponent penumbra)
        {
            VelcroWorld = new World(new Vector2(0, 9.8f));
            if (VelcroDebugger == null)
            {
                VelcroDebugger = new DebugView(VelcroWorld);


                
            }

            Penumbra = penumbra;
            Random = new Random();
        }

        public static void Clear()
        {
            VelcroWorld.Clear();
        }

        public static void LoadContent(ContentManager content, GraphicsDevice graphics, SpriteFont spriteFont)
        {
            VelcroDebugger.LoadContent(graphics, content);
            VelcroDebugger.AppendFlags(DebugViewFlags.DebugPanel);

            

        }
        public static void Update(GameTime gameTime)
        {
            VelcroWorld.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * .001f);
        }

        public static void Draw(GraphicsDevice graphics, Camera2D camera)
        {
            Matrix proj = Matrix.CreateOrthographicOffCenter(new Rectangle((int)(-graphics.Viewport.Width / 2) * (int)camera.Zoom + (int)camera.GetZoomOffSetPatch().X * 2,
               (int)(-graphics.Viewport.Height / 2) * (int)camera.Zoom + (int)camera.GetZoomOffSetPatch().Y * 2, graphics.Viewport.Width, graphics.Viewport.Height), 1f, -1f);
            Matrix view = camera.GetViewMatrix(Vector2.One);

            VelcroDebugger.RenderDebugData(proj, view);
        }
        public static Light GetPointLight(Vector2 position, bool startOn = true, float scale = 400f)
        {
            Light light = new PointLight()
            {
                Position = position,
                Enabled = startOn,
                Scale = new Vector2(scale), // Range of the light source (how far the light will travel)
                ShadowType = ShadowType.Occluded // Will not lit hulls themselves
            };
            return light;
        }


        public static HullBody CreateCircularHullBody(BodyType bodyType, Vector2? position,float? radius, List<Category>? collisionCategories, List<Category>? categoriesCollidesWith,
            OnCollisionEventHandler? cDelegate, OnSeparationEventHandler? sDelegate, float density = 1f,
            float restitution = 1f, float friction = 1f, float mass = 1f, float inertia = 0, bool sleepingAllowed = true,bool isSensor = false, bool ignoreGravity = false,
            object userData = null, int xOffset = 0, int yOffset = 0,bool blocksLight = false, Light light = null)
        {
            radius = radius ?? 6f;
            position = position ?? Vector2.Zero;
            Body body = VelcroWorld.CreateCircle((float)radius, density, (Vector2)position, bodyType);

           
            if(categoriesCollidesWith != null)
                foreach (Category category in categoriesCollidesWith)
                {
                    foreach (Fixture fixture in body.FixtureList)
                        fixture.CollidesWith = category;
                }

            if(collisionCategories != null)
            {
                //body.SetCollisionCategory(Category.None);
                foreach (Category category in collisionCategories)
                {
                    foreach (Fixture fixture in body.FixtureList)
                        fixture.CollidesWith = category;
                }
            }


            body.IgnoreGravity = ignoreGravity;
            body.SleepingAllowed = sleepingAllowed;
            body.OnCollision += cDelegate;
            body.OnSeparation += sDelegate;
            body.SetIsSensor(isSensor);

            if(light != null)
                Penumbra.Lights.Add(light);
            Hull hull = null;
            if (blocksLight)
                hull = Hull.CreateCircle(position, radius);
            if (hull != null)
                Penumbra.Hulls.Add(hull);
            return new HullBody(body, hull);

        }

        public static HullBody CreateRectangularHullBody(BodyType bodyType, Vector2? position, float? width,float? height, List<Category>? collisionCategories, List<Category>? categoriesCollidesWith,
            OnCollisionEventHandler? cDelegate, OnSeparationEventHandler? sDelegate, float density = 1f, float rotation = 0f,
            float restitution = 1f, float friction = 1f, float mass = 1f, float inertia = 0, bool sleepingAllowed = true, bool isSensor = false, bool ignoreGravity = false,
            object userData = null, int xOffset = 0, int yOffset = 0, bool blocksLight = false, Light light = null)
        {
            
            width = width ?? 4f;
            height = height ?? 4f;
            position = position ?? Vector2.Zero;
            Body body = PhysicsManager.VelcroWorld.CreateRectangle( (float)width,(float) height, density,(Vector2)position, rotation, bodyType);



            if (categoriesCollidesWith != null)
                foreach (Category category in categoriesCollidesWith)
                {
                    foreach (Fixture fixture in body.FixtureList)
                        fixture.CollidesWith = category;
                }

            if (collisionCategories != null)
            {
                //body.SetCollisionCategory(Category.None);
                foreach (Category category in collisionCategories)
                {
                    foreach (Fixture fixture in body.FixtureList)
                        fixture.CollidesWith = category;
                }
            }

            body.IgnoreGravity = ignoreGravity;
            body.SleepingAllowed = sleepingAllowed;
            body.OnCollision += cDelegate;
            body.OnSeparation += sDelegate;
            body.SetIsSensor( isSensor);

            if (light != null)
                Penumbra.Lights.Add(light);
            Hull hull = null;
            if (blocksLight)
                hull = Hull.CreateRectangle(position, new Vector2((float)width, (float)height), 0f);
            if (hull != null)
                Penumbra.Hulls.Add(hull);
            return new HullBody(body, hull);

        }

        public static HullBody CreatePolygonHullBody(BodyType bodyType, Vector2? position, Vertices vertices, List<Category>? collisionCategories, List<Category>? categoriesCollidesWith,
            OnCollisionEventHandler? cDelegate, OnSeparationEventHandler? sDelegate, float density = 1f, float rotation = 0f,
            float restitution = 1f, float friction = 1f, float mass = 1f, float inertia = 0, bool sleepingAllowed = true, bool isSensor = false, bool ignoreGravity = false,
            object userData = null, int xOffset = 0, int yOffset = 0, bool blocksLight = false, Light light = null)
        {

 
            position = position ?? Vector2.Zero;

            Body body = VelcroWorld.CreatePolygon(vertices, density, (Vector2)position, rotation, bodyType);
            if (categoriesCollidesWith != null)
                foreach (Category category in categoriesCollidesWith)
                {
                    foreach (Fixture fixture in body.FixtureList)
                        fixture.CollidesWith = category;
                }

            if (collisionCategories != null)
            {
                //body.SetCollisionCategory(Category.None);
                foreach (Category category in collisionCategories)
                {
                    foreach (Fixture fixture in body.FixtureList)
                        fixture.CollidesWith = category;
                }
            }


            body.IgnoreGravity = ignoreGravity;
            body.SleepingAllowed = sleepingAllowed;
            body.OnCollision += cDelegate;
            body.OnSeparation += sDelegate;
            body.SetIsSensor(isSensor);

            if (light != null)
                Penumbra.Lights.Add(light);
            Hull hull = null;
            //if (blocksLight)
            //    hull = Hull.CreateRectangle(position, new Vector2((float)width, (float)height), 0f);
            if (hull != null)
                Penumbra.Hulls.Add(hull);
            return new HullBody(body, hull);

        }

        /// <summary>
        /// Welds two bodies together and returns the created joint.
        /// </summary>
        /// <param name="bodyA"></param>
        /// <param name="bodyB"></param>
        /// <param name="bodyAAnchor"></param>
        /// <param name="bodyBAnchor"></param>
        /// <param name="dampingRatio"></param>
        /// <param name="frequencyHz"></param>
        /// <returns></returns>
        public static WeldJoint Weld(Body bodyA, Body bodyB, Vector2? bodyAAnchor, Vector2? bodyBAnchor, float? dampingRatio, float? frequencyHz)
        {
            bodyAAnchor = bodyAAnchor ?? Vector2.Zero;
            bodyBAnchor = bodyBAnchor ?? Vector2.Zero;

            dampingRatio = dampingRatio ?? 20f;
            frequencyHz = frequencyHz ?? 30f;

            WeldJoint weldJoint = JointFactory.CreateWeldJoint(VelcroWorld, bodyA, bodyB, new Vector2(0f, 1f), new Vector2(0f, 4f));
            weldJoint.DampingRatio = 2f;
            weldJoint.CollideConnected = false;
            weldJoint.FrequencyHz = 5f;
            return weldJoint;

        }
       
    }
}
