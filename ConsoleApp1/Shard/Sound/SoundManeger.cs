using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using OpenTK.Mathematics;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;

namespace Shard
{

    class defultLisener : Lisener
    {

        private Vector3 pos = new Vector3(0.0f, 0.0f, 4.0f);
        private Vector3 dir = new Vector3(0.0f, 0.0f, -1.0f);
        private Vector3 up =  new Vector3(0.0f, 1.0f, 0.0f);
        private Vector3 vel = new Vector3(0.0f, 0.0f, 0.0f);

        public override Vector3 getUp()
        {
            return up;
        }

        public override Vector3 getDir()
        {
            return dir;
        }

        public override Vector3 getPos()
        {
            return pos;
        }

        public override Vector3 getVel()
        {
            return vel;
        }
    }

    class SoundManeger
    {

        private static SoundManeger me;
        private Dictionary<string, int> buffers;
        private List<SoundSource> sources;
       

        private Lisener lisener = new defultLisener();

        public void update() {
            updateLisenr();
            updateSources();
        }

        public void setLisenr(Lisener l)
        {
            lisener = l;
            updateLisenr();
        }

        private void updateLisenr() {
            Vector3 pos = lisener.getPos();
            AL.Listener(ALListener3f.Position, ref pos);


            Vector3 vel = lisener.getVel();
            AL.Listener(ALListener3f.Velocity, ref vel);


            Vector3 dir = lisener.getDir();
            Vector3 up = lisener.getUp();
            AL.Listener(ALListenerfv.Orientation, ref dir, ref up);
        }

        private SoundManeger() { 
            
        }

        private static SoundManeger getInstance()
        {
            if (me == null) {
                me = new SoundManeger();
            }

            return me;
        }


        unsafe public bool addSound(string id, string file)
        {
            int buffer = AL.GenBuffer();
            int channels, bits_per_sample, sample_rate;

            byte[] sound_data = LoadWave(File.Open(file, FileMode.Open), out channels, out bits_per_sample, out sample_rate);

            fixed (byte* p = sound_data)
            {
                IntPtr ptr = (IntPtr)p;
                AL.BufferData(buffer, GetSoundFormat(channels, bits_per_sample), ptr, sound_data.Length, sample_rate);
            }

            return true;
        }

        public bool addSound(string file) {
  
            return addSound(file,file);
        }

        public bool deleteSound(string id)
        {
            int buffer;
           
            if (buffers.TryGetValue(id, out buffer)) {

                foreach (SoundSource s in sources)
                {
                    s.stop(id);
                }

                AL.DeleteBuffer(buffer);
                return buffers.Remove(id);
            }

            return false;
        }

        // From: https://github.com/mono/opentk/blob/e5859900d3a41885e03be46b492bfd382442f130/Source/Examples/OpenAL/1.1/Playback.cs
        private static byte[] LoadWave(Stream stream, out int channels, out int bits, out int rate) {

            if (stream == null)
                throw new ArgumentNullException("stream");

            using (BinaryReader reader = new BinaryReader(stream))
            {

                // WAVE header
                string format_signature = new string(reader.ReadChars(4));
                if (format_signature != "fmt ")
                    throw new NotSupportedException("Specified wave file is not supported.");

                int format_chunk_size = reader.ReadInt32();
                int audio_format = reader.ReadInt16();
                int num_channels = reader.ReadInt16();
                int sample_rate = reader.ReadInt32();
                int byte_rate = reader.ReadInt32();
                int block_align = reader.ReadInt16();
                int bits_per_sample = reader.ReadInt16();

                string data_signature = new string(reader.ReadChars(4));
                if (data_signature != "data")
                    throw new NotSupportedException("Specified wave file is not supported.");

                int data_chunk_size = reader.ReadInt32();

                channels = num_channels;
                bits = bits_per_sample;
                rate = sample_rate;

                return reader.ReadBytes((int)reader.BaseStream.Length);
            }
        }

        private static ALFormat GetSoundFormat(int channels, int bits)
        {
            switch (channels)
            {
                case 1: return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
                case 2: return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
                default: throw new NotSupportedException("The specified sound format is not supported.");
            }
        }

        //stream file

        public void addSource(SoundSource source)
        {
            if (sources.Contains(source)) {
                return;
            }
            source.Id = AL.GenSource();

            sources.Add(source);
        }

        public void removeSource(SoundSource source) {

            source.stop();
            AL.DeleteSource(source.Id);
            sources.Remove(source);
        }

        public bool loadSource(SoundSource source, string id) {
            int sound;
            bool exist = buffers.TryGetValue(id, out sound);

            if (exist) {
                source.loadSound(sound);
            }
            
            return exist;
        }

        private void updateSources()
        {
            foreach(SoundSource source in sources) {
                source.update();
            }
        }

    }
}
