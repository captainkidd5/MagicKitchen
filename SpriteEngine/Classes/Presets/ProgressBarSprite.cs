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
        private static Rectangle _sourceRectangle = new Rectangle(0, 32, 32, 16);
        private DestinationRectangleSprite _outLineSprite;
        public int Width => (int)((float)_foreGroundSprite.Width * _scale.X);
        public int Height => (int)((float)_foreGroundSprite.Height * _scale.Y);
        private Vector2 _scale;
        private Sprite _foreGroundSprite;

        private float _goal;
        private float _currentAmount;
        private float _globalStartTime;
        public bool Started { get; set; }
        public bool FullyCharged => _currentAmount >= _goal;


        public ProgressBarSprite()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rate"></param>
        /// <param name="position"></param>
        /// <param name="layerDepth"></param>
        public void Load(Vector2 position, float layerDepth, Vector2? positionOffSet = null,Vector2? scale = null, ElementType elementType = ElementType.World, Color? color = null)
        {
            if(positionOffSet != null)
                _positionOffSet += positionOffSet.Value;

            _position = position + _positionOffSet;

            _scale = scale ?? Vector2.One;
     

            if(elementType == ElementType.World)
            _foreGroundSprite = SpriteFactory.CreateWorldSprite(_position, _sourceRectangle, SpriteFactory.StatusIconTexture,
                customLayer: layerDepth + SpriteUtility.GetMinimumOffSet(), scale: _scale);
            else
                _foreGroundSprite = SpriteFactory.CreateUISprite(_position, _sourceRectangle, SpriteFactory.StatusIconTexture,
               customLayer: layerDepth + SpriteUtility.GetMinimumOffSet(), scale: _scale);

            _outLineSprite = SpriteFactory.CreateDestinationSprite(32, 16, _position, new Rectangle(0, 0, _foreGroundSprite.Width, _foreGroundSprite.Height),
         SpriteFactory.StatusIconTexture, elementType, customLayer: layerDepth, primaryColor: color == null ? Color.Green : color.Value);
            _outLineSprite.RectangleWidth = _foreGroundSprite.Width;
            _outLineSprite.RectangleHeight = _foreGroundSprite.Height;
        }
      
        public void Start(int secondsToRunFor)
        {
            Started = true;
            _globalStartTime = Clock.TotalTime;
            _goal = _globalStartTime + _globalStartTime;
            
        }
        public void Update(GameTime gameTime, Vector2 position)
        {
            if (Started && !FullyCharged)
            {

                _currentAmount = Clock.TotalTime - _globalStartTime;
               Vector2 lerpedScale = Vector2.Lerp(_outLineSprite.Scale, new Vector2((float)((float)_currentAmount / (float)_goal), _outLineSprite.Scale.Y), .5f);
                _outLineSprite.SwapScale(lerpedScale);

               // _outLineSprite.RectangleWidth = (int)((float)((float)_currentAmount / (float)_goal) * (float)_sourceRectangle.Width * _scale.X);
            }
            _position = position + _positionOffSet;

            _outLineSprite.Update(gameTime, _position);
            _foreGroundSprite.Update(gameTime, _position);
        }

        /// <summary>
        /// Call this if progress isn't based on a timer system
        /// </summary>
        /// <param name="currentAmt"></param>
        /// <param name="totalAmt"></param>
        public void ManualSetCurrentAmountAndUpdate(float currentAmt, float totalAmt)
        {
            _currentAmount = currentAmt;
            _goal = totalAmt;
            Vector2 lerpedScale = Vector2.Lerp(_outLineSprite.Scale, new Vector2((float)((float)currentAmt / (float)totalAmt), _outLineSprite.Scale.Y), .1f);

            _outLineSprite.SwapScale(lerpedScale);
            //_outLineSprite.RectangleWidth = (int)((float)((float)currentAmt / (float)totalAmt) * (float)_sourceRectangle.Width * _scale.X);

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _outLineSprite.Draw(spriteBatch);
            _foreGroundSprite.Draw(spriteBatch);
        }
    }
}
