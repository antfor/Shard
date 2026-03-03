/*
*
*   The abstract display class setting out the consistent interface all display implementations need.  
*   @author Michael Heron
*   @version 1.0
*   
*/

using Shard.Graphics;
using System.Drawing;

namespace Shard
{

    abstract class Display : IDisplay2D
    {
        protected int _height, _width;

        public int Height { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int Width { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

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
            return _height;
        }

        public int getWidth()
        {
            return _width;
        }

        public virtual void setSize(int w, int h)
        {
            _height = h;
            _width = w;
        }

        public abstract void initialize();
        public abstract void clearDisplay();
        public abstract void display();

        public abstract void showText(string text, double x, double y, int size, int r, int g, int b);
        public abstract void showText(char[,] text, double x, double y, int size, int r, int g, int b);

        public void useVsync(VsyncSetting setting)
        {
            throw new System.NotImplementedException();
        }

        public void cursorVisible(bool b)
        {
            throw new System.NotImplementedException();
        }

        public void setState(DisplayState state)
        {
            throw new System.NotImplementedException();
        }

        public void setBoarder(DisplayBorder border)
        {
            throw new System.NotImplementedException();
        }

        public void setName(string name)
        {
            throw new System.NotImplementedException();
        }

        public bool resizeDisplay(int w, int h)
        {
            throw new System.NotImplementedException();
        }
    }
}
