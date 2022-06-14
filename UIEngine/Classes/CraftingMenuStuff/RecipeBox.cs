using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine.Classes;

namespace UIEngine.Classes.CraftingMenuStuff
{
    internal class RecipeBox : MenuSection
    {
        private static readonly Rectangle s_backGroundSourceRectangle = new Rectangle(624, 496, 240, 112);

        private Sprite _backGroundSprite;


        //In that top bar where the name goes. This is the unscaled offset from top left
        private static Vector2 _nameTextOffset = new Vector2(40, 2);

        private Vector2 _nameTextPosition;
        private Text _recipeNameText;

        //In the top right large box. This is the unscaled offset from top left
        private static Vector2 _finishedIconOffset = new Vector2(160, 32);

        private Vector2 _finishedIconPosition;
        private Sprite _finishedRecipeIcon;

        //Bottom right stats section
       // private RecipeGuideBox _recipeGuideBox;
        private Vector2 _recipeGuideBoxPositionOffSet = new Vector2(0, 16);
        private Vector2 _recipeGuideBoxPosition = new Vector2(0, 16);

        //private RecipeStatsBox _recipeStatsBox;
        private Vector2 _recipeStatsBoxPositionOffSet = new Vector2(148, 79);
        private Vector2 _recipeStatsBoxPosition;

        private Vector2 _scale = new Vector2(2f, 2f);

        public RecipeBox(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }

        public override void LoadContent()
        {

            ClearGrid();
            ChildSections.Clear();
            TotalBounds = parentSection.TotalBounds;

            Position = RectangleHelper.PlaceRectangleAtBottomLeftOfParentRectangle(
                TotalBounds, s_backGroundSourceRectangle);

            _backGroundSprite = SpriteFactory.CreateUISprite(Position, s_backGroundSourceRectangle, 
                UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Low), scale: _scale);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsActive)
            {
                _backGroundSprite.Update(gameTime, Position);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (IsActive)
            {
                _backGroundSprite.Draw(spriteBatch);
            }
        }
    }
}
