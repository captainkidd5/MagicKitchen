using EntityEngine.Classes.CharacterStuff;
using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes;

namespace EntityEngine.Classes.BehaviourStuff.DamageResponses
{
    internal class FleeBehaviour : Behaviour
    {
        private readonly Entity _otherEntity;
        //When entity is this distance away from chased entity, stop attacking and chase again (basically, outside of hit zone)
        private int _distanceToStop = 160;
        private float _speedModifier = 4f;
        public FleeBehaviour(Entity entity, Entity otherEntity, StatusIcon statusIcon, Navigator navigator, TileManager tileManager, float? timerFrequency) : base(entity, statusIcon, navigator, tileManager, timerFrequency)
        {
            _otherEntity = otherEntity;

            SimpleTimer.SetNewTargetTime(.25f);
        }

        public override void Update(GameTime gameTime, ref Vector2 velocity)
        {
            base.Update(gameTime, ref velocity);
            if (Entity.DamageBody.Body.FixtureList[0].CollidesWith > tainicom.Aether.Physics2D.Dynamics.Category.None)
            {
                Entity.DeactivateDamageBody();
            }
           bool farEnoughAwayToStop = Vector2Helper.MoveAwayFromVector(_otherEntity.CenteredPosition, Entity.Position, ref velocity,
                gameTime, _distanceToStop, _speedModifier);
           
        }

    }
}
