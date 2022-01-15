using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SoundEngine.Classes;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEngine.Classes.ButtonStuff
{
    internal abstract class ButtonBase : InterfaceSection
    {
        protected static readonly Point _samplePoint = new Point(32, 32);

        protected Rectangle SourceRectangle = new Rectangle(0, 0, 48, 48);
        protected Rectangle hitBox;
        internal override Rectangle HitBox { get => BackGroundSprite.HitBox; set => hitBox = value; }

        protected readonly int DefaultButtonWidth = 64;
        protected readonly int DefaultButtonHeight = 64;

        protected Action OnClick { get; set; }

        protected Sprite ForegroundSprite { get; set; }
        public Color Color { get; set; }

        //If locked, button will not respond to hover or click events.
        private bool Locked { get; set; }
        private bool HoverTransparency { get; set; }
        protected BaseSprite BackGroundSprite { get; set; }
        /// <summary>
        /// Nineslice constructor
        /// </summary>

        public ButtonBase(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
             Vector2 position,float layerDepth, Rectangle? sourceRectangle, Sprite? foregroundSprite, Texture2D? texture, Point? samplePoint, Action buttonAction = null, bool hoverTransparency = true)
        : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            if (sourceRectangle != null)
                SourceRectangle = (Rectangle)sourceRectangle;

            ForegroundSprite = foregroundSprite;

            Color = Color.White;

            //Allows you to provide your own texture.
            Texture2D spriteTexture = texture ?? UI.ButtonTexture;

            OnClick = buttonAction ?? new Action(ButtonAction);


            HoverTransparency = hoverTransparency;
        }
        public override void Load()
        {
            base.Load();
            HitBox = BackGroundSprite.HitBox;


        }


        public void SwapForeGroundSprite(Sprite sprite)
        {
            ForegroundSprite = sprite;
        }

        public void SwapBackgroundSprite(Sprite sprite)
        {
            BackGroundSprite = sprite;
        }
        public void SwapForeGroundSprite(Rectangle newSourceRectangle)
        {
            ForegroundSprite.SwapSourceRectangle(newSourceRectangle);
        }

        public void SwapBackgroundSprite(Rectangle newSourceRectangle)
        {
            BackGroundSprite.SwapSourceRectangle(newSourceRectangle);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (BackGroundSprite == null)
                throw new Exception($"Forgot to add backgroundsprite to button");

            if (ForegroundSprite != null)
                ForegroundSprite.Update(gameTime, Position);
            if (!Locked)
            {
                BackGroundSprite.Update(gameTime, Position);
                if (Hovered)
                {
                    if (HoverTransparency)
                        BackGroundSprite.TriggerIntensityEffect();


                    if (Clicked)
                    {
                        OnClick();
                        SoundFactory.PlaySoundEffect("Click1");
                    }
                }
                else if (WasHovered)
                    BackGroundSprite.TriggerReduceEffect();
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            BackGroundSprite.Draw(spriteBatch);
            if (ForegroundSprite != null)
                ForegroundSprite.Draw(spriteBatch);
        }

        /// <summary>
        /// Button will execute this action if pressed.
        /// </summary>
        protected virtual void ButtonAction()
        {

        }
    }
}
