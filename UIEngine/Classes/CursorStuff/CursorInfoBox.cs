using InputEngine.Classes;
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
using UIEngine.Classes.ButtonStuff;
using UIEngine.Classes.Storage;

namespace UIEngine.Classes.CursorStuff
{
    internal class CursorInfoBox : InterfaceSection
    {
        public NineSliceTextButton NineSliceTextButton { get; set; }

        public Vector2 _offSet;

        public bool IsDrawn { get; set; } = true;

        private const int _maxWidth = 480;

        public CursorInfoBox(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position,
            float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }

        public override void LoadContent()
        {
            //base.LoadContent();

        }

        public void LoadNewText( List<string> newText)
        {
            ChildSections.Clear();
;
            List<Text> text = new List<Text>();

            for(int i =0; i < newText.Count; i++)
            {
                float scale = 2f;
                if (i > 0)
                    scale = 1.5f;
                text.Add(TextFactory.CreateUIText(newText[i], Position, null, _maxWidth, GetLayeringDepth(UILayeringDepths.High), scale: scale));

            }
      

            NineSliceTextButton = new NineSliceTextButton(this, graphics, content, Position,
               GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Low), text, null);
            _offSet = new Vector2(0, NineSliceTextButton.Height * -1);
        }
        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
            if (NineSliceTextButton != null)
                NineSliceTextButton.MovePosition(Position + _offSet);

        }
        public override void Update(GameTime gameTime)
        {
            if (IsDrawn )
            {

                MovePosition(Controls.MouseUIPosition);
                base.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsDrawn)
                base.Draw(spriteBatch);
            IsDrawn = false;

        }
    }
}
