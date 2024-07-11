using Haukcode.HighResolutionTimer;
using System;
using System.Windows.Forms.Design;

namespace ControlGuiLed
{
    public class UpdateTimer
    {
        public enum CallbackType
        {
            Color,
            Party,
            Rainbow,
            Ambilight
        }

        private CallbackType callbackType;
        private MainApp mainApp;
        private HighResolutionTimer timer;
        private Thread? thread;
        private bool threadDie = false;
        public int Interval { get; set; }

        public UpdateTimer(CallbackType callback, MainApp mainApp)
        {
            this.mainApp = mainApp;
            this.callbackType = callback;

        }

        public void Change(int dueTime, int period)
        {

            Interval = period;
            if (thread != null)
            {
                threadDie = true;
                thread.Join();
                threadDie = false;
                thread = null;
            }
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
            if (dueTime < 0 || period < 0)
            {
                return;
            }
            
            timer = new HighResolutionTimer();
            timer.SetPeriod(period);
            StartTimer();
            thread = new Thread(ExecuteCallback);
            thread.Start();
        }

        private void StartTimer()
        {
            timer.Start();
        }

        private void ExecuteCallback()
        {
            while (true)
            {
                switch (callbackType)
                {
                    case CallbackType.Color:
                        mainApp.ColorTimer_Tick();
                        break;
                    case CallbackType.Party:
                        mainApp.PartyTimer_Tick();
                        break;
                    case CallbackType.Rainbow:
                        mainApp.RainbowTimer_Tick();
                        break;
                    case CallbackType.Ambilight:
                        mainApp.AmbilightTimer_Tick();
                        break;
                    default:
                        break;
                }
                if (threadDie == true)
                    return;
                if (timer != null)
                {
                    if (threadDie == true)
                        return;
                    timer.WaitForTrigger();
                }
            }
        }
    }
}
