using Globals.Classes.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemEngine.Classes.CraftingStuff
{
    public delegate void ProgressDone();

    public class CraftedItemMetre
    {
        public event ProgressDone ProgressDone;


        public float StartTime;
        private int _oldProgress;
        public int CurrentProgress;
        public int ProgressRequired;
        private readonly FuelMetre _fuelMetre;

        public bool Done => CurrentProgress > ProgressRequired;

        public float Ratio => ProgressRequired > 0 ? ((float)CurrentProgress / (float)ProgressRequired) : 0;

        public bool Active {get; private set;}

        public int? IdCurrentlyMaking { get; private set; }
        public CraftedItemMetre(FuelMetre fuelMetre)
        {
            _fuelMetre = fuelMetre;
        }
        public void Start(int progressRequired, int idCurrentlyMaking)
        {
            ProgressRequired = progressRequired;
            IdCurrentlyMaking = idCurrentlyMaking;
            StartTime = Clock.TotalTime;
            _oldProgress = 0;

            Active = true;
        }
        public void Update()
        {
            if (!Active)
                return;
            CurrentProgress = (int)Clock.TotalTime - (int)StartTime;
            if(CurrentProgress - _oldProgress > 0)
            {
                _oldProgress = CurrentProgress;
                _fuelMetre.ConsumeFuel(1);
            }

            if (Done)
            {
                OnProgressDone();

            }

        }
        public void OnProgressDone()
        {
            ProgressDone?.Invoke();
            //Reset();
        }

        public void Reset()
        {
            IdCurrentlyMaking = null;
            StartTime = 0;
            ProgressRequired = 0;
            Active = false;
            _oldProgress = 0;

        }
    }
}
