using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;
using TextEngine.Classes;
using static UIEngine.Classes.UI;

namespace UIEngine.Classes.ButtonStuff
{
    internal class NineSliceTextButton : NineSliceButton
    {
        public override bool Hovered { get => base.Hovered; protected set => base.Hovered = value; }


        private List<Text> _textList;

        private int? _forcedWidth;
        private int? _forcedHeight;
        private bool _centerText;

        public bool Displaybackground { get; set; } = true;
        public NineSliceTextButton(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2 position, float layerDepth,
             List<Text> textList, Action buttonAction, int? forcedWidth = null, int? forcedHeight = null, bool centerText = false,
            Sprite? foregroundSprite = null, Point? samplePoint = null, Rectangle? sourceRectangle = null, bool hoverTransparency = true) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth, sourceRectangle, buttonAction, foregroundSprite, samplePoint, hoverTransparency)
        {

            _textList = textList;
            if (_textList.Count < 1)
            {
                throw new Exception($"test");

            }
            _centerText = centerText;
            _forcedWidth = forcedWidth;
            _forcedHeight = forcedHeight;
            MovePosition(position);
           // IgnoreDefaultHoverSoundEffect = true;

        }
        public override void MovePosition(Vector2 newPos)
        {
            Position = newPos;
            if (_forcedWidth == null)
            {

                BackGroundSprite = SpriteFactory.CreateNineSliceTextSprite(new Vector2(Position.X - 8, Position.Y - 8), _textList, UI.ButtonTexture, LayerDepth);
            }

            else
            {
                BackGroundSprite = SpriteFactory.CreateNineSliceSprite(Position, _forcedWidth.Value,
                    _forcedHeight.Value, UI.ButtonTexture, LayerDepth);
            }

            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, BackGroundSprite.Width, BackGroundSprite.Height);



            Color sampleCol = TextureHelper.SampleAt(ButtonTextureDat, _samplePoint, ButtonTexture.Width);
            BackGroundSprite.AddSaturateEffect(sampleCol, false);

            //if (_centerText)
            //{
            //    GeneratePositionsForLines(new Vector2(newTextPosX, newTextPosY));

            //}

        }


        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Unload()
        {
            base.Unload();
        }
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
            Vector2 textPos = Position;
            for (int i = 0; i < _textList.Count; i++)
            {
                Text text = _textList[i];

                if (_centerText)
                {
                    textPos = new Vector2(BackGroundSprite.Position.X + BackGroundSprite.Width / 2 - text.Width /2 +32, textPos.Y);
                }
               
                    text.Update(textPos, null, Width);

                
                textPos = new Vector2(textPos.X, textPos.Y + text.Height);
                //text.ChangeColor(Color);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Displaybackground)
                base.Draw(spriteBatch);

            for (int i = 0; i < _textList.Count; i++)
            {
                _textList[i].Draw(spriteBatch);
            }


        }



        protected override void ButtonAction()
        {
            base.ButtonAction();
        }


    }
}
