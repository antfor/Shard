using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;

namespace Shard
{
    public class SoundSource : Sound
    {
  
        private float gain = 1f;
        private bool gainChange = false;
        private float pitch = 1f;
        private bool pitchChange = false;
        private Vector3 pos = new (0.0f, 0.0f, 0.0f);
        private Vector3 vel = new (0.0f, 0.0f, 0.0f);
        private int id;
        private string sound;
        private bool canPlay = false;
        private ISoundSourceObject sourceObj = null;
        private bool gotSourceObject = false;
       


        public SoundSource(ISoundSourceObject obj) {
            setSourceObject(obj);
        }

        public void setSourceObject(ISoundSourceObject obj) {
            sourceObj = obj;
            gotSourceObject = true;
        }

        public SoundSource()
        {
     
        }

        internal int Id { get => this.id; set => this.id = value; }
        public float Gain { get => gain; set => gain = value; }

        public float Pitch { get => pitch; set => pitch = value;}
        public Vector3 Pos { get => pos; set => pos = value; }
        public Vector3 Vel { get => vel; set => vel = value; }

        private void setGain()
        {
            AL.Source(id, ALSourcef.Gain, gain);
        }

        private void setPitch()
        {
            AL.Source(id, ALSourcef.Pitch, pitch);
        }

        public override void loop(bool b)
        {
            looping = b;
            AL.Source(id, ALSourceb.Looping, looping);

        }

        public override bool pause()
        {
            playing = false;
            paused = true;
            if (!canPlay) {
                return false;
            }
            AL.SourcePause(id);
            return true;
        }

        public override bool play()
        {
            return unPause();
        }

        public override bool playNew()
        {
            stop();
            return play();
        }

        public override void stop()
        {
            if (canPlay) {
                AL.SourceStop(id);
            }
            paused = false;
            playing = false;
        }

        internal void stop(string soundToStop) {
            if (sound == soundToStop) {
                stop();
                canPlay = false;
            }
        }

        public override bool unPause()
        {
            if (canPlay)
            {
                return false;
            }
            paused = false;
            playing = true;
            AL.SourcePlay(id);
            return true;
        }

        internal void loadSound(int sound)
        {
            AL.Source(id, ALSourcei.Buffer, sound);
            canPlay = true;
        }

        private void setPos() {
            AL.Source(id,ALSource3f.Position, ref pos);
        }

        private void setVel()
        {
            AL.Source(id, ALSource3f.Position, ref vel);
        }

        internal override void update() {

            updatePlaying();
            if (pitchChange)
            {
                setPitch();
                pitchChange = false;
            }
            if (gainChange)
            {
                setGain();
                gainChange = false;
            }

            if (gotSourceObject) {
                pos = sourceObj.getSoundPos();
                setPos();
                vel = sourceObj.getSoundVel();
                setVel();
            } 


        }

        private void updatePlaying() {
            if (playing) {
                int state;
                AL.GetSource(id, ALGetSourcei.SourceState, out state);
                playing = (ALSourceState)state == ALSourceState.Playing;
            }
        }


    }
}
