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
    internal class GamepadControls : IInput
    {
        public bool DidClick => throw new NotImplementedException();

        public bool DidRightClick => throw new NotImplementedException();

        public bool EscapePressed => throw new NotImplementedException();

        public Vector2 MouseUIPosition => throw new NotImplementedException();

        public Vector2 MouseWorldPosition => throw new NotImplementedException();

        public List<Keys> PressedKeys => throw new NotImplementedException();

        public List<Buttons> TappedButtons => throw new NotImplementedException();


        public Direction GetDirectionFacing()
        {

        }

        public Direction SecondaryDirectionFacing => throw new NotImplementedException();

        public bool ScrollWheelIncreased => throw new NotImplementedException();

        public bool ScrollWheelDecreased => throw new NotImplementedException();

        public List<Keys> AcceptableKeysForTyping => throw new NotImplementedException();


        private GamePadState _newGamePadState;
        private GamePadState _oldGamePadState;

        public GamepadControls()
        {
            TappedButtons = new List<Buttons>();
        }
        public bool WasButtonPressed(Buttons button)
        {
            if (_oldGamePadState.IsButtonDown(button) && _newGamePadState.IsButtonUp(button))
                return true;
            return false;
        }
        void IInput.ClearRecentlyPressedKeys()
        {
            throw new NotImplementedException();
        }

        bool IInput.IsHoveringRectangle(Settings.ElementType elementType, Rectangle rectangle)
        {
            throw new NotImplementedException();
        }

        void IInput.Update(GameTime gameTime)
        {
            //  GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);

            TappedButtons.Clear();

            _oldGamePadState = _newGamePadState;
            _newGamePadState = GamePad.GetState(PlayerIndex.One);


            if (_newGamePadState.IsButtonUp(Buttons.A))
            {
                Console.WriteLine("test");
            }


        }

    }
}
