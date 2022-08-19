using DataModels.ItemStuff;
using EntityEngine.Classes.CharacterStuff;
using EntityEngine.Classes.NPCStuff;
using ItemEngine.Classes;
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
    internal class OrderingFoodBehaviour : PBehaviourBase
    {

        private Direction _directionSeated; 
        //The item placed in front of the npc, not neccssarily what they want
        private PlacedOnItem _placedItemGiven;

        //The item the npc actually wants
        private ItemData _desiredItem;
        public OrderingFoodBehaviour(BehaviourManager behaviourManager, PatronBehaviourManager patronBehaviour, DiningTable diningTable, Direction directionSeatedAt,
            NPC entity, StatusIcon statusIcon, TileManager tileManager, float? timerFrequency) :
            base(behaviourManager,patronBehaviour, diningTable, entity, statusIcon, tileManager, timerFrequency)
        {
            _directionSeated = directionSeatedAt;

        }



        private ItemData DecideWhatToEat()
        {

            //TODO: Have npc pick from an available recipe

            return ItemFactory.GetItemData("Stone");
        }
        public override void Update(GameTime gameTime, ref Vector2 velocity)
        {
            base.Update(gameTime, ref velocity);
            if(_placedItemGiven == null)
            {
                _placedItemGiven = TableSeatedAt.GetPlacedItemFromSeatedDirection(_directionSeated);
            }
            if (_desiredItem == null)
            {
                _desiredItem = DecideWhatToEat();

                StatusIcon.SetStatusRequestFood(_desiredItem.Id);

            }

            //Item is placed at seat
            if(_placedItemGiven.ItemId > 0)
            {
                //Patron has the item they want in front of them
                if (_placedItemGiven.ItemId == _desiredItem.Id)
                {
                    StatusIcon.SetStatus(StatusIconType.Happy);
                    PatronBehaviourManager.ChangePatronStateToEating(TableSeatedAt, _directionSeated);
                }
                //else they have the wrong item in front of them
                else
                {
                    StatusIcon.SetStatus(StatusIconType.Unhappy);

                }
            }          

        }
    }
}
