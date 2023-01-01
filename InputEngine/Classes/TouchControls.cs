using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
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
            foreach (var touch in NewTouchState)
            {
                if (touch.State == TouchLocationState.Pressed)
                {
                    Console.WriteLine("test");
                    //do what you want here when users tap the screen
                }
            }

            OldTouchState = NewTouchState;
        }
    }
}
