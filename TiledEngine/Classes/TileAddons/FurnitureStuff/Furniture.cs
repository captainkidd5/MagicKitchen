using DataModels.ItemStuff;
using DataModels.MapStuff;
using InputEngine.Classes;
using InputEngine.Classes.Input;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;
using UIEngine.Classes;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Dynamics;
using static DataModels.Enums;

namespace TiledEngine.Classes.TileAddons.FurnitureStuff
{
    public class Furniture : LocateableTileAddon
    {

        protected FurnitureData FurnitureData { get; private set; }

        public Furniture(FurnitureData furnitureData, Tile tile, TileManager tileManager,
            IntermediateTmxShape intermediateTmxShape, string actionType) :
            base(tile, tileManager, intermediateTmxShape, actionType)
        {
            Key = "furniture";
            SubKey = this.GetType().Name;   
            FurnitureData = furnitureData;

        }


        public override void Load()
        {
            base.Load();
  


        }

 

        
        public static Furniture GetFurnitureFromProperty(string value,
            Tile tile, TileManager tileManager, IntermediateTmxShape tmxShape)
        {
            string[] parsedValue = value.Split(',');
            FurnitureType furnitureType = (FurnitureType)Enum.Parse(typeof(FurnitureType), parsedValue[0]);
            FurnitureData data = TileLoader.FurnitureLoader.FurnitureData[furnitureType];

            if (parsedValue.Length > 1)
            {
                data.StorageRows = int.Parse(parsedValue[1]);
                data.StorageColumns = int.Parse(parsedValue[2]);
            }


            Furniture furniture = (Furniture)System.Reflection.Assembly.GetExecutingAssembly()
                           .CreateInstance($"TiledEngine.Classes.TileAddons.FurnitureStuff.{parsedValue[0]}", true, System.Reflection.BindingFlags.CreateInstance,
                           null, new object[] { data, tile, tileManager, tmxShape, "None" }, null, null);

            return furniture;

        }


        protected override void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnCollides(fixtureA, fixtureB, contact);
        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnSeparates(fixtureA, fixtureB, contact);
            if (fixtureB.CollisionCategories.HasFlag(
                VelcroPhysics.Collision.Filtering.Category.FrontalSensor))
            {
                UI.DeactivateSecondaryInventoryDisplay();
            }
        }
    }
}
