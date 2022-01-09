using Globals.Classes;
using InputEngine.Classes.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.InterfaceStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static UIEngine.Classes.UserInterface;
using static Globals.Classes.Settings;

namespace UIEngine.Classes.ButtonStuff
{
    internal class Button : InterfaceSection
    {

        protected Rectangle SourceRectangle = new Rectangle(0, 0, 48, 48);
        private Rectangle hitBox;
        private readonly int DefaultButtonWidth = 64;
        private readonly int DefaultButtonHeight = 64;

        internal  override Rectangle HitBox { get => NineSliceSprite.HitBox;  set => hitBox = value; }


        protected Action OnClick { get; set; }
        protected NineSliceSprite NineSliceSprite { get; set; }

        protected Sprite ForegroundSprite { get; set; }
        public Color Color { get; set; }

        //If locked, button will not respond to hover or click events.
        private bool Locked { get; set; }
        private bool HoverTransparency { get; set; }

        /// <summary>
        /// Nineslice constructor
        /// </summary>

        public Button(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
             Vector2 position, Rectangle? sourceRectangle, Sprite? foregroundSprite, Action buttonAction = null,
            Texture2D texture = null, bool hoverTransparency = false)
        : base(interfaceSection, graphicsDevice, content, position)
        {
            if (sourceRectangle != null)
                SourceRectangle = (Rectangle)sourceRectangle;

            ForegroundSprite = foregroundSprite;

            Color = Color.White;

            //Allows you to provide your own texture.
            Texture2D spriteTexture = texture ?? UserInterface.ButtonTexture;

            NineSliceSprite = SpriteFactory.CreateNineSliceSprite(position, DefaultButtonWidth, DefaultButtonHeight, null, null, null, null, Layers.buildings);

            OnClick = buttonAction ?? new Action(ButtonAction);


            HoverTransparency = hoverTransparency;
        }


        public void SwapForeGroundSprite(Sprite sprite)
        {
            ForegroundSprite = sprite;
        }
        /// <summary>
        /// Non-nineslice constructor
        /// </summary>
        public Button(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,

            Vector2 position, Rectangle? sourceRectangle)
        : base(interfaceSection,graphicsDevice, content, position)
        {
            if (sourceRectangle != null)
                SourceRectangle = (Rectangle)sourceRectangle;
            else
                SourceRectangle = new Rectangle(0, 0, 48, 48);
            Color = Color.White;
            OnClick = new Action(ButtonAction);
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            if (ForegroundSprite != null)
                ForegroundSprite.Update(gameTime, Position);
            if (!Locked)
            {
                NineSliceSprite.Update(gameTime, Position);
                if (Hovered)
                {
                    
                    if (HoverTransparency)
                        NineSliceSprite.Color = Color * .5f;

                    if (Clicked)
                        OnClick();
                }
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            NineSliceSprite.Draw(spriteBatch);
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
