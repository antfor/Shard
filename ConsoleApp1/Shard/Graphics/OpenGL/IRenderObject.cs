﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    public interface IRenderObject
    {
        string Program { get; }
        string Buffer { get; }
    }
}
