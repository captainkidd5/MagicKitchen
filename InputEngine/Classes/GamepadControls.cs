using DataModels;
using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataModels.Enums;

namespace InputEngine.Classes
{
    internal class GamepadControls 
    {


     



        public Direction GetDirectionFacing()
        {
            var gamePadThumbStick = _newGamePadState.ThumbSticks.Left;
            var absX = Math.Abs(gamePadThumbStick.X);
            var absY = Math.Abs(gamePadThumbStick.Y);

            if (absX > absY)
            {
                if (gamePadThumbStick.X > 0)
                    return Direction.Right;
                else
                    return Direction.Left;
            }
            else
            {
                if (gamePadThumbStick.Y > 0)
                    return Direction.Down;
                else
                    return Direction.Up;
            }
        }

        public Direction SecondaryDirectionFacing => throw new NotImplementedException();



        private GamePadState _newGamePadState;
        private GamePadState _oldGamePadState;
        private GamePadCapabilities _gamePadCapabilities;

        public GamepadControls()
        {
        }
        public bool WasButtonTapped(Buttons button)
        {
            if (_oldGamePadState.IsButtonDown(button) && _newGamePadState.IsButtonUp(button))
                return true;
            return false;
        }
       

        public void Update(GameTime gameTime)
        {


            _oldGamePadState = _newGamePadState;
            _newGamePadState = GamePad.GetState(PlayerIndex.One);


            if (WasButtonTapped(Buttons.A))
            {
                Console.WriteLine("test");
            }


        }

    }
}
