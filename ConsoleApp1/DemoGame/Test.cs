using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using Shard.Graphics.OpenGL.Rendering;
using Shard.Misc;
namespace Shard
{
    class Test : GameObject
    {
        float time = 0;
        float fullTime = 2;
        public override void initialize()
        {
            RenderManager rm = RenderManager.getInstance();

            float[] vertices = {
             -0.5f, -0.5f, 0.0f, //Bottom-left vertex
              0.5f, -0.5f, 0.0f, //Bottom-right vertex
              0.0f,  0.5f, 0.0f  //Top vertex
             };

            rm.addArrayBuffer("buffer", vertices);
            rm.addShader("vert", @"D:\chalmers\tda572\shard\1.0.0\Shard\ConsoleApp1\Shard\Graphics\OpenGL\Rendering\defult.vert", Shader.Vertex);
            rm.addShader("frag", @"D:\chalmers\tda572\shard\1.0.0\Shard\ConsoleApp1\Shard\Graphics\OpenGL\Rendering\defult.frag", Shader.Fragment);
            rm.addProgram("prog", "vert", "frag");

            RenderObject.render("prog", "buffer");

        }

        public override void update()
        {
            float dt = (float)Bootstrap.getDeltaTime();
            time += dt;
            if (time > fullTime) { time = time - fullTime; }
            float fullRot = (float)(2 * Math.PI);
            Transform3D.rotate(0, fullRot * dt / fullTime, 0);
         
            //PConsole.WriteLine(""+ dt / fullTime);
            Transform3D.setPos(0.0f, 1, 0.0f);
             //Transform3D.setScale(0.75f, 1,1);

           // PConsole.WriteLine(Transform3D.getScale().ToString());

        }
    }
}
