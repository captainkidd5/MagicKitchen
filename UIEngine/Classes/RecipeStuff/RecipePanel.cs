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
        private static readonly Vector2 s_scale = new Vector2(2f, 2f);

        private Sprite _backGroundSprite; 
        

        //In that top bar where the name goes. This is the unscaled offset from top left
        private Vector2 _nameTextOffset = new Vector2(40, 2);

        private Vector2 _nameTextPosition;
        private Text _recipeNameText;

        //In the top right large box. This is the unscaled offset from top left
        private Vector2 _finishedIconOffset = new Vector2(160, 32);

        private Vector2 _finishedIconPosition;
        private Sprite _finishedRecipeIcon;


        private RecipeGuideBox _recipeGuideBox;
        private Vector2 _recipeGuideBoxPositionOffSet = new Vector2(0, 16);
        public RecipePanel(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }

  
        public override void LoadContent()
        {
            _backGroundSprite = SpriteFactory.CreateUISprite(Position,
                s_backGroundSourceRectangle, UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Low), scale: s_scale);
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, _backGroundSprite.Width * (int)s_scale.X, _backGroundSprite.Height * (int)s_scale.Y);

            _recipeNameText = TextFactory.CreateUIText(RecipeInfo.Name, GetLayeringDepth(UILayeringDepths.Medium));
            _finishedRecipeIcon = SpriteFactory.CreateUISprite(_finishedIconPosition, Item.GetItemSourceRectangle(ItemFactory.GetItemData(RecipeInfo.Name).Id),
                ItemFactory.ItemSpriteSheet, GetLayeringDepth(UILayeringDepths.Medium), scale: s_scale * 2);

            Vector2 recipeGuideBoxPosition = Position + _recipeGuideBoxPositionOffSet * s_scale;
            _recipeGuideBox = new RecipeGuideBox(this, graphics, content, recipeGuideBoxPosition, GetLayeringDepth(UILayeringDepths.Medium));
            _recipeGuideBox.LoadRecipe(RecipeInfo);
            _recipeGuideBox.LoadContent();
           // _selectedStepText = RecipeInfo.
            base.LoadContent();
        }

        private void GetNextStepString(RecipeInfo recipeInfo)
        {

        }

        public override void MovePosition(Vector2 newPos)
        {

            base.MovePosition(newPos);
            _nameTextPosition = new Vector2(Position.X + _nameTextOffset.X * s_scale.X, Position.Y + _nameTextOffset.Y * s_scale.Y);
            _finishedIconPosition = new Vector2(Position.X + _finishedIconOffset.X * s_scale.X, Position.Y + _finishedIconOffset.Y * s_scale.Y);



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
