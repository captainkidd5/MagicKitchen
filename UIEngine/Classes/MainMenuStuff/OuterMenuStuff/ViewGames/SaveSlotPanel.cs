using Globals.Classes;
using IOEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;
using TextEngine.Classes;
using UIEngine.Classes.ButtonStuff;

namespace UIEngine.Classes.MainMenuStuff.OuterMenuStuff.ViewGames
{
    /// <summary>
    /// Panel which contains information on an existing save file, available to load into
    /// </summary>
    internal class SaveSlotPanel : InterfaceSection
    {

        private readonly SaveFile _saveFile;

        private NineSliceTextButton _slotButton;
        private static Rectangle _slotButtonDimensions = new Rectangle(0, 0, 96, 80);

        private Text _nameText;

        private Text _dateText;
        private Text _timeText;


        public SaveSlotPanel(SaveFile saveFile, InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            _saveFile = saveFile;
        }

        public override void LoadContent()
        {
            Action saveAction = LoadSave;
            _nameText = TextFactory.CreateUIText(_saveFile.Name, GetLayeringDepth(UILayeringDepths.High));
            _dateText = TextFactory.CreateUIText(_saveFile.DateCreated.Date.ToString("d"), GetLayeringDepth(UILayeringDepths.High));
            _timeText = TextFactory.CreateUIText(_saveFile.DateCreated.ToString("HH:mm"), GetLayeringDepth(UILayeringDepths.High));

            _slotButton = new NineSliceTextButton(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low), _slotButtonDimensions, null, null, new List<Text>() { _nameText, _dateText, _timeText }, null, saveAction, true);

            CloseButton = UI.ButtonFactory.CreateCloseButton(this,new Rectangle((int)Position.X, (int)Position.Y, _slotButtonDimensions.Width, _slotButtonDimensions.Height), GetLayeringDepth(UILayeringDepths.Medium));
            CloseButton.AddConfirmationWindow("Are you sure you want to delete this savefile?");
            CloseButton.LoadContent();
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, _slotButtonDimensions.Width, _slotButtonDimensions.Height);
            base.LoadContent();

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
            //_slotButton.Update(gameTime);

        }

        public override void Deactivate()
        {
            base.Deactivate();
            _saveFile.Delete();
            FlaggedForRemoval = true;
            parentSection.Reset();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //_slotButton.Draw(spriteBatch);

        }

        private void LoadSave()
        {
            Flags.IsNewGame = false;
            UI.LoadGame(_saveFile);
        }


        internal override void Reset()
        {
            base.Reset();
            FlaggedForRemoval = true;

        }




    }
}
