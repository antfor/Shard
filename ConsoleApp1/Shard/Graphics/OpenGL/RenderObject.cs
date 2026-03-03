using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;
using Shard.Graphics.OpenGL.Rendering;

namespace Shard
{
    class RenderObject
    {
        private GameObject parent;
        private string progID;
        private string bufferID;
        private string vboID;
        private bool added;
        List<Uniform> uniforms = new List<Uniform> { };
        RenderManager rm;
        private int prio;

        public RenderObject(GameObject p) {
            parent = p;
            added = false;
            rm = RenderManager.getInstance();
            prio = 0;
        }

        public RenderObject()
        {
            added = false;
            rm = RenderManager.getInstance();
            prio = 0;
        }

        public void changePrio(int np) {
            if (prio != np) {
                if (added) {
                    stopRender();
                    render(progID, bufferID);
                }
            }
        }

        public void render(string prog, string buffer) {
            render(prog, buffer, buffer);
        }

        public void render(string prog, string vbo, string buffer) {
            progID   = prog;
            vboID    = vbo;
            bufferID = buffer;
            if (!added) {
                rm.addRenderObject(this, prio);
                added = true;
            }
        }

        public void stopRender() {
            if (added) {
                rm.removeRenderObject(this, 0);
                added = false;
            }
        }

        public void addUniform(Uniform uniform) {
            uniforms.Add(uniform);
        }

        public void removeUniform(Uniform uniform)
        {
            uniforms.Remove(uniform);
        }

        public string Program { get => progID; }
        public string Buffer { get => bufferID; }
        public string VBO      { get => vboID;    }


        public virtual Matrix4 getModelMatrix() {
            return parent.Transform3D.getModelMatrix();
        }

        public void setUniforms(int prog)
        {
            foreach (Uniform uniform in uniforms)
            {
                Uniforms.setUniform(prog, uniform);
            }
            
        }
    }
}
