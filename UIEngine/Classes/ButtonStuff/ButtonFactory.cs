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
        public ButtonFactory(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
        }

        public Button CreateCloseButton(InterfaceSection section, Rectangle backGroundRectangleToPlaceOn, float layer)
        {
            Vector2 positionToPlace = RectangleHelper.PlaceRectangleAtTopRightOfParentRectangle(backGroundRectangleToPlaceOn, s_redExRectangle);
            return new Button(section, graphics, content, positionToPlace, layer, s_redExRectangle,
                null, UI.ButtonTexture, null, new Action(section.Close), scale: 1f);
        }

      
    }
}
