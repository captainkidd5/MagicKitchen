using IOEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEngine.Classes.MainMenuStuff
{
    internal class ViewGamesMenu : InterfaceSection
    {

        public ViewGamesMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }

        public override void Load()
        {
            base.Load();
            List<SaveFile> saveFiles = SaveLoadManager.SaveFiles;
            foreach (SaveFile file in saveFiles)
            {
                SaveSlotPanel panel = new SaveSlotPanel(file, this, graphics, content, Position, LayerDepth);

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
