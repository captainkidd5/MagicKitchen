using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhysicsEngine.Classes.Gadgets
{
    /// <summary>
    /// Moves collidable towards give position
    /// </summary>
    public class Magnetizer : PhysicsGadget
    {
        private readonly Collidable _collidableToMoveTowards;
        private readonly int _errorMargin;
        private readonly float _speed;

        public Magnetizer(Collidable collidable, Collidable collidableToMoveTowards,int errorMargin = 2, float speed = 2f) :base(collidable)
        {
            _collidableToMoveTowards = collidableToMoveTowards;
            _errorMargin = errorMargin;
            _speed = speed;
        }
        public override void Update(GameTime gameTime)
        {
            if(!_collidableToMoveTowards.MainHullBody.Destroyed)
             JettisonTowardsVector();
        }
        private bool JettisonTowardsVector()
        {
            // If we're already at the goal return immediatly
            Vector2 currentPos = CollidableToInteractWith.CenteredPosition;
            Vector2 goal = _collidableToMoveTowards.CenteredPosition;
            if (Vector2Helper.WithinRangeOf(currentPos, goal, _errorMargin))
                return true;

            // Find direction from current MainHull.Position to goal
            Vector2 direction = Vector2.Normalize(goal - currentPos);

            // If we moved PAST the goal, move it back to the goal
            if (Math.Abs(Vector2.Dot(direction, Vector2.Normalize(goal - currentPos)) + 1) < 0.1f)
                currentPos = goal;

            // Return whether we've reached the goal or not, leeway of 2 pixels 
            if (currentPos.X + _errorMargin > goal.X && currentPos.Y - _errorMargin < goal.X
               && currentPos.Y + _errorMargin > goal.Y && currentPos.Y - _errorMargin < goal.Y)
            {
                return true;
            }
            CollidableToInteractWith.Jettison(direction, null,_speed);

            return false;
        }
    }
}
