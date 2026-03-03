/*
*
*   Any game object interested in listening for input events will need to register itself 
*       with this manager.   It handles the informing of all listener objects when an 
*       event is raised.
*   @author Michael Heron
*   @version 1.0
*   
*/

using SDL2;
using System.Collections.Generic;
using Shard.Misc;
using System;
using System.Runtime.InteropServices;
using OpenTK.Mathematics;

namespace Shard
{

    class InputOpenGL: InputFramework
    {
      
        private System.IntPtr window;

        public virtual void initialize()
        {
        }

        public InputOpenGL():base()
        {
          

            SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING);


            SDL_ttf.TTF_Init();


            window = SDL.SDL_CreateWindow("Shard Game Engine",
                0,
                0,
                100,
                100,
                0);// SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS|SDL.SDL_WindowFlags.SDL_WINDOW_SKIP_TASKBAR);

            // SDL.SDL_CaptureMouse(SDL.SDL_bool.SDL_TRUE);
            // SDL.SDL_SetWindowInputFocus(window);
             //SDL.SDL_SetRelativeMouseMode(SDL.SDL_bool.SDL_TRUE);

            //  focusInput();   



        }

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public void DoMouseClick()
        {
            //Call the imported function with the cursor's current position
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public void focusInput()
        {
            PConsole.WriteLine("start");
            SDL.SDL_SetWindowInputFocus(window);
            SDL.SDL_bool rea = SDL.SDL_SetHint(SDL.SDL_HINT_GRAB_KEYBOARD, SDL.SDL_bool.SDL_TRUE.ToString());
            SDL.SDL_SetWindowGrab(window, SDL.SDL_bool.SDL_TRUE);
            DoMouseClick();
        }

        public void grabInput(bool b)
        {
            SDL.SDL_bool value = b ? SDL.SDL_bool.SDL_TRUE : SDL.SDL_bool.SDL_FALSE;

            SDL.SDL_SetWindowMouseGrab(window, value);
            SDL.SDL_SetWindowKeyboardGrab(window, value);

        }

        public Vector2i getSize() {
            int h, w;
            SDL.SDL_GetWindowSize(window, out w, out h);
            return new Vector2i(w,h);
        }
    }
}
