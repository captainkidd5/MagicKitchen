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

        private List<Vector2> _textPositions;

        private List<Text> _textList;
        

        public NineSliceTextButton(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2 position, float layerDepth,
            Rectangle? sourceRectangle, List<Text> textList, Action buttonAction,
            Sprite? foregroundSprite, Point? samplePoint,  bool hoverTransparency = false) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth, sourceRectangle, buttonAction, foregroundSprite, samplePoint, hoverTransparency)
        {
            _textList = textList;
            MovePosition(position);

        }
        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
            Text combinedtext = TextFactory.CombineText(_textList, LayerDepth);
            int characterWidth = (int)TextFactory.SingleCharacterWidth();
            int width = (int)combinedtext.TotalStringWidth + characterWidth;
            int height = (int)combinedtext.TotalStringHeight + characterWidth;
            Position = new Vector2(Position.X - width / 2, Position.Y - height / 2);

            _textPositions = new List<Vector2>();
            GeneratePositionsForLines(new Vector2(Position.X + characterWidth, Position.Y));

            BackGroundSprite = SpriteFactory.CreateNineSliceTextSprite(Position, combinedtext, UI.ButtonTexture, LayerDepth, true);
            Color sampleCol = TextureHelper.SampleAt(ButtonTextureDat,  _samplePoint, ButtonTexture.Width);
            BackGroundSprite.AddSaturateEffect(sampleCol, false);
        }
        /// <summary>
        /// Fills <see cref="_textPositions"/> for each line of text provided. Increases by height x => x.Height == text.TotalStringHeight
        /// </summary>
        /// <param name="textIndexPos">Increases each loop</param>
        private void GeneratePositionsForLines(Vector2 startPosition)
        {
            Vector2 textIndexPos = startPosition;
            float y = startPosition.Y;
            Rectangle backgroundRec = TotalBounds;
            for (int i = 0; i < _textList.Count; i++)
            {
             
                y += _textList[i].TotalStringHeight;
                textIndexPos = new Vector2(textIndexPos.X, y);


                _textPositions.Add(textIndexPos);
            }
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
            for(int i = _textList.Count - 1; i >= 0; i--)
            {
                Text text = _textList[i];
                text.Update(gameTime, _textPositions[i]);
                    text.ChangeColor(Color);
            }
            if (DidPositionChange)
                GeneratePositionsForLines(Position);
            

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
     
            base.Draw(spriteBatch);
            for (int i = _textList.Count - 1; i >= 0; i--)
            {
                _textList[i].Draw(spriteBatch, true);

            }

        }



        protected override void ButtonAction()
        {
            base.ButtonAction();
        }

       
    }
}
