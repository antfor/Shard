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

        private Dictionary<string, (IThread, Thread)> threads = new Dictionary<string, (IThread, Thread)> { };

        private ThreadManager() {

        }

        public static ThreadManager getInstance() {
            if (me is null) {
                me = new ThreadManager();
            }
            return me;
        }

        public void addThread<T>(string id, IThread<T> thread) where T : struct {

            //todo
            Thread InstanceCaller = new Thread(new ThreadStart(thread.runMethod));
            InstanceCaller.IsBackground = true;
            threads.Add(id, (thread, InstanceCaller));
        }

        public bool removeThread(string id)
        {
            (IThread, Thread) thread;

            if (threads.TryGetValue(id, out thread)) {

                if (thread.Item2.IsAlive) {
                    thread.Item2.Abort();
                }

                threads.Remove(id);
                return true;
            }
            return false;
        }

        public bool runThread(string id) {

            (IThread, Thread) thread;

            if (threads.TryGetValue(id, out thread))
            {
                if (!thread.Item2.IsAlive) {
                    thread.Item2.Start();
                    return true;
                }
            }
            return false;
        }


        /*
        private Maybe<T> waitForCallBack<T>(string id) //where T : struct
        {


            (IThread, Thread) thread;

            if (threads.TryGetValue(id, out thread))
            {
                 IThread<thread.Item1.GetType> value = (IThread<thread.Item1.GetType>)thread.Item1;

                return new Maybe<T>();
            }

            return new Maybe<T>();
        }
        */
        
        public bool join (string id) {

            (IThread, Thread) thread;

            if (threads.TryGetValue(id, out thread))
            {
                thread.Item2.Join();
                
                return removeThread(id); ;
            }
            return false;
        }


        public bool setPriority(string id, ThreadPriority prio) {
            (IThread, Thread) thread;

            if (threads.TryGetValue(id, out thread))
            {
                thread.Item2.Priority = prio;
                return true;
            }
            return false;
        }

    }
}
