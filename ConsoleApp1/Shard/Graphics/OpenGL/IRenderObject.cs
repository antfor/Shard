using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

namespace Shard
{
    public interface IRenderObject
    {
        string Program { get; }
        string Buffer { get; }

        public Matrix4 getModelMatrix();
    }
}
