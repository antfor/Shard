using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Shard.DemoGame;
using Shard.DemoGame.Floor;
using Shard.Graphics.OpenGL;

namespace Shard
{
    class Demo : Game, InputListener
    {
        private Floor floor;
        private Test test;
        private SoundStatic soundPlayer;

        public void handleInput(InputEvent inp, string eventType)
        {
            
        }

        public override void initialize()
        {
            floor = new Floor();

            test = new Test();

            Bootstrap.getCamera().getTransform().setPos(0,1.8f,4);

            //Sound
            SoundManager soundManager = Bootstrap.getSound();
            soundManager.addSound("doom", @"D:\chalmers\tda572\music\doomMono.wav");

            soundPlayer = new SoundStatic();
            soundManager.loadSource(soundPlayer, "doom");
            soundPlayer.setGain(0.1f);
            soundPlayer.setRolloff(0.0f);


            setFPS(50);
        }

        public override void update()
        {
            input();
        }

        private void input() {

            start += Bootstrap.getDeltaTime();

            if (willTick() == false)
            {
                return;
            }

            WindowOpenGL wo = ((DisplayOpenGL)Bootstrap.getDisplay()).getWindow();
            KeyboardState ks = wo.getKeyboardState();
            MouseState ms = wo.getMouseState();


            if (ks.IsKeyDown(Keys.P))
            {
                if (soundPlayer.Playing)
                {
                    soundPlayer.pause();
                }
                else
                {
                    soundPlayer.unPause();
                }

            }

            if (ks.IsKeyDown(Keys.O))
            {
                soundPlayer.setGain(soundPlayer.Gain + soundPlayer.Gain * 0.1f);
            }

            if (ks.IsKeyDown(Keys.I))
            {
                soundPlayer.setGain(soundPlayer.Gain - soundPlayer.Gain * 0.1f);
            }

            if (ks.IsKeyDown(Keys.L))
            {
                soundPlayer.setPitch(1.5f);
            }

            if (ks.IsKeyDown(Keys.K))
            {
                soundPlayer.setPitch(1);
            }

            if (ks.IsKeyDown(Keys.J))
            {
                soundPlayer.setPitch(0.5f);
            }


            start -= TimeInterval;
        }

        private double start;
        private double TimeInterval;
        public void setFPS(int fps)
        {
            TimeInterval = 1.0 / (double)fps;
        }

        public bool willTick()
        {
            if (start < TimeInterval)
            {
                return false;
            }

            return true;
        }
    }
}
