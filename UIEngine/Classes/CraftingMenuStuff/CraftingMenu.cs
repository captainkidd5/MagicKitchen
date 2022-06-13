using DataModels.ItemStuff;
using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEngine.Classes.CraftingMenuStuff
{
    internal class CraftingMenu : MenuSection
    {
        private CraftingPage _craftingPage;
        private Rectangle _totalRectangle = new Rectangle(0, 0, 240, 336);
        public CraftingMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) : 
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            Activate();
            Position = RectangleHelper.CenterRectangleOnScreen(_totalRectangle);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void LoadContent()
        {
         //   base.LoadContent();
            _craftingPage = new CraftingPage(CraftingCategory.Tool, this, graphics, content, Position,
                GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Low));
            _craftingPage.LoadContent();
        }

        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
