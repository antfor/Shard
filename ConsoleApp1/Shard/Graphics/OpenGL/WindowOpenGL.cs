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
using OpenTK.Graphics.OpenGL4;
using System.Threading;

using Shard.Misc;
using OpenTK.Windowing.Common.Input;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Runtime.InteropServices;
using Shard.Graphics.OpenGL.Rendering;

namespace Shard.Graphics.OpenGL
{

    class WindowOpenGL : GameWindow , IThread, IRenderContext
    {

        private IDisplay3D renderCall;
        private readonly string threadID = "window";
        private bool running = true;

        object resizeLock = new object();

        public WindowOpenGL(GameWindowSettings gameWindowSettings,
                              NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {

            this.UpdateFrequency = 500;
            this.RenderFrequency = 500;
            cursorVisible(false);
            setName("Shard");
            setIcon(@"D:\chalmers\tda572\shard\1.0.0\Shard\ConsoleApp1\bin\Debug\net5.0\Crystal_Shards.png");


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

        public unsafe void setIcon(string file) {
            var image = (Image<Rgba32>)SixLabors.ImageSharp.Image.Load(Configuration.Default, file);
            image.DangerousTryGetSinglePixelMemory (out var imageSpan);
            var imageBytes = MemoryMarshal.AsBytes(imageSpan.Span).ToArray();
            var windowIcon = new WindowIcon(new OpenTK.Windowing.Common.Input.Image(image.Width, image.Height, imageBytes));

            this.Icon = windowIcon;
        }

        public void cursorVisible(bool b) {
            this.CursorVisible = b;
        }

        public void setName(string name) {
            this.Title = name;
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
     
        internal void addRenderCall(IDisplay3D obj)
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

           // GL.ClearColor(0.39f, 0.58f, 0.93f, 1.0f);
            GL.ClearColor(0.0f, 0.5f, 0.5f, 1.0f);


            float dir = 1;
            float time = 1.5f;
            float ct = 0;

            //GL.Viewport(0, 0, 600, 600);

            while (running)
            {
            
                GL.Viewport(0, 0, this.Size.X, this.Size.Y);
                

                float dt = (float)Bootstrap.getDeltaTime();

                    ct = Math.Max(Math.Min(ct + 2.0f * dir * dt, time), 0);

                    if (ct == time)
                    {

                        ct = 0;
                        time = 1.5f;

                    }

                    //GL.ClearColor(0.4f * (1.0f - ct/time) + 0.2f, 0.0f, 0.0f, 1.0f);
                    GL.Clear(ClearBufferMask.ColorBufferBit);


                lock (resizeLock)
                {
                    renderCall.render();
                }

                this.Context.SwapBuffers();
                

            }

            this.Context.MakeNoneCurrent();

        }

    
        protected override void OnResize(ResizeEventArgs e)
        {
            lock (resizeLock) {
                this.Size = e.Size; 
            }
        }
        protected override void OnUnload()
        {
            //running = false;        
            base.OnUnload();
            Bootstrap.Close();

        }

    }
}
