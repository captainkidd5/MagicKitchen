using DataModels.ItemStuff;
using EntityEngine.Classes.CharacterStuff;
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
    internal class OrderingFoodBehaviour : Behaviour
    {

        private DiningTable _tableSeatedAt;
        private Direction _directionSeated; 
        //The item placed in front of the npc, not neccssarily what they want
        private PlacedItem _placedItemGiven;

        //The item the npc actually wants
        private ItemData _desiredItem;
        public OrderingFoodBehaviour(DiningTable diningTableSeatedAt,Direction directionSeatedAt, Entity entity, StatusIcon statusIcon, Navigator navigator, TileManager tileManager, float? timerFrequency)
            : base(entity, statusIcon, navigator, tileManager, timerFrequency)
        {
            _tableSeatedAt = diningTableSeatedAt;
            _directionSeated = directionSeatedAt;
        }

        private ItemData DecideWhatToEat()
        {
            return ItemFactory.GetItemData("Stone");
        }
        public override void Update(GameTime gameTime, ref Vector2 velocity)
        {
            base.Update(gameTime, ref velocity);
            if(_placedItemGiven == null)
            {
                _placedItemGiven = _tableSeatedAt.GetPlacedItemFromSeatedDirection(_directionSeated);
            }

            //Item is placed at seat
            if(_placedItemGiven != null)
            {
                //Patron has the item they want in front of them
                if (_placedItemGiven.ItemId == _desiredItem.Id)
                    StatusIcon.SetStatus(StatusIconType.Speak);
                else
                    StatusIcon.SetStatus(StatusIconType.Speak);
            }
            

            if(_placedItemGiven.ItemId > 0)
                Console.WriteLine("test");
            StatusIcon.SetStatus(StatusIconType.WantFood);

        }

        public override void DrawDebug(SpriteBatch spriteBatch)
        {
            base.DrawDebug(spriteBatch);
        }
    }
}
