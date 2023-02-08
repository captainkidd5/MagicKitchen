using DataModels;
using EntityEngine.Classes.StageStuff;
using EntityEngine.ItemStuff;
using Globals.Classes;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes.ShadowStuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes;

namespace EntityEngine.Classes
{
    public abstract class EntityContainer : Component, ISaveable, ILightDrawable
    {

        internal List<Entity> Entities { get; set; }

        protected List<Entity> EntitiesToAdd { get; set; }

        internal virtual Entity GetEntity(string name) => Entities.FirstOrDefault(x => x.Name == name);



       
        protected StageManager StageManager { get; private set; }   

        public EntityContainer(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
          
        }

        public virtual void Initialize(StageManager stageManager)
        {
            StageManager = stageManager;
            Entities = new List<Entity>();
            EntitiesToAdd = new List<Entity>();

        }

        public virtual void Update(GameTime gameTime)
        {
            for (int i = Entities.Count - 1; i >= 0; i--)
            {
                Entity entity = Entities[i];
                entity.Update(gameTime);
                if (entity.FlaggedForRemoval)
                {
                    entity.SetToDefault();
                    Entities.RemoveAt(i);

                }
            }


            foreach (Entity entity in EntitiesToAdd)
                Entities.Add(entity);

            EntitiesToAdd.Clear();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity entity in Entities)
            {
                entity.Draw(spriteBatch);

            }
        }

        public virtual void DrawLights(SpriteBatch spriteBatch)
        {
            foreach (Entity entity in Entities)
            {
                entity.DrawLights(spriteBatch);

            }
        }


        public virtual void LoadSave(BinaryReader reader)
        {
            foreach (Entity entity in Entities)
            {
                entity.LoadSave(reader);
            }
        }

        public virtual void Save(BinaryWriter writer)
        {
            foreach (Entity entity in Entities)
            {
                entity.Save(writer);
            }

        }

        public virtual void SetToDefault()
        {
            foreach (Entity entity in Entities)
            {
                entity.SetToDefault();
            }
            Entities.Clear();
        }


    }
}
