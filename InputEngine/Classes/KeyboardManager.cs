using InputEngine.Classes.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace InputEngine.Classes
{
    internal class KeyboardManager
    {

        private KeyboardState OldKeyBoardState { get; set; }
        private KeyboardState NewKeyBoardState { get; set; }
        internal List<Keys> PressedKeys { get; private set; }

        //These are keys which were pressed LAST FRAME and released THIS FRAME
        internal List<Keys> TappedKeys { get; private set; }

        private List<KeyTimer> KeysRecentlyPressed { get; set; }

        //Keys which were just pressed this frame, but have not yet been added to recently pressed keys.
        internal List<Keys> UseableRecentKeys { get; private set; }

        private List<Keys> MovementKeys { get; set; }

        internal Keys ActivePrimaryMovementKey { get; private set; }

        internal Keys ActiveSecondaryMovementKey { get; private set; }

        internal Direction PrimaryDirection { get; private set; }
        internal Direction SecondaryDirection { get; private set; }

        internal KeyboardManager()
        {
            NewKeyBoardState = Keyboard.GetState();
            OldKeyBoardState = NewKeyBoardState;
            TappedKeys = new List<Keys>();
            KeysRecentlyPressed = new List<KeyTimer>();
            UseableRecentKeys = new List<Keys>();
            MovementKeys = new List<Keys>() { Keys.None };
            PressedKeys = new List<Keys>();

        }

        internal void Update(GameTime gameTime)
        {
            TappedKeys.Clear();
            //UseableRecentKeys.Clear();
            OldKeyBoardState = NewKeyBoardState;
            NewKeyBoardState = Keyboard.GetState();

            PressedKeys = NewKeyBoardState.GetPressedKeys().ToList();
            ClearUseableKeys();
            CalculateUseableKeys();
            CalculateRecentlyPressedKeys();

            //Tick down time on recently pressed keys, and remove them if necessary.
            for (int i = 0; i < KeysRecentlyPressed.Count; i++)
                KeysRecentlyPressed[i].Update(gameTime);

            CalculateTappedKeys(OldKeyBoardState.GetPressedKeys());

            HandleMovementKeys();

        }

        /// <summary>
        ///Add all newly pressed keys to recently pressed keys.
        /// </summary>
        private void CalculateRecentlyPressedKeys()
        {
            foreach (Keys key in PressedKeys)
            {
                KeysRecentlyPressed.Add(new KeyTimer(key, KeysRecentlyPressed));
            }
        }

        /// <summary>
        ///Add to useable recent keys if qualified.
        /// </summary>
        private void CalculateUseableKeys()
        {
            foreach (Keys key in PressedKeys)
            {
                if (CanUsePressedKey(key))
                    UseableRecentKeys.Add(key);
            }
        }

        public void ClearUseableKeys()
        {
            UseableRecentKeys.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>Returns true if the key is pressed, and has not yet been added to recently pressed keys.</returns>
        public bool CanUsePressedKey(Keys key)
        {
            for (int i = 0; i < PressedKeys.Count; i++)
            {
                //If pressed keys this frame contains the key, and recently pressed keys does NOT contain this key.
                if (PressedKeys[i] == key && KeysRecentlyPressed.Find(x => x.Key == key) == null)
                    return true;
            }
            return false;
        }

        private void HandleMovementKeys()
        {
            AddMovementKeys();
            RemoveMovementKeys();


            //active movement key is the one at the front of the list

            this.ActivePrimaryMovementKey = MovementKeys[MovementKeys.Count - 1];
            if ((MovementKeys.Count - 2) >= 0)
            {
                ActiveSecondaryMovementKey = MovementKeys[MovementKeys.Count - 2];
            }
            else
            {
                ActiveSecondaryMovementKey = Keys.None;
            }

            PrimaryDirection = GetMovementDirection(ActivePrimaryMovementKey);
            SecondaryDirection = GetMovementDirection(ActiveSecondaryMovementKey);
        }

        /// <summary>
        /// Fills Tapped Keys with keys which were tapped last frame.
        /// </summary>
        private void CalculateTappedKeys(Keys[] oldPressedKeys)
        {
            foreach (Keys key in oldPressedKeys)
            {
                if (WasKeyPressed(key))
                {
                    TappedKeys.Add(key);
                }
            }
        }

        private void AddMovementKeys()
        {
            if (NewKeyBoardState.IsKeyDown(Keys.D) && !MovementKeys.Contains(Keys.D))
                MovementKeys.Add(Keys.D);

            if (NewKeyBoardState.IsKeyDown(Keys.A) && !MovementKeys.Contains(Keys.A))
                MovementKeys.Add(Keys.A);


            if (NewKeyBoardState.IsKeyDown(Keys.W) && !MovementKeys.Contains(Keys.W))
                MovementKeys.Add(Keys.W);


            if (NewKeyBoardState.IsKeyDown(Keys.S) && !MovementKeys.Contains(Keys.S))
                MovementKeys.Add(Keys.S);
        }

        private void RemoveMovementKeys()
        {
            if (WasKeyPressed(Keys.D))
                MovementKeys.Remove(Keys.D);

            if (WasKeyPressed(Keys.A))
                MovementKeys.Remove(Keys.A);

            if (WasKeyPressed(Keys.W))
                MovementKeys.Remove(Keys.W);

            if (WasKeyPressed(Keys.S))
                MovementKeys.Remove(Keys.S);
        }

        private Direction GetMovementDirection(Keys key)
        {
            switch (key)
            {
                case Keys.W:
                    return Direction.Up;
                case Keys.S:
                    return Direction.Down;
                case Keys.A:
                    return Direction.Left;
                case Keys.D:
                    return Direction.Right;
                case Keys.None:
                    return Direction.None;
            }
            return Direction.None;
        }

        public bool WasKeyPressed(Keys key)
        {
            if (OldKeyBoardState.IsKeyDown(key) && NewKeyBoardState.IsKeyUp(key))
                return true;
            return false;
        }

        /// <summary>
        /// Returns true if this key was pressed in the last half second
        /// </summary>
        public bool WasKeyPressedInLastHalfSecond(Keys key)
        {
            foreach (KeyTimer keyTimer in KeysRecentlyPressed)
            {
                if (keyTimer.Key == key)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Used to calculate recently pressed keys.
        /// </summary>
        private class KeyTimer
        {
            //The amount of time we want to keep this key in the recently pressed keys list.
            private readonly float TimeToCheck = .05f;
            internal Keys Key { get; private set; }
            internal float TimeLastPressed { get; private set; }
            internal List<KeyTimer> RecentlyPressedKeys { get; private set; }
            internal KeyTimer(Keys key, List<KeyTimer> recentlyPressedKeys)
            {
                this.Key = key;
                this.TimeLastPressed = 0f;
                this.RecentlyPressedKeys = recentlyPressedKeys;
            }

            /// <summary>
            /// If this has been in the recently pressed keys list for longer than TimeToCheck, remove it.
            /// </summary>
            internal void Update(GameTime gameTime)
            {

                TimeLastPressed += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (TimeLastPressed > TimeToCheck)
                    RecentlyPressedKeys.Remove(this);
            }
        }
    }
}
