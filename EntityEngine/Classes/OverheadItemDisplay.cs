using Globals.Classes;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityEngine.Classes
{
    internal class OverheadItemDisplay
    {

        private readonly float _displayTime = 2f;
        /// <summary>
        /// This should be the item currently selected by the entity in their inventory, and appears above their head
        /// </summary>
        protected Sprite? ActivelySelectedItemSprite { get; private set; }

        public int? ActivelySelectedItemId { get; private set; }

        private SimpleTimer? _simpleTimer;

        private bool _isDrawn;
        public void SelectItem(Item item, Vector2 position)
        {
            if (item == null)
            {
                ActivelySelectedItemId = null;
                ActivelySelectedItemSprite = null;
                return;
            }

            if (ActivelySelectedItemSprite == null || ActivelySelectedItemId == null || ActivelySelectedItemId.Value != item.Id)
            {
                ActivelySelectedItemId = item.Id;
                ActivelySelectedItemSprite = SpriteFactory.CreateWorldSprite(
                    position, Item.GetItemSourceRectangle(item.Id),
                    ItemFactory.ItemSpriteSheet, scale: new Vector2(.75f, .75f)); ;

                _simpleTimer = new SimpleTimer(_displayTime, false);
                _isDrawn = true;
            }

        }

        public void Update(GameTime gameTime, Vector2 entityPosition)
        {
            if (_isDrawn)
            {

                if (_simpleTimer.Run(gameTime))
                {
                    _isDrawn=false;
                    return;
                   
                }
                if (ActivelySelectedItemSprite != null)
                    ActivelySelectedItemSprite.Update(gameTime, entityPosition);

            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_isDrawn)
            {
                if (ActivelySelectedItemSprite != null)
                    ActivelySelectedItemSprite.Draw(spriteBatch);
            }
        }
    }
}
