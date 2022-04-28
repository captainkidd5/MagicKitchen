﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public enum CookAction
    {
        None = 0,
        Chop  = 1,
        Bake = 2
    }
    public class RecipeInfo
    {
        public string Name{ get; set; }
        //if true, player will not start out with this recipe unlocked
        public bool StartsLocked { get; set; }
        public CookAction CookAction { get; set; }
        public float CookTime { get; set; }
        public List<string> Ingredients { get; set; }
    }
}
