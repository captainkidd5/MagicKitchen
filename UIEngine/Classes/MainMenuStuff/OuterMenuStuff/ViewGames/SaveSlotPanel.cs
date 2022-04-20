using Globals.Classes;
using Globals.Classes.Helpers;
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
        private static int _width = 256;
        private static int _height = 80;

        private NineSliceButton _returnToMainMenuButton;


        public SaveSlotPanel(SaveFile saveFile, InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            _saveFile = saveFile;
        }

        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
            ChildSections.Clear();
            LoadContent();
        }
        public override void LoadContent()
        {
            Action action = LoadSave;

            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, _width - ButtonFactory.s_redExRectangle.Width * 2, _height);

            _slotButton = UI.ButtonFactory.CreateNSliceTxtBtnManualDimensions(this, Position, _width - ButtonFactory.s_redExRectangle.Width * 2, _height, GetLayeringDepth(UILayeringDepths.Medium),
                new List<string>() { _saveFile.Name, _saveFile.DateCreated.Date.ToString("d"), _saveFile.DateCreated.ToString("HH:mm") },  action);

            base.LoadContent();

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

        public void DeleteSave()
        {
            _saveFile.Delete();
            Deactivate();

        }
        public override void Deactivate()
        {
            base.Deactivate();
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
