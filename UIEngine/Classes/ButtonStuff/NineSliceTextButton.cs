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

        private int? _forcedWidth;
        private int? _forcedHeight;
        private bool _centerText;

        public NineSliceTextButton(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2 position, float layerDepth,
             List<Text> textList, Action buttonAction, int? forcedWidth = null, int? forcedHeight = null, bool centerText = false,
            Sprite? foregroundSprite = null, Point? samplePoint = null, Rectangle? sourceRectangle = null, bool hoverTransparency = true) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth, sourceRectangle, buttonAction, foregroundSprite, samplePoint, hoverTransparency)
        {
            _textList = textList;
            _centerText = centerText;
            _forcedWidth = forcedWidth;
            _forcedHeight = forcedHeight;
            MovePosition(position);

            

        }
        public override void MovePosition(Vector2 newPos)
        {
            // base.MovePosition(newPos);

            
            Text combinedtext = TextFactory.CombineText(_textList, LayerDepth);

            int characterWidth = (int)TextFactory.SingleCharacterWidth();
            int width = _forcedWidth ?? (int)combinedtext.TotalStringWidth + characterWidth;
            int height = _forcedHeight ?? (int)combinedtext.TotalStringHeight + characterWidth;
            Position = newPos;

            float newTextPosX = Position.X;
            if (_centerText)
            {
                int halfStringWidth = (int)(combinedtext.TotalStringWidth + characterWidth);
                newTextPosX = Position.X + width / 2 - halfStringWidth;
            }
            _textPositions = new List<Vector2>();
            GeneratePositionsForLines(new Vector2(newTextPosX, Position.Y + characterWidth));
            if (_forcedWidth == null)
                BackGroundSprite = SpriteFactory.CreateNineSliceTextSprite(Position, combinedtext, UI.ButtonTexture, LayerDepth);
            else
                BackGroundSprite = SpriteFactory.CreateNineSliceSprite(Position, _forcedWidth.Value,
                    _forcedHeight.Value, UI.ButtonTexture, LayerDepth);
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
           
            for (int i = 0; i < _textList.Count; i++)
            {
             
                if(i > 0)
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
                    //text.ChangeColor(Color);
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
