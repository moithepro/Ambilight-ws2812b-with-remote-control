using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ControlGuiLed
{
    public class UpdateTimer
    {
        private Action<object> callback;
        private Thread exceutionThread;
        private int dueTime;
        public int Interval { get; set; }

        

        public UpdateTimer(Action<object> callback)
        {
            this.callback = callback;
            
        }

        public void Change(int dueTime, int period)
        {
            Interval = period;
            this.dueTime = dueTime;
            if (exceutionThread != null && exceutionThread.IsAlive)
            {
                exceutionThread.Abort();
            }
            exceutionThread = new Thread(run);
            exceutionThread.Start();
        }
        private void run()
        {
            if (dueTime >= 0)
            {
                Thread.Sleep(dueTime);
            }
            else
            {
                return;
            }
            while (true)
            {
                callback(null);
                if (Interval > 0)
                {
                    Thread.Sleep(Interval);
                }
                else if (Interval == 0)
                {

                }
                else {
                    return;
                }

            }
        }

    }
}
