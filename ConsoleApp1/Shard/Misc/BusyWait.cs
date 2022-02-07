using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shard.Misc
{

    public class BusyWait
    {

        private static int timeToBeBusy = 10;
        public static void SmartWait(int ms) {
            
            long Startticks = Stopwatch.GetTimestamp();
            int sleep = ms - timeToBeBusy;
            if (sleep > 0)
                Thread.Sleep(sleep);
      
            long msLeft = ms - (Stopwatch.GetTimestamp() - Startticks) / (Stopwatch.Frequency / 1000);
            BusyWaitHz(msLeft, 1000);
        }

        public static void BusyWaitMS(int ms) {
            BusyWaitHz(ms, 1000);
        }

        public static void BusyWaitHz(long time, long hertz)
        {
            long Startticks = Stopwatch.GetTimestamp();
            long freq = Stopwatch.Frequency;

            if (freq < hertz) {
                long dif = hertz / freq;
                time /= dif;
                hertz /= dif;
            }

            long totalTime = 0;
            while (time > totalTime)
            {
                totalTime = (Stopwatch.GetTimestamp() - Startticks) / (freq / hertz);
            }

        }

    }
}
