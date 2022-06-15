﻿using Globals.Classes;
using Globals.Classes.Helpers;
using InputEngine.Classes;
using InputEngine.Classes.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
using UIEngine.Classes.Components;
using UIEngine.Classes.CraftingMenuStuff;
using static DataModels.Enums;

namespace UIEngine.Classes.EscMenuStuff
{
    internal class EscMenu : MenuSection
    {

        private Rectangle _returnToMainMenuButtonBackgroundDimensions = new Rectangle(0, 0, 80, 96);
        private NineSliceButton _returnToMainMenuButton;



        private Sprite _backGroundSprite;

        public Rectangle BackGroundSource = new Rectangle(624, 224, 240, 256);

        private Action _returnToMainMenuAction;
        private Vector2 _scale = new Vector2(2f, 2f);


        private StackPanel _tabsStackPanel;
        private int _tabWidth = 32;


        private MenuSection _activePage;


        private readonly Rectangle _unclickedTabRectangle = new Rectangle(640, 192, 32, 32);
        private readonly Rectangle _selectedTabRectangle = new Rectangle(672, 186, 32, 38);

        EscPrimary _escPrimary;
        private Button _escPrimaryTabButton;

        CraftingMenu _craftingMenu;
        private Button _craftingTabButton;

        Dictionary<MenuSection, Button> _tabPairs;

        public int TitleOffSet { get; set; }

        private Vector2 _tabForgroundOffset = new Vector2(4, 4);


        private Point _tabSelectedPoint;
        public EscMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            Deactivate();

            NormallyActivated = false;
            Selectables = new InterfaceSection[1, 3];
            CurrentSelectedPoint = new Point(0, 0);


        }

        private void SwapActivePage(MenuSection newSection)
        {
            _activePage = newSection;
            AssignControlSectionAtEdge(Direction.Down, _activePage);

            _activePage.LoadContent();
            _tabSelectedPoint = CurrentSelectedPoint;
            foreach (var pair in _tabPairs)
            {
                if (pair.Key == newSection)
                    pair.Value.SwapBackgroundSprite(_selectedTabRectangle);
                else
                    pair.Value.SwapBackgroundSprite(_unclickedTabRectangle);
            }
        }
        private void GenerateTabs()
        {
            _tabsStackPanel = new StackPanel(this, graphics, content, new Vector2(TotalBounds.X, TotalBounds.Y - _tabWidth * _scale.Y), GetLayeringDepth(UILayeringDepths.Low));
            StackRow stackRow1 = new StackRow(TotalBounds.Width);

            Sprite cogSprite = SpriteFactory.CreateUISprite(
                Position, new Rectangle(64, 80, 32, 32), UI.ButtonTexture,
                GetLayeringDepth(UILayeringDepths.High), scale: _scale);
            _escPrimaryTabButton = new Button(_tabsStackPanel, graphics, content, Position,
                GetLayeringDepth(UILayeringDepths.Medium), _unclickedTabRectangle,
                new Action(() => { SwapActivePage(_escPrimary); }), hoverTransparency: true,
                foregroundSprite: cogSprite);
            _escPrimaryTabButton.SetForegroundSpriteOffSet(_tabForgroundOffset);
            stackRow1.AddItem(_escPrimaryTabButton, StackOrientation.Left);

            AddSectionToGrid(_escPrimaryTabButton, 0, 0);


            Sprite craftingSprite = SpriteFactory.CreateUISprite(
               Position, new Rectangle(96, 80, 32, 32), UI.ButtonTexture,
               GetLayeringDepth(UILayeringDepths.High), scale: _scale);

            _craftingTabButton = new Button(_tabsStackPanel, graphics, content, Position,
                GetLayeringDepth(UILayeringDepths.Medium), _unclickedTabRectangle,
                new Action(() => { SwapActivePage(_craftingMenu); }), hoverTransparency: true,
                foregroundSprite: craftingSprite);
            _craftingTabButton.SetForegroundSpriteOffSet(_tabForgroundOffset);
            stackRow1.AddItem(_craftingTabButton, StackOrientation.Left);

            AddSectionToGrid(_craftingTabButton, 0, 1);


            _tabsStackPanel.Add(stackRow1);

          

        }

        private void GeneratePages()
        {
            _escPrimary = new EscPrimary(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
            ChildSections.Remove(_escPrimary);

            _craftingMenu = new CraftingMenu(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
            ChildSections.Remove(_craftingMenu);

        }
        public override void LoadContent()
        {
            TransferSections.Clear();
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, (int)(BackGroundSource.Width * _scale.X), (int)(BackGroundSource.Height * _scale.Y));
            TitleOffSet = (int)(32 * _scale.Y);
            _returnToMainMenuAction = new Action(ReturnToMainMenu);
            Vector2 escMenuPos = RectangleHelper.CenterRectangleOnScreen(TotalBounds);
            escMenuPos = new Vector2(escMenuPos.X, escMenuPos.Y);

            TotalBounds = new Rectangle((int)escMenuPos.X, (int)escMenuPos.Y, TotalBounds.Width, TotalBounds.Height);
            _backGroundSprite = SpriteFactory.CreateUISprite(escMenuPos, BackGroundSource,
                UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Back), scale: _scale);

            _backGroundSprite.LoadContent();


            GenerateTabs();
            GeneratePages();
            _tabPairs = new Dictionary<MenuSection, Button>();
            _tabPairs.Add(_craftingMenu, _craftingTabButton);
            _tabPairs.Add(_escPrimary, _escPrimaryTabButton);
            CloseButton = UI.ButtonFactory.CreateCloseButton(this, TotalBounds, GetLayeringDepth(UILayeringDepths.Medium),
                new Action(() =>
                {
                    Deactivate();
                    Flags.Pause = false;
                }));
            CloseButton.LoadContent();

            AddSectionToGrid(CloseButton, 0, 2);

            SwapActivePage(_escPrimary);
            base.LoadContent();

        }
        public override void Deactivate()
        {
            base.Deactivate();
            if(UI.StorageDisplayHandler != null)
            UI.StorageDisplayHandler.AllowInteractions = true;

        }
        private void ReturnToMainMenu()
        {
            UI.ReturnToMainMenu();
            _returnToMainMenuButton.Deactivate();
            Deactivate();
        }
        public override void Update(GameTime gameTime)
        {
            if (Controls.WasKeyTapped(Keys.Escape) || Controls.WasGamePadButtonTapped(GamePadActionType.Escape))
            {
                Toggle();

                Flags.Pause = IsActive;

            }
            base.Update(gameTime);
            if (IsActive)
            {
                UI.StorageDisplayHandler.AllowInteractions = false;
                if (_activePage != null)
                    _activePage.Update(gameTime);
            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                _backGroundSprite.Draw(spriteBatch);
                if (_activePage != null)
                    _activePage.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);

        }

        internal override void ReceiveControl(Direction direction)
        {
            HasControl = true;
            CurrentSelectedPoint = _tabSelectedPoint;
            CurrentSelected = Selectables[CurrentSelectedPoint.X, CurrentSelectedPoint.Y];


        }



    }
}
