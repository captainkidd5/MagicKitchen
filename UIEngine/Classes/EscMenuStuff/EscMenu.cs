using Globals.Classes;
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

namespace UIEngine.Classes.EscMenuStuff
{
    internal class EscMenu : MenuSection
    {

        private Rectangle _returnToMainMenuButtonBackgroundDimensions = new Rectangle(0, 0, 80, 96);
        private NineSliceButton _returnToMainMenuButton;



        private Sprite _backGroundSprite;

        private Rectangle _backGroundSourceRectangle = new Rectangle(624, 224, 240, 256);

        private Action _returnToMainMenuAction;
        private Vector2 _scale = new Vector2(2f, 2f);


        private StackPanel _tabsStackPanel;
        private int _tabWidth = 32;

        private NineSliceButton _returnTabButton;
        public EscMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            Deactivate();

            NormallyActivated = false;
        }

        private void GenerateTabs()
        {
            _tabsStackPanel = new StackPanel(this, graphics, content, new Vector2(TotalBounds.X, TotalBounds.Y - _tabWidth * _scale.Y), GetLayeringDepth(UILayeringDepths.Low));
            StackRow stackRow1 = new StackRow(TotalBounds.Width);
            _returnTabButton = new NineSliceButton(_tabsStackPanel, graphics, content, Position,
                GetLayeringDepth(UILayeringDepths.Medium), new Rectangle(0,0,(int)(32 * _scale.X),(int)(32 * _scale.Y)),hoverTransparency:true);
            stackRow1.AddItem(_returnTabButton, StackOrientation.Left);
            _tabsStackPanel.Add(stackRow1);


        }
        public override void LoadContent()
        {
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, (int)(_backGroundSourceRectangle.Width * _scale.X), (int)(_backGroundSourceRectangle.Height * _scale.Y));

            _returnToMainMenuAction = new Action(ReturnToMainMenu);
            Vector2 escMenuPos = RectangleHelper.CenterRectangleOnScreen(TotalBounds);
            TotalBounds = new Rectangle((int)escMenuPos.X, (int)escMenuPos.Y, TotalBounds.Width, TotalBounds.Height);
            _backGroundSprite = SpriteFactory.CreateUISprite(escMenuPos, _backGroundSourceRectangle,
                UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Back), scale: _scale);

            _backGroundSprite.LoadContent();


            GenerateTabs();



            _returnToMainMenuButton = UI.ButtonFactory.CreateNSliceTxtBtn(this,
                RectangleHelper.CenterRectangleInRectangle(_returnToMainMenuButtonBackgroundDimensions, _backGroundSprite.HitBox),GetLayeringDepth(UILayeringDepths.Low),
                new List<string>() {"Return to main menu"}, _returnToMainMenuAction);

            _returnToMainMenuButton.AddConfirmationWindow($"Return to main menu?");

            _returnToMainMenuButton.LoadContent();

            AddSectionToGrid(_returnToMainMenuButton,1,1);

            CloseButton = UI.ButtonFactory.CreateCloseButton(this, TotalBounds, GetLayeringDepth(UILayeringDepths.Medium),
                new Action(() =>
                {
                    Deactivate();
                    Flags.Pause = false;
                }));
            CloseButton.LoadContent();

            AddSectionToGrid(CloseButton, 1, 0);


            base.LoadContent();

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
          
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                _backGroundSprite.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
           
        }

       

        
    }
}
