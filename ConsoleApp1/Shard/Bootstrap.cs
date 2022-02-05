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

using Shard.Sound;

namespace Shard
{
    class Bootstrap
    {
        public static string DEFAULT_CONFIG = "config.cfg";


        private static Game runningGame;
        private static Display displayEngine;
        private static SoundManager soundEngine;
        private static InputSystem input;
        private static PhysicsManager phys;

        private static int targetFrameRate;
        private static int millisPerFrame;
        private static double deltaTime;
        private static double timeElapsed;
        private static int frames;
        private static long startTime;

        public static double TimeElapsed { get => timeElapsed; set => timeElapsed = value; }

        public static void setup()
        {
            setup(DEFAULT_CONFIG);
        }

        public static double getDeltaTime()
        {

            return deltaTime;
        }

        public static Display getDisplay()
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
                        displayEngine = (Display)ob;
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

        static void Main(string[] args)
        {
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

            DisplayOpenGL dis = new DisplayOpenGL();
            dis.initialize();
            
             // This is our game loop.
             while (true)
             {
                 frames += 1;

                 timeInMillisecondsStart = getCurrentMillis();
                //Console.WriteLine("då");
                //Console.WriteLine(Bootstrap.getDeltaTime());
                /*
                                 // Clear the screen.
                                 Bootstrap.getDisplay().clearDisplay();

                                 // Update 
                                 runningGame.update();
                                 // Input

                                 //soundEngine.addSound("doom", @"D:\chalmers\tda572\music\ex.wav");
                                 //SoundSource  s = new SoundSource();
                                 //soundEngine.addSource(s);
                                 //soundEngine.loadSource(s, "doom");
                                 //s.play();

                                 // test


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

                                 // Render the screen.
                                 Bootstrap.getDisplay().display();
                                  */
                
               dis.display();

               // Console.Clear();
               Console.WriteLine("fps: " + getFPS());
                

                timeInMillisecondsEnd = getCurrentMillis();

                 interval = timeInMillisecondsEnd - timeInMillisecondsStart;

                 sleep = (int)(millisPerFrame - interval);


                 TimeElapsed += deltaTime;

                 if (sleep >= 0)
                 {
                    //Console.WriteLine("hej");
                     // Frame rate regulator.
                     Thread.Sleep(sleep);
                 }

                 timeInMillisecondsEnd = getCurrentMillis();
                 deltaTime = (timeInMillisecondsEnd - timeInMillisecondsStart) / 1000.0f;

                 millisPerFrame = 1000 / targetFrameRate;

                 lastTick = timeInMillisecondsStart;

                 //                deltaTime = (getCurrentMillis() - timeInMillisecondsStart) / 1000.0f;

             }
          

        }
    }
}
