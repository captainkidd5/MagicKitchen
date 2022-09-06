using EntityEngine.Classes.CharacterStuff;
using EntityEngine.Classes.NPCStuff;
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
        private float _speedModifier = 3f;
        private bool _farEnoughAway;
        public FleeBehaviour(BehaviourManager behaviourManager, NPC entity, Entity otherEntity, StatusIcon statusIcon, TileManager tileManager, float? timerFrequency) :
            base(behaviourManager, entity, statusIcon, tileManager, timerFrequency)
        {
            _otherEntity = otherEntity;

            SimpleTimer.SetNewTargetTime(6f);
        }

        public override void Update(GameTime gameTime, ref Vector2 velocity)
        {
            base.Update(gameTime, ref velocity);
            if (Entity.DamageBody.Body.FixtureList[0].CollidesWith > tainicom.Aether.Physics2D.Dynamics.Category.None)
            {
                Entity.DeactivateDamageBody();
            }

             _farEnoughAway = Vector2.Distance(_otherEntity.Position, Entity.Position) > _distanceToStop;
                
                
                

            if (_farEnoughAway)
            {
                if(SimpleTimer.Run(gameTime))
                BehaviourManager.ChangeBehaviour(DataModels.EndBehaviour.Wander);

                Entity.Halt();
            }
            else
            {
                SimpleTimer.ResetToZero();
                Vector2Helper.MoveAwayFromVector(_otherEntity.CenteredPosition, Entity.Position, ref velocity,
                gameTime, _distanceToStop, _speedModifier);
            }
        }

    }
}
