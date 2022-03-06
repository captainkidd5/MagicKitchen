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
        public ButtonFactory(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
        }

        public Button CreateCloseButton(InterfaceSection section, Rectangle backGroundRectangleToPlaceOn, float layer)
        {
            Rectangle redExRectangle = new Rectangle(0, 80, 32, 32);
            Vector2 positionToPlace = RectangleHelper.PlaceRectangleAtTopRightOfParentRectangle(backGroundRectangleToPlaceOn, redExRectangle);
            return new Button(section, graphics, content, positionToPlace, layer, redExRectangle,
                null, UI.ButtonTexture, null, new Action(section.Close), scale: 1f);
        }
    }
}
