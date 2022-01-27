/*
*
*   This is here for... reasons.  There is a Vector2 class avaialble in .NET (and indeed, we use
*       it here to simplify some of the code).   However, there was a SUPER WEIRD performance 
*       issue that came from using Vector2 as the basis of the transform class.  Every subsequent
*       access of X or Y would add a little bit of lag to the system, and over the course of ten 
*       seconds (because we access those variables a lot), it would turn a 0ms physics tick into 
*       one that took fifty or sixty milliseconds, at which point everything crashed to a halt.
*       
*   None of that happens with this simple implementation.
*   
*   If any of you work out why the Vector2 class may have been doing that weird thing, it's worth
*       some bonus marks. 
*       
*   Seriously.  It stops me sleeping at night.
*       
*   @author Michael Heron
*   @version 1.0
*   
*/

using System;
using System.Numerics;

namespace Shard
{
    class Vector
    {
        private float x, y;

        public Vector()
        {
        }


        public Vector(Vector d) : this(d.X, d.Y)
        {
        }

        public bool isValid()
        {
            if (Double.IsNaN(x))
            {
                return false;
            }

            if (Double.IsNaN(y))
            {
                return false;
            }

            return true;
        }

        public void reverse()
        {
            x *= -1;
            y *= -1;

        }

        public bool checkZero()
        {
            if (x == 0 && y == 0)
            {
                return true;
            }
            return false;
        }

        public Vector copy()
        {
            Vector v = new Vector();
            v.X = x;
            v.Y = y;

            return v;
        }

        public void multiply(float num)
        {
            x *= num;
            y *= num;
        }

        public void normalize()
        {
            Vector2 v;

            v = new Vector2(x, y);

            v = Vector2.Normalize(v);

            x = v.X;
            y = v.Y;
        }

        public Vector(float x, float y)
        {


            this.x = x;
            this.y = y;
        }

        public Vector(double x, double y) : this((float)x, (float)y)
        {
        }

        public float X { get => x; set => x = value; }
        public float Y { get => y; set => y = value; }

        public override String ToString()
        {
            return "(" + x + ", " + y + ")";
        }
    }
}
