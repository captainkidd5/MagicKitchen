using EntityEngine.Classes.CharacterStuff;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes;

namespace EntityEngine.Classes.BehaviourStuff.PatronStuff
{

    public enum PatronState
    {
        None =0,
        FindingSeating = 1,
        Ordering = 2
    }
    internal class PatronBehaviour : Behaviour
    {
        private PatronState _patronState;
        public PatronBehaviour(Entity entity, StatusIcon statusIcon, Navigator navigator,
            TileManager tileManager, float? timerFrequency) :
            base(entity, statusIcon, navigator, tileManager, timerFrequency)
        {

        }

        public override void Update(GameTime gameTime, ref Vector2 velocity)
        {
            base.Update(gameTime, ref velocity);
            switch (_patronState)
            {
                case PatronState.None:
                    break;
                case PatronState.FindingSeating:
                    break;
                case PatronState.Ordering:
                    break;
            }
            if (SimpleTimer.Run(gameTime))
            {
                CheckForUpdatedSchedule();
            }
            //if (Navigator.HasActivePath)
            //    Navigator.FollowPath(gameTime, Entity.Position, ref velocity);
            //else
            //    Entity.Halt();

            //if (_activeSchedule != null)
            //    Entity.TargetStage = _activeSchedule.StageEndLocation;
        }
    }
}
