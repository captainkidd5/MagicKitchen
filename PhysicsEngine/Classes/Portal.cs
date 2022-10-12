using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine.Classes
{
    public class Portal
    {

        public Portal(string from, string to)
        {
            From = from;
            To = to;
        }
        public string From { get; private set; }
        public string To { get; private set; }


        public static Portal GetPortal(ref string unparsedString)
        {
            string[] splitString = unparsedString.Split(',');
            string from = splitString[0];
            string to = splitString[1];

            unparsedString = from;
            return new Portal(from, to);



        }
    }
}
