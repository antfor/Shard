using OpenTK.Mathematics;

namespace Shard
{
    abstract class Lisener
    {

        public abstract Vector3 getPos();
        public abstract Vector3 getVel();

        public abstract Vector3 getDir();
        public abstract Vector3 getUp();

    }
}