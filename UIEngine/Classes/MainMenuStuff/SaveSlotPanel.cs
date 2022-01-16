using IOEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Classes.ButtonStuff;

namespace UIEngine.Classes.MainMenuStuff
{
    internal class SaveSlotPanel : InterfaceSection
    {
        private NineSliceButton _slotButton;
        private static Rectangle _slotButtonDimensions = new Rectangle(0, 0, 96, 80);
        private readonly SaveFile _saveFile;

        public SaveSlotPanel(SaveFile saveFile, InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            _saveFile = saveFile;
        }

        public override void Load()
        {
            base.Load();
            _slotButton = new NineSliceButton(parentSection, graphics, content, Position, LayerDepth, _slotButtonDimensions, null, null, null);
        }

        public override void Unload()
        {
            base.Unload();
            _slotButton = null;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _slotButton.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            _slotButton.Draw(spriteBatch);
        }

       

       
    }
}
