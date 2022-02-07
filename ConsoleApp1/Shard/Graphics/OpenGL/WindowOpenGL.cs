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

        private IRenderObject renderCall;
        private readonly string threadID = "window";
        private bool running = true;


        public WindowOpenGL(GameWindowSettings gameWindowSettings,
                              NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {

            this.UpdateFrequency = 1;
            this.RenderFrequency = 1;

        }

        public unsafe override void Run()
        {
            OnLoad();

            while (GLFW.WindowShouldClose(WindowPtr) == false)
            {
               ProcessEvents();
               Thread.Sleep(1);
            }
            OnUnload();
            
        }

        public void setBoarder(DisplayBorder border) {
            switch (border){
                case DisplayBorder.Resizable:  this.WindowBorder = WindowBorder.Resizable; break;
                case DisplayBorder.BorderLess: this.WindowBorder = WindowBorder.Hidden; break;
                case DisplayBorder.Fixed:      this.WindowBorder = WindowBorder.Fixed; break;

            }
        }

        public void setState(DisplayState state) {

            switch (state)
            {
                case DisplayState.Normal:     this.WindowState = WindowState.Normal; break;
                case DisplayState.Fullscreen: this.WindowState = WindowState.Fullscreen; break;
                case DisplayState.Maximized:  this.WindowState = WindowState.Maximized; break;
                case DisplayState.Minimized:  this.WindowState = WindowState.Minimized; break;

            }
            
        }


        public void cursorVisible(bool b) {
            this.CursorVisible = b;
        }

        public void setName(string name) {
            this.setName(name);
        }

        public void useVsync(VsyncSetting setting) {
            switch (setting) {
                case VsyncSetting.ON : this.VSync = VSyncMode.On; break;
                case VsyncSetting.OFF: this.VSync = VSyncMode.On; break;
                case VsyncSetting.ADAPTIVE: this.VSync = VSyncMode.On; break;
            }
        }

        public void resize(int w, int h) {
            this.Size = new Vector2i(w, h);
        }
     
        internal void addRenderCall(IRenderObject obj)
        {
            renderCall = obj;
        }


        protected unsafe override void OnLoad()
        {

            this.Context.MakeNoneCurrent();
        
            ThreadManager tm = ThreadManager.getInstance();
            tm.addThread(threadID, this);
            tm.setPriority(threadID, ThreadPriority.Highest);
            tm.runThread(threadID);

        }

        public void runMethod()
        {
            this.Context.MakeCurrent();
            VSync = VSyncMode.Off;

            RenderLoop();
        }

        void RenderLoop()
        {
            GL.ClearColor(0.39f, 0.58f, 0.93f, 1.0f);

            while (running)
            {
                GL.Clear(ClearBufferMask.ColorBufferBit);

                renderCall.render();

                this.Context.SwapBuffers();
            }

            this.Context.MakeNoneCurrent();

        }

        protected override void OnUnload()
        {
            ThreadManager tm = ThreadManager.getInstance();
            running = false;
            tm.join(threadID);
            tm.removeThread(threadID);
            base.OnUnload();
        }

    }
}
