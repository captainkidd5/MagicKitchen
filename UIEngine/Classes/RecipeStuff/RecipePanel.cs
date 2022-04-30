using DataModels;
using DataModels.ItemStuff;
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
using TextEngine;
using TextEngine.Classes;
using UIEngine.Classes.RecipeStuff.PanelStuff;

namespace UIEngine.Classes.RecipeStuff
{
    internal class RecipePanel : RecipeSection
    {
        private static readonly Rectangle s_backGroundSourceRectangle = new Rectangle(368, 144, 208, 128);

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
        private RecipeGuideBox _recipeGuideBox;
        private Vector2 _recipeGuideBoxPositionOffSet = new Vector2(0, 16);
private Vector2 _recipeGuideBoxPosition = new Vector2(0, 16);

        private RecipeStatsBox _recipeStatsBox;
        private Vector2 _recipeStatsBoxPositionOffSet = new Vector2(148, 79);
        private Vector2 _recipeStatsBoxPosition;
        public RecipePanel(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }

  
        public override void LoadContent()
        {
            _backGroundSprite = SpriteFactory.CreateUISprite(Position,
                s_backGroundSourceRectangle, UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Low), scale: Scale);
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, _backGroundSprite.Width * (int)Scale.X, _backGroundSprite.Height * (int)Scale.Y);

            _recipeNameText = TextFactory.CreateUIText(RecipeInfo.Name, GetLayeringDepth(UILayeringDepths.Medium));
            _finishedRecipeIcon = SpriteFactory.CreateUISprite(_finishedIconPosition, Item.GetItemSourceRectangle(ItemFactory.GetItemData(RecipeInfo.Name).Id),
                ItemFactory.ItemSpriteSheet, GetLayeringDepth(UILayeringDepths.Medium), scale: Scale * 2);
            _recipeGuideBox = new RecipeGuideBox(this, graphics, content, _recipeGuideBoxPosition, GetLayeringDepth(UILayeringDepths.Medium));
            _recipeGuideBox.LoadRecipe(RecipeInfo);
            _recipeGuideBox.LoadContent();

            _recipeStatsBox = new RecipeStatsBox(this, graphics, content, _recipeStatsBoxPosition,
                GetLayeringDepth(UILayeringDepths.Medium));
            _recipeStatsBox.LoadRecipe(RecipeInfo);
            _recipeStatsBox.LoadContent();

            MovePosition(Position);
           // _selectedStepText = RecipeInfo.
            base.LoadContent();
        }

        private void GetNextStepString(RecipeInfo recipeInfo)
        {

        }

        public override void MovePosition(Vector2 newPos)
        {

            base.MovePosition(newPos);

            _recipeGuideBoxPosition = Position + _recipeGuideBoxPositionOffSet * Scale;
            _recipeGuideBox.MovePosition(_recipeGuideBoxPosition);

            _recipeStatsBoxPosition = Position + _recipeStatsBoxPositionOffSet * Scale;
            _recipeStatsBox.MovePosition(_recipeStatsBoxPosition);

            _nameTextPosition = new Vector2(Position.X + _nameTextOffset.X * Scale.X, Position.Y + _nameTextOffset.Y * Scale.Y);
            _recipeNameText.ForceSetPosition(_nameTextPosition);

            _finishedIconPosition = new Vector2(Position.X + _finishedIconOffset.X * Scale.X, Position.Y + _finishedIconOffset.Y * Scale.Y);
            _finishedRecipeIcon.ForceSetPosition(_finishedIconPosition);


        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsActive)
            {
                _backGroundSprite.Update(gameTime, Position);
                _recipeNameText.Update(gameTime, _nameTextPosition);
                _finishedRecipeIcon.Update(gameTime, _finishedIconPosition);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (IsActive)
            {
                _backGroundSprite.Draw(spriteBatch);
                _recipeNameText.Draw(spriteBatch, true);
                _finishedRecipeIcon.Draw(spriteBatch);
            }
        }

    }  
}
