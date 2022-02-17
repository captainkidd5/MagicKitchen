using EntityEngine.Classes.NPCStuff;
using EntityEngine.Classes.PlayerStuff;
using Globals.Classes;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityEngine.Classes
{
    public class EntityManager : Component
    {
        private CharacterContainer _characterManager;
        private PlayerContainer _playerManager;
        private List<EntityContainer> _containers;

        public EntityManager(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            _characterManager = new CharacterContainer(graphics, content);
            _playerManager = new PlayerContainer(graphics, content);

        }
    }

}
}
