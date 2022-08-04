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
        private NineSliceTextButton _newItemUnlockedBox;
        private static readonly string s_itemUnlockedString = "New Item Unlocked!";
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
            Text newItemUnlockedText = TextFactory.CreateUIText(s_itemUnlockedString, GetLayeringDepth(UILayeringDepths.Medium)); 
            base.LoadContent();
            _newItemUnlockedBox = new NineSliceTextButton(this, graphics, content, Position + BackgroundOffSet + new Vector2(0, -32),
               GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Low), new List<Text>() { newItemUnlockedText }, null);
            _newItemUnlockedBox.MovePosition(new Vector2(_newItemUnlockedBox.Position.X - _newItemUnlockedBox.Width / 2,32));

        }
        protected override void TimerExpired()
        {
            base.TimerExpired();
            _newItemUnlockedBox.FadeOut();
        }
    }
}
