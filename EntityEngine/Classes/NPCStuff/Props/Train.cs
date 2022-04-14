using DataModels;
using EntityEngine.Classes.Animators;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes;

namespace EntityEngine.Classes.NPCStuff.Props
{
    internal class Train : Entity
    {
        public Train(GraphicsDevice graphics, ContentManager content) : 
            base(graphics, content)
        {
            AnimationFrame[] frames = new AnimationFrame[2];
            frames[0] = new AnimationFrame(0, 0, 0, .25f);
            frames[1] = new AnimationFrame(1, 0, 0, .25f);
            AnimatedSprite trainSprite = SpriteFactory.CreateWorldAnimatedSprite(Position,
                new Rectangle(272, 144, 248, 80), EntityFactory.Props_1, frames);
            AnimatedSprite trainSprite1 = SpriteFactory.CreateWorldAnimatedSprite(Position,
               new Rectangle(272, 144, 248, 80), EntityFactory.Props_1, frames);
            AnimatedSprite trainSprite2 = SpriteFactory.CreateWorldAnimatedSprite(Position,
               new Rectangle(272, 144, 248, 80), EntityFactory.Props_1, frames);
            AnimatedSprite trainSprite3 = SpriteFactory.CreateWorldAnimatedSprite(Position,
               new Rectangle(272, 144, 248, 80), EntityFactory.Props_1, frames);
            Animator = new NPCAnimator(this, new AnimatedSprite[4] { trainSprite,
                trainSprite1, trainSprite2, trainSprite3 }, 0, 0);
        }

        public override void LoadContent(ItemManager itemManager)
        {
            base.LoadContent(itemManager);
        }
        public override void Update(GameTime gameTime)
        {
            IsInStage = true;  
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void SwitchStage(string newStageName, TileManager tileManager, ItemManager itemManager)
        {
            CurrentStageName = newStageName;
            IsInStage = true;
            var zones = tileManager.GetZones("train");

            if(zones != null)
            {
                var zone = zones.FirstOrDefault(x => x.Value == "start");
                if(zone != null)
                    Move(zone.Position);

            }

            base.SwitchStage(newStageName, tileManager, itemManager);
            InjectScript(EntityFactory.GetSubscript("MoveTrain"));
        }




    }
}
