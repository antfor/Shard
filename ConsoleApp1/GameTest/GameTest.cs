using GameTest;
using SDL2;
using System;
using System.Drawing;

namespace Shard
{
    class GameTest : Game, InputListener
    {
        GameObject background;
        SoundStatic sound; 
        public override void update()
        {

            Bootstrap.getDisplay().addToDraw(background);

            Bootstrap.getDisplay().showText("FPS: " + Bootstrap.getFPS(), 10, 10, 12, 255, 255, 255);


        }

        public void createShip()
        {
            GameObject ship = new Spaceship();
            Random rand = new Random();
            int offsetx = 0, offsety = 0;

            GameObject asteroid;


            asteroid = new Asteroid();
            asteroid.Transform.translate(500 + 100, 500);
//            asteroid.MyBody.Kinematic = true;
     


            background = new GameObject();
            background.Transform.SpritePath = "background2.jpg";
            background.Transform.X = 0;
            background.Transform.Y = 0;


        }

        public override void initialize()
        {
            Bootstrap.getInput().addListener(this);
            createShip();
            SoundManager soundManager = Bootstrap.getSound();
            soundManager.addSound("doom", @"D:\chalmers\tda572\music\doomMono.wav");
            
            sound = new SoundStatic();
            soundManager.loadSource(sound, "doom");

        }

        public void handleInput(InputEvent inp, string eventType)
        {


            if (eventType == "MouseDown" && inp.Button == 1)
            {
                Asteroid asteroid = new Asteroid();
                asteroid.Transform.X = inp.X;
                asteroid.Transform.Y = inp.Y;
            }

            if (eventType == "KeyDown")
            {

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_P)
                {
                    if (sound.Playing)
                    {
                        sound.pause();
                    }
                    else
                    {
                        sound.unPause();
                    }

                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_O)
                {
                    sound.setGain(sound.Gain + sound.Gain * 0.1f);
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_I)
                {
                    sound.setGain(sound.Gain - sound.Gain * 0.1f);
                }                   

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_L)
                {
                    sound.setPitch(1.5f);
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_K)
                {
                    sound.setPitch(1);
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_J)
                {
                    sound.setPitch(0.5f);
                }
            }
        }   
    }
}
