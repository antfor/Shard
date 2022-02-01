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
    public class SoundStatic : IopenAL
    {

        private float gain = 1f;
        private bool gainChange = false;
        private float pitch = 1f;
        private bool pitchChange = false;
        
        protected int id;
        private string sound;
        private bool canPlay = false;


        private bool playing = false;
        private bool paused = false;
        private bool looping = false;

        public bool Looping { get => looping; }
        public bool Paused { get => paused; }
        public bool Playing { get => playing; }


        public SoundStatic()
        {
            init(0.0f);
        }

        protected void init(float rolloff)
        {
            Bootstrap.getSound().addSource(this);
            AL.Source(id, ALSourcef.RolloffFactor, rolloff);
        }

        public float Gain { get => gain; }

        public float Pitch { get => pitch; }
        int IopenAL.Id { get => id; set =>id = value; }
        
        public void setGain(float newGain)
        {
            gain = Math.Max(Math.Min(newGain, 10.0f), 0.0f);
            gainChange = true;
        }
        private void setGain()
        {
            AL.Source(id, ALSourcef.Gain, gain);
        }

        private void setPitch()
        {
            AL.Source(id, ALSourcef.Pitch, pitch);
        }

        public void setPitch(float newPitch)
        {
            pitch = newPitch;
            pitchChange = true;
        }

        public void loop(bool b)
        {
            looping = b;
            AL.Source(id, ALSourceb.Looping, looping);

        }

        public bool pause()
        {
            playing = false;
            paused = true;
            if (!canPlay)
            {
                return false;
            }
            AL.SourcePause(id);
            return true;
        }

        public bool play()
        {
            return unPause();
        }

        public bool playNew()
        {
            stop();
            return play();
        }

        public void stop()
        {
            if (canPlay)
            {
                AL.SourceStop(id);
            }
            paused = false;
            playing = false;
        }

        void IopenAL.stop(string soundToStop)
        {
            if (sound == soundToStop)
            {
                stop();
                canPlay = false;
            }
        }

        public bool unPause()
        {
            if (!canPlay)
            {
                return false;
            }

            paused = false;
            playing = true;

            AL.SourcePlay(id);

            return true;
        }

        void IopenAL.loadSound(string key, int buffer)
        {
            sound = key;
            AL.Source(id, ALSourcei.Buffer, buffer);
            canPlay = true;
        }


        public virtual void update()
        {

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

        }


        private void updatePlaying()
        {
            if (playing)
            {
                int state;
                AL.GetSource(id, ALGetSourcei.SourceState, out state);
                playing = (ALSourceState)state == ALSourceState.Playing;
            }
        }

    }
}
