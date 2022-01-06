using Globals.Classes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using static Globals.Classes.Settings;

namespace SpriteEngine.Classes
{

    public static class SpriteUtility
    {


        public static float LayerMultiplier = .00001f;
        public static float defaultForeGroundLayer = .3f;

        public static float GetYAxisLayerDepth(Vector2 position, Rectangle sourceRectangle)
        {
            float depth = defaultForeGroundLayer + (position.Y + sourceRectangle.Height) * LayerMultiplier;

            return depth;
        }
        /// <summary>
        /// Returns a float value which is at least slightly larger than the given layerDepth, and at most .099 greater than the value.
        /// If a dictionary is provided, it will make sure that the value is not already contained within the dictionary.
        /// </summary>
        /// <param name="dictionary">optional dictionary to search through.</param>
        internal static float GetSpriteVariedLayerDepth(Layers layerDepths, Dictionary<string, float> dictionary = null)
        {
            float variedLayerDepth = GetLayerDepth(layerDepths) + Settings.Random.Next(1, 999) * LayerMultiplier;
            if (dictionary != null)
            {
                if (dictionary.ContainsValue(variedLayerDepth))
                {
                    return GetSpriteVariedLayerDepth(layerDepths, dictionary);
                }
                return variedLayerDepth;
            }
            return variedLayerDepth;
        }

        public static float GetUILayer(Layers layer)
        {
            return (int)layer * .01f;
        }
    }
}
