using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
namespace Shard.Graphics
{
    interface IDisplay
    {

        int Height { get; set; }
        int Width { get; set; }


        public virtual void setFullscreen()
        {
        }

        public virtual void addToDraw(GameObject gob)
        {
        }

        public virtual void removeToDraw(GameObject gob)
        {
        }
        public int getHeight()
        {
            return Height;
        }

        public int getWidth()
        {
            return Width;
        }

        public virtual void setSize(int w, int h)
        {
            Height = h;
            Width = w;
        }

        public abstract void initialize();
        public abstract void clearDisplay();
        public abstract void display();
        public bool resizeDisplay(int w, int h);

    }
}
