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

        public Rectangle? DestinationRectangle { get; set; }
        public Vector2 Origin { get; set; }

        protected float Scale { get; set; }

        protected float LayerDepth { get; set; }

        public int Width => (int)(HitBox.Width * Scale);
        public int Height => (int)(HitBox.Height * Scale);
        public Rectangle HitBox { get; protected set; }

        /// <see cref="UpdateColor(Color)"/>
        public Color PrimaryColor { get; protected set; }

        protected SpriteEffects SpriteEffectsAnchor { get; set; } //this is set when sprite is created and never changed

        public SpriteEffects SpriteEffects { get; set; }

        public IEnumerable<ISpriteAddon> SpriteAddons { get; set; }

        public bool IsOpaque => PrimaryColor.A == 255;
        public bool IsTransparent => PrimaryColor.A == 51;

        Fader Fader { get; set; }

        public BaseSprite(GraphicsDevice graphics, ContentManager content, Vector2 position, Texture2D texture, Color primaryColor,
             Vector2 origin, float scale, Layers layer) : base(graphics, content)
        {
            Position = position;
            Texture = texture;
            PrimaryColor = primaryColor ;
            Origin = origin ;
            Scale = scale;
        }

        public BaseSprite(GraphicsDevice graphics, ContentManager content, Texture2D texture, Rectangle destinationRectangle, Color primaryColor,
            Vector2 origin, float scale, Layers layer) : base(graphics, content)
        {
            Texture = texture;
            DestinationRectangle = destinationRectangle;
            PrimaryColor = primaryColor;
            Origin = origin;
            Scale = scale;


        }

        public virtual void Update(GameTime gameTime, Vector2 position)
        {
            if (Fader != null)
            {
                Fader.Update(gameTime, this);
                if (Fader.FlaggedForRemovalUponFinish && Fader.IsOpaque)
                    Fader = null;
            }
            Position = position;

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }


        public void TurnTransparent()
        {
            Fader.TriggerTurnTransparent();
        }
        public void TriggerOpaque()
        {
            Fader.TriggerReturnOpaque();
        }
        public void AddFader(float? minOpac, float? maxOpac, float? speed, bool immediatelyTriggerFade = false)
        {
            if (Fader != null)
                throw new Exception($"Fader is already instantiated");
            Fader = new Fader(minOpac, maxOpac, speed);
            if(immediatelyTriggerFade)
                TurnTransparent(); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flagForRemoval">Will remove fader after fader has returned opacity to full</param>
        public void RemoveFader(bool flagForRemoval = false)
        {
            if (Fader == null)
                throw (new Exception($"Fader does not exists"));
            if (flagForRemoval)
            {
                Fader.FlaggedForRemovalUponFinish = true;
                Fader.TriggerReturnOpaque();
            }
            else
                Fader = null;
        }
        public void SetEffectToDefault()
        {
            SpriteEffects = SpriteEffectsAnchor;
        }

        public void UpdateColor(Color colorToUse)
        {
            PrimaryColor = colorToUse;
        }


        
    }
}
