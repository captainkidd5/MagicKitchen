using Globals.Classes;
using ItemEngine.Classes;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityEngine.Classes
{
    public class EntityContainer : Component, ISaveable
    {

        public List<Entity> Entities { get; set; }
        public EntityContainer(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            Entities = new List<Entity>();

        }

        public void GiveEntityItem(string entityName, WorldItem worldItem)
        {
            Entity entity = Entities.FirstOrDefault(x => x.Name == entityName);
            if (entity == null)
            {
                throw new Exception($"Could not find entity with name {entityName}");
            }
            entity.GiveItem(worldItem);
        }

        public void LoadSave(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public void Save(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
