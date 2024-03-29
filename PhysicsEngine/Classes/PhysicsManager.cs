﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Globals.Classes;
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
        None = 0x0, //0
        Player = 0x1,//1
        SolidLow = 0x2,//2
        TransparencySensor = 0x4,//3
        SolidHigh = 0x8,//4
        Item = 0x10,//5
        NPC = 0x20,//6
        PlayerBigSensor = 0x40,//7
        PlayArea = 0x80,//8
        Tool = 0x100,//9
        Portal = 0x200,//10
        Cursor = 0x400,//11
        NPCBigSensor = 0x800,
        ActionTile = 0x1000,
        LightSource = 0x2000,
        SpecialZone = 0x4000,
        Grass = 0x8000,
        ArtificialFloor = 0x10000,
        FrontalSensor = 0x20000,
        Projectile = 0x40000,
        Damage = 0x80000,
        ClickBox = 0x100000,
        ArraySensor = 0x200000,
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

        public static World VelcroWorld { get; set; }
        public static DebugView PhysicsDebugger { get; set; }

        internal static Random Random;

        public static void Initialize( )
        {
            VelcroWorld = new World(new Vector2(0, 9.8f));
            if (PhysicsDebugger == null)
            {
                PhysicsDebugger = new DebugView(VelcroWorld);

                //tainicom.Aether.Physics2D.Settings.MaxPolygonVertices = 15;
              //  tainicom.Aether.Physics2D.Settings.UseConvexHullPolygons = false;

            }


            Random = new Random();
        }

        public static void Clear()
        {
            VelcroWorld.Clear();
        }

        public static void LoadContent(ContentManager content, GraphicsDevice graphics, SpriteFont spriteFont)
        {
            PhysicsDebugger.LoadContent(graphics, content);
            PhysicsDebugger.AppendFlags(DebugViewFlags.DebugPanel);

            PhysicsDebugger.RemoveDebugCategory(Category.All);
            PhysicsDebugger.AddDebugCategory((Category)PhysCat.SolidLow);
            CommandConsole.RegisterCommand("add_f", "adds debug shape filter", AddCollisionFilterCommand);

            CommandConsole.RegisterCommand("rem_f", "removes debug shape filter", RemoveCollisionFilterCommand);

        }

        private static void AddCollisionFilterCommand(string[] args)
        {
            PhysicsDebugger.AddDebugCategory((Category)Enum.Parse(typeof(PhysCat), args[0],true));
        }
        private static void RemoveCollisionFilterCommand(string[] args)
        {
            PhysicsDebugger.RemoveDebugCategory((Category)Enum.Parse(typeof(PhysCat), args[0], true));
        }
        public static void Update(GameTime gameTime)
        {
            VelcroWorld.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * .001f);
        }

        public static void Draw(GraphicsDevice graphics, Camera2D camera)
        {
            Matrix proj = Matrix.CreateOrthographicOffCenter(new Rectangle((int)(-graphics.Viewport.Width / 2) * (int)camera.Zoom + (int)camera.GetDebuggerOffSetPatch().X * 2,
               (int)(-graphics.Viewport.Height / 2) * (int)camera.Zoom + (int)camera.GetDebuggerOffSetPatch().Y * 2, graphics.Viewport.Width, graphics.Viewport.Height), 1f, -1f);
            Matrix view = camera.GetViewMatrix(Vector2.One);

            PhysicsDebugger.RenderDebugData(proj, view);
        }



        public static HullBody CreateCircularHullBody(BodyType bodyType, Vector2? position,float? radius, List<Category>? collisionCategories, List<Category>? categoriesCollidesWith,
            OnCollisionEventHandler? cDelegate, OnSeparationEventHandler? sDelegate, float density = 1.5f,
            float restitution = 1f, float friction = 1f, float mass = 1f, float inertia = 0, bool sleepingAllowed = true,bool isSensor = false, bool ignoreGravity = false,
            object userData = null, int xOffset = 0, int yOffset = 0,bool blocksLight = false)
        {
            radius = radius ?? 6f;
            position = position ?? Vector2.Zero;
            Body body = PhysicsManager.VelcroWorld.CreateCircle((float)radius, density, (Vector2)position, bodyType);
            CreateCommonSettings(collisionCategories, categoriesCollidesWith, cDelegate, sDelegate, sleepingAllowed, isSensor, ignoreGravity, body, mass,friction);

            body.Tag = userData;
            return new HullBody(body);

        }

        private static void CreateCommonSettings(List<Category> collisionCategories, List<Category> categoriesCollidesWith,
            OnCollisionEventHandler cDelegate, OnSeparationEventHandler sDelegate, bool sleepingAllowed,
            bool isSensor, bool ignoreGravity, Body body, float mass, float friction)
        {
            if (categoriesCollidesWith != null)
                body.SetCollidesWith(GetCat(categoriesCollidesWith));


            if (collisionCategories != null)
                body.SetCollisionCategories(GetCat(collisionCategories));


            body.IgnoreGravity = ignoreGravity;
            body.SleepingAllowed = sleepingAllowed;
            body.OnCollision += cDelegate;
            body.OnSeparation += sDelegate;
            body.SetIsSensor(isSensor);
            body.Mass = mass;
            body.SetFriction(friction);
        }


        public static HullBody CreateRectangularHullBody(BodyType bodyType, Vector2? position, float? width,float? height, List<Category>? collisionCategories, List<Category>? categoriesCollidesWith,
            OnCollisionEventHandler? cDelegate, OnSeparationEventHandler? sDelegate, float density = 1f, float rotation = 0f,
            float restitution = 1f, float friction = 1f, float mass = 1f, float inertia = 0, bool sleepingAllowed = true, bool isSensor = false, bool ignoreGravity = false,
            object userData = null, int xOffset = 0, int yOffset = 0, bool blocksLight = false)
        {
            
            width = width ?? 4f;
            height = height ?? 4f;
            position = position ?? Vector2.Zero;
            Body body = PhysicsManager.VelcroWorld.CreateRectangle( (float)width,(float) height, density,(Vector2)position, rotation, bodyType);

            CreateCommonSettings(collisionCategories, categoriesCollidesWith, cDelegate, sDelegate, sleepingAllowed, isSensor, ignoreGravity, body, mass, friction);



            body.Tag = userData;

            return new HullBody(body);

        }

        public static HullBody CreatePolygonHullBody(BodyType bodyType, Vector2? position, Vertices vertices, List<Category>? collisionCategories, List<Category>? categoriesCollidesWith,
            OnCollisionEventHandler? cDelegate, OnSeparationEventHandler? sDelegate, float density = 1f, float rotation = 0f,
            float restitution = 1f, float friction = 1f, float mass = 1f, float inertia = 0, bool sleepingAllowed = true, bool isSensor = false, bool ignoreGravity = false,
            object userData = null, int xOffset = 0, int yOffset = 0, bool blocksLight = false)
        {

 
            position = position ?? Vector2.Zero;

            Body body = VelcroWorld.CreatePolygon(vertices, density, (Vector2)position, rotation, bodyType);
            CreateCommonSettings(collisionCategories, categoriesCollidesWith, cDelegate, sDelegate, sleepingAllowed, isSensor, ignoreGravity, body, mass, friction);




            body.Tag = userData;

            return new HullBody(body);

        }

        public static Category GetCat(List<Category> cats)
        {
            Category category = Category.None;
            foreach (Category cat in cats)
            {
                category |= cat; 
            }
            return category;
        }
        /// <summary>
        /// Welds two bodies together and returns the created joint.
        /// </summary>
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

        public enum RotateSpeed
        {
            None =0,
            VerySlow = 1,
            Slow = 2,
            Medium = 3,
            Fast = 4,
            Woah = 5
        }
        /// <summary>
        /// Welds two bodies together and returns the created joint.
        /// </summary>
        /// <returns></returns>
        public static RevoluteJoint RotateWeld(RotateSpeed rotateSpeed, Body bodyA, Body bodyB, Vector2? bodyAAnchor, Vector2? bodyBAnchor, float? dampingRatio, float? frequencyHz, bool counterClockWise)
        {
            if (rotateSpeed == RotateSpeed.None)
                throw new Exception($"Forgot to set rotate speed in inherited swingable tool class");
            bodyAAnchor = bodyAAnchor ?? Vector2.Zero;
            bodyBAnchor = bodyBAnchor ?? Vector2.Zero;

            dampingRatio = dampingRatio ?? 20f;
            frequencyHz = frequencyHz ?? 30f;
            RevoluteJoint joint = JointFactory.CreateRevoluteJoint(VelcroWorld, bodyA, bodyB, bodyAAnchor.Value, bodyBAnchor.Value);
            joint.MotorEnabled = true;
            joint.MaxMotorTorque = 8000 * (int)rotateSpeed;
            joint.MotorSpeed = 100000; //1 turn per second clockwise
            if (counterClockWise)
                joint.MotorSpeed = joint.MotorSpeed * -1;
    
            return joint;

        }

    }
}
