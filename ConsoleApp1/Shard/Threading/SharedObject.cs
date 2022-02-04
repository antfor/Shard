using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Threading
{

    class SharedObject<T> : ISharedObject
    {
        private T obj;
        private readonly object objLock = new object();
        private bool locked;

        public SharedObject(T obj)
        {
            setObject(obj);
        }

        public SharedObject()
        {
            
        }

        public void setObject(T obj) {

            try {
                getLock();

                this.obj = obj;
            }
            finally {
                releseLock();
            } 
        }

        public T getObject() {
            try
            {
                getLock();

                return obj;
            }
            finally
            {
                releseLock();
            }

        }

        public void getLock() {
            System.Threading.Monitor.Enter(objLock, ref locked);
        }

        public void releseLock() {
            lock (objLock) {
                if (locked)
                    System.Threading.Monitor.Exit(obj);
            }
        }

    }
}
