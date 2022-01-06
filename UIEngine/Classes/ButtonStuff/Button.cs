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
    internal class Button : InterfaceElement
    {

        protected Rectangle SourceRectangle = new Rectangle(0, 0, 48, 48);
        private Rectangle hitBox;
        private readonly int DefaultButtonWidth = 64;
        private readonly int DefaultButtonHeight = 64;

        public override  Rectangle HitBox { get => NineSliceSprite.HitBox; protected set => hitBox = value; }


        protected Action OnClick { get; set; }
        protected UINineSliceSprite NineSliceSprite { get; set; }

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
        : base(graphicsDevice, content, interfaceSection, position)
        {
            if (sourceRectangle != null)
                SourceRectangle = (Rectangle)sourceRectangle;

            ForegroundSprite = foregroundSprite;

            Color = Color.White;
            OnClick = new Action(ButtonAction);


            //Allows you to provide your own texture.
            Texture2D spriteTexture;
            if (texture == null)
                spriteTexture = UserInterface.ButtonTexture;
            else
                spriteTexture = texture;


            NineSliceSprite = SpriteFactory.CreateNineSliceSprite(position, DefaultButtonWidth, DefaultButtonHeight, null, null, null, null, Layers.buildings);

            if (buttonAction != null)
                OnClick = buttonAction;
            else
                OnClick = new Action(ButtonAction);

            HoverTransparency = hoverTransparency;
        }

        protected override Layers SetUILayer()
        {
            return Layers.buildings;
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
        : base(graphicsDevice, content, interfaceSection, position)
        {
            if (sourceRectangle != null)
                SourceRectangle = (Rectangle)sourceRectangle;
            else
                SourceRectangle = new Rectangle(0, 0, 48, 48);
            Color = Color.White;
            OnClick = new Action(ButtonAction);
        }

        internal void Lock()
        {
            Locked = true;
        }

        internal void Unlock()
        {
            Locked = false;
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
