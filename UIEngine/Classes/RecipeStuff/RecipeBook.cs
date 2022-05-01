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
using UIEngine.Classes.ButtonStuff;

namespace UIEngine.Classes.RecipeStuff
{
    public class RecipeBook : InterfaceSection
    {
        private Rectangle _backGroundSourceRectangle = new Rectangle(48, 272, 560, 288);

        private Sprite _backGroundSprite;

        private int _recipesPerPage = 2;
        private int _currentPage = 0;
        private int _totalPages;
        private RecipePage _recipePageLeft;
        private RecipePage _recipePageRight;

        private List<RecipeInfo> _availableRecipes;
        private Vector2 _scale = new Vector2(2f, 2f);


        private Vector2 _leftArrowPosition;
        private Rectangle _leftArrowSourceRectangle = new Rectangle(112, 0, 32, 16);
        private Button _leftArrowButton;

        private Vector2 _rightArrowPosition;
        private Rectangle _rightArrowSourceRectangle = new Rectangle(144, 0, 32, 16);
        private Button _rightArrowButton;

        private Button _exitMenuButton;
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


            _totalPages = (int)Math.Ceiling((float)_availableRecipes.Count / (float)2);

            _leftArrowPosition = RectangleHelper.CenterRectangleInRectangle(_leftArrowSourceRectangle, TotalBounds);
            _leftArrowPosition = new Vector2(_leftArrowPosition.X - _leftArrowSourceRectangle.Width * 2f, _leftArrowPosition.Y);
            _leftArrowButton = new Button(this, graphics, content, _leftArrowPosition, GetLayeringDepth(UILayeringDepths.Medium), _leftArrowSourceRectangle,
                new Action(() => { FlipPageLeft(); }));
            ChildSections.Remove(_leftArrowButton);

            _rightArrowPosition = RectangleHelper.CenterRectangleInRectangle(_leftArrowSourceRectangle, TotalBounds);
            _rightArrowPosition = new Vector2(_rightArrowPosition.X + _rightArrowSourceRectangle.Width, _rightArrowPosition.Y);

            _rightArrowButton = new Button(this, graphics, content, _rightArrowPosition, GetLayeringDepth(UILayeringDepths.Medium), _rightArrowSourceRectangle,
                new Action(() => { FlipPageRight(); }));
            ChildSections.Remove(_rightArrowButton);

            _exitMenuButton = new Button(this, graphics, content, RectangleHelper.PlaceRectangleAtTopRightOfParentRectangle(TotalBounds, ButtonFactory.s_redExRectangle),
                GetLayeringDepth(UILayeringDepths.Medium), ButtonFactory.s_redExRectangle, new Action(() => { Deactivate(); }));
            Deactivate();
            base.LoadContent();

        }

        private void FlipPageLeft()
        {
            _currentPage = ScrollHelper.GetIndexFromScroll(Enums.Direction.Up, _currentPage, _totalPages);
            _currentPage = ScrollHelper.GetIndexFromScroll(Enums.Direction.Up, _currentPage, _totalPages);

            FillRecipePages();
        }
        private void FlipPageRight()
        {
            _currentPage = ScrollHelper.GetIndexFromScroll(Enums.Direction.Down, _currentPage, _totalPages);
            _currentPage = ScrollHelper.GetIndexFromScroll(Enums.Direction.Down, _currentPage, _totalPages);

            FillRecipePages();

        }
        public void LoadAvailableRecipes(List<int> playerUnlockedRecipes)
        {
            _availableRecipes = new List<RecipeInfo>();

            for (int i = 0; i < playerUnlockedRecipes.Count; i++)
            {
                RecipeInfo info = ItemFactory.GetItemData(playerUnlockedRecipes[i]).RecipeInfo;
                if(!info.Name.Contains("raw"))
                _availableRecipes.Add(info);


            }


        }

        private void FillRecipePages()
        {
            List<RecipeInfo> recipesLeftPage = new List<RecipeInfo>();
            List<RecipeInfo> recpesRightPage = new List<RecipeInfo>();

            int leftPageIndex = _currentPage * _recipesPerPage;
            for (int i = leftPageIndex; i < leftPageIndex + 2; i++)
            {
                if(i < _availableRecipes.Count)
                recipesLeftPage.Add(_availableRecipes[i]);

            }
            int rightPageIndex = leftPageIndex + 2;
            for (int i = rightPageIndex; i < rightPageIndex + 2; i++)
            {
                if (i < _availableRecipes.Count)
                    recpesRightPage.Add(_availableRecipes[i]);


            }

            _recipePageLeft.LoadRecipes(recipesLeftPage);
            _recipePageRight.LoadRecipes(recpesRightPage);
            _recipePageLeft.LoadContent();
            _recipePageRight.LoadContent();

        }

       
        

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (IsActive)
        {
            _backGroundSprite.Update(gameTime, Position);
            if (_currentPage > 0)
                _leftArrowButton.Update(gameTime);
            if (_currentPage < _totalPages - 1)
                _rightArrowButton.Update(gameTime);

        }
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        if (IsActive)
        {

            _backGroundSprite?.Draw(spriteBatch);

            if (_currentPage > 0)
                _leftArrowButton.Draw(spriteBatch);
            if (_currentPage < _totalPages - 1)
                _rightArrowButton.Draw(spriteBatch);
        }

    }
}
}
