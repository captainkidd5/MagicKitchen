using Globals.Classes.Helpers;
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
        internal override Rectangle TotalBounds { get => BackGroundSprite.HitBox; set => hitBox = value; }

        protected readonly int DefaultButtonWidth = 64;
        protected readonly int DefaultButtonHeight = 64;
        private bool _requireConfirmation;

        protected Action OnClick { get; set; }

        protected Sprite ForegroundSprite { get; set; }
        public Color Color => BackGroundSprite.PrimaryColor;

        //If locked, button will not respond to hover or click events.
        private bool Locked { get; set; }
        private bool HoverTransparency { get; set; }
        protected BaseSprite BackGroundSprite { get; set; }

        private ConfirmationWindow _confirmationWindow;

        //Offsets foreground sprite from background sprite
        protected Vector2? ForeGroundSpriteOffSet { get; set; }

        public bool IgnoreDefaultClickSoundEffect { get; set; }
        public bool IgnoreDefaultHoverSoundEffect { get; set; }

        /// <summary>
        /// Nineslice constructor
        /// </summary>

        public ButtonBase(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
             Vector2 position,float layerDepth, Rectangle? sourceRectangle, Action buttonAction, Sprite? foregroundSprite,  Point? samplePoint,  bool hoverTransparency = true)
        : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            if (sourceRectangle != null)
                SourceRectangle = (Rectangle)sourceRectangle;

            ForegroundSprite = foregroundSprite;
            if (ForegroundSprite != null)
                ForeGroundSpriteOffSet = Vector2.Zero;


            //Allows you to provide your own texture.
            Texture2D spriteTexture = UI.ButtonTexture;

            OnClick = buttonAction ?? new Action(ButtonAction);


            HoverTransparency = hoverTransparency;

           
        }

        public void SetForegroundSpriteOffSet(Vector2 pos)
        {
            ForeGroundSpriteOffSet = pos;
            ForegroundSprite.ForceSetPosition(Position + ForeGroundSpriteOffSet.Value);
        }

        public void AddConfirmationWindow(string confirmationText = null)
        {
            _requireConfirmation = true;
            _confirmationWindow = new ConfirmationWindow(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium), OnClick, confirmationText: confirmationText);

        }
        public override void LoadContent()
        {
            base.LoadContent();
            TotalBounds = BackGroundSprite.HitBox;

            if (_requireConfirmation)
            {
                _confirmationWindow.LoadContent();
            }
        }

        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);

       

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

        public void FadeOut()
        {
            BackGroundSprite.SetEffectToDefault();
            BackGroundSprite.AddFaderEffect(null, null, true);

            if (ForegroundSprite != null)
            {
                ForegroundSprite.SetEffectToDefault();
                ForegroundSprite.AddFaderEffect(null, null, true);
            }
         
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (BackGroundSprite == null)
                throw new Exception($"Forgot to add backgroundsprite to button");

            if (ForegroundSprite != null)
            {
                Vector2 foregroundPos = Position;
                if (ForeGroundSpriteOffSet != null)
                    foregroundPos += ForeGroundSpriteOffSet.Value;
                ForegroundSprite.Update(gameTime, foregroundPos);

            }


            if (!Locked)
            {
                BackGroundSprite.Update(gameTime, Position);
                if (Hovered)
                {
                    if (HoverTransparency)
                        BackGroundSprite.TriggerIntensityEffect();

                    if(JustHovered)
                        if (!IgnoreDefaultHoverSoundEffect && UI.MayPlayButtonHoverSound)
                        {
                            SoundFactory.PlayEffectPackage("ButtonHover");
                            UI.MayPlayButtonHoverSound = false;
                        }

                    if (Clicked)
                    {
                        if(!IgnoreDefaultClickSoundEffect)
                            SoundFactory.PlaySoundEffect("Click1");
                        if (_requireConfirmation)
                        {
                            _confirmationWindow.Activate();
                        }
                        else
                        {
                            OnClick();
                        }

                    }
                }
                else if (WasHovered)
                    BackGroundSprite.TriggerReduceEffect();
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
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

        public void SetLock(bool newVal)
        {
            Locked = newVal;
        }

        internal override void CleanUp()
        {
            base.CleanUp();
            BackGroundSprite.ResetColors();
        }
    }
}
