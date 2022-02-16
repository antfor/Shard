/*
*
*   The Bootstrap - this loads the config file, processes it and then starts the game loop
*   @author Michael Heron
*   @version 1.0
*   
*/

using System;
using System.Collections.Generic;
using System.Threading;
using Shard.Misc;
using Shard.Sound;
using Shard.Graphics;

using Shard.Graphics.OpenGL.Rendering;

namespace Shard
{
    class Bootstrap
    {
        public static string DEFAULT_CONFIG = "config.cfg";


        private static Game runningGame;
        private static IDisplay displayEngine;
        private static SoundManager soundEngine;
        private static InputSystem input;
        private static PhysicsManager phys;
        private static ThreadManager tm;
        private static RenderManager rm;
        private static Camera camera;

        private static int targetFrameRate;
        private static int millisPerFrame;
        private static double deltaTime;
        private static double timeElapsed;
        private static int frames;
        private static long startTime;
        private static bool running = true; 

        public static double TimeElapsed { get => timeElapsed; set => timeElapsed = value; }

        public static void setup()
        {
            setup(DEFAULT_CONFIG);
        }

        public static double getDeltaTime()
        {

            return deltaTime;
        }

        public static IDisplay getDisplay()
        {
            return displayEngine;
        }

        public static SoundManager getSound()
        {
            return soundEngine;
        }

        public static InputSystem getInput()
        {
            return input;
        }

        public static Game getRunningGame()
        {
            return runningGame;
        }

        public static void setup(string path)
        {
            Dictionary<string, string> config = BaseFunctionality.getInstance().readConfigFile(path);
            Type t;
            object ob;
            bool bailOut = false;

            phys = PhysicsManager.getInstance();

            foreach (KeyValuePair<string, string> kvp in config)
            {
                t = Type.GetType("Shard." + kvp.Value);

                if (t == null)
                {
                    Debug.getInstance().log("Missing Class Definition: " + kvp.Value + " in " + kvp.Key, Debug.DEBUG_LEVEL_ERROR);
                    Environment.Exit(0);
                }

                ob = Activator.CreateInstance(t);


                switch (kvp.Key)
                {
                    case "display":
                        displayEngine = (IDisplay)ob;
                        displayEngine.initialize();
                        break;
                    case "sound":
                        soundEngine = (SoundManager)ob;
                        break;
                    case "game":
                        runningGame = (Game)ob;
                        targetFrameRate = runningGame.getTargetFrameRate();
                        millisPerFrame = 1000 / targetFrameRate;
                        break;
                    case "input":
                        input = (InputSystem)ob;
                        input.initialize();
                        break;
                    case "camera":
                        camera = (Camera)ob;
                        break;
                }
                
                Debug.getInstance().log("Config file... setting " + kvp.Key + " to " + kvp.Value);
            }

            if (runningGame == null)
            {
                Debug.getInstance().log("No game set", Debug.DEBUG_LEVEL_ERROR);
                bailOut = true;
            }

            if (displayEngine == null)
            {
                Debug.getInstance().log("No display engine set", Debug.DEBUG_LEVEL_ERROR);
                bailOut = true;
            }

            if (soundEngine == null)
            {
                Debug.getInstance().log("No sound engine set", Debug.DEBUG_LEVEL_ERROR);
                bailOut = true;
            }

            if (bailOut)
            {
                Environment.Exit(0);
            }
        }

        public static long getCurrentMillis()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public static int getFPS()
        {
            int fps;
            double seconds;

            seconds = (getCurrentMillis() - startTime) / 1000.0;

            fps = (int)(frames / seconds);

            return fps;
        }

        public static int getCurrentFrame()
        {
            return frames;
        }

        // init stuff
        private static void init() {
            PConsole.init();
            tm = ThreadManager.getInstance();
            tm.setMain(Thread.CurrentThread);

            rm = RenderManager.getInstance();
        }


        static void Main(string[] args)
        {
            init();

            long timeInMillisecondsStart, lastTick, timeInMillisecondsEnd;
            long interval;
            int sleep;
            int tfro = 1;
            bool physUpdate = false;
            bool soundUpdate = false;

            // Setup the engine.
            setup();
            

            // When we start the program running.
            startTime = getCurrentMillis();
            frames = 0;
            // Start the game running.
            runningGame.initialize();

            timeInMillisecondsStart = startTime;
            lastTick = startTime;

            phys.Debugging = true;
            phys.GravityModifier = 0.1f;

            // DisplayOpenGL dis = new DisplayOpenGL();
            //dis.initialize();

            RenderManager rm = RenderManager.getInstance();

            float[] vertices = {
             -0.5f, -0.5f, 0.0f, //Bottom-left vertex
              0.5f, -0.5f, 0.0f, //Bottom-right vertex
              0.0f,  0.5f, 0.0f  //Top vertex
             };

            rm.addArrayBuffer("buffer", vertices); 
            rm.addShader("vert", @"D:\chalmers\tda572\shard\1.0.0\Shard\ConsoleApp1\Shard\Graphics\OpenGL\Rendering\defult.vert", Shader.Vertex);
            rm.addShader("frag", @"D:\chalmers\tda572\shard\1.0.0\Shard\ConsoleApp1\Shard\Graphics\OpenGL\Rendering\defult.frag", Shader.Fragment);
            rm.addProgram("prog","vert","frag");

            Test test = new Test();
            



            // This is our game loop.
            while (running)
             {
                 frames += 1;

                timeInMillisecondsStart = getCurrentMillis();
                test.update();
                input.getInput();
                
                
                /*
                                 // Clear the screen.
                                 Bootstrap.getDisplay().clearDisplay();

                                 // Update 
                                 runningGame.update();

                                 // Input

                                 if (runningGame.isRunning() == true)
                                 {

                                     // Get input, which works at 50 FPS to make sure it doesn't interfere with the 
                                     // variable frame rates.
                                     input.getInput();

                                     // Update runs as fast as the system lets it.  Any kind of movement or counter 
                                     // increment should be based then on the deltaTime variable.
                                     GameObjectManager.getInstance().update();

                                     // This will update every 20 milliseconds or thereabouts.  Our physics system aims 
                                     // at a 50 FPS cycle.
                                     if (phys.willTick())
                                     {
                                         GameObjectManager.getInstance().prePhysicsUpdate();
                                     }

                                     // Update the physics.  If it's too soon, it'll return false.   Otherwise 
                                     // it'll return true.
                                     physUpdate = phys.update();

                                     if (physUpdate)
                                     {
                                         // If it did tick, give every object an update
                                         // that is pinned to the timing of the physics system.
                                         GameObjectManager.getInstance().physicsUpdate();
                                     }

                                     phys.drawDebugColliders();

                                     // Sound
                                     // runs at 50 fps at the mometn (same as physics)
                                     soundUpdate = soundEngine.update();
                                     if (soundUpdate)
                                     {
                                         GameObjectManager.getInstance().soundUpdate();
                                     }
                                 }
                */
                // Render the screen.
                Bootstrap.getDisplay().display();



               // PConsole.WriteLine("fps: " + getFPS());

                timeInMillisecondsEnd = getCurrentMillis();

                 interval = timeInMillisecondsEnd - timeInMillisecondsStart;

                 sleep = (int)(millisPerFrame - interval);


                 TimeElapsed += deltaTime;

                 if (sleep >= 0)
                 {
                    // Frame rate regulator.

                    //BusyWait.BusyWaitMS(sleep);
                    BusyWait.SmartWait(sleep);
                    //Thread.Sleep(sleep);
                    
                 }

                 timeInMillisecondsEnd = getCurrentMillis();
                 deltaTime = (timeInMillisecondsEnd - timeInMillisecondsStart) / 1000.0f;

                 millisPerFrame = 1000 / targetFrameRate;

                 lastTick = timeInMillisecondsStart;

                 //                deltaTime = (getCurrentMillis() - timeInMillisecondsStart) / 1000.0f;

             }
          

        }

        internal static Camera getCamera()
        {
            return camera;
        }

        public static bool isRunning() {
            return running;
        }

        internal static void Close()
        {
            running = false;
        }
    }
}
