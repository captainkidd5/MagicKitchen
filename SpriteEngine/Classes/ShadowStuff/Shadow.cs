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



        private static Rectangle[] s_itemRectangles = new Rectangle[]
        {
            new Rectangle(32, 48, 16, 16)
        };

        private static Rectangle[] s_npcRectangles = new Rectangle[]
        {
            new Rectangle(0, 384, 16, 16),
            new Rectangle(16, 384, 16, 16),
            new Rectangle(32, 384, 32, 16),

        };

        public Sprite Sprite { get; private set; }
        
        public Shadow(ShadowType shadowType, Vector2 position, ShadowSize shadowSize, Texture2D texture)
        {
            
            switch (shadowType)
            {
                case ShadowType.None:
                    throw new Exception("$Must provide shadowtype");
                case ShadowType.Item:
                    Sprite = SpriteFactory.CreateWorldSprite(position, s_itemRectangles[(int)shadowSize - 1], texture);
                    break;
                case ShadowType.NPC:
                    Sprite = SpriteFactory.CreateWorldSprite(position, s_npcRectangles[(int)shadowSize - 1], texture);
                    break;
            }
        }
        public void Update(float parentLayer, GameTime gameTime, Vector2 position, bool offSet = true)
        {
            Sprite.CustomLayer = parentLayer - SpriteUtility.GetMinimumOffSet();
            if (offSet)
            {
                Sprite.Update(gameTime, new Vector2(position.X - Sprite.SourceRectangle.Width / 2, position.Y - 4));

            }
            else
            {
                Sprite.Update(gameTime,position);

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }
    }
}
