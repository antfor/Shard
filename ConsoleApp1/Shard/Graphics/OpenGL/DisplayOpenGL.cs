using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Shard;
using Shard.Graphics.OpenGL;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Windowing.Desktop;
using OpenTK.Platform.Windows;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
//using OpenTK.Windowing.Desktop;

namespace Shard
{
    class DisplayOpenGL: IDisplay, IThread<UpdatingState>
    {
        private int _height, _width;
        private WindowOpenGL window;

        public int Height { get => _height; set => resizeDisplay(_width, value); }
        public int Width { get => _width; set => resizeDisplay(value, _height); }

        public DisplayOpenGL() {
            
        }

        public bool resizeDisplay(int w, int h) {
            if (!(w > 0 && h > 0)) {
                return false;
            }

            _width  = w;
            _height = h;
            return true;
        }

        public void initialize()
        {
            ThreadManager tm = ThreadManager.getInstance();
            tm.addThread("display", this);
            tm.runThread("display");

        }

        public void update() { 
        
        }

        public void clearDisplay()
        {
            throw new NotImplementedException();
        }

        public void display()
        {
            throw new NotImplementedException();
        }

        public void addCallBack(Callback<UpdatingState> callback)
        {
            throw new NotImplementedException();
        }

        public void runMethod()
        {
            window = new WindowOpenGL(GameWindowSettings.Default, NativeWindowSettings.Default);
            window.Run();
        }
    }
}
