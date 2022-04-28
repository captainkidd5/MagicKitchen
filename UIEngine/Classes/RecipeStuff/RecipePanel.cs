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
        private static readonly Rectangle s_backGroundSourceRectangle = new Rectangle(368, 144, 208, 128);
        private static readonly Vector2 s_scale = new Vector2(2f, 2f);

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
                s_backGroundSourceRectangle, UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Low), scale: s_scale);
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, _backGroundSprite.Width * (int)s_scale.X, _backGroundSprite.Height * (int)s_scale.Y);
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
