using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK;

using Shard.Misc;

using GL = OpenTK.Graphics.OpenGL4.GL;
using GLSaderType = OpenTK.Graphics.OpenGL4.ShaderType;
using OGL = OpenTK.Graphics.OpenGL4;

namespace Shard.Graphics.OpenGL.Rendering
{

    public enum Shader { 

    Vertex,
    Fragment

    }

    class RenderManager
    {
        private static RenderManager me;
        private IRenderContext renderContext;
        private Dictionary<string, (Shader, int)> shaders = new Dictionary<string, (Shader, int)> { };
        private Dictionary<string, int> programs = new Dictionary<string, int> { };
        private Dictionary<string, int> buffers = new Dictionary<string, int> { };

        private RenderManager() {

        }

        private RenderManager getInsance() {
            if (me is null)
            {
                me = new RenderManager();
            }
            return me;
        }

        public void setRenderContext(IRenderContext con) {
            renderContext = con;
        }

        public void addShader(string file, Shader type) {

            string source = FileManager.readFile(file);

            int shader = GL.CreateShader(getShaderType(type));

            GL.ShaderSource(shader, source);
            GL.CompileShader(shader);

        }

        public void addProgram(string vertID, string fragID) { 
            
        
        }

        public bool loadArrayBuffer(string id, float[] arr) {

            if (buffers.ContainsKey(id)) {
                return false;
            }

            int buffer;
            GL.GenBuffers(1,out buffer);
            GL.BindBuffer(OGL.BufferTarget.ArrayBuffer, buffer);
            
            GL.BufferData(OGL.BufferTarget.ArrayBuffer, arr.Length, arr, OGL.BufferUsageHint.StaticDraw);

            buffers.Add(id, buffer);

            GL.BindBuffer(OGL.BufferTarget.ArrayBuffer, -1);

            return true;
        }

        public void update() { 
        
        }

        private GLSaderType getShaderType(Shader shader) {

            switch (shader) {
                case Shader.Vertex:   return GLSaderType.VertexShader;
                case Shader.Fragment: return GLSaderType.FragmentShader;
            }
            throw new Exception("shader is not implemented ");
        }
    }
}
