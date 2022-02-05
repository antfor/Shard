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

        private static Stopwatch stopWatch = new Stopwatch();
        public static void smartWait(int ms) {
            long Startticks = Stopwatch.GetTimestamp();
            if (ms > 25)
                Thread.Sleep(ms - 10);
      
            long msLeft = (Stopwatch.GetTimestamp() - Startticks) / (Stopwatch.Frequency / 1000);
            BusyWaitPre(msLeft * 1000, 1000000);
        }
        public static void BusyWaitMS(int ms) {
            BusyWaitNano(ms* 1000);
        }

        public static void BusyWaitNano(int ns)
        {
            BusyWaitPre(ns, 1000000);
        }

        public static void BusyWaitPre(long PreS, long pre)
        {
            long Startticks = Stopwatch.GetTimestamp();
            long freq = Stopwatch.Frequency;
            long totalTime = 0;
            while (PreS > totalTime)
            {
                totalTime = (Stopwatch.GetTimestamp() - Startticks) / (freq / pre);
        
            }

        }

    }
}
