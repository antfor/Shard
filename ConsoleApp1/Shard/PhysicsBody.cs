/*
*
*   The physics body class does... a lot.  It handles the computation of internal values such as 
*       the min and max values for X and Y (used by the Sweep and Prune algorithm, as well as 
*       collision detection in general).  It registers and processes the colliders that belong to 
*       an object.  It handles the application of forces and torque as well as drag and angular drag.
*       It lets an object add colliders, and then exposes those colliders for narrow phase collision 
*       detection.  It handles some naive default collision responses such as a simple reflection
*       or 'stop on collision'.
*       
*   Important to note though that while this is called a PhysicsBody, no claims are made for the 
*       *accuracy* of the physics.  If you are planning to do anything that requires the physics
*       calculations to be remotely correct, you're going to have to extend the engine so it does 
*       that.  All I'm interested in here is showing you how it's *architected*. 
*       
*   This is also the subsystem which I am least confident about people relying on, because it is 
*       virtually untestable in any meaningful sense.  I spent three days trying to track down a 
*       bug that mean that an object would pass through another one at a rate of approximately
*       once every half hour...
*       
*   @author Michael Heron
*   @version 1.0
*   
*/

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Shard
{
    class PhysicsBody
    {
        List<Collider> myColliders;
        List<Collider> collisionCandidates;
        GameObject parent;
        CollisionHandler colh;
        Transform trans;
        private float angularDrag;
        private float drag;
        private float torque;
        private float mass;
        private double timeInterval;
        private float maxForce, maxTorque;
        private bool kinematic;
        private bool stopOnCollision;
        private bool reflectOnCollision;
        private bool impartForce;
        private bool passThrough;
        private bool usesGravity;
        private Color debugColor;
        public Color DebugColor { get => debugColor; set => debugColor = value; }

        private Dictionary<Vector, float> myForces;
        private Dictionary<Vector, float> forceOffsets;
        private float[] minAndMaxX;
        private float[] minAndMaxY;

        public void applyGravity(float modifier)
        {
            addForce(new Vector(0, 1), modifier);
        }

        public float AngularDrag { get => angularDrag; set => angularDrag = value; }
        public float Drag { get => drag; set => drag = value; }
        internal GameObject Parent { get => parent; set => parent = value; }
        internal Transform Trans { get => trans; set => trans = value; }
        public float Mass { get => mass; set => mass = value; }
        public float[] MinAndMaxX { get => minAndMaxX; set => minAndMaxX = value; }
        public float[] MinAndMaxY { get => minAndMaxY; set => minAndMaxY = value; }
        public float MaxForce { get => maxForce; set => maxForce = value; }
        public float MaxTorque { get => maxTorque; set => maxTorque = value; }
        public bool Kinematic { get => kinematic; set => kinematic = value; }
        public bool PassThrough { get => passThrough; set => passThrough = value; }
        public bool UsesGravity { get => usesGravity; set => usesGravity = value; }
        public bool StopOnCollision { get => stopOnCollision; set => stopOnCollision = value; }
        public bool ReflectOnCollision { get => reflectOnCollision; set => reflectOnCollision = value; }
        public bool ImpartForce { get => this.impartForce; set => this.impartForce = value; }

        public void drawMe()
        {
            if (PhysicsManager.getInstance().Debugging)
            {
                foreach (Collider col in myColliders)
                {
                    col.drawMe(DebugColor);
                }
            }
        }

        public float[] getMinAndMax(bool x)
        {
            float min = Int32.MaxValue;
            float max = -1 * min;
            float[] tmp;

            foreach (Collider col in myColliders)
            {

                if (x)
                {
                    tmp = col.MinAndMaxX;
                }
                else
                {
                    tmp = col.MinAndMaxY;
                }


                if (tmp[0] < min)
                {
                    min = tmp[0];
                }

                if (tmp[1] > max)
                {
                    max = tmp[1];
                }
            }


            return new float[2] { min, max };
        }

        public PhysicsBody(GameObject p)
        {
            DebugColor = Color.Green;

            myColliders = new List<Collider>();
            collisionCandidates = new List<Collider>();

            myForces = new Dictionary<Vector, float>();
            forceOffsets = new Dictionary<Vector, float>();
            Parent = p;
            Trans = p.Transform;
            colh = (CollisionHandler)p;

            AngularDrag = 0.01f;
            Drag = 0.01f;
            Drag = 0.01f;
            Mass = 1;
            MaxForce = 10;
            MaxTorque = 2;
            usesGravity = false;
            stopOnCollision = true;
            reflectOnCollision = false;

            MinAndMaxX = new float[2];
            MinAndMaxY = new float[2];

            timeInterval = PhysicsManager.getInstance().TimeInterval;
            //            Debug.getInstance().log ("Setting physics enabled");

            PhysicsManager.getInstance().addPhysicsObject(this);
        }

        public void addTorque(float dir)
        {
            if (Kinematic)
            {
                return;
            }

            torque += dir / Mass;

            if (torque > MaxTorque)
            {
                torque = MaxTorque;
            }

            if (torque < -1 * MaxTorque)
            {
                torque = -1 * MaxTorque;
            }

        }

        public void reverseForces(float prop)
        {
            List<Vector> keys;

            if (Kinematic)
            {
                return;
            }

            keys = new List<Vector>(myForces.Keys);

            foreach (Vector d in keys)
            {
                myForces[d] = -1 * myForces[d] * prop;
            }

        }

        public void impartForces(PhysicsBody other, float massProp)
        {
            List<Vector> keys = new List<Vector>(myForces.Keys);

            foreach (Vector d in keys)
            {
                Debug.Log ("Imparting force on " + other.Parent + ", x" + massProp + ", " + d);
                other.addForce(new Vector(d.X, d.Y), (myForces[d] * massProp));
            }

            recalculateColliders();

        }

        public void stopForces()
        {
            myForces.Clear();
        }

        public void reflectForces(Vector impulse)
        {
            Vector reflect = new Vector(1, 1);


            // We're being pushed to the right, so we must have collided with the right.
            if (impulse.X > 0)
            {
                reflect.X *= -1;
            }

            // We're being pushed to the left, so we must have collided with the left.
            if (impulse.X < 0)
            {
                reflect.X *= -1;

            }

            // We're being pushed upwards, so we must have collided with the top.
            if (impulse.Y < 0)
            {
                reflect.Y *= -1;
            }

            // We're being pushed downwards, so we must have collided with the bottom.
            if (impulse.Y > 0)
            {
                reflect.Y *= -1;

            }

            Debug.Log("Reflect is " + reflect);

            foreach (KeyValuePair<Vector, float> kvp in myForces)
            {
                kvp.Key.X *= reflect.X;
                kvp.Key.Y *= reflect.Y;
            }

        }

        public float getForce(Vector dir)
        {
            if (myForces.ContainsKey(dir) == false)
            {
                return 0;
            }

            return myForces[dir];
        }

        public void reduceForces (float prop) {
            foreach (KeyValuePair<Vector, float> kvp in myForces)
            {
                myForces[kvp.Key] *= prop;
            }
        }

        public void addForce(Vector dir, float force)
        {
            if (Kinematic)
            {
                return;
            }

            if (dir == null)
            {
                return;
            }

            force = (force / Mass);

            Debug.Log (Parent + " adding force " + dir + ", " + force);
            // Set a lower bound.
            if (Math.Abs(force) < 0.01)
            {
                return;
            }

            // This lets us reduce the number of specific forces that can be generated in the 
            // same direction and to make the system more consistent.   You change intensity 
            // of a vector with the force, not with the direction.
            dir.normalize();

            // It's possible that bad input will cause a problem here such as normalizing a 
            // 0.0 vector.   If the vector we normalize is not a valid vector, just get rid 
            // of it. 
            if (dir.isValid() == false)
            {
                return;
            }

            if (myForces.ContainsKey(dir) == false)
            {
                myForces[dir] = 0;
            }



            myForces[dir] += force;

            if (myForces[dir] > MaxForce)
            {
                myForces[dir] = MaxForce;
            }

            if (myForces[dir] < -1 * MaxForce)
            {
                myForces[dir] = -1 * MaxForce;
            }


        }

        public void recalculateColliders()
        {
            foreach (Collider col in getColliders())
            {
                col.recalculate();
            }

            MinAndMaxX = getMinAndMax(true);
            MinAndMaxY = getMinAndMax(false);
        }

        public void physicsTick()
        {
            List<Vector> toRemove;
            Vector dir;
            float force;
            float rot = 0;
            float ke;

            toRemove = new List<Vector>();

            ke = (float)((Math.Abs(torque)));

            if (torque < 0)
            {
                rot = -1 * ke;

                torque += AngularDrag;
            }
            if (torque > 0)
            {
                rot = ke;
                torque -= AngularDrag;
            }


            if (Math.Abs(torque) < AngularDrag)
            {
                torque = 0;
            }


            //            Debug.Log ("Rotation is " + rot);

            trans.rotate(rot);




            foreach (KeyValuePair<Vector, float> kvp in myForces)
            {
                dir = kvp.Key;
                force = kvp.Value;

                ke = (float)((Math.Abs(force)));

                if (force < 0)
                {
                    force += Drag;

                    trans.translate(-1 * dir.X * ke, -1 * dir.Y * ke);
                }

                if (force > 0)
                {
                    force -= Drag;
                    trans.translate(1 * dir.X * ke, 1 * dir.Y * ke);
                }

                if (Math.Abs(force) < Drag)
                {
                    toRemove.Add(dir);
                    force = 0;
                }


                forceOffsets[dir] = force;

            }



            foreach (KeyValuePair<Vector, float> kvp in forceOffsets)
            {
                dir = kvp.Key;
                force = kvp.Value;

                myForces[dir] = force;
            }


            forceOffsets.Clear();

            foreach (Vector d in toRemove)
            {
                myForces.Remove(d);
            }

            //            recalculateColliders();

        }


        public ColliderRect addRectCollider()
        {
            ColliderRect cr = new ColliderRect((CollisionHandler)parent, parent.Transform);

            addCollider(cr);

            return cr;
        }

        public ColliderCircle addCircleCollider()
        {
            ColliderCircle cr = new ColliderCircle((CollisionHandler)parent, parent.Transform);

            addCollider(cr);

            return cr;
        }

        public ColliderCircle addCircleCollider(int x, int y, int rad)
        {
            ColliderCircle cr = new ColliderCircle((CollisionHandler)parent, parent.Transform, x, y, rad);

            addCollider(cr);

            return cr;
        }


        public ColliderRect addRectCollider(int x, int y, int wid, int ht)
        {
            ColliderRect cr = new ColliderRect((CollisionHandler)parent, parent.Transform, x, y, wid, ht);

            addCollider(cr);

            return cr;
        }


        public void addCollider(Collider col)
        {
            myColliders.Add(col);
        }

        public List<Collider> getColliders()
        {
            return myColliders;
        }

        public Vector checkCollisions(Vector other)
        {
            Vector d;


            foreach (Collider c in myColliders)
            {
                d = c.checkCollision(other);

                if (d != null)
                {
                    return d;
                }
            }

            return null;
        }


        public Vector checkCollisions(Collider other)
        {
            Vector d;

            Debug.Log("Checking collision with " + other);
            foreach (Collider c in myColliders)
            {
                d = c.checkCollision(other);

                if (d != null)
                {
                    return d;
                }
            }

            return null;
        }
    }
}