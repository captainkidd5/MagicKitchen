﻿using DataModels;
using DataModels.NPCStuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace SpriteEngine.Classes.Animations.EntityAnimations
{
    public class NPCAnimator : Animator
    {
        private AnimatedSprite _currentAnimation;
        private NPCData _npcData;

        public override int CurrentFrame => _currentAnimation.CurrentFrame;
        public int LastFrame => _currentAnimation.FrameLastFrame;
        public bool IsAttackFrame => _attackFrame != null && CurrentFrame == _attackFrame;
        public bool JustChangedToAttackFrame => IsAttackFrame && LastFrame != _attackFrame;
        private int? _attackFrame;

        public override bool IsPerformingAnimation()
        {
            return CurrentActionType != ActionType.Walking;
        }
        public NPCAnimator(NPCData npcData, int? xOffset, int? yOffset)
            : base(xOffset, yOffset)
        {
            _npcData = npcData;
        }

        public override void LoadInitialAnimations()
        {
            PerformAction(null, Direction.Up, ActionType.Walking);
        }

        public override void PerformAction(Action action, Direction direction, ActionType actionType, float speedModifier = 1f)
        {
            base.PerformAction(action,direction, actionType, speedModifier);
            List<AnimatedSprite> sprites = new List<AnimatedSprite>();
            foreach (AnimationInfo info in _npcData.AnimationInfo.Where(x => x.ActionType == actionType))
            {

                sprites.Add(SpriteFactory.AnimationInfoToWorldSprite(
                    Position, info, SpriteFactory.GetTextureFromNPCType(_npcData.NPCType),
                    new Rectangle(info.SpriteX * 16,
                    info.SpriteY * 16
                    , _npcData.SpriteWidth,
                    _npcData.SpriteHeight), _npcData.SpriteWidth / 2 * -1, _npcData.SpriteHeight, info.Flip));


                if (actionType == ActionType.Attack)
                {
                    _attackFrame = info.DamageFrame;
                }
                else
                {
                    _attackFrame = null;
                }
            }

            var spriteArray = sprites.ToArray();
            AnimatedSprites = spriteArray;
            CurrentActionType = actionType;
        }
        public override void Update(GameTime gameTime, Direction directionMoving, bool isMoving, Vector2 position, float speedRatio)
        {
            _currentAnimation = AnimatedSprites[(int)directionMoving - 1];

            Vector2 positionOffSet = new Vector2(position.X, position.Y  );
            //TODO: Layer offset for npcs is wrong
            float entityLayer  = SetPositionAndGetEntityLayer(position);

            Position = positionOffSet;
            bool resetToResting = !isMoving && WasMovingLastFrame;
            Layer = entityLayer;
            _currentAnimation.CustomLayer = entityLayer;

            if (resetToResting)
            {
                _currentAnimation.ResetToZero(Position, entityLayer);
            }
            _currentAnimation.Paused = !isMoving;

            if (OverridePause)
                _currentAnimation.Paused = false;
            _currentAnimation.Update(gameTime, Position);

            if(_currentAnimation.HasLoopedAtLeastOnce && CurrentActionType != ActionType.Walking)
            {
                //CurrentActionType = ActionType.Walking;
                //PerformAction(ActionToPerform, directionMoving, CurrentActionType);
            }




            WasMovingLastFrame = isMoving;

        }

        public override void Draw(SpriteBatch spriteBatch, SubmergenceLevel submergenceLevel, bool alwaysSubmerged = false)
        {
            if (_currentAnimation != null)
            {
                if (submergenceLevel > SubmergenceLevel.None && !alwaysSubmerged)
                    _currentAnimation.SwapSourceRectangle(
                        new Rectangle(_currentAnimation.SourceRectangle.X, _currentAnimation.SourceRectangle.Y,
                        _currentAnimation.SourceRectangle.Width, YOffset / 2));
                else
                    _currentAnimation.SwapSourceRectangle(
                     new Rectangle(_currentAnimation.SourceRectangle.X, _currentAnimation.SourceRectangle.Y,
                     _currentAnimation.SourceRectangle.Width, YOffset));
                _currentAnimation.Draw(spriteBatch);

            }

        }
    }
}
