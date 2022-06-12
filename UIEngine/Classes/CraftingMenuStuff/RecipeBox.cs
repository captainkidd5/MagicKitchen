using Microsoft.Xna.Framework;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine.Classes;

namespace UIEngine.Classes.CraftingMenuStuff
{
    internal class RecipeBox
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
    }
}
