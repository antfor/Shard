using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using OpenTK.Mathematics;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;

using Shard.Sound;

namespace Shard
{

    public class defultLisener : Listener
    {
        
        private Vector3 pos = new Vector3(0.0f, 0.0f, 5.0f);
        private Vector3 dir = new Vector3(0.0f, 0.0f, -1.0f);
        private Vector3 up =  new Vector3(0.0f, 1.0f, 0.0f);
        private Vector3 vel = new Vector3(0.0f, 0.0f, 0.0f);
        private bool isStatic;

        public bool IsStatic { get => isStatic;  set => isStatic = value; }

        public defultLisener() {
            isStatic = true;
        }

        public Vector3 getUp()
        {
            return up;
        }

        public Vector3 getDir()
        {
            return dir;
        }

        public Vector3 getPos()
        {
            return pos;
        }

        public Vector3 getVel()
        {
            return vel;
        }
    }

    public class SoundManager
    {

        private Dictionary<string, int> buffers = new Dictionary<string, int> { };
        private List<IopenAL> sources = new List<IopenAL> { };

        private double start;
        private double TimeInterval;


        private Listener listener;

        internal Listener Listener { get => listener; set => listener = value; }

        public unsafe  SoundManager()
        {
            start = 0;
            setFPS(50);

            //Initialize
            var device = ALC.OpenDevice(null);
            var context = ALC.CreateContext(device, (int*)null);

            ALC.MakeContextCurrent(context);

            var version = AL.Get(ALGetString.Version);
            var vendor = AL.Get(ALGetString.Vendor);
            var renderer = AL.Get(ALGetString.Renderer);
            

            //AL.RegisterOpenALResolver();
            Console.WriteLine(version);
            Console.WriteLine(vendor);
            Console.WriteLine(renderer);

            setListener(new defultLisener());

             AL.DistanceModel(ALDistanceModel.ExponentDistance);
            //AL.DistanceModel(ALDistanceModel.LinearDistance);
            

        }
        
        
        public void setFPS(int fps) {
            TimeInterval = 1.0 / (double)fps;
        }

        public bool willTick()
        {
            if (start < TimeInterval)
            {
                return false;
            }

            return true;
        }

        public bool update() {

            start += Bootstrap.getDeltaTime();

            if (willTick() == false)
            {
                return false;
            }

            if (!listener.IsStatic)
            {
                updateListener();
            }
            updateSources();

            start -= TimeInterval;

            return true;

        }

        public void setListener(Listener l)
        {
            this.Listener = l;
            updateListener();
        }

        private void updateListener() {
  
                Vector3 pos = listener.getPos();
                AL.Listener(ALListener3f.Position, ref pos);


                Vector3 vel = listener.getVel();
                AL.Listener(ALListener3f.Velocity, ref vel);


                Vector3 dir = listener.getDir();
                Vector3 up = listener.getUp();
                AL.Listener(ALListenerfv.Orientation, ref dir, ref up);
              //  AL.Listener(ALListenerfv.Orientation,);
        }


       

        unsafe public bool addSound(string id, string file)
        {
            int buffer = AL.GenBuffer();
            int channels, bits_per_sample, sample_rate;

            Stream s = File.Open(file, FileMode.Open);
            byte[] sound_data = LoadWave(s, out channels, out bits_per_sample, out sample_rate);

            fixed (byte* p = sound_data)
            {
                IntPtr ptr = (IntPtr)p;
                AL.BufferData(buffer, GetSoundFormat(channels, bits_per_sample), ptr, sound_data.Length, sample_rate);
            }
            buffers.Add(id, buffer);
            return true;
        }

        public bool addSound(string file) {
  
            return addSound(file,file);
        }

        public bool deleteSound(string id)
        {
            int buffer;
           
            if (buffers.TryGetValue(id, out buffer)) {

                foreach (IopenAL s in sources)
                {
                    s.stop(id);
                }

                AL.DeleteBuffer(buffer);
                return buffers.Remove(id);
            }

            return false;
        }

       
        private static byte[] LoadWave(Stream stream, out int channels, out int bits, out int rate) {

            if (stream == null)
                throw new ArgumentNullException("stream");

            using (BinaryReader reader = new BinaryReader(stream))
            {

                // RIFF header
                string signature = new string(reader.ReadChars(4));
                if (signature != "RIFF")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                int riff_chunck_size = reader.ReadInt32();

                string format = new string(reader.ReadChars(4));
                if (format != "WAVE")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                
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
                //case 2: return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
                default: throw new NotSupportedException("The specified sound format is not supported.");
            }
        }

        //stream file

        public void addSource(IopenAL source)
        {
            if (sources.Contains(source)) {
                return;
            }
            source.Id = AL.GenSource();

            sources.Add(source);
        }

        public void removeSource(IopenAL source) {

            source.stop();
            AL.DeleteSource(source.Id);
            sources.Remove(source);
        }

        public bool loadSource(IopenAL source, string id) {
            int sound;
            bool exist = buffers.TryGetValue(id, out sound);

            if (exist) {
                source.loadSound(id, sound);
            }
            
            return exist;
        }

        private void updateSources()
        {
                foreach (IopenAL source in sources)
                {
                    source.update();
                }
        }

    }
}
