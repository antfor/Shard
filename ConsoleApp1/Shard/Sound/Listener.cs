using OpenTK.Mathematics;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;

namespace Shard.Sound
{
    public interface Listener
    {

        bool IsStatic { get; set; }

        public abstract Vector3 getPos();
        public abstract Vector3 getVel();

        public abstract Vector3 getDir();
        public abstract Vector3 getUp();

    }
}