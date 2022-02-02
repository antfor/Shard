/*
*
*   The abstract collider class that is the base of all collisions.   Specific variations should
*       extend from this or one of its children.
*   @author Michael Heron
*   @version 1.0
*   
*/

using System.Drawing;

namespace Shard
{
    abstract class Collider
    {
        private CollisionHandler gameObject;
        private float[] minAndMaxX;
        private float[] minAndMaxY;

        public abstract void recalculate();
        public Collider(CollisionHandler gob)
        {
            gameObject = gob;
            MinAndMaxX = new float[2];
            MinAndMaxY = new float[2];

        }

        internal CollisionHandler GameObject { get => gameObject; set => gameObject = value; }
        internal float[] MinAndMaxX { get => minAndMaxX; set => minAndMaxX = value; }
        internal float[] MinAndMaxY { get => minAndMaxY; set => minAndMaxY = value; }
        public abstract Vector checkCollision(ColliderRect c);

        public abstract Vector checkCollision(Vector c);

        public abstract Vector checkCollision(ColliderCircle c);

        public virtual Vector checkCollision(Collider c)
        {

            if (c is ColliderRect)
            {
                return checkCollision((ColliderRect)c);
            }

            if (c is ColliderCircle)
            {
                return checkCollision((ColliderCircle)c);
            }

            Debug.getInstance().log("Bug");
            // Not sure how we got here but c'est la vie
            return null;
        }

        public abstract void drawMe(Color col);

        public abstract float[] getMinAndMaxX();
        public abstract float[] getMinAndMaxY();

    }
}
