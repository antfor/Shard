using OpenTK.Mathematics;

namespace Shard
{
    public abstract class Listener
    {

        private bool isStatic = false;

        public bool IsStatic { get => isStatic; }

        protected void setStatic(bool b) {
            isStatic = b;
        }

        public abstract Vector3 getPos();
        public abstract Vector3 getVel();

        public abstract Vector3 getDir();
        public abstract Vector3 getUp();

        

    }
}