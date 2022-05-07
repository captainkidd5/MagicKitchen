using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEngine.Classes.Presets
{
    public class ProgressBarSprite
    {
        private Vector2 _position;
        private Vector2 _positionOffSet = new Vector2(_sourceRectangle.Width / 2, -16);
        private static Rectangle _sourceRectangle = new Rectangle(0, 16, 32, 16);
        private DestinationRectangleSprite _outLineSprite;

        private Sprite _foreGroundSprite;

        private int _goal = 20;
        private int _currentAmount;

        public bool Done => _currentAmount >= _goal;

        private SimpleTimer _simpleTimer;

        public ProgressBarSprite()
        {

        }

        public void Load( float rate, Vector2 position,Vector2 positionOffSet, float layerDepth)
        {
            _simpleTimer = new SimpleTimer(rate);
            _positionOffSet = positionOffSet;
            _position = position + _positionOffSet;
            _outLineSprite = SpriteFactory.CreateWorldDestinationSprite(1, 16, _position, new Rectangle(0, 0, 1, 1), SpriteFactory.StatusIconTexture, customLayer: layerDepth, primaryColor: Color.Green);
            _foreGroundSprite = SpriteFactory.CreateWorldSprite(_position, _sourceRectangle, SpriteFactory.StatusIconTexture, customLayer: layerDepth + SpriteUtility.GetMinimumOffSet());
        }

        /// <summary>
        /// Returns true is progressbar is done
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public bool Update(GameTime gameTime)
        {
            if (_simpleTimer.Run(gameTime))
            {
                _currentAmount++;
                if (!Done)
                    _outLineSprite.RectangleWidth = (int)((float)((float)_currentAmount / (float)_goal) * (float)_sourceRectangle.Width);
            }

            _outLineSprite.Update(gameTime, _position);
            _foreGroundSprite.Update(gameTime, _position);
            return Done;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _outLineSprite.Draw(spriteBatch);
            _foreGroundSprite.Draw(spriteBatch);
        }
    }
}
