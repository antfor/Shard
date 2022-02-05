using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Shard
{
    public class ThreadManager
    {

        private static ThreadManager me;

        public readonly string Main = "main";

        private Dictionary<string, Thread> threads = new Dictionary<string, Thread> { };

        private Dictionary<string, Barrier> barriers = new Dictionary<string,Barrier>{};

        private ThreadManager() {

        }

        public static ThreadManager getInstance() {
            if (me is null) {
                me = new ThreadManager();
            }
            return me;
        }

        public bool addThread(string id, IThread thread) {

            if (id == "main") {
                return false;
            }
            Thread InstanceCaller = new Thread(new ThreadStart(thread.runMethod));
            InstanceCaller.IsBackground = true;
            threads.Add(id,InstanceCaller);
            return true;
        }

        public bool removeThread(string id)
        {
            Thread thread;

            if (threads.TryGetValue(id, out thread)) {

                if (thread.IsAlive) {
                    thread.Abort();
                }

                threads.Remove(id);
                return true;
            }
            return false;
        }

        public bool runThread(string id) {

            Thread thread;

            if (threads.TryGetValue(id, out thread))
            {
                if (!thread.IsAlive) {
                    thread.Start();
                    return true;
                }
            }
            return false;
        }

        
        public bool join (string id) {

            Thread thread;

            if (threads.TryGetValue(id, out thread))
            {
                thread.Join();
                
                return removeThread(id); ;
            }
            return false;
        }


        public bool setPriority(string id, ThreadPriority prio) {
            Thread thread;

            if (threads.TryGetValue(id, out thread))
            {
                thread.Priority = prio;
                return true;
            }
            return false;
        }

        public void addBarrier(string id, int participants) {
            barriers.Add(id,new Barrier(participants));
        }

        public void removeBarrier(string id, int participants)
        {
            Barrier barrier;
            if (barriers.TryGetValue(id, out barrier))
            {
                barrier.Dispose();
                barriers.Remove(id);
            }
        }

        public void sync(string id) {

            Barrier barrier;
            if (barriers.TryGetValue(id,out barrier)) {
                barrier.SignalAndWait();
            }
            
        }

    }
}
