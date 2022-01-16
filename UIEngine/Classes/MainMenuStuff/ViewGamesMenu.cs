using Globals.Classes.Helpers;
using IOEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Classes.ButtonStuff;

namespace UIEngine.Classes.MainMenuStuff
{
    /// <summary>
    /// Menu which CONTAINS a list of your saves, and additionally a button at the top for creating a new game
    /// </summary>
    internal class ViewGamesMenu : InterfaceSection
    {
        private NineSliceButton _createNewButton;
            
        private static Rectangle _saveSlotRectangle = new Rectangle(0, 0, 128, 64);
        public ViewGamesMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }

        public override void Load()
        {
            base.Load();
            Position = RectangleHelper.CenterRectangleOnScreen(_saveSlotRectangle);
            Vector2 _saveSlotPosition = Position;
            _createNewButton = new NineSliceButton(this, graphics, content, Position, LayerDepth, _saveSlotRectangle, null, null, null, null,true);
            List<SaveFile> saveFiles = SaveLoadManager.SaveFiles;

            for(int i =0; i < saveFiles.Count; i++)
            {
                _saveSlotPosition = new Vector2(_saveSlotPosition.X, _saveSlotPosition.Y + (i + 1) * _saveSlotRectangle.Height);

                SaveSlotPanel panel = new SaveSlotPanel(saveFiles[i], this, graphics, content, _saveSlotPosition, LayerDepth);
                panel.Load();
            }
        }

        public override void Unload()
        {
            base.Unload();
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
