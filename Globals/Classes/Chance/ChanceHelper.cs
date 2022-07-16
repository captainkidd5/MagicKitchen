using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals.Classes.Chance
{
    public static class ChanceHelper
    {

        /// <summary>
        /// Returns a single weighted element from given list.
        /// </summary>
        /// <returns></returns>
        public static IWeightable GetWheelSelection(List<IWeightable> myList, Random random)
        {
            int poolSize = 0;
            for (int i = 0; i < myList.Count; i++)
            {
                poolSize += myList[i].Weight;
            }

            // Get a random integer from 0 to PoolSize.
            int randomNumber = random.Next(0, poolSize) + 1;

            // Detect the item, which corresponds to current random number.
            int accumulatedProbability = 0;
            for (int i = 0; i < myList.Count; i++)
            {
                accumulatedProbability += myList[i].Weight;
                if (randomNumber <= accumulatedProbability)
                    return myList[i];
            }
            return null;
        }

        /// <summary>
        /// Returns new list with weighted elements selected from dice roll based on probability of weight
        /// </summary>
        /// <returns></returns>
        public static List<IWeightable> GetWeightedSelection(List<IWeightable> myList, Random random)
        {
            List<IWeightable> weightedList = new List<IWeightable>();
            foreach(IWeightable weightable in myList)
            {
                if(random.Next(1, 101) < weightable.Weight)
                    weightedList.Add(weightable);
            }
            return weightedList;
        }

        public static float RandomFloat(float min, float max)
        {
            return (float)(Settings.Random.NextDouble() * (max - min)) + min;
        }
    }
}
