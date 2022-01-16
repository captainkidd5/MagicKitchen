using IOEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;
using TextEngine.Classes;
using UIEngine.Classes.ButtonStuff;

namespace UIEngine.Classes.MainMenuStuff.ViewGames
{
    /// <summary>
    /// Panel which contains information on an existing save file, available to load into
    /// </summary>
    internal class SaveSlotPanel : InterfaceSection
    {

        private readonly SaveFile _saveFile;

        private NineSliceButton _slotButton;
        private static Rectangle _slotButtonDimensions = new Rectangle(0, 0, 96, 80);

        private Text _nameText;
        private Vector2 _nameTextPosition;

        private Text _dateText;
        private Vector2 _dateTextPosition;


        public SaveSlotPanel(SaveFile saveFile, InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            _saveFile = saveFile;
        }

        public override void Load()
        {
            base.Load();
            Action saveAction = LoadSave;
            _slotButton = new NineSliceButton(parentSection, graphics, content, Position, LayerDepth, _slotButtonDimensions, null, null, null, saveAction, true);

            _nameText = TextFactory.CreateUIText(_saveFile.Name);
            Vector2 _centeredTextInRectanglePosition = Text.CenterInRectangle(_slotButton.HitBox, _nameText);
            _nameTextPosition = new Vector2(_centeredTextInRectanglePosition.X, _centeredTextInRectanglePosition.Y - _slotButtonDimensions.Height / 4);

            _dateText = TextFactory.CreateUIText(_saveFile.DateCreated.ToString());
            _centeredTextInRectanglePosition = Text.CenterInRectangle(_slotButton.HitBox, _dateText);
            _dateTextPosition = new Vector2(_centeredTextInRectanglePosition.X, _nameTextPosition.Y + 16);

            //_loadFileAction = _saveFile.LoadSave
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
            _nameText.Update(gameTime, _nameTextPosition);
            _dateText.Update(gameTime, _dateTextPosition);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            _slotButton.Draw(spriteBatch);
            _nameText.Draw(spriteBatch, true);
            _dateText.Draw(spriteBatch, true);
        }

        private void LoadSave()
        {
            UI.LoadGame(_saveFile);
        }

        
       

       
    }
}
