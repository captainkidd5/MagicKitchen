using Globals.Classes;
using Globals.Classes.Helpers;
using IOEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations.BodyPartStuff;
using SpriteEngine.Classes.InterfaceStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;
using TextEngine.Classes;
using UIEngine.Classes.ButtonStuff;
using UIEngine.Classes.Components;
using UIEngine.Classes.TextStuff;

namespace UIEngine.Classes.MainMenuStuff.OuterMenuStuff.CreateNewGameStuff
{
    internal class CreateNewSaveMenu : InterfaceSection
    {


        private Text _createNewText;
        private Vector2 _createNewTextPosition;

        private TypingBox _nameTypingBox;
        private int _nameWindowWidth = 128;
        private int _nameWindowHeight = 32;

        private Rectangle _createNewGameButtonRectangle = new Rectangle(400, 720, 32, 32);
        private int _newGameWidth = 32;
        private int _newGameHeight = 32;
        private Button _createNewGameButton;
        private Action _createNewGameAction;

        private Button _backButton;

        private PlayerAvatarViewer _playerAvatarViewer;

        private StackPanel _stackPanel;

        private AvatarPartSwapper _hairSwapper;

        public CreateNewSaveMenu(InterfaceSection 
            
            
            
            interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }

        public override void LoadContent()
        {
            BodyPiece[] bodyPieces = new BodyPiece[]
    {
                    new Pants(0),
                    new Shoes(0),
                    new Shirt(0),
                    new Shoulders(0),
                    new Arms(0),
                    new Eyes(0),
                    new Head(0),
                      new Hair(0),

    };

            Position = new Vector2(parentSection.TotalBounds.X, parentSection.TotalBounds.Y);
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, parentSection.TotalBounds.Width, parentSection.TotalBounds.Height);

            _createNewText = TextFactory.CreateUIText("Create New Game", GetLayeringDepth(UILayeringDepths.High));
            _createNewTextPosition = Text.CenterInRectangle(parentSection.TotalBounds, _createNewText);
            _createNewTextPosition = new Vector2(_createNewTextPosition.X, parentSection.TotalBounds.Y + 4);
            _createNewText.ForceSetPosition(_createNewTextPosition);

            Vector2 typingBoxPos = RectangleHelper.CenterRectangleInRectangle(_nameWindowWidth,_nameWindowHeight, parentSection.TotalBounds);
            typingBoxPos = new Vector2(typingBoxPos.X, _createNewTextPosition.Y + _createNewText.TotalStringHeight * 2);
            _nameTypingBox = new TypingBox(this,graphics, content, typingBoxPos, GetLayeringDepth(UILayeringDepths.Low), _nameWindowWidth, _nameWindowHeight);

            Vector2 stackPanelPos = new Vector2(Position.X, typingBoxPos.Y + 64);
            _stackPanel = new StackPanel(this, graphics, content, stackPanelPos, GetLayeringDepth(UILayeringDepths.Low));


            StackRow avatarStackRow = new StackRow(Width);

            _playerAvatarViewer = new PlayerAvatarViewer(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium));
            _playerAvatarViewer.LoadContent();
            _playerAvatarViewer.SetPieces(bodyPieces);
            avatarStackRow.AddItem(_playerAvatarViewer, StackOrientation.Center);
            _stackPanel.Add(avatarStackRow);

            _hairSwapper = new AvatarPartSwapper(bodyPieces[7], _stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
            _hairSwapper.LoadContent();

            StackRow hairStackRow = new StackRow(Width);
            hairStackRow.AddItem(_hairSwapper, StackOrientation.Center);
            _stackPanel.Add(hairStackRow);

            _createNewGameAction = CreateNewSaveAction;

            _createNewGameButton = new Button(this, graphics, content, RectangleHelper.PlaceBottomRightQuadrant(parentSection.TotalBounds, _createNewGameButtonRectangle),
                GetLayeringDepth(UILayeringDepths.Medium), _createNewGameButtonRectangle, CreateNewSaveAction);

            _createNewGameButton.SetLock(true);

            Vector2 backButtonPosition = RectangleHelper.PlaceRectangleAtBottomLeftOfParentRectangle(
             parentSection.TotalBounds, UISourceRectangles._backButtonRectangle);

            _backButton = UI.ButtonFactory.CreateButton(this, backButtonPosition,
                GetLayeringDepth(UILayeringDepths.Medium), UISourceRectangles._backButtonRectangle,
                new Action(() =>
                {
                    (parentSection as OuterMenu).ChangeState(OuterMenuState.ViewGames);
                })
              , scale: 2f);


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

            _backButton.Update(gameTime);

            _stackPanel.Update(gameTime);
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
            _backButton.Draw(spriteBatch);
            _stackPanel.Draw(spriteBatch);
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
