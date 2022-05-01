using Globals.Classes;
using Globals.Classes.Helpers;
using IOEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.InterfaceStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;
using TextEngine.Classes;
using UIEngine.Classes.ButtonStuff;
using UIEngine.Classes.TextStuff;

namespace UIEngine.Classes.MainMenuStuff.OuterMenuStuff.CreateNewGameStuff
{
    internal class CreateNewSaveMenu : InterfaceSection
    {
        private Rectangle _backGroundRectangle;

        private Text _createNewText;
        private Vector2 _createNewTextPosition;

        private TypingBox _nameTypingBox;
        private int _nameWindowWidth = 128;
        private int _nameWindowHeight = 32;

        private Rectangle _createNewGameButtonRectangle = new Rectangle(0, 0, 32, 32);
        private int _newGameWidth = 32;
        private int _newGameHeight = 32;
        private NineSliceTextButton _createNewGameButton;
        private Action _createNewGameAction;
        public CreateNewSaveMenu(InterfaceSection interfaceSection,Rectangle backGroundRectangle, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            _backGroundRectangle = backGroundRectangle;
        }

        public override void LoadContent()
        {


            _createNewText = TextFactory.CreateUIText("Create New Game", GetLayeringDepth(UILayeringDepths.High));
            _createNewTextPosition = Text.CenterInRectangle(_backGroundRectangle, _createNewText);
            _createNewTextPosition = new Vector2(_createNewTextPosition.X, _backGroundRectangle.Y + 4);


            Vector2 typingBoxPos = RectangleHelper.CenterRectangleInRectangle(_nameWindowWidth,_nameWindowHeight, _backGroundRectangle);
            typingBoxPos = new Vector2(typingBoxPos.X, _createNewTextPosition.Y + _createNewText.TotalStringHeight * 2);
            _nameTypingBox = new TypingBox(this,graphics, content, typingBoxPos, GetLayeringDepth(UILayeringDepths.Low), _nameWindowWidth, _nameWindowHeight);

            _createNewGameAction = CreateNewSaveAction;
            _createNewGameButton = UI.ButtonFactory.CreateNSliceTxtBtn(this,
                RectangleHelper.PlaceBottomRightQuadrant(_backGroundRectangle, _createNewGameButtonRectangle), GetLayeringDepth(UILayeringDepths.Low), new List<string>()
                { "Go!" },  _createNewGameAction);
            _createNewGameButton.SetLock(true);

            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, _backGroundRectangle.Width, _backGroundRectangle.Height);
            base.LoadContent();

        }

        public override void Unload()
        {
            base.Unload();
        }
       
        

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
            CheckButtonLock();

            _nameTypingBox.Update(gameTime);
            _createNewText.Update(gameTime, _createNewTextPosition);
            _createNewGameButton.Update(gameTime);
            // _backGroundSprite.Update(gameTime, Position);
        }

        /// <summary>
        /// Disallow create new game if name is empty 
        /// </summary>
        private void CheckButtonLock()
        {
            if (_nameTypingBox.IsEmpty)
                _createNewGameButton.SetLock(true);
            else
                _createNewGameButton.SetLock(false);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //base.Draw(spriteBatch);
            _nameTypingBox.Draw(spriteBatch);
            _createNewText.Draw(spriteBatch, true);
            _createNewGameButton.Draw(spriteBatch);
           // _backGroundSprite.Draw(spriteBatch);  
        }


        private void CreateNewSaveAction()
        {
            Dictionary<string, SaveFile> saveFiles = SaveLoadManager.SaveFiles;

            if (saveFiles.Keys.Contains(_nameTypingBox.CurrentString)){
                
                return;
            }
            SaveLoadManager.CreateNewSave(_nameTypingBox.CurrentString);
            SaveLoadManager.SetCurrentSave(_nameTypingBox.CurrentString);
            Flags.IsNewGame = true;
            UI.LoadGame(SaveLoadManager.CurrentSave);
        }
    }
}
