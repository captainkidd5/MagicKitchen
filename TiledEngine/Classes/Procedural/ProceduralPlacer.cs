using DataModels.MapStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiledEngine.Classes.Procedural
{
    internal class ProceduralPlacer
    {
        private PoissonSampler s_poissonSampler;

        private List<PoissonData> _poissonData; 
        public ProceduralPlacer()
        {
        }

        public void Load()
        {
            s_poissonSampler = new PoissonSampler(4, 12);

        }
    }
}
