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
