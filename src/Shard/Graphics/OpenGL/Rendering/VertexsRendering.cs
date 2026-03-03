using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Threading;

namespace Shard.Graphics.OpenGL.Rendering
{


    class VertexsRendering {

        private float[] vertices =  {
                               -0.5f, -0.5f, 0.0f,
                               0.5f, -0.5f, 0.0f,
                               0.0f,  0.5f, 0.0f };

        protected float[] Vertices { get => vertices; set => vertices = value; }


        protected void initBuffer() {
            uint VBO;
           // lGenBuffers(1, &VBO);

        }


    }
}
