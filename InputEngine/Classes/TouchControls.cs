using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using SpriteEngine.Classes.RenderTargetStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputEngine.Classes
{
    internal class TouchControls
    {
        private TouchCollection OldTouchState { get; set; }

        private TouchCollection NewTouchState { get; set; }
        public void Update(GameTime gameTime)
        {
            NewTouchState = TouchPanel.GetState();
            foreach (TouchLocation touch in NewTouchState)
            {

                if (touch.State == TouchLocationState.Moved || touch.State == TouchLocationState.Pressed)
                {
                    Controls.LatestTouchUIPosition = GetUIOffSet(touch.Position);

                }
            }

            OldTouchState = NewTouchState;
        }

        public bool DidTouchOccurHere(Rectangle rectangle)
        {

            return rectangle.Contains(Controls.LatestTouchUIPosition);
        }
        private Vector2 GetUIOffSet(Vector2 unadjustedPos)
        {
            Vector2 renderTargetResolution = new Vector2(RenderTargetManager.CurrentOverlayTarget.Width / (float)Settings.ScreenRectangle.Width, RenderTargetManager.CurrentOverlayTarget.Height / (float)Settings.ScreenRectangle.Height);

            return unadjustedPos * renderTargetResolution;
        }
        //public bool IsHeld(TouchLocation touch)
        //{
        //    return (OldTouchState.IsButtonDown(_gamePadMappings[gamePadActionType].Button) &&
        //       _newGamePadState.IsButtonDown(_gamePadMappings[gamePadActionType].Button));
        //}
        //public bool WasTapped(TouchLocation touch)
        //{
        //    return (_oldGamePadState.IsButtonDown(_gamePadMappings[gamePadActionType].Button) &&
        //        _newGamePadState.IsButtonUp(_gamePadMappings[gamePadActionType].Button));
        //}
    }
}
