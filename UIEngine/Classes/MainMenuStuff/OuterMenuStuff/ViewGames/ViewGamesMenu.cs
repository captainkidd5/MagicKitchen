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
using UIEngine.Classes.Components;

namespace UIEngine.Classes.MainMenuStuff.OuterMenuStuff.ViewGames
{
    /// <summary>
    /// Menu which CONTAINS a list of your saves, and additionally a button at the top for creating a new game
    /// </summary>
    internal class ViewGamesMenu : MenuSection
    {
        private NineSliceTextButton _createNewButton;
          

        private int _totalWidth;
        private StackPanel _stackPanel;
        public ViewGamesMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }

        public override void LoadContent()
        {
            ClearGrid();
            Position = new Vector2(parentSection.TotalBounds.X,
                parentSection.TotalBounds.Y);
            _totalWidth = parentSection.TotalBounds.Width;
            _stackPanel = new StackPanel(this, graphics, content, Position, LayerDepth);


            SaveLoadManager.FetchAllMetadata();
            Vector2 _saveSlotPosition = Position;

            _createNewButton = UI.ButtonFactory.CreateNSliceTxtBtn(_stackPanel, Position,GetLayeringDepth(UILayeringDepths.Low),
                new List<string>() {
                    "Create New"}, ChangeToCreateNewSaveMenu);
            AddSectionToGrid(_createNewButton, 1, 0);

            Dictionary<string,SaveFile> saveFiles = SaveLoadManager.SaveFiles;
            StackRow stackRow1 = new StackRow(_totalWidth);
            stackRow1.AddItem(_createNewButton, StackOrientation.Center);
            _stackPanel.Add(stackRow1);

            int currentGridY = 1;
            foreach(KeyValuePair<string,SaveFile> file in saveFiles) { 

                StackRow stackRow = new StackRow(_totalWidth);

                SaveSlotPanel panel = new SaveSlotPanel(file.Value, _stackPanel, graphics,
                    content, Position, GetLayeringDepth(UILayeringDepths.Low));
                panel.LoadContent();
                //panel.MovePosition(Position);
                AddSectionToGrid(panel, 1, currentGridY);


                stackRow.AddItem(panel, StackOrientation.Left);

                Button button = new Button(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium),
                    ButtonFactory.s_redExRectangle, new Action(() => { panel.DeleteSave(); Reset(); }));

                //Adds button directly to the right of save panel in the grid
                AddSectionToGrid(button, 2, currentGridY);

                button.AddConfirmationWindow("Really Delete Save?");
                button.LoadContent();

                stackRow.AddItem(button, StackOrientation.Left);



                _stackPanel.Add(stackRow);
                currentGridY++;
            }
            TotalBounds = parentSection.TotalBounds;
            CurrentSelected = _createNewButton;
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
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

       private void ChangeToCreateNewSaveMenu()
        {
            (parentSection as OuterMenu).ChangeState(OuterMenuState.CreateNewSave);
        }

       
    }
}
