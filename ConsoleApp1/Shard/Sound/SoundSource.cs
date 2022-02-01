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
        /*
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
                  init();
              }

              public void setSourceObject(ISoundSourceObject obj) {
                  sourceObj = obj;
                  gotSourceObject = true;
                  updateOBJ();

              }

              public SoundSource()
              {
                  init();
              }

              private void init() {
                  Bootstrap.getSound().addSource(this);
                  AL.Source(id, ALSourcef.RolloffFactor, 2.0f);
              }


              internal int Id { get => this.id; set => this.id = value; }
              public float Gain { get => gain; }

              public float Pitch { get => pitch; }
              public Vector3 Pos { get => pos; set => pos = value; }
              public Vector3 Vel { get => vel; set => vel = value; }


              public void setGain(float newGain)
              {
                  gain = newGain;
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
                  if (!canPlay)
                  {
                      return false;
                  }

                  paused = false;
                  playing = true;

                  AL.SourcePlay(id);

                  return true;
              }

              internal void loadSound(string key, int buffer)
              {
                  sound = key;
                  AL.Source(id, ALSourcei.Buffer, buffer);
                  canPlay = true;
              }

              private void setPos() {
                  AL.Source(id, ALSource3f.Position, ref pos);
              }

              private void setVel()
              {
                  AL.Source(id, ALSource3f.Velocity, ref vel);
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
                      updateOBJ();
                  }

              }

              private void updateOBJ() {
                  pos = sourceObj.getSoundPos();
                  setPos();

                  vel = sourceObj.getSoundVel();
                  setVel();
              }

              private void updatePlaying() {
                  if (playing) {
                      int state;
                      AL.GetSource(id, ALGetSourcei.SourceState, out state);
                      playing = (ALSourceState)state == ALSourceState.Playing;
                  }
              }

              */
    }
}
