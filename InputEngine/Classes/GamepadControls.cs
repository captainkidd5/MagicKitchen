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

    public enum GamePadActionType
    {
        None = 0,
        Select = 1,
        Cancel = 2,
        Escape = 3,
        DPadUp =4,
        DPadDown =5,
        DPadLeft =6,
        DPadRight =7,
    }
    internal class GamepadControls 
    {




        public Dictionary<GamePadActionType, ControllerMappings> _gamePadMappings;

        public Direction GetDirectionFacing()
        {
            float thumbstickTolerance = 0.35f;
            var gamePadThumbStick = _newGamePadState.ThumbSticks.Left;
            var absX = Math.Abs(gamePadThumbStick.X);
            var absY = Math.Abs(gamePadThumbStick.Y);

            if (absX > absY && absX > thumbstickTolerance) 
            {
                if (gamePadThumbStick.X > 0)
                    return Direction.Right;
                else if (gamePadThumbStick.X < 0)
                    return Direction.Left;
            }
            else if(absX < absY && absY > thumbstickTolerance)
            {
                if (gamePadThumbStick.Y > 0)
                    return Direction.Up;
                else if (gamePadThumbStick.Y < 0)
                    return Direction.Down;
            }
            return Direction.None;
        }




        private GamePadState _newGamePadState;
        private GamePadState _oldGamePadState;

        public GamepadControls()
        {
            _gamePadMappings = new Dictionary<GamePadActionType, ControllerMappings>();

            ControllerMappings mapping = new ControllerMappings(GamePadActionType.Select);
            mapping.Remap(Buttons.A);
            _gamePadMappings.Add(GamePadActionType.Select, mapping);

            mapping = new ControllerMappings(GamePadActionType.Cancel);
            mapping.Remap(Buttons.B);
            _gamePadMappings.Add(GamePadActionType.Cancel, mapping);

            mapping = new ControllerMappings(GamePadActionType.Escape);
            mapping.Remap(Buttons.Start);
            _gamePadMappings.Add(GamePadActionType.Escape, mapping);

            mapping = MapDPad();

        }

        private ControllerMappings MapDPad()
        {
            ControllerMappings mapping = new ControllerMappings(GamePadActionType.DPadUp);
            mapping.Remap(Buttons.DPadUp);
            _gamePadMappings.Add(GamePadActionType.DPadUp, mapping);

            mapping = new ControllerMappings(GamePadActionType.DPadDown);
            mapping.Remap(Buttons.DPadDown);
            _gamePadMappings.Add(GamePadActionType.DPadDown, mapping);

            mapping = new ControllerMappings(GamePadActionType.DPadLeft);
            mapping.Remap(Buttons.DPadLeft);
            _gamePadMappings.Add(GamePadActionType.DPadLeft, mapping);

            mapping = new ControllerMappings(GamePadActionType.DPadRight);
            mapping.Remap(Buttons.DPadRight);
            _gamePadMappings.Add(GamePadActionType.DPadRight, mapping);
            return mapping;
        }

        public bool WasActionTapped(GamePadActionType gamePadActionType)
        {
            if (_oldGamePadState.IsButtonDown(_gamePadMappings[gamePadActionType].Button) &&
                _newGamePadState.IsButtonUp(_gamePadMappings[gamePadActionType].Button))
                return true;
            return false;
        }
       
        
        public void Update(GameTime gameTime)
        {


            _oldGamePadState = _newGamePadState;
            _newGamePadState = GamePad.GetState(PlayerIndex.One);


            if (WasActionTapped(GamePadActionType.Escape))
            {
                Console.WriteLine("test");
            }


        }

      

    }
}
