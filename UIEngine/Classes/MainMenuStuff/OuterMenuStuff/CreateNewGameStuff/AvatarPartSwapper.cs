using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes.Animations.BodyPartStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;
using TextEngine.Classes;
using UIEngine.Classes.ButtonStuff;

namespace UIEngine.Classes.MainMenuStuff.OuterMenuStuff.CreateNewGameStuff
{
    internal class AvatarPartSwapper : InterfaceSection
    {

        protected BodyPiece BodyPiece1 { get; set; }
        private Rectangle _leftSourceRectangle = new Rectangle(400, 704, 16, 16);

        private Rectangle _rightSourceRectangle = new Rectangle(416, 704, 16, 16);

        private Vector2 _leftButtonOffSet = new Vector2(-16, 0);
        private Vector2 _rightButtonOffSet = new Vector2(164, 0);
        private Button _cycleLeftButton;
        private Button _cycleRightButton;

        private string _textString;
        private Text _text;

        public int Index => BodyPiece1.Index;
        public AvatarPartSwapper(string text, BodyPiece bodyPiece, InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            BodyPiece1 = bodyPiece;
            _textString = text;
        }

        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
            ChildSections.Clear();
            _text = TextFactory.CreateUIText(_textString, GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Low));
            _cycleLeftButton = new Button(this, graphics, content, Position + _leftButtonOffSet, GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Low),
               _leftSourceRectangle, ForwardAction);

            _cycleRightButton = new Button(this, graphics, content, Position + _rightButtonOffSet, GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Low),
               _rightSourceRectangle, BackwardsAction);
            _text.Update(new Vector2(Position.X + 60, Position.Y));

        }

        public virtual void ForwardAction()
        {
            BodyPiece1.CycleBackwards();
        }
        public virtual void BackwardsAction()
        {
            BodyPiece1.CycleForward();

        }
        public override void LoadContent()
        {
           
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, 180, 32);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _text.Update(new Vector2(Position.X + 60, Position.Y));
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            _text.Draw(spriteBatch);
        }

       
    }
}
