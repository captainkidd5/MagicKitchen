using Globals.Classes;
using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TiledEngine.Classes.ZoneStuff
{
    public class SpecialZone : ISaveable
    {
        public string PropertyName { get; private set; }

        public string Value { get; set; }
        public Rectangle Rectangle { get; private set; }

        public Vector2 Position => new Vector2(Rectangle.X, Rectangle.Y);
        public SpecialZone() : base()
        {

        }
        public SpecialZone(string name,string value, Rectangle rectangle)
        {
            PropertyName = name;
            Value = value;
            Rectangle = rectangle;
  
        }

    

        public void Save(BinaryWriter writer)
        {
          writer.Write(PropertyName);
            writer.Write(Value);
            RectangleHelper.WriteRectangle(writer, Rectangle);
        }

        public void LoadSave(BinaryReader reader)
        {
            PropertyName = reader.ReadString();
            Value = reader.ReadString();
            Rectangle = RectangleHelper.ReadRectangle(reader);
        }

        public void CleanUp()
        {
            //throw new NotImplementedException();
        }
    }
}
