using EntityEngine.Classes.Animators;
using Globals.Classes;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes;

namespace EntityEngine.Classes
{
    internal class WarpHelper
    {
        private const float TimeBetweenWarp = 6f;
        private readonly Entity entity;

        private SimpleTimer WarpTimer { get; set; }
        public bool AbleToWarp { get; set; }
        public bool IsWarping { get; set; }
        public WarpHelper(Entity entity)
        {
            WarpTimer = new SimpleTimer(TimeBetweenWarp, false);
            this.entity = entity;
        }

        private Vector2 _intermediateWarpPosition;
        private string _intermediateStageTo;

        public void CheckWarp(GameTime gameTime)
        {
            if (!AbleToWarp && WarpTimer.Run(gameTime))
                AbleToWarp = true;
        }
        /// <summary>
        /// Fades animator in, and finally actually moves the npc to the new stage
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="tileManager"></param>
        /// <returns>Returns true if entity is now in the players stage, else returns false.</returns>
        public bool FinishedWarpAndReady(Animator animator, TileManager tileManager)
        {

            animator.FadeIn();

            entity.Move(_intermediateWarpPosition);
            entity.LoadToNewStage(_intermediateStageTo, tileManager);

            IsWarping = false;

            if (entity.IsInStage)
                return true;
            
            return false;
        }
        /// <summary>
        /// Intiates warp and starts fade out. Will trigger Finish warp once timer runs down.
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="stageTo"></param>
        /// <param name="positionTo"></param>
        /// <param name="tileManager"></param>
        public void StartWarp(Animator animator, string stageTo, Vector2 positionTo, TileManager tileManager)
        {
            animator.FadeOut();

            _intermediateStageTo = stageTo;
            _intermediateWarpPosition = positionTo;
            AbleToWarp = false;
            WarpTimer.SetNewTargetTime(TimeBetweenWarp);
            WarpTimer.ResetToZero();

            IsWarping = true;
        }
    }
}
