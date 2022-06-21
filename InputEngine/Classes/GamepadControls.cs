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
        Y = 3,//Y, drop item
        X = 4,
        DPadUp = 5,
        DPadDown =6,
        DPadLeft =7,
        DPadRight =8,
        TriggerLeft = 9,
        TriggerRight = 10,
        BumperLeft = 11,
        BumperRight = 12,
        Escape = 13,
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

        public Vector2 GetThumbStickVector(Direction direction)
        {
            Vector2 thumbStick = Vector2.Zero;
            if (direction == Direction.Left)
                thumbStick = _newGamePadState.ThumbSticks.Left;
            else if (direction == Direction.Right)
                thumbStick = _newGamePadState.ThumbSticks.Right;
            else
                throw new Exception($"Must provide either left or right");

            float thumbstickTolerance = 0.15f;

            var absX = Math.Abs(thumbStick.X);
            var absY = Math.Abs(thumbStick.Y);
            //Inverting y here cuz
            return new Vector2(absX > thumbstickTolerance ? thumbStick.X : 0, absY > thumbstickTolerance ? thumbStick.Y * -1 : 0);
        }

        public float GetThumbStickRotation(Direction direction)
        {
            Vector2 thumbStick = GetThumbStickVector(direction);
            float rotation = (float)Math.Atan2(thumbStick.Y, thumbStick.X);
            return rotation;
        }

        private GamePadState _newGamePadState;
        private GamePadState _oldGamePadState;

        public GamepadControls()
        {
            _gamePadMappings = new Dictionary<GamePadActionType, ControllerMappings>();

            ControllerMappings mapping = new ControllerMappings(GamePadActionType.Select);
            mapping = MapPrimaryButtons(mapping);

            mapping = new ControllerMappings(GamePadActionType.Escape);
            mapping.Remap(Buttons.Start);
            _gamePadMappings.Add(GamePadActionType.Escape, mapping);

            mapping = new ControllerMappings(GamePadActionType.TriggerLeft);
            mapping.Remap(Buttons.LeftTrigger);
            _gamePadMappings.Add(GamePadActionType.TriggerLeft, mapping);

            mapping = new ControllerMappings(GamePadActionType.TriggerRight);
            mapping.Remap(Buttons.RightTrigger);
            _gamePadMappings.Add(GamePadActionType.TriggerRight, mapping);

            mapping = new ControllerMappings(GamePadActionType.BumperLeft);
            mapping.Remap(Buttons.LeftShoulder);
            _gamePadMappings.Add(GamePadActionType.BumperLeft, mapping);

            mapping = new ControllerMappings(GamePadActionType.BumperRight);
            mapping.Remap(Buttons.RightShoulder);
            _gamePadMappings.Add(GamePadActionType.BumperRight, mapping);

            mapping = MapDPad();

        }

        private ControllerMappings MapPrimaryButtons(ControllerMappings mapping)
        {
            mapping.Remap(Buttons.A);
            _gamePadMappings.Add(GamePadActionType.Select, mapping);

            mapping = new ControllerMappings(GamePadActionType.Cancel);
            mapping.Remap(Buttons.B);
            _gamePadMappings.Add(GamePadActionType.Cancel, mapping);

            mapping = new ControllerMappings(GamePadActionType.Y);
            mapping.Remap(Buttons.Y);
            _gamePadMappings.Add(GamePadActionType.Y, mapping);

            mapping = new ControllerMappings(GamePadActionType.X);
            mapping.Remap(Buttons.X);
            _gamePadMappings.Add(GamePadActionType.X, mapping);
            return mapping;
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
        public bool IsActionHeld(GamePadActionType gamePadActionType)
        {
            return (_oldGamePadState.IsButtonDown(_gamePadMappings[gamePadActionType].Button) &&
               _newGamePadState.IsButtonDown(_gamePadMappings[gamePadActionType].Button));
        }
        public bool WasActionTapped(GamePadActionType gamePadActionType)
        {
            return (_oldGamePadState.IsButtonDown(_gamePadMappings[gamePadActionType].Button) &&
                _newGamePadState.IsButtonUp(_gamePadMappings[gamePadActionType].Button));
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
