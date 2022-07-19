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
        private Button _createNewButton;

        private Button _backButton;
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
            _createNewButton = new Button(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium),
            new Rectangle(208, 400, 96, 48), ChangeToCreateNewSaveMenu);

            Dictionary<string, SaveFile> saveFiles = SaveLoadManager.SaveFiles;

            Selectables = new InterfaceSection[saveFiles.Count + 1,3];

            StackRow stackRow1 = new StackRow(_totalWidth);
            stackRow1.AddItem(_createNewButton, StackOrientation.Center);
            _stackPanel.Add(stackRow1);
            AddSectionToGrid(_createNewButton, 0, 1);

            int currentGridY = 1;
            foreach(KeyValuePair<string,SaveFile> file in saveFiles) { 

                StackRow stackRow = new StackRow(_totalWidth);

                SaveSlotPanel panel = new SaveSlotPanel(file.Value, _stackPanel, graphics,
                    content, Position, GetLayeringDepth(UILayeringDepths.Low));
                panel.LoadContent();
                //panel.MovePosition(Position);
                AddSectionToGrid(panel, currentGridY, 1);


                stackRow.AddItem(panel, StackOrientation.Left);

                Button button = new Button(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium),
                    ButtonFactory.s_redExRectangle, new Action(() => { panel.DeleteSave(); Reset(); }));

                //Adds button directly to the right of save panel in the grid
                AddSectionToGrid(button, currentGridY, 2);

                button.AddConfirmationWindow("Really Delete Save?");
                button.LoadContent();

                stackRow.AddItem(button, StackOrientation.Left);



                _stackPanel.Add(stackRow);
                currentGridY++;
            }
            Vector2 backButtonPosition = RectangleHelper.PlaceRectangleAtBottomLeftOfParentRectangle(
                parentSection.TotalBounds, UISourceRectangles._backButtonRectangle);

            _backButton = UI.ButtonFactory.CreateButton(this, backButtonPosition,
                GetLayeringDepth(UILayeringDepths.Medium), UISourceRectangles._backButtonRectangle,
                (parentSection as OuterMenu).ChangeToPlayOrExitState, scale: 2f);
            _backButton.CustomClickSoundName = "BackButton1";

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
