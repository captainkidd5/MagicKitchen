﻿using EntityEngine.Classes.Animators;
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

        public SimpleTimer WarpTimer { get; private set; }
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
        /// 
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="tileManager"></param>
        /// <returns>Returns true if entity is now in the players stage, else returns false.</returns>
        public bool FinishWarp(Animator animator, TileManager tileManager)
        {
            animator.FadeIn();

            entity.Move(_intermediateWarpPosition);
            entity.LoadToNewStage(_intermediateStageTo, tileManager);

            IsWarping = false;

            if (entity.IsInStage)
                return true;
            
            return false;
        }
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
