using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;
using OpenTK.Audio.OpenAL;

using Shard.Sound;

namespace Shard
{
    public class SoundSource : SoundStatic
    {
        private Vector3 pos = new(0.0f, 0.0f, 0.0f);
        private Vector3 vel = new(0.0f, 0.0f, 0.0f);

        private ISoundSourceObject sourceObj = null;
        private bool gotSourceObject = false;


        public Vector3 Pos { get => pos; set => pos = value; }
        public Vector3 Vel { get => vel; set => vel = value; }

        public SoundSource(ISoundSourceObject obj)
        {
            setSourceObject(obj);
            init(2.0f);
        }

        public void setSourceObject(ISoundSourceObject obj)
        {
            sourceObj = obj;
            gotSourceObject = true;
            updateOBJ();

        }

        public SoundSource()
        {
            init(2.0f);
            
        }

        private void setPos()
        {
            AL.Source(id, ALSource3f.Position, ref pos);
        }

        private void setVel()
        {
            AL.Source(id, ALSource3f.Velocity, ref vel);
        }

        public override void update()
        {

            base.update();

            if (gotSourceObject)
            {
                updateOBJ();
            }

        }

        private void updateOBJ()
        {
            pos = sourceObj.getSoundPos();
            setPos();

            vel = sourceObj.getSoundVel();
            setVel();
        }
        
    }
}
