using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes.Addons;
using System;
using System.Collections.Generic;
using System.Text;
using static Globals.Classes.Settings;

namespace SpriteEngine.Classes
{

    public abstract class BaseSprite : Component
    {
        protected ElementType SpriteType { get; set; }
        protected Texture2D Texture { get; set; }
        protected Vector2 Position { get; set; }
        public Rectangle SourceRectangle { get; protected set; }

        public Rectangle? DestinationRectangle { get; set; }
        public Vector2 Origin { get; set; }

        protected float Scale { get; set; }
        public float Rotation { get; set; }

        public float? CustomLayer { get; set; }
        protected float LayerDepth { get; set; }

        public int Width { get { if (DestinationRectangle == null) return SourceRectangle.Width; else return DestinationRectangle.Value.Width; } }
        public int Height { get { if (DestinationRectangle == null) return SourceRectangle.Height; else return DestinationRectangle.Value.Height; } }
        public virtual Rectangle HitBox
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, (int)(Width * Scale), (int)(Height * Scale)); }
        }

        /// <see cref="UpdateColor(Color)"/>
        public virtual Color PrimaryColor { get; protected set; }

        protected SpriteEffects SpriteEffectsAnchor { get; set; } //this is set when sprite is created and never changed

        public SpriteEffects SpriteEffects { get; set; }


        public bool IsOpaque => PrimaryColor.A == 255;
        public bool IsTransparent => PrimaryColor.A == 51;

        ColorShifter ColorShifter { get; set; }

        public BaseSprite(GraphicsDevice graphics, ContentManager content, ElementType spriteType, Vector2 position, Rectangle sourceRectangle, Texture2D texture, Color primaryColor,
             Vector2 origin, float scale, float rotation, Layers layer,
            bool randomizeLayers, bool flip, float? customLayer) : base(graphics, content)
        {
            Position = position;
            Texture = texture;
            PrimaryColor = primaryColor ;
            Origin = origin ;
            Scale = scale;
            SharedConstructor(spriteType, sourceRectangle, rotation, layer, randomizeLayers, flip, customLayer);

        }

        public BaseSprite(GraphicsDevice graphics, ContentManager content, ElementType spriteType, Rectangle destinationRectangle, Rectangle sourceRectangle, Texture2D texture, Color primaryColor,
             Vector2 origin, float scale, float rotation, Layers layer,
            bool randomizeLayers, bool flip, float? customLayer) : base(graphics, content)
        {
            Texture = texture;
            DestinationRectangle = destinationRectangle;
            PrimaryColor = primaryColor;
            Origin = origin;
            Scale = scale;

            SharedConstructor(spriteType, sourceRectangle, rotation, layer, randomizeLayers, flip, customLayer);

        }
        private void SharedConstructor(ElementType spriteType, Rectangle sourceRectangle, float rotation, Layers layer, bool randomizeLayers, bool flip, float? customLayer)
        {
            SpriteType = spriteType;
            SourceRectangle = sourceRectangle;


            if (randomizeLayers)
                this.LayerDepth = SpriteUtility.GetSpriteVariedLayerDepth(layer);
            else
                this.LayerDepth = Settings.GetLayerDepth(layer);

            if (customLayer != null)
                CustomLayer = customLayer;
            if (flip)
                SpriteEffectsAnchor = SpriteEffects.FlipHorizontally;
            else
                SpriteEffectsAnchor = SpriteEffects.None;

            Rotation = rotation;
        }

        public BaseSprite(GraphicsDevice graphics, ContentManager content, Vector2 position, Texture2D texture, Color primaryColor,
            Vector2 origin, float scale, Layers layer) : base(graphics, content)
        {
            Position = position;
            Texture = texture;
            PrimaryColor = primaryColor;
            Origin = origin;
            Scale = scale;
        }
        public virtual void Update(GameTime gameTime, Vector2 position)
        {
            if (ColorShifter != null)
            {
                ColorShifter.Update(gameTime, this);
                if (ColorShifter.FlaggedForRemovalUponFinish && ColorShifter.IsNormal)
                    ColorShifter = null;
            }
            Position = position;

            if(DestinationRectangle != null)
                DestinationRectangle = new Rectangle((int)Position.X + (int)Origin.X, (int)Position.Y + (int)Origin.Y, Width, Height);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }


        public void TriggerIntensityEffect()
        {
            ColorShifter.TriggerIntensifyEffect();
        }
        public void TriggerReduceEffect()
        {
            ColorShifter.TriggerReduceEffect();
        }

        public void AddFaderEffect(float? maxOpac, float? speed, bool immediatelyTriggerFade = false)
        {
            CheckCE();
            ColorShifter = new ColorShifter(PrimaryColor, speed, maxOpac);
            if(immediatelyTriggerFade)
                TriggerIntensityEffect(); 
        }

        public void AddSaturateEffect(Color? targetColor, bool immediatelyTriggerSaturation = false)
        {
            CheckCE();
            ColorShifter = new ColorShifter(PrimaryColor,null, targetColor);
            if (immediatelyTriggerSaturation)
                TriggerIntensityEffect();
        }
        private void CheckCE()
        {
            if (ColorShifter != null)
                throw new Exception($"Color Effect is already instantiated");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="flagForRemoval">Will remove fader after fader has returned opacity to full</param>
        public void RemoveColorEffect(bool flagForRemoval = false)
        {
            if (ColorShifter == null)
                throw (new Exception($"Fader does not exists"));
            if (flagForRemoval)
            {
                ColorShifter.FlaggedForRemovalUponFinish = true;
                ColorShifter.TriggerReduceEffect();
            }
            else
                ColorShifter = null;
        }
        public void SetEffectToDefault()
        {
            SpriteEffects = SpriteEffectsAnchor;
        }

        public void UpdateColor(Color colorToUse)
        {
            PrimaryColor = colorToUse;
        }

        public void UpdateColor(byte? r, byte? g, byte? b, byte? a)
        {
            byte r0 = r ?? PrimaryColor.R;
            byte g0 = g ?? PrimaryColor.G;
            byte b0 = b ?? PrimaryColor.B;
            byte a0 = a ?? PrimaryColor.A;
            PrimaryColor = new Color(r0,g0,b0,a0);
        }

        /// <summary>
        /// Returns a layer depth which is relative to how far up or down the sprite is on the map.
        /// Useful for allowing the sprite to move "in front" or "behind of" other objects.
        /// </summary>
        protected float GetYAxisLayerDepth()
        {
            return SpriteUtility.GetYAxisLayerDepth(Position, SourceRectangle);
        }
        public void SwapSourceRectangle(Rectangle newRectangle)
        {
            SourceRectangle = newRectangle;
        }

        public void SwapTexture(Texture2D texture)
        {
            Texture = texture;
        }

        public void SwapScale(float scale)
        {
            Scale = scale;
        }

    }
}
