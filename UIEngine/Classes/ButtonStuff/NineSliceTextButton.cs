using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine.Classes;

namespace UIEngine.Classes.ButtonStuff
{
    internal class NineSliceTextButton : NineSliceButton
    {
        public override bool Hovered { get => base.Hovered; protected set => base.Hovered = value; }

        private Text _text;
        private Vector2 _textPosition;


        public NineSliceTextButton(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2 position, float layerDepth, Rectangle? sourceRectangle,
            Sprite foregroundSprite, Texture2D texture,Text text, Point? samplePoint, Action buttonAction = null, bool hoverTransparency = false) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth, sourceRectangle, foregroundSprite, texture, samplePoint, buttonAction, hoverTransparency)
        {
            _text = text;
            _textPosition = Text.CenterInRectangle(HitBox, _text);
        }



        public override void Load()
        {
            base.Load();
        }

        public override void Unload()
        {
            base.Unload();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _text.Update(gameTime, Position);
            if(DidPositionChange)
                _textPosition = Text.CenterInRectangle(HitBox, _text);
            _text.ChangeColor(Color);

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            _text.Draw(spriteBatch, true);
        }



        protected override void ButtonAction()
        {
            base.ButtonAction();
        }


    }
}
