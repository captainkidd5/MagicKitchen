﻿using Globals.Classes;
using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEngine.Classes.ButtonStuff
{
    internal class ButtonFactory : Component
    {

        public static readonly Rectangle s_redExRectangle = new Rectangle(0, 80, 32, 32);
        public static readonly Rectangle s_greenCheckRectangle = new Rectangle(32, 80, 32, 32);

        private float _scale = 1f;
        public ButtonFactory(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
        }

        public Button CreateButton(InterfaceSection interfaceSection,
            Vector2 position, float layerDepth, Rectangle? sourceRectangle, 
            Action buttonAction = null, Sprite foregroundSprite = null, Point? samplePoint = null, bool hoverTransparency = true, float scale = 1f)
        {
            return new Button(interfaceSection,graphics,content,position,layerDepth,sourceRectangle,foregroundSprite,samplePoint,buttonAction,hoverTransparency,scale);
        }
        public Button CreateCloseButton(InterfaceSection section, Rectangle backGroundRectangleToPlaceOn, float layer, Action? customAction = null)
        {
            Vector2 positionToPlace = RectangleHelper.PlaceRectangleAtTopRightOfParentRectangle(backGroundRectangleToPlaceOn, new Rectangle(0,0, (int)(s_redExRectangle.Width * _scale), (int)(s_redExRectangle.Height * _scale)));
            return new Button(section, graphics, content, positionToPlace, layer, s_redExRectangle,
                null,  null,customAction ?? new Action(section.Deactivate), scale: _scale);
        }

      
    }
}
