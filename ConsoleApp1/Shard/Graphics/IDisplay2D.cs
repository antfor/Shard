using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Shard.Graphics
{
    interface IDisplay2D : IDisplay
    {

        public virtual void drawLine(int x, int y, int x2, int y2, int r, int g, int b, int a)
        {
        }

        public virtual void drawLine(int x, int y, int x2, int y2, Color col)
        {
            drawLine(x, y, x2, y2, col.R, col.G, col.B, col.A);
        }


        public virtual void drawCircle(int x, int y, int rad, int r, int g, int b, int a)
        {
        }

        public virtual void drawCircle(int x, int y, int rad, Color col)
        {
            drawCircle(x, y, rad, col.R, col.G, col.B, col.A);
        }

        public virtual void drawFilledCircle(int x, int y, int rad, Color col)
        {
            drawFilledCircle(x, y, rad, col.R, col.G, col.B, col.A);
        }

        public virtual void drawFilledCircle(int x, int y, int rad, int r, int g, int b, int a)
        {
            while (rad > 0)
            {
                drawCircle(x, y, rad, r, g, b, a);
                rad -= 1;
            }
        }

        public void showText(string text, double x, double y, int size, Color col)
        {
            showText(text, x, y, size, col.R, col.G, col.B);
        }

        public abstract void showText(string text, double x, double y, int size, int r, int g, int b);
        public abstract void showText(char[,] text, double x, double y, int size, int r, int g, int b);
    }
}
