using DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Globals.Classes.Chance
{
    public static class WheelSelection
    {

        /// <summary>
        /// Returns a single weighted element from given list.
        /// </summary>
        /// <returns></returns>
        public static IWeightable GetSelection(List<IWeightable> myList, Random random)
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
    }
}
