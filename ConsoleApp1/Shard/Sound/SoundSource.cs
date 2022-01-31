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
    class SoundSource : Sound
    {

        private float gain = 1f;
        private float pitch = 1f;
        private Vector3 pos = new (0.0f, 0.0f, 0.0f);
        private Vector3 vel = new (0.0f, 0.0f, 0.0f);
        private int id;
        private string sound;
        private bool canPlay = false;
        private GameObject parent;


        public SoundSource(GameObject p) {
            parent = p;
        }
        internal int Id { get => this.id; set => this.id = value; }
        public float Gain { get => gain; set => gain = setGain(value); }

        public float Pitch { get => pitch; set => pitch = setPitch(value); }


        private float setGain(float value)
        {
            AL.Source(id, ALSourcef.Gain, value);
            return value;
        }

        private float setPitch(float value)
        {
            AL.Source(id, ALSourcef.Pitch, value);
            return value;
        }

        public override void loop(bool b)
        {
            looping = b;
            AL.Source(id, ALSourceb.Looping, looping);

        }

        public override bool pause()
        {
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

            playing = true;
            AL.SourcePlay(id);
            return true;
        }

        internal void loadSound(int sound)
        {
            AL.Source(id, ALSourcei.Buffer, sound);
            canPlay = true;
        }

        internal override void update()
        {
            throw new NotImplementedException();
        }
    }
}
