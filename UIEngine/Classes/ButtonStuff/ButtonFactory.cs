using Globals.Classes;
using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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

        public Button CreateCloseButton(InterfaceSection section, Rectangle backGroundRectangleToPlaceOn, float layer, Action? customAction = null)
        {
            Vector2 positionToPlace = RectangleHelper.PlaceRectangleAtTopRightOfParentRectangle(backGroundRectangleToPlaceOn, new Rectangle(0,0, (int)(s_redExRectangle.Width * _scale), (int)(s_redExRectangle.Height * _scale)));
            return new Button(section, graphics, content, positionToPlace, layer, s_redExRectangle,
                null, UI.ButtonTexture, null,customAction ?? new Action(section.Deactivate), scale: _scale);
        }

      
    }
}
