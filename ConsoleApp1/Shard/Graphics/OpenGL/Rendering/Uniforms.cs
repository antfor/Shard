using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using GL = OpenTK.Graphics.OpenGL4.GL;

using Shard.Misc;

namespace Shard.Graphics.OpenGL.Rendering
{

    interface Uniform {
    }
    internal class Uniform<T>:Uniform  {
        protected T value;
        protected string name;

        public T Value { get => value; set => this.value = value; }
        public string Name { get => name; set => this.name = value; }

        internal Uniform(string ID, T value){
            this.value = value;
            this.name = ID;
        }

        internal Uniform(string ID)
        {
            this.name = ID;
        }

    }


    class UniformFactory{
        public static Uniform<float> createUniform(string name, float value) {
            return new Uniform<float>(name,value);
        }

        public static Uniform<bool> createUniform(string name, bool value)
        {
            return new Uniform<bool>(name, value);
        }

        public static Uniform<int> createUniform(string name, int value)
        {
            return new Uniform<int>(name, value);
        }

        public static Uniform<Vector3> createUniform(string name, Vector3 value)
        {
            return new Uniform<Vector3>(name, value);
        }

        public static Uniform<Vector2> createUniform(string name, Vector2 value)
        {
            return new Uniform<Vector2>(name, value);
        }

        public static Uniform<Vector4> createUniform(string name, Vector4 value)
        {
            return new Uniform<Vector4>(name, value);
        }

        public static Uniform<Matrix4> createUniform(string name, Matrix4 value)
        {
            return new Uniform<Matrix4>(name, value);
        }
    }
    class Uniforms
    {


        public static void setUniform(int program, Uniform uniform) {


            if (uniform is Uniform<float>)
            {
                setUniform(program, ((Uniform<float>)uniform).Name, ((Uniform<float>)uniform).Value);
            }
            else if (uniform is Uniform<bool>)
            {
                setUniform(program, ((Uniform<bool>)uniform).Name, ((Uniform<bool>)uniform).Value);
            }
            else if (uniform is Uniform<int>)
            {
                setUniform(program, ((Uniform<int>)uniform).Name, ((Uniform<int>)uniform).Value);
            }
            else if (uniform is Uniform<Matrix4>)
            {
                setUniform(program, ((Uniform<Matrix4>)uniform).Name, ((Uniform<Matrix4>)uniform).Value);
            }
            else if (uniform is Uniform<Vector4>)
            {
                setUniform(program, ((Uniform<Vector4>)uniform).Name, ((Uniform<Vector4>)uniform).Value);
            }
            else if (uniform is Uniform<Vector3>)
            {
                setUniform(program, ((Uniform<Vector3>)uniform).Name, ((Uniform<Vector3>)uniform).Value);
            }
            else if (uniform is Uniform<Vector2>)
            {
                setUniform(program, ((Uniform<Vector2>)uniform).Name, ((Uniform<Vector2>)uniform).Value);
            }
            else {
                throw new Exception("not valid type");
            }

        }

        public static void setUniform(int program, string name, bool value) {
            int Location = GL.GetUniformLocation(program, name);
            if (value)
            {
                GL.Uniform1(Location, 1);
            }
            else {
                GL.Uniform1(Location, 0);
            }

        }
        public static void setUniform(int program, string name, int value)
        {
            int Location = GL.GetUniformLocation(program, name);
            GL.Uniform1(Location, value);
        }

        public static void setUniform(int program, string name, float value)
        {
            int Location = GL.GetUniformLocation(program, name);
            GL.Uniform1(Location, value);
        }

        public static void setUniform(int program, string name, Vector2 value)
        {
            int Location = GL.GetUniformLocation(program, name);
            GL.Uniform2(Location, ref value);
        }

        public static void setUniform(int program, string name, Vector3 value)
        {
            int Location = GL.GetUniformLocation(program, name);
            GL.Uniform3(Location, ref value);
        }

        public static void setUniform(int program, string name, Vector4 value)
        {
            int Location = GL.GetUniformLocation(program, name);
            GL.Uniform4(Location, ref value);
        }

        public static void setUniform(int program, string name, Matrix4 value) {
            int Location = GL.GetUniformLocation(program, name);
            GL.UniformMatrix4(Location, false, ref value);
        }
    }
}
