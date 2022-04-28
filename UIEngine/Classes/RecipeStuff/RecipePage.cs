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
using UIEngine.Classes.Components;

namespace UIEngine.Classes.RecipeStuff
{
    internal class RecipePage : InterfaceSection
    {
        private StackPanel _stackPanel;

        private int _width;

        private List<RecipePanel> _panels;
        public RecipePage(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            _panels = new List<RecipePanel>();
            Position = new Vector2(position.Value.X + 64, position.Value.Y + 32);

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

        }

        public override void LoadContent()
        {
            _width = parentSection.Width;

            foreach(RecipePanel recipePanel in _panels)
            {
                recipePanel.LoadContent();
                StackRow stackRow = new StackRow(_width);
                stackRow.AddItem(recipePanel, StackOrientation.Left);
                _stackPanel.Add(stackRow);

            }

            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, parentSection.Width / 2, parentSection.Height);
            base.LoadContent();

        }

        public void LoadRecipes(List<RecipeInfo> recipeInfo)
        {
            _stackPanel = new StackPanel(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));

            foreach (RecipeInfo recipe in recipeInfo)
            {
                RecipePanel panel = new RecipePanel(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
                panel.LoadRecipe(recipe);
                _panels.Add(panel);

            }
        }

        
    }
}
