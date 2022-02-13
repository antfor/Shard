using OpenTK.Mathematics;
using SDL2;
using Shard;
using System;
using System.Drawing;


namespace GameTest
{
    class Spaceship : GameObject, InputListener, CollisionHandler, ISoundSourceObject
    {
        bool up, down, turnLeft, turnRight;

        float w = Bootstrap.getDisplay().getWidth();
        float h = Bootstrap.getDisplay().getHeight();
        float sw = 32;
        float sh = 64;
        float dw;
        float dh;
        float cw;
        float ch;

        public override void initialize()
        {

            dw = (w - sw);
            dh = (h - sh);
            cw = dw / 2.0f;
            ch = dh / 2.0f; 
            this.Transform.X = cw;
            this.Transform.Y = ch;
            this.Transform.SpritePath = "spaceship.png";


            Bootstrap.getInput().addListener(this);

            up = false;
            down = false;

            setPhysicsEnabled();

            MyBody.Mass = 4;
            MyBody.MaxForce = 10;
            MyBody.AngularDrag = 0.01f;
            MyBody.Drag = 0f;
            MyBody.UsesGravity = false;
            MyBody.StopOnCollision = false;
            MyBody.ReflectOnCollision = false;
            MyBody.ImpartForce = true;


            MyBody.PassThrough = false;
//            MyBody.addCircleCollider(0, 0, 5);
//            MyBody.addCircleCollider(0, 34, 5);
//            MyBody.addCircleCollider(60, 18, 5);
            MyBody.addCircleCollider();


            //Sound
            SoundManager soundManeger = Bootstrap.getSound();
            soundManeger.addSound("engine", @"D:\chalmers\tda572\music\spaceship.wav");

            SoundSource soundPlayer = new SoundSource(this);
            soundManeger.loadSource(soundPlayer, "engine");
            soundPlayer.loop(true);
            soundPlayer.setGain(8);
            soundPlayer.play();
         

            addTag("Spaceship");


        }

        public void fireBullet()
        {
            Bullet b = new Bullet();

            b.setupBullet(this, this.Transform.Centre.X, this.Transform.Centre.Y);

            b.Transform.rotate(this.Transform.Rotz);
        }

        public void handleInput(InputEvent inp, string eventType)
        {



            if (eventType == "KeyDown")
            {
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_W)
                {
                    up = true;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_S)
                {
                    down = true;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
                {
                    turnRight = true;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
                {
                    turnLeft = true;
                }

            }
            else if (eventType == "KeyUp")
            {
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_W)
                {
                    up = false;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_S)
                {
                    down = false;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
                {
                    turnRight = false;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
                {
                    turnLeft = false;
                }


            }



            if (eventType == "KeyUp")
            {
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE)
                {
                    fireBullet();
                }
            }
        }

        public override void physicsUpdate()
        {

            if (turnLeft)
            {
                MyBody.addTorque(-0.3f);
            }

            if (turnRight)
            {
                MyBody.addTorque(0.3f);
            }

            if (up)
            {

                MyBody.addForce(this.Transform.Forward, 0.5f);

            }

            if (down)
            {
                MyBody.addForce(this.Transform.Forward, -0.2f);
            }


        }

        public override void update()
        {
            Bootstrap.getDisplay().addToDraw(this);
        }

        public void onCollisionEnter(PhysicsBody x)
        {
            if (x.Parent.checkTag("Bullet") == false)
            {
                MyBody.DebugColor = Color.Red;
            }
        }

        public void onCollisionExit(PhysicsBody x)
        {

            MyBody.DebugColor = Color.Green;
        }

        public void onCollisionStay(PhysicsBody x)
        {
            MyBody.DebugColor = Color.Blue;
        }

        public override string ToString()
        {
            return "Spaceship: [" + Transform.X + ", " + Transform.Y + ", " + Transform.Wid + ", " + Transform.Ht + "]";
        }

        public Vector3 getSoundPos()
        {
           
            float scale = 0.01f;

            float x = scale * ((float)this.Transform.X - cw);
            float y = scale * ((float)this.Transform.Y - ch);
            //float z = scale * ((float)this.Transform.Z);
            Console.WriteLine("pos: x=" + x + " y= " + y + " z= " + 0);
            return new Vector3(x,y,0);
        }

        public Vector3 getSoundVel()
        {
            return new Vector3(0, 0, 0);
        }
    }
}
