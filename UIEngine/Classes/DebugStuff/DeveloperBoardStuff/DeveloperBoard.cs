using Globals.Classes;
using Globals.Classes.Helpers;
using InputEngine.Classes;
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
using UIEngine.Classes.Components;

namespace UIEngine.Classes.DebugStuff.DeveloperBoardStuff
{
    internal class DeveloperBoard : MenuSection
    {
        private Rectangle _backGroundSpriteDimensions = new Rectangle(0, 0, 980, 448);

        private NineSliceSprite _backgroundSprite;

        private List<MenuSection> _allPages;
        private MenuSection _activeSection;

        private StackPanel _tabsStackPanel;
        public DeveloperBoard(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            NormallyActivated = false;
            _allPages = new List<MenuSection>();
        }



        public override void MovePosition(Vector2 newPos)
        {
            //base.MovePosition(newPos);
        }
        public override void LoadContent()
        {
            Position = RectangleHelper.CenterRectangleInRectangle(_backGroundSpriteDimensions,
             Settings.ScreenRectangle);
            _allPages.Clear();
            ChildSections.Clear();

            MainDevPage page1 = new MainDevPage(null, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
            page1.LoadContent();
            _allPages.Add(page1);

            PhysDevPage physDevPage = new PhysDevPage(null, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
            physDevPage.LoadContent();
            _allPages.Add(physDevPage);


            _activeSection = _allPages[0];
            _backgroundSprite = SpriteFactory.CreateNineSliceSprite(Position, _backGroundSpriteDimensions.Width, _backGroundSpriteDimensions.Height, UI.ButtonTexture,
                GetLayeringDepth(UILayeringDepths.Low));

            _tabsStackPanel = new StackPanel(this, graphics, content, new Vector2(Position.X, Position.Y - 80), GetLayeringDepth(UILayeringDepths.Medium));
            StackRow stackRow = new StackRow(_backgroundSprite.Width);

            NineSliceButton mainTabButton = new NineSliceButton(_tabsStackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low), null,
                new Action(() => { _activeSection = page1; }));
            mainTabButton.LoadContent();
            stackRow.AddItem(mainTabButton, StackOrientation.Left);


            NineSliceButton physTabButton = new NineSliceButton(_tabsStackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low), null,
             new Action(() => { _activeSection = physDevPage; }));
            physTabButton.LoadContent();
            stackRow.AddItem(physTabButton, StackOrientation.Left);

            _tabsStackPanel.Add(stackRow);
            Deactivate();

            // NormallyActivated = false;
        }

        public override void Update(GameTime gameTime)
        {
#if DEBUG
            if (!Flags.DisablePlayerUIInteractions)
            {

                if (Controls.WasKeyTapped(Microsoft.Xna.Framework.Input.Keys.L))
                    Toggle();
            }
            if (IsActive)
            {
                _activeSection.Update(gameTime);

            }
            base.Update(gameTime);
#endif 

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                _backgroundSprite.Draw(spriteBatch);

                _activeSection.Draw(spriteBatch);
            }
               
            base.Draw(spriteBatch);

        }
    }
}
