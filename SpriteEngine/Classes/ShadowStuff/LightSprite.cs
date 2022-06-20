using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEngine.Classes.ShadowStuff
{

    
    public enum LightType
    {
        None =0,
        Warm = 1,
        Nautical = 2
    }
    public class LightSprite 
    {
        public static Color ColorFromLightType(LightType lightType)
        {
            switch (lightType)
            {
                case LightType.None:
                    break;
                case LightType.Warm:
                    return WarmColor;
                case LightType.Nautical:
                    return NauticalColor;
            }
            throw new Exception($"Must have valid light type");
        }
        public static readonly Color WarmColor = Color.Yellow;
        public static readonly Color NauticalColor = Color.LightBlue;
        public Sprite Sprite { get; set; }
        private Vector2 _offSet;
        public LightSprite(Sprite sprite, Vector2 offSet)
        {
            Sprite = sprite;
            _offSet = offSet;
        }
        public void Update(GameTime gameTime, Vector2 position)
        {
            Sprite.Update(gameTime, position + _offSet);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }
    }
}
