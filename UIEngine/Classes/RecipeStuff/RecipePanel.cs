using DataModels;
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
    internal class RecipePanel : InterfaceSection
    {
        private static readonly Rectangle _backGroundSourceRectangle = new Rectangle(420, 0, 224, 160);
        private Sprite _backGroundSprite; 
        private RecipeInfo _recipeInfo;
        public RecipePanel(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }

        public void LoadRecipe(RecipeInfo recipeInfo)
        {
            _recipeInfo = recipeInfo;
        }
        public override void LoadContent()
        {
            _backGroundSprite = SpriteFactory.CreateUISprite(Position,
                _backGroundSourceRectangle, UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Low));
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
