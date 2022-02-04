using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Misc
{
    public class Maybe<T>
    {
        private T value;
        private bool nothing = true;


        public Maybe()
        {

        }
        public Maybe(T v)
        {
            setValue(v);
        }
        public bool isJust()
        {
            return !nothing;
        }

        public T fromJust()
        {
            return value;
        }

        public T fromMaybe(T defult)
        {
            if (nothing)
            {
                return defult;
            }
            return value;
        }

        public void setValue(T v)
        {
            value = v;
            nothing = false;
        }

    }
}
