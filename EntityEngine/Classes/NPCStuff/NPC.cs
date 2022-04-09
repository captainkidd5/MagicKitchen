using DataModels;
using EntityEngine.Classes.Animators;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Dynamics;

namespace EntityEngine.Classes.NPCStuff
{
    internal class NPC : Entity
    {
        private readonly NPCData _npcData;

        public NPC(GraphicsDevice graphics, ContentManager content, NPCData npcData) : base(graphics, content)
        {
            _npcData = npcData;
        }

        public override void LoadContent(ItemManager itemManager)
        {
            base.LoadContent(itemManager);
            EntityAnimator = new NPCAnimator()
        }

        protected override void CreateBody(Vector2 position)
        {
            base.CreateBody(position);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

     

      

      

        protected override void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnCollides(fixtureA, fixtureB, contact);
        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnSeparates(fixtureA, fixtureB, contact);
        }

        protected override void UpdateBehaviour(GameTime gameTime)
        {
            base.UpdateBehaviour(gameTime);
        }
    }
}
