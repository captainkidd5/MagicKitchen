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

        public void OnProgressDone() => ProgressDone?.Invoke();

        public float StartTime;
        public float CurrentProgress;
        public float ProgressRequired;
        public bool Done => CurrentProgress > ProgressRequired;

        public float Ratio => CurrentProgress / ProgressRequired;

        private bool _active;
        public void Start(int progressRequired)
        {
            ProgressRequired = progressRequired;
            StartTime = Clock.TotalTime;
            _active = true;
        }
        public void Update()
        {
            if (!_active)
                return;
            CurrentProgress = Clock.TotalTime - StartTime;

            if (Done)
            {
                OnProgressDone();

            }

        }

        private void Reset()
        {
            StartTime = 0;
            ProgressRequired = 0;
            _active = false;

        }
    }
}
