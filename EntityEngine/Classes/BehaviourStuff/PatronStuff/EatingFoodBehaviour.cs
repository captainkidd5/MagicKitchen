using DataModels.ItemStuff;
using EntityEngine.Classes.CharacterStuff;
using EntityEngine.Classes.NPCStuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes;
using TiledEngine.Classes.TileAddons.FurnitureStuff;
using static DataModels.Enums;

namespace EntityEngine.Classes.BehaviourStuff.PatronStuff
{
    internal class EatingFoodBehaviour : PBehaviourBase
    {
        //The item placed in front of the npc, not neccssarily what they want
        private PlacedOnItem _placedItemGiven;

        //The item the npc actually wants

        private bool _isEating;
        private readonly Direction _directionSeatedAt;

        public EatingFoodBehaviour(PatronBehaviourManager patronBehaviour, DiningTable diningTable, Direction directionSeatedAt,
            NPC entity, StatusIcon statusIcon, TileManager tileManager, float? timerFrequency) :
            base(patronBehaviour, diningTable, entity, statusIcon, tileManager, timerFrequency)
        {
            _directionSeatedAt = directionSeatedAt;
        }
        public override void Update(GameTime gameTime, ref Vector2 velocity)
        {
            base.Update(gameTime, ref velocity);

            if (!_isEating)
            {
                Entity.AddProgressBar();
                _isEating = true;
            }
            if (Entity.IsUsingProgressBar && Entity.IsProgressComplete())
            {
                Entity.RemoveProgressBar();
                TableSeatedAt.DeleteItemFromTable(_directionSeatedAt);
                TableSeatedAt.AddMoneyToCentralSlot(1);
                Entity.InjectScript(EntityFactory.GetSubscript("HeadToTrain"));
                //TODO: Change to leaving restaurant behaviour
            }

        }
        public override void DrawDebug(SpriteBatch spriteBatch)
        {
            base.DrawDebug(spriteBatch);
        }

        
    }
}
