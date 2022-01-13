using InputEngine.Classes.Input;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using static Globals.Classes.Settings;

namespace InputEngine.Classes
{
    internal interface IInput
    {
        bool DidClick { get; }
        bool DidRightClick { get; }
        bool EscapePressed { get; }
        Vector2 MouseUIPosition { get; }
        Vector2 MouseWorldPosition { get;  }



        List<Keys> PressedKeys { get; }
        List<Keys> TappedKeys { get; }

        Direction GetDirectionFacing { get; }
        Direction SecondaryDirectionFacing { get;}

        bool ScrollWheelIncreased { get; }
        bool ScrollWheelDecreased { get; }


        List<Keys> AcceptableKeysForTyping { get; }

        internal void Update(GameTime gameTime);

        internal bool IsHoveringRectangle(ElementType elementType,Rectangle rectangle);

        internal void ClearRecentlyPressedKeys();
    }
}
