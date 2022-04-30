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
    public enum UILayeringDepths
    {
        Back = 0,
        Low = 1,
        Medium = 2,
        High = 3,
        Front = 4

    }
    public abstract class BaseSprite : Component
    {
        protected ElementType SpriteType { get; set; }
        protected Texture2D Texture { get; set; }
        protected Vector2 Position { get; set; }
        public Rectangle SourceRectangle { get; protected set; }

        public Vector2 Origin { get; set; }

        public Vector2 Scale { get; protected set; }
        public float Rotation { get; set; }

        public float? CustomLayer { get; set; }
        protected float LayerDepth { get; set; }

        public virtual int Width { get {  return SourceRectangle.Width;} }
        public virtual int Height { get { return SourceRectangle.Height;} }
        public virtual Rectangle HitBox
        {
            get { return new Rectangle((int)(Position.X + OffSet.X), (int)(Position.Y + OffSet.Y), (int)(Width * Scale.X), (int)(Height * Scale.Y)); }
        }

        /// <see cref="UpdateColor(Color)"/>
        public virtual Color PrimaryColor { get; protected set; }

        protected SpriteEffects SpriteEffectsAnchor { get; set; } //this is set when sprite is created and never changed

        public SpriteEffects SpriteEffects { get; set; }


        public bool IsOpaque => PrimaryColor.A == 255;
        public bool IsTransparent => PrimaryColor.A == 51;

        ColorShifter ColorShifter { get; set; }

        public virtual void ResetColors() => ColorShifter.Reset(this);

        public Vector2 OffSet { get; set; } = Vector2.Zero;

        internal BaseSprite(GraphicsDevice graphics, ContentManager content, ElementType spriteType, Vector2 position, Rectangle sourceRectangle, Texture2D texture, Color primaryColor,
             Vector2 origin, Vector2 scale, float rotation,
            bool randomizeLayers, bool flip, float? customLayer) : base(graphics, content)
        {
            Position = position;
            Texture = texture;
            PrimaryColor = primaryColor ;
            Origin = origin ;
            Scale = scale;
            SpriteType = spriteType;
            SourceRectangle = sourceRectangle;


          

            if (customLayer != null)
                CustomLayer = customLayer;
            if (flip)
                SpriteEffectsAnchor = SpriteEffects.FlipHorizontally;
            else
                SpriteEffectsAnchor = SpriteEffects.None;

            Rotation = rotation;

        }

        private void SharedConstructor(ElementType spriteType, Rectangle sourceRectangle, float rotation, bool randomizeLayers, bool flip, float? customLayer)
        {
            SpriteType = spriteType;
            SourceRectangle = sourceRectangle;


          

            if (customLayer != null)
                CustomLayer = customLayer;
            if (flip)
                SpriteEffectsAnchor = SpriteEffects.FlipHorizontally;
            else
                SpriteEffectsAnchor = SpriteEffects.None;

            Rotation = rotation;
        }

        ///
        public BaseSprite(GraphicsDevice graphics, ContentManager content, Vector2 position, Texture2D texture, Color primaryColor,
            Vector2 origin, Vector2 scale, float layer) : base(graphics, content)
        {
            Position = position;
            Texture = texture;
            PrimaryColor = primaryColor;
            Origin = origin;
            Scale = scale;
            CustomLayer = layer;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="position"></param>
        /// <param name="updatePeripheralActoins">If true, will update more than just the sprite position/destination rectangle, such as animations or color shifters</param>
        public virtual void Update(GameTime gameTime, Vector2 position, bool updatePeripheralActoins = true)
        {
            if (updatePeripheralActoins)
            {
                if (ColorShifter != null)
                {
                    ColorShifter.Update(gameTime, this);
                    if (ColorShifter.FlaggedForRemovalUponFinish && ColorShifter.IsNormal)
                        ColorShifter = null;
                }
            }
            
            Position = position;

        }
        public void ForceSetPosition(Vector2 position)
        {
            Position = position;
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
            //if (ColorShifter != null)
              //  throw new Exception($"Color Effect is already instantiated");
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
            if(PrimaryColor != colorToUse)
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
        protected float GetYAxisLayerDepth( )
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

        public void SwapScale(Vector2 scale)
        {
            Scale = scale;
        }

    }
}
