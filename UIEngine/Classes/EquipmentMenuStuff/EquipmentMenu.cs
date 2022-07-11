using InputEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEngine.Classes.EquipmentMenuStuff
{
    internal class EquipmentMenu : MenuSection
    {

        private Rectangle _backGroundSourceRectangle = new Rectangle(432, 624, 208, 224);
        private Sprite _backGroundSprite;
        private Vector2 _scale = new Vector2(2f, 2f);
        public EquipmentMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            Deactivate();
        }
        public override void LoadContent()
        {

            _backGroundSprite = SpriteFactory.CreateUISprite(
                Position, _backGroundSourceRectangle, UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Low),scale:_scale);
            TotalBounds = _backGroundSprite.HitBox;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Controls.WasKeyTapped(Keys.C))
                Toggle();
            base.Update(gameTime);
            if (IsActive)
                _backGroundSprite.Update(gameTime, Position);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (IsActive)
                _backGroundSprite.Draw(spriteBatch);
        }

       
    }
}
