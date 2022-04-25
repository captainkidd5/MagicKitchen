using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiledEngine.Classes.TileAddons.Furniture
{
    public class DiningTable : ITileAddon
    {
        public Tile Tile => throw new NotImplementedException();

        public int TotalSeatingCapacity { get; private set; } = 4;
        public int CurrentSeatingCapacity { get; internal set; }
        public DiningTable()
        {

        }
      
        public void Load()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public void Interact(bool isPlayer)
        {
            throw new NotImplementedException();
        }


        public void CleanUp()
        {
            throw new NotImplementedException();
        }

    }
}
