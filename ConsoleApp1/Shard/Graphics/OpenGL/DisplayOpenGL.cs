﻿
using System.Collections.Generic;

using Shard.Graphics.OpenGL;


using OpenTK.Windowing.Desktop;

using Shard.Graphics;
using Shard.Graphics.OpenGL.Rendering;

namespace Shard
{
    class DisplayOpenGL: IDisplay3D, IThread
    {
        private int _height, _width;
        private WindowOpenGL window;
        private RenderManager rm;
       


        private readonly string threadId = "display";
        private readonly string barrierId = "displayBarrier";

        private ThreadManager tm;

        public int Height { get => _height; set => resizeDisplay(_width, value); }
        public int Width { get => _width; set => resizeDisplay(value, _height); }

        public DisplayOpenGL() {
            
        }

        public bool resizeDisplay(int w, int h) {
            if (!(w > 0 && h > 0)) {
                return false;
            }
            window.resize(w,h);
            _width  = w;
            _height = h;
            return true;
        }

        public void initialize()
        {

            tm = ThreadManager.getInstance();
            tm.addBarrier(barrierId, 2);
            
            tm.addThread(threadId, this);
            tm.setPriority(threadId, System.Threading.ThreadPriority.AboveNormal);
            tm.runThread(threadId);

            rm = RenderManager.getInstance();

        }

        public void runMethod()
        {

            window = new WindowOpenGL(GameWindowSettings.Default, NativeWindowSettings.Default);
            window.addRenderCall(this);
            window.Run();
        }

        public void update() {
            display();
        }



        public void render()
        {
            tm.sync(barrierId);

            // update camera

            //render
            rm.update();

            tm.sync(barrierId);
        }


        public void display()
        {
            tm.sync(barrierId);
            tm.sync(barrierId);
        }

        public void clearDisplay()
        {
            //do nothing
        }

        public void useVsync(VsyncSetting setting)
        {
            window.useVsync(setting);
        }

        public void cursorVisible(bool b)
        {
            window.cursorVisible(true);
        }

        public void setState(DisplayState state)
        {
            window.setState(state);
        }

        public void setBoarder(DisplayBorder border)
        {
            window.setBoarder(border);
        }

        public void setName(string name)
        {
            window.setName(name);
        }
    }
}
