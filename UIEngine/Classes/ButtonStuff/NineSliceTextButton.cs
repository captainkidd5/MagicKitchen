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

        public NineSliceTextButton(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2 position, float layerDepth, Rectangle? sourceRectangle,
            Sprite foregroundSprite, Texture2D texture,List<Text> textList, Point? samplePoint, Action buttonAction = null, bool hoverTransparency = false) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth, sourceRectangle, foregroundSprite, texture, samplePoint, buttonAction, hoverTransparency)
        {
            _textPositions = new List<Vector2>();
           _textList = textList;
            GeneratePositionsForLines();

            BackGroundSprite = SpriteFactory.CreateNineSliceTextSprite(position,TextFactory.CombineText(textList, LayerDepth), UI.ButtonTexture, LayerDepth, null, null);
            Color sampleCol = TextureHelper.SampleAt(ButtonTextureDat, samplePoint ?? _samplePoint, ButtonTexture.Width);
            BackGroundSprite.AddSaturateEffect(sampleCol, false);
        }

        /// <summary>
        /// Fills <see cref="_textPositions"/> for each line of text provided. Increases by height x => x.Height == text.TotalStringHeight
        /// </summary>
        /// <param name="textIndexPos">Increases each loop</param>
        private void GeneratePositionsForLines()
        {
            Vector2 textIndexPos = Position;
            float y = Position.Y;
            Rectangle backgroundRec = new Rectangle((int)Position.X, (int)Position.Y, BackGroundSprite.HitBox.Width, BackGroundSprite.HitBox.Height);
            for (int i = 0; i < _textList.Count; i++)
            {
                if(_textList[i] != null && _textList[i].FullString.Contains("Pl"))
                    Console.WriteLine("test");
                textIndexPos = Text.CenterInRectangle(backgroundRec, _textList[i]);
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
                GeneratePositionsForLines();
            

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
