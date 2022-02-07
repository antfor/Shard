using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Graphics.OpenGL.Rendering
{

    enum Shader { 

    Vertex,
    Fragment

    }

    class RenderManager
    {
        private static RenderManager me;


        private RenderManager() {

        }

        private RenderManager getInsance() {
            if (me is null)
            {
                me = new RenderManager();
            }
            return me;
        }

        public void addShaders() { 
        }

        public void addShader()
        {
        }

    }
}
