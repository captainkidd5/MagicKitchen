using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Classes.ButtonStuff;
using UIEngine.Classes.Storage;

namespace UIEngine.Classes.CraftingMenuStuff
{
    internal class CraftingMenu : InventoryDisplay
    {
        private static Rectangle s_craftButtonSourceRectangle = new Rectangle(32, 80, 32, 32);
        private Button _craftButton;
        private StorageContainer _outPutStorageContainer;
        
        public CraftingMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }

        protected override void GenerateUI(bool displayWallet)
        {
            base.GenerateUI(displayWallet);
            _craftButton = new Button(this, graphics, content, Position, GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Medium),
                s_craftButtonSourceRectangle,)
        }

        private void CraftAction()
        {

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

       
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        
    }
}
