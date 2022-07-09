using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataModels.Enums;

namespace SpriteEngine.Classes.ShadowStuff
{
   
    public enum ShadowType
    {
        None =0,
        Item = 1,
        NPC = 2
    }
    public class Shadow
    {
        private static Rectangle s_itemSmallRectangle = new Rectangle(32, 48, 16, 16);

        private static Rectangle s_npcSmallRectangle = new Rectangle(0, 384, 16, 16);
        private static Rectangle s_npcMediumRectangle = new Rectangle(16, 384, 16, 16);



        public Sprite Sprite { get; set; }
        
        public Shadow(ShadowType shadowType, Vector2 position, ShadowSize shadowSize, Texture2D texture)
        {
            Rectangle rectangleToUse = s_itemSmallRectangle;
            switch (shadowType)
            {
                case ShadowType.None:
                    throw new Exception("$Must provide shadowtype");
                case ShadowType.Item:
                    rectangleToUse = s_itemSmallRectangle;
                    break;
                case ShadowType.NPC:
                    rectangleToUse = s_npcSmallRectangle;

                    break;
            }
            Sprite = SpriteFactory.CreateWorldSprite(position, rectangleToUse, texture);
        }
        public void Update(GameTime gameTime, Vector2 position)
        {
            Sprite.Update(gameTime, position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }
    }
}
