using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes.Animations.BodyPartStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Classes.ButtonStuff;

namespace UIEngine.Classes.MainMenuStuff.OuterMenuStuff.CreateNewGameStuff
{
    internal class AvatarPartSwapper : InterfaceSection
    {
        private readonly BodyPiece _bodyPiece;
        private Rectangle _leftSourceRectangle = new Rectangle(400, 704, 16, 16);

        private Rectangle _rightSourceRectangle = new Rectangle(416, 704, 16, 16);


        private Button _cycleLeftButton;
        private Button _cycleRightButton;

        public AvatarPartSwapper(BodyPiece bodyPiece, InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            _bodyPiece = bodyPiece;
        }
        public override void LoadContent()
        {
            _cycleLeftButton = new Button(this, graphics, content, Position, GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Low),
                _leftSourceRectangle, new Action(() =>
                {
                    _bodyPiece.
                })) ;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        
    }
}
