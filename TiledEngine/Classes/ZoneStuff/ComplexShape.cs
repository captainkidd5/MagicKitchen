using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiledEngine.Classes.ZoneStuff
{
    internal class ComplexShape : Zone
    {
        public ComplexShape(string name, string value, Rectangle rectangle, string stageName) :
            base(name, value, rectangle, stageName)
        {
        }
    }
}
