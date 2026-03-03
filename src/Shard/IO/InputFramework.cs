/*
*
*   SDL provides an input layer, and we're using that.  This class tracks input, anchors it to the 
*       timing of the game loop, and converts the SDL events into one that is more abstract so games 
*       can be written more interchangeably.
*   @author Michael Heron
*   @version 1.0
*   
*/

using SDL2;
using Shard.Misc;
using System;
using System.Runtime.InteropServices;

namespace Shard
{

    // We'll be using SDL2 here to provide our underlying input system.
    class InputFramework : InputSystem
    {

        double tick, timeInterval;
        byte[] keys = { };
        int numKeys;
        public override void getInput()
        {

            SDL.SDL_Event ev;
            int res;
            InputEvent ie;

            tick += Bootstrap.getDeltaTime();

            if (tick < timeInterval)
            {
                return;
            }

            while (tick >= timeInterval)
            {

                IntPtr keysPtr = SDL.SDL_GetKeyboardState(out numKeys);
                keys = new byte[numKeys];
                Marshal.Copy(keysPtr, keys, 0, numKeys);

                res = SDL.SDL_PollEvent(out ev);

                if (res != 1)
                {
                    return;
                }
                

                ie = new InputEvent();

                if (ev.type == SDL.SDL_EventType.SDL_MOUSEMOTION)
                {
                    SDL.SDL_MouseMotionEvent mot;

                    mot = ev.motion;

                    ie.X = mot.x;
                    ie.Y = mot.y;
                    ie.XRel = mot.xrel;
                    ie.YRel = mot.yrel;

                    informListeners(ie, "MouseMotion");
                }

                if (ev.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN)
                {
                    SDL.SDL_MouseButtonEvent butt;

                    butt = ev.button;

                    ie.Button = (int)butt.button;
                    ie.X = butt.x;
                    ie.Y = butt.y;

                    informListeners(ie, "MouseDown");
                }

                if (ev.type == SDL.SDL_EventType.SDL_MOUSEBUTTONUP)
                {
                    SDL.SDL_MouseButtonEvent butt;

                    butt = ev.button;

                    ie.Button = (int)butt.button;
                    ie.X = butt.x;
                    ie.Y = butt.y;

                    informListeners(ie, "MouseUp");
                }

                if (ev.type == SDL.SDL_EventType.SDL_MOUSEWHEEL)
                {
                    SDL.SDL_MouseWheelEvent wh;

                    wh = ev.wheel;

                    ie.X = (int)wh.direction * wh.x;
                    ie.Y = (int)wh.direction * wh.y;

                    informListeners(ie, "MouseWheel");
                }

                
                if (ev.type == SDL.SDL_EventType.SDL_KEYDOWN)
                {
                    PConsole.WriteLine("input");
                    ie.Key = (int)ev.key.keysym.scancode;
                    Debug.getInstance().log("Keydown: " + ie.Key);
                    informListeners(ie, "KeyDown");
                }

                if (ev.type == SDL.SDL_EventType.SDL_KEYUP)
                {
                    ie.Key = (int)ev.key.keysym.scancode;
                    informListeners(ie, "KeyUp");
                }

                tick -= timeInterval;
            }


        }

        public override bool isDown(SDL.SDL_Scancode code)
        {
            if (keys.Length > 0) {
                return 1 == keys[(int)code];
            }
            return false;
        }

        public override uint getMouseState(out int x, out int y)
        {
            return SDL.SDL_GetMouseState(out x, out y);
        }

        public override void initialize()
        {
            tick = 0;
            timeInterval = 1.0 / 60.0;
        }

    }
}