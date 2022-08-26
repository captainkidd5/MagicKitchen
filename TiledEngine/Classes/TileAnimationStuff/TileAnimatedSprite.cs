using DataModels;
using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SoundEngine.Classes;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiledEngine.Classes.TileAnimationStuff
{
    internal class TileAnimatedSprite : AnimatedSprite
    {
        private readonly TileObject _tileObject;

        public TileAnimatedSprite(TileObject tileObject, GraphicsDevice graphics, ContentManager content, Settings.ElementType spriteType, Vector2 position, Rectangle sourceRectangle, Texture2D texture, AnimationFrame[] animationFrames, float standardDuration, Color primaryColor, Vector2 origin, Vector2 scale, float rotation, Enums.Layers layer, bool randomizeLayers, bool flip, float? customLayer, int idleFrame = 0) : base(graphics, content, spriteType, position, sourceRectangle, texture, animationFrames, standardDuration, primaryColor, origin, scale, rotation, layer, randomizeLayers, flip, customLayer, idleFrame)
        {
            _tileObject = tileObject;
        }

        public override void Update(GameTime gameTime, Vector2 position, bool updatePeripheralActoins = true, float speedModifier = 1)
        {
            base.Update(gameTime, position, updatePeripheralActoins, speedModifier);
            if(CurrentFrame != FrameLastFrame)
            {
                string prop = _tileObject.GetProperty("animationSounds");
                if (!string.IsNullOrEmpty(prop))
                {
                    string soundPackageName = ParseString(prop);
                    if (!string.IsNullOrEmpty(soundPackageName))
                    {
                        SoundFactory.PlayEffectPackage(soundPackageName);

                    }
                }
            }
        }

        private string ParseString(string propString)
        {
            string[] val = propString.Split(',');
            //if (val.Length < 2)
            //    throw new Exception($"Invalid format for prop string, must be in format (0:soundName, 1:soundName, etc)");

            foreach (string str in val)
            {
                string[] keyPair = str.Split(':');
                if (int.Parse(keyPair[0]) == CurrentFrame)
                {
                    string soundName = keyPair[1];
                    return soundName;
                }
            }
            return String.Empty;
        }
    }
}
