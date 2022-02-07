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

        
        public static void SmartWait(int ms) {
            
            long Startticks = Stopwatch.GetTimestamp();
            int sleep = ms - 10;
            if (sleep > 0)
                Thread.Sleep(ms - 10);
      
            long msLeft = ms - (Stopwatch.GetTimestamp() - Startticks) / (Stopwatch.Frequency / 1000);
            BusyWaitPre(msLeft, 1000);
        }

        public static void BusyWaitMS(int ms) {
            BusyWaitPre(ms, 1000);
        }

        public static void BusyWaitNS(int ns) {
            BusyWaitPre(ns/100, 10000000);
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
