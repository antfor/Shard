using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Shard.Graphics.OpenGL;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Windowing.Desktop;
using OpenTK.Platform.Windows;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Shard.Threading;
using System.Threading;

namespace Shard
{
    class DisplayOpenGL: IDisplay, IThread , IRenderObject
    {
        private int _height, _width;
        private WindowOpenGL window;
        private SortedList<int, List<IRenderObject>> renderObjects = new SortedList<int, List<IRenderObject>>() { };

        private LockObject updatelock;
        //private LockObject renderlock;
        private readonly string threadId = "display";
        private readonly string barrierId = "displayBarrier";

        ThreadManager tm;

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
            updatelock = new LockObject(true);

            tm = ThreadManager.getInstance();
            tm.addBarrier(barrierId, 2);
            
            tm.addThread(threadId, this);
            tm.setPriority(threadId, System.Threading.ThreadPriority.AboveNormal);
            tm.runThread(threadId);
            
        }

        public void update() {
            display();
        }

        public int addRenderObject(IRenderObject obj, int prio)
        {

            List<IRenderObject> list;
            if (renderObjects.TryGetValue(prio, out list))
            {
                list.Add(obj);
            }
            else
            {
                renderObjects.Add(prio, new List<IRenderObject> { obj });
            }

            return prio;
        }

        public bool removeRenderObject(IRenderObject obj, int prio)
        {

            List<IRenderObject> list;

            if (renderObjects.TryGetValue(prio, out list))
            {

                return list.Remove(obj);
            }

            return false;
        }

        public void render()
        {
            tm.sync(barrierId);

            // update camera

            //render

            foreach (List<IRenderObject> list in renderObjects.Values)
            {
                foreach (IRenderObject obj in list)
                {
                    obj.render();
                }
            }
            tm.sync(barrierId);
        }


        public void display()
        {
            tm.sync(barrierId);
            tm.sync(barrierId);
        }


        public void runMethod()
        {
            //renderlock = new LockObject(true);
            window = new WindowOpenGL(GameWindowSettings.Default, NativeWindowSettings.Default);
            window.addRenderCall(this);
            window.Run();
        }

        public void clearDisplay()
        {
            
        }
    }
}
