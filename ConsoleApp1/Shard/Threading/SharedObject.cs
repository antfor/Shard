using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Threading
{


    public class SharedObject<T> : LockObject
    {
        private T obj;
        

        public SharedObject(T obj, bool l) : base(l)
        {
            setObject(obj);
         
        }

        public SharedObject(bool l):base(l)
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

    }
}
