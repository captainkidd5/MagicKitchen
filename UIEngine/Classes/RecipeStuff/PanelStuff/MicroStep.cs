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
using UIEngine.Classes.ButtonStuff;

namespace UIEngine.Classes.RecipeStuff.PanelStuff
{
    internal class MicroStep : RecipeSection
    {
        //If is first step we don't want to render the prior action sprite
        private readonly bool _isFirstStep;


        private Rectangle _smallBoxSourceRectangle = new Rectangle(336, 256, 16, 16);

        private Vector2 _baseIngredientSpritePosition;
        private static Vector2 _baaseIngredientSpritePositionOffSet = new Vector2(0, 0);
        private Sprite _baseIngredientSprite;

        private Button _baseIngredientButton;

        //small down arrow
        private static Rectangle s_downToArrowSourceRectangle = new Rectangle(336,240,16,16);
        private Vector2 _downToArrowPosition;
        private Vector2 _downToArrowPositionOffSet = new Vector2(0, -8);

        private Sprite _downToArrowSprite;


        private Vector2 _supplementaryIngredientPosition;
        private Vector2 _supplementaryIngredientSpritePositionOffSet = new Vector2(0, -16);
        private Sprite  _supplementaryIngredientSprite;

        private Vector2 _scale = new Vector2(2f, 2f);
        private readonly RecipeGuideBox _recipeGuideBox;

        //Right arrow (left of base ingredient)
        private Vector2 _priorActionPosition;
        private static Vector2 s_priorActionSpritePositionOffSet = new Vector2(-16, 0);
        private Sprite  _priorActionSprite;

        public MicroStep(RecipeGuideBox recipeGuideBox, bool isFirstStep, InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            _recipeGuideBox = recipeGuideBox;
            _isFirstStep = isFirstStep;
        }
        public override void LoadContent()
        {
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, 16 * (int)_scale.X, 48 * (int)_scale.Y);

            _baseIngredientSprite = SpriteFactory.CreateUISprite(_baseIngredientSpritePosition,
                Item.GetItemSourceRectangle(ItemFactory.GetItemData(RecipeInfo.BaseIngredient).Id),
                ItemFactory.ItemSpriteSheet, GetLayeringDepth(UILayeringDepths.High), scale: _scale);

            _supplementaryIngredientSprite = SpriteFactory.CreateUISprite(_supplementaryIngredientPosition,
               Item.GetItemSourceRectangle(ItemFactory.GetItemData(RecipeInfo.SupplementaryIngredient).Id),
               ItemFactory.ItemSpriteSheet, GetLayeringDepth(UILayeringDepths.High), scale: _scale);

            _downToArrowSprite = SpriteFactory.CreateUISprite(_downToArrowPosition, s_downToArrowSourceRectangle,
                UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Medium));

            _baseIngredientButton = new Button(this, graphics, content,
                _baseIngredientSpritePosition, GetLayeringDepth(UILayeringDepths.Medium),
                _smallBoxSourceRectangle, new Action(() => { MicroStepButtonClickAction(); }), _baseIngredientSprite, scale: _scale.X);

            _priorActionSprite = SpriteFactory.CreateUISprite(_priorActionPosition, GetPriorActionSourceRectangleFromCookAction(RecipeInfo.CookAction), UI.ButtonTexture,
                GetLayeringDepth(UILayeringDepths.Medium), scale: _scale);

            base.LoadContent();

        }

        /// <summary>
        /// Sets the text underneath the name of the recipe to show what the current selected step requires
        /// </summary>
        private void MicroStepButtonClickAction()
        {
            string instructions = $"{RecipeInfo.CookAction.ToString()} " +
                $"{RecipeInfo.SupplementaryIngredient} to {RecipeInfo.BaseIngredient}";

            if (RecipeInfo.SecondAction != CookAction.None)
            {
                instructions += $", \nthen {RecipeInfo.SecondAction.ToString()}";
            }
            _recipeGuideBox.SetStepInstructionsText(instructions);
        }
        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);

            _downToArrowPosition = Position + _downToArrowPositionOffSet * _scale;

            _baseIngredientSpritePosition = Position + _baaseIngredientSpritePositionOffSet * _scale;
            _baseIngredientButton.Position = _baseIngredientSpritePosition;

            _priorActionPosition = _baseIngredientSpritePosition + s_priorActionSpritePositionOffSet * _scale;

            _supplementaryIngredientPosition = Position + _supplementaryIngredientSpritePositionOffSet * _scale;


        }

        /// <summary>
        /// Gets source rectangle for prior cook action (Adding food, cooking food, chopping food, etc.
        /// This is located to the LEFT of the little base ingredient sprite
        /// </summary>
        private Rectangle GetPriorActionSourceRectangleFromCookAction(CookAction cookAction)
        {
            switch (cookAction)
            {
                case CookAction.None:
                    return Rectangle.Empty;

                    //Right facing arrow
                case CookAction.Add:
                    return new Rectangle(336, 224, 16, 16);
                case CookAction.Chop:
                    break;
                case CookAction.Bake:
                    return new Rectangle(352, 224, 16, 16);

                default:
                    throw new Exception($"Invalid cook action");
            }
            throw new Exception($"Invalid cook action");

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsActive)
            {
                _supplementaryIngredientSprite.Update(gameTime, _supplementaryIngredientPosition);
                _downToArrowSprite.Update(gameTime, _downToArrowPosition);

                if(!_isFirstStep)
                _priorActionSprite.Update(gameTime, _priorActionPosition);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (IsActive)
            {
                _supplementaryIngredientSprite.Draw(spriteBatch);
                _downToArrowSprite.Draw(spriteBatch);
                if (!_isFirstStep)

                    _priorActionSprite.Draw(spriteBatch);
            }
        }

    }
}
