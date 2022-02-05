using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Shard.Threading;
using Shard.Misc;

namespace Shard
{
    public class ThreadManager
    {

        private static ThreadManager me;

        public readonly string Main = "main";

        private static Thread main;

        private Dictionary<string, Thread> threads = new Dictionary<string, Thread> { };

        private Dictionary<string, Barrier> barriers = new Dictionary<string,Barrier>{};

        private Dictionary<string, Semaphore> semaphores = new Dictionary<string, Semaphore> { };

        private Dictionary<string, LockObject> locks = new Dictionary<string, LockObject> { };

        private ThreadManager() {

        }

        public static ThreadManager getInstance() {
            if (me is null) {
                me = new ThreadManager();
            }
            return me;
        }

        public void setMain(Thread t) {
            main = t;
            main.Priority = ThreadPriority.Highest;
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

        // barrier

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

        //Sem
        public void addSem(string id)
        {
            addSem(id,0);
        }

        public void addSem(string id, int init) {
            addSem(id, init, Int32.MaxValue);
        }

        public bool addSem(string id, int init, int max) {
            if (!semaphores.ContainsKey(id)) {
                semaphores.Add(id, new Semaphore(init, max));
                return true;
            }
            return false;
        }

        public void deleteSem(string id) {
            Semaphore sem;

            if (semaphores.TryGetValue(id, out sem))
            {
                sem.Dispose();

                removeThread(id); 
            }
        }

        public void waitOne(string id) {
            Semaphore sem;

            if (semaphores.TryGetValue(id, out sem))
            {
                sem.WaitOne();
            }
        }
        public void release(string id)
        {
            release(id, 1);

        }
        public void release(string id, int num)
        {
            Semaphore sem;

            if (semaphores.TryGetValue(id, out sem))
            {
                sem.Release(num);
            }
        }


        //lock
        public bool addLock(string id, bool lockNow) {

            if (!locks.ContainsKey(id))
            {
                locks.Add(id, new LockObject(lockNow));
                return true;
            }

            return false;
        }

        public bool addSharedObject<T>(string id, T obj, bool lockNow) {
            if (!locks.ContainsKey(id))
            {
                locks.Add(id, new SharedObject<T>(obj,lockNow));
                return true;
            }

            return false;
        }
        
        public bool deleteLock(string id) {
            return locks.Remove(id);
        }

        public void getLock(string id) {
            LockObject lockObj;
            if (locks.TryGetValue(id, out lockObj)) {
                lockObj.getLock();
            }
        }

        public void releseLock(string id)
        {
            LockObject lockObj;
            if (locks.TryGetValue(id, out lockObj))
            {
                lockObj.releseLock();
            }
        }

        public Maybe<T> getObject<T>(string id) {

            LockObject lockObj;
            if (locks.TryGetValue(id, out lockObj)) {
                try {
                    T obj = ((SharedObject<T>)lockObj).getObject();
                    return new Maybe<T>(obj);
                } catch
                {
                    return new Maybe<T>();
                }
            }
            return new Maybe<T>();
        }
        public bool setObject<T>(string id, T obj) {

            LockObject lockObj;
            if (locks.TryGetValue(id, out lockObj))
            {
                try
                {
                    ((SharedObject<T>)lockObj).setObject(obj);

                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

    }
}
