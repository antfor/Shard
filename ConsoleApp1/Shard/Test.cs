﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using Shard.Graphics.OpenGL.Rendering;

namespace Shard
{
    class Test : IRenderObject
    {
        public static string buffer = "tri";
        public static string program = "prog";

        public string Program => program;

        public string Buffer => buffer;

        public Matrix4 getModelMatrix()
        {
            throw new NotImplementedException();
        }
    }
}
