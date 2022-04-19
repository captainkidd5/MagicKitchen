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
    /// Menu which CONTAINS a list of your saves, and additionally a button at the top for creating a new game
    /// </summary>
    internal class ViewGamesMenu : InterfaceSection
    {
        private NineSliceTextButton _createNewButton;
        private Text _createNewGameText;
            
        private static Rectangle _saveSlotRectangle = new Rectangle(0, 0, 128, 64);

        
        public ViewGamesMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }

        public override void LoadContent()
        {
            Position = Settings.CenterScreen;
            SaveLoadManager.FetchAllMetadata();
            Vector2 _saveSlotPosition = Position;
            _createNewGameText = TextFactory.CreateUIText("Create New", GetLayeringDepth(UILayeringDepths.Medium));

            Action _createNewButtonAction = ChangeToCreateNewSaveMenu;
            _createNewButton = new NineSliceTextButton(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low),
                _saveSlotRectangle, null, new List<Text>() { 
                    _createNewGameText, 
                    TextFactory.CreateUIText("Second Line", GetLayeringDepth(UILayeringDepths.Medium)),
                TextFactory.CreateUIText("Long Third Lineeeeee", GetLayeringDepth(UILayeringDepths.Medium))},null, _createNewButtonAction, true);
           Dictionary<string,SaveFile> saveFiles = SaveLoadManager.SaveFiles;

            int i = 0;
            foreach(KeyValuePair<string,SaveFile> file in saveFiles) { 
                _saveSlotPosition = new Vector2(_saveSlotPosition.X, _saveSlotPosition.Y + (i + 1) * _saveSlotRectangle.Height);

                SaveSlotPanel panel = new SaveSlotPanel(file.Value, this, graphics, content, _saveSlotPosition, GetLayeringDepth(UILayeringDepths.Low));
                panel.LoadContent();
            }
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, _saveSlotRectangle.Width, _saveSlotRectangle.Height);
            base.LoadContent();

        }

        internal override void Reset()
        {
            base.Reset();
            LoadContent();
        }
        public override void Unload()
        {
            base.Unload();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
          //  _createNewGameText.Update(gameTime, _createNewButton.Position);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //_createNewGameText.Draw(spriteBatch, true);
        }

       private void ChangeToCreateNewSaveMenu()
        {
            (parentSection as OuterMenu).ChangeState(OuterMenuState.CreateNewSave);
        }

       
    }
}
