﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    public interface IThread {
        public void runMethod();
    }
    /*
    public delegate void Callback<T>(T reult);

    public interface IThread<T> : IThread where T : struct
    {
        public void addCallBack(Callback<T> callback);
    }
    */
}
