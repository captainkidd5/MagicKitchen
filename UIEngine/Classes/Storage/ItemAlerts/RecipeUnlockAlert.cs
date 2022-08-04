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
using UIEngine.Classes.ButtonStuff;

namespace UIEngine.Classes.Storage.ItemAlerts
{
    internal class RecipeUnlockAlert : ItemAlert
    {

        public RecipeUnlockAlert(ItemData itemData, InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, 
            Vector2? position, float layerDepth) : base(itemData, interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            TTl = 4f;
        }

        public override void Increment(int amt)
        {
            throw new Exception($"Do not increment recipe unlock");
        }
        protected override Text SetInitialText()
        {
            return TextFactory.CreateUIText($"{ItemData.Name}", GetLayeringDepth(UILayeringDepths.Medium));
        }
        public override void LoadContent()
        {
            base.LoadContent();


        }
        protected override void TimerExpired()
        {
            base.TimerExpired();
            //_newItemUnlockedBox.FadeOut();
        }
    }
}
