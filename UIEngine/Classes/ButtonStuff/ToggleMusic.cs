using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SoundEngine.Classes.SongStuff;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEngine.Classes.ButtonStuff
{
    internal class ToggleMusic : InterfaceSection
    {

        public NineSliceButton ToggleMusicButton { get; set; }

        private readonly Rectangle MusicNoteSourceRectangle = new Rectangle(336, 0, 32, 32);
        private readonly Rectangle MusicNoteMUTEDSourceRectangle = new Rectangle(368, 0, 32, 32);

        public ToggleMusic(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth,
            bool suppressParentSection = true) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth, suppressParentSection)
        {
            ToggleMusicButton = new NineSliceButton(this, graphicsDevice, content, Position, GetLayeringDepth(UILayeringDepths.Back), MusicNoteSourceRectangle,
              SpriteFactory.CreateUISprite(Position, MusicNoteSourceRectangle, UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Medium)),
              null,null,new Action(() =>
               {
                   ToggleMusicAction();
               }),true);
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        private void ToggleMusicAction()
        {
            SongManager.Muted = !SongManager.Muted;
            if (SongManager.Muted)
                ToggleMusicButton.SwapForeGroundSprite(MusicNoteMUTEDSourceRectangle);
            else
                ToggleMusicButton.SwapForeGroundSprite(MusicNoteSourceRectangle);

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
