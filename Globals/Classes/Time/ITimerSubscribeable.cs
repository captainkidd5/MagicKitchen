using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals.Classes.Time
{
    public interface ITimerSubscribeable
    {
        public void TimerFrameChanged(object sender, EventArgs args);
    }
}
