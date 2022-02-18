using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shard.DemoGame;
using Shard.DemoGame.Floor;

namespace Shard
{
    class Demo : Game, InputListener
    {
        private Floor floor;
        private Test test;
        private SoundStatic soundPlayer;

        public void handleInput(InputEvent inp, string eventType)
        {
            
        }

        public override void initialize()
        {
            floor = new Floor();

            test = new Test();

            Bootstrap.getCamera().getTransform().setPos(0,1.8f,4);
        }

        public override void update()
        {
            
        }
    }
}
