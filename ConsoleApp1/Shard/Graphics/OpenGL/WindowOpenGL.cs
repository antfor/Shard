using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics;
//using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Shard.Graphics.OpenGL
{
    public enum UpdatingState{
        Waiting = 0,
        Updating =1,
        Done = 2
    }

    public interface IEventUpdateListener {

        public void onUpdateEvent(UpdatingState state);
    
    }

    class WindowOpenGL : GameWindow
    {

        private GameWindowSettings gws;
        private NativeWindowSettings nws;
        private UpdatingState updating = UpdatingState.Waiting;
        private List<IEventUpdateListener> listeners = new List<IEventUpdateListener> {};


        public void addEventDoneListener(IEventUpdateListener obj) {
            listeners.Add(obj);
        }


        public WindowOpenGL(GameWindowSettings gameWindowSettings,
                              NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            gws = gameWindowSettings;
            nws = nativeWindowSettings;

            
           // gws.IsMultiThreaded = true;
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
        
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // This gets called every 1/60 of a second.
            if (KeyboardState.IsKeyDown(Keys.Escape))
                Close();

            base.OnUpdateFrame(e);
        }

        float time = 20.0f;
        float ct = 0f;
        float dir = 1.0f;
     

        public UpdatingState getUpdatingState() {
            return updating;
        }

        private void setUpdatingState(UpdatingState state) {

            updating = state;
            
        }

        protected override void OnRenderFrame(FrameEventArgs e) {

            GL.ClearColor(0.39f, 0.58f, 0.93f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);


            // Show in the window the results of the rendering calls.
            SwapBuffers();
        }

    }
}
