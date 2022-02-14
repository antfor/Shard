using System;
using System.Collections.Generic;


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

    class Buffer {
        private int id;
        private int size;

        public Buffer(int id, int size) {
            this.id = id;
            this.size = size;
        }

        public int Size { get => size; }
        public int ID { get => id; }
    }

    class UnLinkedProgram{
        private string vert;
        private string frag;

        public UnLinkedProgram(string vert , string frag) {
            this.vert = vert;
            this.frag = frag;
        }

        public string Vert { get => vert; }
        public string Frag { get => frag; }
    }

    class RenderManager
    {
        private static RenderManager me;
        private IRenderContext renderContext;
        private Dictionary<string, (Shader, string)> shaders = new Dictionary<string, (Shader, string)> { };

        private Dictionary<string, int> programs = new Dictionary<string, int> { };
        private Dictionary<string, UnLinkedProgram> unLinkedprograms = new Dictionary<string, UnLinkedProgram> { }; 

        private Dictionary<string, Buffer> buffers = new Dictionary<string, Buffer> { };
        private Dictionary<string, float[]> unLodedbuffers = new Dictionary<string, float[]> { };

        private SortedList<int, List<IRenderObject>> renderObjects = new SortedList<int, List<IRenderObject>>() { };

        private Dictionary<string , Uniform> uniforms = new Dictionary<string, Uniform> { };

        private bool initialized = false;

        private RenderManager() {

        }
                                    
        public static RenderManager getInstance() {
            if (me is null)
            {
                me = new RenderManager();
            }
            return me;
        }

        public void setRenderContext(IRenderContext con) {
            renderContext = con;
        }

        public void addShader(string id,string file, Shader type) {

            string source = FileManager.readFile(file);

            shaders.Add(id,(type,source));

        }

        private int compileShader(string shaderID) {

            (Shader, string) shaderInfo;

            if (!shaders.TryGetValue(shaderID, out shaderInfo)) {
                return -1;
            }
            Shader type = shaderInfo.Item1;
            string source = shaderInfo.Item2;

            int shader = GL.CreateShader(getShaderType(type));

            GL.ShaderSource(shader, source);
            GL.CompileShader(shader);

            string infoLogVert = GL.GetShaderInfoLog(shader);
            if (infoLogVert != System.String.Empty)
                PConsole.WriteLine(infoLogVert);

            return shader;
        }

        public void addProgram(string progID, string vertID, string fragID) {
            unLinkedprograms.Add(progID, new UnLinkedProgram(vertID,fragID));
        }

        private bool linkProgram(string progID) {

            if (programs.ContainsKey(progID)) {
                return false;
            }

            UnLinkedProgram unLinkedprogram;

            if (!unLinkedprograms.TryGetValue(progID, out unLinkedprogram)) {
                return false;
            }


            int VertexShader = compileShader(unLinkedprogram.Vert);
            int FragmentShader = compileShader(unLinkedprogram.Frag);

            if (VertexShader == -1 || FragmentShader == -1) {
                return false;
            }

            int prog = GL.CreateProgram();
            GL.AttachShader(prog, VertexShader);
            GL.AttachShader(prog, FragmentShader);


            GL.LinkProgram(prog);

            GL.DetachShader(prog, VertexShader);
            GL.DetachShader(prog, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);


            string infoLogProg = GL.GetProgramInfoLog(prog);
            if (infoLogProg != System.String.Empty)
                PConsole.WriteLine(infoLogProg);

            unLinkedprograms.Remove(progID);

            return programs.TryAdd(progID, prog);
        }
        public void addArrayBuffer(string bufferID, float[] data) {
           unLodedbuffers.Add(bufferID, data);
        }

        private bool loadArrayBuffer(string id) {

            if (buffers.ContainsKey(id)) {
                return false;
            }

            float[] arr;

            if (!unLodedbuffers.TryGetValue(id, out arr)) {
                return false;
            }


            int vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            int buffer;
            GL.GenBuffers(1,out buffer);
            GL.BindBuffer(OGL.BufferTarget.ArrayBuffer, buffer);
            
            GL.BufferData(OGL.BufferTarget.ArrayBuffer, sizeof(float) * arr.Length, arr, OGL.BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, OGL.VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindVertexArray(0);
            // GL.BindBuffer(OGL.BufferTarget.ArrayBuffer, 0);

            unLodedbuffers.Remove(id);

            return buffers.TryAdd(id, new Buffer(vao,arr.Length));
        }

        private int useProgram(string progID) {
            int prog;
            if (!programs.TryGetValue(progID, out prog)) {

                if (linkProgram(progID))
                {
                    if (!programs.TryGetValue(progID, out prog))
                        return -1;
                }
                else {
                    return -1;
                }
            }

            GL.UseProgram(prog);


            return prog;
        }

        private Buffer getBuffer(string bufferID) {
            Buffer buffer;
            if (!buffers.TryGetValue(bufferID, out buffer)) {
                if (loadArrayBuffer(bufferID)) {
                    return getBuffer(bufferID);
                }
                else
                {
                    throw new Exception("buffer not added");
                }
            }

            return buffer;
        }

  
        private void render(string progID, string bufferID) {

            int prog = useProgram(progID);

            Buffer buffer = getBuffer(bufferID);

            GL.BindVertexArray(buffer.ID);

            //obj uniforms
            Uniforms.setUniform(0,new Uniform<float>("hej",0));
            //gen uniforms

            GL.DrawArrays(OGL.PrimitiveType.Triangles,0, 3);
            string error = GL.GetError().ToString();

        }

        public void update() {


            if (!initialized) {
                initialized = true;
            }

            updateUniforms();

            foreach (List<IRenderObject> list in renderObjects.Values)
            {
                foreach (IRenderObject obj in list)
                {
                    render(obj.Program, obj.Buffer);

                }
            }
        }

        private void updateUniforms()
        {
            Bootstrap.getDeltaTime();
            
        }

        public int addRenderObject(IRenderObject obj, int prio)
        {

            List<IRenderObject> list;
            if (renderObjects.TryGetValue(prio, out list))
            {
                list.Add(obj);
            }
            else
            {
                renderObjects.Add(prio, new List<IRenderObject> { obj });
            }

            return prio;
        }

        public bool removeRenderObject(IRenderObject obj, int prio)
        {

            List<IRenderObject> list;

            if (renderObjects.TryGetValue(prio, out list))
            {

                return list.Remove(obj);
            }

            return false;
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
