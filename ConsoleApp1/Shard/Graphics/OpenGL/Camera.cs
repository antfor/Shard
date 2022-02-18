using OpenTK.Mathematics;
using SDL2;
using Shard.Sound;
using Shard.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shard.Graphics;

namespace Shard
{
    class Camera: IListener, InputListener
    {

        Vector2i prevMouseCoords = new Vector2i(-1, -1 );
        Vector2i MouseCoords = new Vector2i(-1, -1);
        bool isMouseDragging = false;
        bool isMouseRightDragging = false;


        Transform3D transform;
        private bool isStatic;
        private float metersPerSecond = 4;
        private float fov = 90f;
        private float nearPlane = 0.001f;
        private float farPlane = 10000;
        private bool space = false;
        float rotation_speed = 2.0f;

        public bool IsStatic { get => isStatic; set => isStatic = value; }

        public Camera() {
            transform = new Transform3D();
            transform.setPos(0, 0, 0);
            isStatic = false;

            Bootstrap.getInput().addListener(this);
            makeListener();
        }

        public void makeListener() {
            Bootstrap.getSound().Listener = this;
        }

       public Matrix4 getViewMatrix() {
            Vector3 target = transform.getPos() + transform.getForward().Xyz;
            return Matrix4.LookAt(transform.getPos(), target, transform.getUp().Xyz);
       }

        public Matrix4 getPerspectiveMatrix() {
            float aspect;
            int w = Bootstrap.getDisplay().Width;
            int h = Bootstrap.getDisplay().Height;
            if (h==0 || w==0) {
                aspect = 1;
            }
            else {
                aspect = w * 1.0f / h;
            }
         
            return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fov), aspect, nearPlane, farPlane);
        }

        public Transform3D getTransform() {
            return transform;
        }

        public void handleInput(InputEvent inp, string eventType)
        {
            if (eventType == "MouseMotion")
            {
                PConsole.WriteLine("mouse" + inp.X);
                MouseCoords.X = inp.X;
                MouseCoords.Y = inp.Y;
                //Vector2i delta = MouseCoords - prevMouseCoords;
               // transform.rotate(transform.getUp().Xyz, -delta.X * rotation_speed);
                
                //prevMouseCoords = MouseCoords;
            }

            if (eventType == "KeyUp")
            {
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE)
                {
                    space = true;
                }
            }
            
        }


        public unsafe void update() {

            InputSystem io = Bootstrap.getInput();
            float dt = (float)Bootstrap.getDeltaTime();
            int x, y;
            io.getMouseState(out x,out y);
            Vector2i delta = new Vector2i(x, y) - prevMouseCoords;

            if (isMouseDragging || true)
            {
               // PConsole.WriteLine("out: " + delta.ToString());
               transform.rotate(transform.getUp().Xyz, rotation_speed * dt );
               //transform.rotate(transform.getRight().Xyz ,rotation_speed * -delta.Y);
               //transform.setRotation(transform.toRad(0,45,0));

            }

            prevMouseCoords = MouseCoords;


            Vector3 dir = new Vector3(0, 0, 0);

            if (io.isDown(SDL.SDL_Scancode.SDL_SCANCODE_W))
            {
                dir.X += 1;
            }

            if (io.isDown(SDL.SDL_Scancode.SDL_SCANCODE_S))
            {
                dir.X -= 1;
            }

            if (io.isDown(SDL.SDL_Scancode.SDL_SCANCODE_D))
            {
                dir.Y += 1;
            }

            if (io.isDown(SDL.SDL_Scancode.SDL_SCANCODE_A))
            {
                dir.Y -= 1;
            }

            if (io.isDown(SDL.SDL_Scancode.SDL_SCANCODE_Q))
            {
                dir.Z += 1;
            }
            if (io.isDown(SDL.SDL_Scancode.SDL_SCANCODE_E))
            {
                dir.Z -= 1;
            }
            //PConsole.WriteLine("1:" + dir.ToString());
            
            if (dir.Length != 0)
                dir.Normalize();
            dir *= metersPerSecond * dt;
            // PConsole.WriteLine("2:" + dir.ToString());
            transform.moveForward(dir.X);
            transform.moveRight(dir.Y);
            transform.moveUp(dir.Z);

            if (space) {
                space = false;
                transform.Y = 1.8f;
            }            
        }

        public Vector3 getPos()
        {
            return transform.getPos();
        }

        public Vector3 getVel()
        {
            return new Vector3(0,0,0); // todo
        }

        public Vector3 getDir()
        {
            return transform.getForward().Xyz;
        }

        public Vector3 getUp()
        {
            return transform.getUp().Xyz;
        }
    }
}
