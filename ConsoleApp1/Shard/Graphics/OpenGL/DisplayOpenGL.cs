using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Shard;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Windowing.Desktop;
using OpenTK.Platform.Windows;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
//using OpenTK.Windowing.Desktop;

namespace Shard.Graphics.OpenGL
{
    class DisplayOpenGL: GameWindow, IDisplay
    {
        private int _height, _width;

        public int Height { get => _height; set => resizeDisplay(_width, value); }
        public int Width { get => _width; set => resizeDisplay(value, _height); }

        public DisplayOpenGL(int width, int height, string title) : base(GameWindowSettings.Default, NativeWindowSettings.Default) {
            //this.RenderFrequency = 0;
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
            throw new NotImplementedException();
        }

        public void clearDisplay()
        {
            throw new NotImplementedException();
        }

        public void display()
        {
            throw new NotImplementedException();
        }

    }
}
