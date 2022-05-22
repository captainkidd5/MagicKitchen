using Globals.Classes;
using Globals.Classes.Time;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEngine.Classes.Presets
{
    public class WorldProgressBarSprite
    {
        private Vector2 _position;
        private Vector2 _positionOffSet = new Vector2(_sourceRectangle.Width / 2, -16);
        private static Rectangle _sourceRectangle = new Rectangle(0, 16, 32, 16);
        private DestinationRectangleSprite _outLineSprite;

        private Sprite _foreGroundSprite;

        private int _goal;
        private int _currentAmount;
        private int _globalStartTime;
        public bool Started { get; set; }
        public bool Done => _currentAmount >= _goal;


        public WorldProgressBarSprite()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rate"></param>
        /// <param name="position"></param>
        /// <param name="layerDepth"></param>
        /// <param name="positionOffSet">Leave null for tiles, defaults to correct position for them</param>
        public void Load( float rate, Vector2 position, float layerDepth, Vector2? positionOffSet = null)
        {
            if(positionOffSet != null)
                _positionOffSet += positionOffSet.Value;

            _position = position + _positionOffSet;

            _outLineSprite = SpriteFactory.CreateWorldDestinationSprite(1, 16, _position, new Rectangle(0, 0, 1, 1),
                SpriteFactory.StatusIconTexture, customLayer: layerDepth, primaryColor: Color.Green);
            _foreGroundSprite = SpriteFactory.CreateWorldSprite(_position, _sourceRectangle, SpriteFactory.StatusIconTexture,
                customLayer: layerDepth + SpriteUtility.GetMinimumOffSet());
        }

        public void Start(int secondsToRunFor)
        {
            Started = true;
            _globalStartTime = Clock.TotalTime;
            _goal = _globalStartTime + _globalStartTime;
            
        }
        public void Update(GameTime gameTime)
        {
            if (Started)
            {

                _currentAmount = Clock.TotalTime - _globalStartTime;
                if (!Done)
                    _outLineSprite.RectangleWidth = (int)((float)((float)_currentAmount / (float)_goal) * (float)_sourceRectangle.Width);
            }

            _outLineSprite.Update(gameTime, _position);
            _foreGroundSprite.Update(gameTime, _position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _outLineSprite.Draw(spriteBatch);
            _foreGroundSprite.Draw(spriteBatch);
        }
    }
}
