using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.IO
{
    class InputOpenGL : InputSystem
    {
        double tick, timeInterval;
        public override void getInput()
        {
            tick += Bootstrap.getDeltaTime();

            if (tick < timeInterval)
            {
                return;
            }

            while (tick >= timeInterval)
            {
                tick -= timeInterval;
            }
        }


        public override void initialize()
        {
            tick = 0;
            timeInterval = 1.0 / 60.0;
        }
    }
}
