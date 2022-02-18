using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shard.Graphics.OpenGL.Rendering;



namespace Shard.DemoGame.Floor
{
    class Floor: GameObject    
    {

        private RenderObject robj;
        private Uniform<float> scale;
        private Uniform<float> size;
        private string floorProgramID = "floor_prog";
        private string floorBufferID = "floor_buffer";
        private string flootVertID = "floorVert";
        private string flootFragID = "floorFrag";
        private RenderManager rm;

        public Floor() {


        }

        public override void initialize()
        {
            rm = RenderManager.getInstance();
            robj = this.RenderObject;

            scale = new Uniform<float>("scale", 300);
            size = new Uniform<float>("size", 1);

            robj.addUniform(scale);
            robj.addUniform(size);

            float[] vertices = {
            // first triangle
             1f,  0, 1f,  // top right
             1f, 0, -1f,  // bottom right
            -1f,  0, 1f,  // top left 
            // second triangle
             1f, 0, -1f,  // bottom right
            -1f, 0, -1f,  // bottom left
            -1f, 0, 1f   // top left
            };

            //this.Transform3D.rotateDeg(90, 0, 0);
            this.Transform3D.scale(scale.Value,scale.Value, scale.Value);

            rm.addArrayBuffer(floorBufferID, vertices);
            rm.addShader(flootVertID, @"D:\chalmers\tda572\shard\1.0.0\Shard\ConsoleApp1\DemoGame\Floor\floor.vert", Shader.Vertex);
            rm.addShader(flootFragID, @"D:\chalmers\tda572\shard\1.0.0\Shard\ConsoleApp1\DemoGame\Floor\floor.frag", Shader.Fragment);
            rm.addProgram(floorProgramID, flootVertID, flootFragID);

            robj.changePrio(3);
            robj.render(floorProgramID, floorBufferID);
        }

    }
}
