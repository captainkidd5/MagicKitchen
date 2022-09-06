using EntityEngine.Classes.CharacterStuff;
using EntityEngine.Classes.NPCStuff;
using Globals.Classes;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using TiledEngine.Classes;

namespace EntityEngine.Classes.BehaviourStuff
{
    internal class WanderBehaviour : Behaviour
    {
        private Point _wanderRange;

        public WanderBehaviour(BehaviourManager behaviourManager, NPC entity, StatusIcon statusIcon, TileManager tileManager,
            Point? wanderRange, float? timerFrequency) : base(behaviourManager,entity, statusIcon, tileManager, timerFrequency)
        {
            //Default range is 5
            _wanderRange = wanderRange ?? new Point(5, 5);
        }

        public override void Update(GameTime gameTime, ref Vector2 velocity)
        {
 
            base.Update(gameTime, ref velocity);
            if (!Entity.HasActivePath)
            {
                if (SimpleTimer.Run(gameTime))
                {
                    Vector2 newRandomPos = GetNewWanderPosition();
                    if (Entity.FindPathTo(newRandomPos))
                    {
                        if (Entity.NPCData != null && Entity.NPCData.Name.ToLower() == "boar")
                            Console.WriteLine("test");
                        Entity.SetNavigatorTarget(newRandomPos);
                    }
                }
            }
            if (Entity.NPCData != null && Entity.NPCData.Name.ToLower() == "boar" && Entity.HasActivePath)
                Console.WriteLine("test");
            if (Entity.HasActivePath)
                Entity.FollowPath(gameTime, ref velocity);
            else
                Entity.Halt();
        }
        private Vector2 GetNewWanderPosition()
        {
            int lowX = Entity.TileOn.X;
            if (lowX - _wanderRange.X < 0)
                lowX = 0;
            else
                lowX -= _wanderRange.X;

            int highX = Entity.TileOn.X;
            if (highX + _wanderRange.X > TileManager.PathGrid.Weight.GetLength(0) - 1)
                highX = TileManager.PathGrid.Weight.GetLength(0) - 1;
            else
                highX += _wanderRange.X;

            int lowY = Entity.TileOn.Y;
            if (lowY - _wanderRange.Y < 0)
                lowY = 0;
            else
                lowY -= _wanderRange.Y;

            int highY = Entity.TileOn.Y;
            if (highY + _wanderRange.Y > TileManager.PathGrid.Weight.GetLength(1) - 1)
                highY = TileManager.PathGrid.Weight.GetLength(1) - 1;
            else
                highY += _wanderRange.Y;

            int finalX = Settings.Random.Next(lowX, highX);
            int finalY = Settings.Random.Next(lowY, highY);
            return new Vector2(finalX * Settings.TileSize,
               finalY * Settings.TileSize);
        }

        public override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if(Entity.NPCData != null && Entity.NPCData.Aggressive)
            {

            if (fixtureA.CollisionCategories==((Category)PhysCat.ArraySensor))
            {
                if(fixtureB.Body.Tag != Entity)
                {
                    BehaviourManager.ChaseAndAttack(fixtureB.Body.Tag as NPC);

                }
                return true;
            }
            }

            return true;
        }
    }
}
