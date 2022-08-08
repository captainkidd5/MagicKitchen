using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations.BodyPartStuff;
using SpriteEngine.Classes.Animations.EntityAnimations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Classes.Components;

namespace UIEngine.Classes.MainMenuStuff.OuterMenuStuff.CreateNewGameStuff
{
    internal class PlayerAvatarViewer : MenuSection
    {
        private Sprite _backgroundSprite;
        private CustomizeableAnimator _animator;
        private Rectangle _avatarBackground = new Rectangle(384, 640, 48, 64);

        private Vector2 _scale = new Vector2(2f, 2f);

        public PlayerAvatarViewer(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) : 
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }

        private BodyPiece[] _pieces;

        public override void LoadContent()
        {


            _backgroundSprite = SpriteFactory.CreateUISprite(Position, _avatarBackground, UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Low), scale: _scale);

            TotalBounds = RectangleHelper.RectFromPosition(Position, _backgroundSprite.HitBox.Width, _backgroundSprite.HitBox.Height);

            base.LoadContent();
        }

        public void SetPieces(BodyPiece[] bodyPieces)
        {
            _pieces = bodyPieces;

        }
        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
        
            _backgroundSprite = SpriteFactory.CreateUISprite(Position, _avatarBackground, UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Low), scale: _scale);
            TotalBounds = RectangleHelper.RectFromPosition(Position, _backgroundSprite.HitBox.Width, _backgroundSprite.HitBox.Height);
            _animator = new CustomizeableAnimator(_pieces, -24, -16);
            _animator.Load(null, Position, new Vector2(3f, 3f));
            _animator.PerformAction(null, DataModels.Enums.Direction.Down, DataModels.ActionType.Walking);


        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _animator.Update(gameTime, DataModels.Enums.Direction.Down, true, Position, 1f);
            _backgroundSprite.Update(gameTime, Position);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            _animator.Draw(spriteBatch, DataModels.Enums.SubmergenceLevel.None);
            _backgroundSprite.Draw(spriteBatch);
        }
    }
}
