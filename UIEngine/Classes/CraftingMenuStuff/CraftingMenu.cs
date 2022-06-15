using DataModels.ItemStuff;
using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Classes.EscMenuStuff;
using static DataModels.Enums;

namespace UIEngine.Classes.CraftingMenuStuff
{
    internal class CraftingMenu : MenuSection
    {

        public static Rectangle _normalBackGroundSourceRectangle = new Rectangle(640, 144, 32, 32);
        //red one
        public static Rectangle _noCraftBackGroundSourceRectangle = new Rectangle(608, 144, 32, 32);
        //green one
        public static Rectangle _yesCraftBackGroundSourceRectangle = new Rectangle(640, 112, 32, 32);
        private CraftingPage _craftingPage;
        public CraftingMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) : 
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            Activate();
            UI.StorageDisplayHandler.PlayerInventoryDisplay.ContentsChanged += PlayerInventoryChanged;
        }
        public bool ActivelyCrafting { get; set; }

        private void PlayerInventoryChanged()
        {
            //Do not want to reload while crafting
            if(IsActive && !ActivelyCrafting)
                LoadContent();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void LoadContent()
        {
            //   base.LoadContent();
            ClearGrid();
            ChildSections.Clear();
            TotalBounds = parentSection.TotalBounds;
            int offSet = (parentSection as EscMenu).TitleOffSet;
            Position = new Vector2(TotalBounds.X, TotalBounds.Y + offSet);


            _craftingPage = new CraftingPage(this, CraftingCategory.Tool, this, graphics, content, Position,
                GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Low));
            _craftingPage.LoadContent();

            _craftingPage.AssignControlSectionAtEdge(Direction.Up, parentSection as MenuSection);



        }

        private void Refresh()
        {
            _craftingPage.LoadContent();
        }

        protected override void GiveSectionControl(Direction direction)
        {
            _craftingPage.HasControl = false;
            base.GiveSectionControl(direction);
        }

        internal override void ReceiveControl(Direction direction)
        {
            _craftingPage.ReceiveControl(direction);
            base.ReceiveControl(direction);
        }
        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
        }

        public override void Update(GameTime gameTime)
        {
            if (ActivelyCrafting)
            {
                Refresh();
                ActivelyCrafting = false;
            }
            base.Update(gameTime);
        }
    }
}
