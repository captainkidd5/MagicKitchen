using EntityEngine.Classes.Animators;
using EntityEngine.Classes.PlayerStuff;
using Globals.Classes;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes;
using static Globals.Classes.Settings;

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
        private Direction _directionToFace;

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
        public bool FinishWarpAndFinalMove(Animator animator, TileManager tileManager, ItemManager itemManager)
        {

            animator.FadeIn();

            entity.Move(_intermediateWarpPosition);
            if(entity.GetType()!=typeof(Player))
             entity.SwitchStage(_intermediateStageTo, tileManager, itemManager);
            entity.FaceDirection(_directionToFace);
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
        public void StartWarp(Animator animator, string stageTo, Vector2 positionTo, Direction directionToFace)
        {
            animator.FadeOut();

            _intermediateStageTo = stageTo;
            _intermediateWarpPosition = positionTo;
            _directionToFace = directionToFace;
            AbleToWarp = false;
            WarpTimer.SetNewTargetTime(TimeBetweenWarp);
            WarpTimer.ResetToZero();

            IsWarping = true;
        }
    }
}
