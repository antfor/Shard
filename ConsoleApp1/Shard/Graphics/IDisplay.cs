
namespace Shard.Graphics
{


    interface IDisplay
    {

        int Height { get; set; }
        int Width { get; set; }


        public virtual void setFullscreen(bool b)
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

        public abstract void useVsync(VsyncSetting setting);

        public void cursorVisible(bool b);

        public void setState(DisplayState state);

        public void setBoarder(DisplayBorder border);

        public void setName(string name);

        public bool resizeDisplay(int w, int h);

    }
}
