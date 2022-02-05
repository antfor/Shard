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

    class WindowOpenGL : GameWindow
    {

        private GameWindowSettings gws;
        private NativeWindowSettings nws;
        private IRenderObject renderCall;


        public WindowOpenGL(GameWindowSettings gameWindowSettings,
                              NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            gws = gameWindowSettings;
            nws = nativeWindowSettings;

            gws.UpdateFrequency = 500;
            gws.RenderFrequency = 0;
            

    
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
        
      /*  protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // This gets called every 1/60 of a second.
            if (KeyboardState.IsKeyDown(Keys.Escape))
                Close();

            base.OnUpdateFrame(e);
        }
      */
     


        protected override void OnRenderFrame(FrameEventArgs e) {
            Thread.Sleep(1);
            /*
            GL.ClearColor(0.39f, 0.58f, 0.93f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            Console.WriteLine("hej");

            // render
            renderCall.render();

            // Show in the window the results of the rendering calls.
            SwapBuffers();
            */
        }

        internal void addRenderCall(IRenderObject obj)
        {
            renderCall = obj;
        }


        void RenderLoop()
        {
            MakeCurrent();
            GL.ClearColor(0.39f, 0.58f, 0.93f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            SwapBuffers();
        }

        Thread rendering_thread;
        protected override void OnLoad()
        {
            MakeNoneCurrent();
            // nws.SharedContext.MakeCurrent();
            //nws.SharedContext.MakeNoneCurrent(); // Release the OpenGL context so it can be used on the new thread.


            rendering_thread = new Thread(RenderLoop);
            rendering_thread.IsBackground = true;
            rendering_thread.Start();
        }
    }
}
