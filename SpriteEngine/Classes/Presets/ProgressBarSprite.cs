using Globals.Classes;
using Globals.Classes.Time;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Globals.Classes.Settings;

namespace SpriteEngine.Classes.Presets
{
    public class ProgressBarSprite
    {
        private Vector2 _position;
        private Vector2 _positionOffSet;
        private static Rectangle _sourceRectangle = new Rectangle(0, 16, 32, 16);
        private DestinationRectangleSprite _outLineSprite;
        public int Width => (int)((float)_foreGroundSprite.Width * _scale.X);
        public int Height => (int)((float)_foreGroundSprite.Height * _scale.Y);
        private Vector2 _scale;
        private Sprite _foreGroundSprite;

        private float _goal;
        private float _currentAmount;
        private float _globalStartTime;
        public bool Started { get; set; }
        public bool Done => _currentAmount >= _goal;


        public ProgressBarSprite()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rate"></param>
        /// <param name="position"></param>
        /// <param name="layerDepth"></param>
        public void Load(Vector2 position, float layerDepth, Vector2? positionOffSet = null,Vector2? scale = null, ElementType elementType = ElementType.World)
        {
            if(positionOffSet != null)
                _positionOffSet += positionOffSet.Value;

            _position = position + _positionOffSet;

            _scale = scale ?? Vector2.One;
            _outLineSprite = SpriteFactory.CreateDestinationSprite(1, 16, _position, new Rectangle(0, 0, 1, 1),
                SpriteFactory.StatusIconTexture, elementType, customLayer: layerDepth, primaryColor: Color.Green);

            if(elementType == ElementType.World)
            _foreGroundSprite = SpriteFactory.CreateWorldSprite(_position, _sourceRectangle, SpriteFactory.StatusIconTexture,
                customLayer: layerDepth + SpriteUtility.GetMinimumOffSet(), scale: _scale);
            else
                _foreGroundSprite = SpriteFactory.CreateUISprite(_position, _sourceRectangle, SpriteFactory.StatusIconTexture,
               customLayer: layerDepth + SpriteUtility.GetMinimumOffSet(), scale: _scale);
        }
      
        public void Start(int secondsToRunFor)
        {
            Started = true;
            _globalStartTime = Clock.TotalTime;
            _goal = _globalStartTime + _globalStartTime;
            
        }
        public void Update(GameTime gameTime)
        {
            if (Started && !Done)
            {

                _currentAmount = Clock.TotalTime - _globalStartTime;
                    _outLineSprite.RectangleWidth = (int)((float)((float)_currentAmount / (float)_goal) * (float)_sourceRectangle.Width * _scale.X);
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
