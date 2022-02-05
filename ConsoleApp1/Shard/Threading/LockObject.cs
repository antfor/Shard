using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Threading
{
    public class LockObject
    {

        protected readonly object objLock = new object();
        protected bool locked;

        public LockObject(bool l)
        {

            if (l)
                getLock();

        }

        public void getLock()
        {
            bool islocked = false;
            System.Threading.Monitor.Enter(objLock, ref islocked);
            locked = islocked;
        }

        public void releseLock()
        {
            lock (objLock)
            {
                if (locked)
                    System.Threading.Monitor.Exit(objLock);
            }
        }
    }
}
