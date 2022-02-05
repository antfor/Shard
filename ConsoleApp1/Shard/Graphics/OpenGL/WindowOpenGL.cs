using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Threading;

namespace Shard.Graphics.OpenGL
{

    class WindowOpenGL : GameWindow , IThread
    {

        private GameWindowSettings gws;
        private NativeWindowSettings nws;
        private IRenderObject renderCall;
        private readonly string threadID = "window";
        private bool running = true;


        public WindowOpenGL(GameWindowSettings gameWindowSettings,
                              NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            gws = gameWindowSettings;
            nws = nativeWindowSettings;

            gws.UpdateFrequency = 1;
            gws.RenderFrequency = 1;
            

    
        }
        

        private void setBoarder(DisplayBorder border) {
            switch (border){
                case DisplayBorder.Resizable:  nws.WindowBorder = WindowBorder.Resizable; break;
                case DisplayBorder.BorderLess: nws.WindowBorder = WindowBorder.Hidden; break;
                case DisplayBorder.Fixed:      nws.WindowBorder = WindowBorder.Fixed; break;

            }
        }

        private void setState(DisplayState state) {

            switch (state)
            {
                case DisplayState.Normal:     nws.WindowState = WindowState.Normal; break;
                case DisplayState.Fullscreen: nws.WindowState = WindowState.Fullscreen; break;
                case DisplayState.Maximized:  nws.WindowState = WindowState.Maximized; break;
                case DisplayState.Minimized:  nws.WindowState = WindowState.Minimized; break;

            }
            
        }
        
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // This gets called every 1/60 of a second.
            if (KeyboardState.IsKeyDown(Keys.Escape))
                Close();

           // base.OnUpdateFrame(e);
        }
     
     
        protected override void OnRenderFrame(FrameEventArgs e) {
            Thread.Sleep(1);
        }

        internal void addRenderCall(IRenderObject obj)
        {
            renderCall = obj;
        }

        
        
       // private IGraphicsContext context;
        protected unsafe override void OnLoad()
        {
            //Window* wptr = this.WindowPtr;
            //context = new GLFWGraphicsContext(wptr);
            this.Context.MakeNoneCurrent();
        
            ThreadManager tm = ThreadManager.getInstance();
            tm.addThread(threadID, this);
            tm.setPriority(threadID, ThreadPriority.AboveNormal);
            tm.runThread(threadID);

        }

        protected override void OnUnload()
        {
            ThreadManager tm = ThreadManager.getInstance();
            running = false;
            tm.join(threadID);
            base.OnUnload();
        }

        public void runMethod()
        {
            RenderLoop();
        }

        void RenderLoop()
        {
            this.Context.MakeCurrent();

            while (running)
            {

                GL.ClearColor(0.39f, 0.58f, 0.93f, 1.0f);
                GL.Clear(ClearBufferMask.ColorBufferBit);

                renderCall.render();

                this.Context.SwapBuffers();
            }

            this.Context.MakeNoneCurrent();

        }
    }
}
