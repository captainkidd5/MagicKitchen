using DataModels;
using DataModels.ItemStuff;
using Globals.Classes;
using Globals.Classes.Helpers;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEngine.Classes.RecipeStuff
{
    public class RecipeBook : InterfaceSection
    {
        private Rectangle _backGroundSourceRectangle = new Rectangle(48, 272, 560, 288);

        private Sprite _backGroundSprite;

        private int _recipesPerPage = 2;
        private RecipePage _recipePageLeft;
        private RecipePage _recipePageRight;

        private List<RecipeInfo> _availableRecipes;
        private Vector2 _scale = new Vector2(2f, 2f);
        public RecipeBook(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
           
            _availableRecipes = new List<RecipeInfo>();
        }

        public override void LoadContent()
        {
            NormallyActivated = false;
           // _scale = Vector2Helper.GetScaleFromRequiredDimensions(_backGroundSourceRectangle, Settings.GetScreenRectangle());
            Position = RectangleHelper.CenterRectangleOnScreen(new Rectangle(_backGroundSourceRectangle.X, _backGroundSourceRectangle.Y,
                _backGroundSourceRectangle.Width * (int)_scale.X, _backGroundSourceRectangle.Height * (int)_scale.Y));

            Position = new Vector2(Position.X, Position.Y - 32);
            _backGroundSprite = SpriteFactory.CreateUISprite(Position, _backGroundSourceRectangle,
                UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Low), scale: _scale);

            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y,
                _backGroundSourceRectangle.Width * (int)_scale.X, _backGroundSourceRectangle.Height * (int)_scale.Y);

            _recipePageLeft = new RecipePage(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
            _recipePageRight = new RecipePage(this, graphics, content, new Vector2(Position.X + TotalBounds.Width / 2, Position.Y), GetLayeringDepth(UILayeringDepths.Low));
            FillRecipePages();
            _recipePageLeft.LoadContent();
            _recipePageRight.LoadContent();
            
            base.LoadContent();

        }

        public void LoadAvailableRecipes(List<int> playerUnlockedRecipes)
        {
            _availableRecipes = new List<RecipeInfo>();

            for (int i =0; i < playerUnlockedRecipes.Count; i++)
            {
                RecipeInfo info = ItemFactory.GetItemData(playerUnlockedRecipes[i]).RecipeInfo;
                _availableRecipes.Add(info);


            }

 
        }

        private void FillRecipePages()
        {
            List<RecipeInfo> recipesLeftPage = new List<RecipeInfo>();
            List<RecipeInfo> recpesRightPage = new List<RecipeInfo>();

            for (int i = 0; i < _availableRecipes.Count; i++)
            {

                if (i < _recipesPerPage)
                {
                    recipesLeftPage.Add(_availableRecipes[i]);
                }
                else
                {
                    recpesRightPage.Add(_availableRecipes[i]);
                }


            }

            _recipePageLeft.LoadRecipes(recipesLeftPage);
            _recipePageRight.LoadRecipes(recpesRightPage);
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

            _backGroundSprite?.Draw(spriteBatch);
            }

        }
    }
}
