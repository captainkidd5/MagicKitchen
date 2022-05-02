using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Classes;

namespace TiledEngine.Classes.TileAddons.FurnitureStuff
{
    internal class ProgressIndicator
    {
        private Vector2 _position;
        private Vector2 _positionOffSet = new Vector2(_sourceRectangle.Width / 2, -16);
        private static Rectangle _sourceRectangle = new Rectangle(16,0,32,32);
        private Sprite _outLineSprite;

        private int _goal = 20;
        private int _currentAmount;

        public bool Done => _currentAmount >= _goal;

        private SimpleTimer _simpleTimer;

        public ProgressIndicator()
        {

        }

        public void Load(float rate, Vector2 position, float layerDepth)
        {
            _simpleTimer = new SimpleTimer(rate);
            _position = position + _positionOffSet;
            _outLineSprite = SpriteFactory.CreateWorldSprite(_position, _sourceRectangle, TileLoader.TileIconTexture, customLayer:layerDepth);
        }

        public void Update(GameTime gameTime)
        {
            if (_simpleTimer.Run(gameTime))
            {
                _currentAmount++;
            }

            _outLineSprite.Update(gameTime, _position);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _outLineSprite.Draw(spriteBatch);
        }
    }
}
