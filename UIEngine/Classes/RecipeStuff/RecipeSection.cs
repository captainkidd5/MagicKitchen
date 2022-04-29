using DataModels.ItemStuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEngine.Classes.RecipeStuff
{
    internal class RecipeSection : InterfaceSection
    {
        protected RecipeInfo RecipeInfo;
        protected Vector2 Scale = new Vector2(2f, 2f);
        public RecipeSection(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }

        public void LoadRecipe(RecipeInfo recipeInfo)
        {
            RecipeInfo = recipeInfo;
        }
    }
}
