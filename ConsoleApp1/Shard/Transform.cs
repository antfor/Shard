/*
*
*   The transform class handles position, independent of physics and forces (although the physics
*       system will make use of the rotation and translation functions here).  Essentially this class
*       is a game object's location (X, Y), rotation and scale.  Usefully it also calculates the 
*       centre of an object as well as relative directions such as forwards and right.  If you want 
*       backwards and left, multiply forward or right by -1.
*       
*   @author Michael Heron
*   @version 1.0
*   
*/


using System;

namespace Shard
{

    class Transform
    {
        private GameObject owner;
        private double x, y;
        private double lx, ly;
        private double rotz;
        private int wid, ht;
        private double scalex, scaley;
        private string spritePath;
        private Vector forward;
        private Vector right, centre;

        public Vector getLastDirection()
        {
            double dx, dy;
            dx = (X - Lx);
            dy = (Y - Ly);

            return new Vector(-dx, -dy);
        }

        public Transform(GameObject ow)
        {
            Owner = ow;
            forward = new Vector();
            right = new Vector();
            centre = new Vector();

            scalex = 1.0;
            scaley = 1.0;

            x = 0;
            y = 0;

            lx = 0;
            ly = 0;

            rotate(0);
        }


        public void recalculateCentre()
        {

            centre.X = (float)(x + ((this.Wid) / 2));
            centre.Y = (float)(y + ((this.Ht) / 2));


        }


        public void translate(double nx, double ny)
        {
            Lx = X;
            Ly = Y;

            x += (float)nx;
            y += (float)ny;


            recalculateCentre();
        }

        public void translate(Vector vect)
        {
            translate(vect.X, vect.Y);
        }



        public void rotate(double dir)
        {
            rotz += (float)dir;

            rotz %= 360;

            float angle = (float)(Math.PI * rotz / 180.0f);
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);

            forward.X = cos;
            forward.Y = sin;


            right.X = -1 * forward.Y;
            right.Y = forward.X;




        }



        public double X
        {
            get => x;
            set => x = value;
        }
        public double Y
        {
            get => y;
            set => y = value;
        }

        public double Rotz
        {
            get => rotz;
            set => rotz = value;
        }


        public string SpritePath
        {
            get => spritePath;
            set => spritePath = value;
        }
        public Vector Forward { get => forward; set => forward = value; }
        public int Wid { get => wid; set => wid = value; }
        public int Ht { get => ht; set => ht = value; }
        public Vector Right { get => right; set => right = value; }
        internal GameObject Owner { get => owner; set => owner = value; }
        public Vector Centre { get => centre; set => centre = value; }
        public double Scalex { get => scalex; set => scalex = value; }
        public double Scaley { get => scaley; set => scaley = value; }
        public double Lx { get => lx; set => lx = value; }
        public double Ly { get => ly; set => ly = value; }
    }
}
