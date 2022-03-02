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
using Shard.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Shard
{
    class Camera: IListener, InputListener
    {

        Vector2 prevMouseCoords = new Vector2i(-1, -1 );
        Vector2 MouseCoords = new Vector2i(-1, -1);
        bool isMouseDragging = false;
        bool isMouseRightDragging = false;

        private Vector3 cameraDirection = new Vector3(0, 0, -1);
        private Vector3 worldUP = new Vector3(0,1,0);
        private float pitchAngle = 0;


        Transform3D transform;
        private bool isStatic;
        private float metersPerSecond = 4;
        private float fov = 90f;
        private float nearPlane = 0.001f;
        private float farPlane = 10000;
        private bool space = false;
        float rotation_speed = 1.0f;

        public bool IsStatic { get => isStatic; set => isStatic = value; }

        public Camera() {
            transform = new Transform3D();
            transform.setPos(0, 0, 0);
            isStatic = false;

            //Bootstrap.getInput().addListener(this);
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
   
        }

        private Vector3 right() {
            return (Vector3.Cross(transform.getForward().Xyz, worldUP)).Normalized();
        }

        public unsafe void update() {


            WindowOpenGL wo = ((DisplayOpenGL)Bootstrap.getDisplay()).getWindow();
            KeyboardState ks = wo.getKeyboardState();
            MouseState ms = wo.getMouseState();
            
            float dt = (float)Bootstrap.getDeltaTime();
     
            MouseCoords = ms.Position;
            Vector2 delta = MouseCoords - prevMouseCoords;


            Matrix4 yaw = Matrix4.CreateFromAxisAngle(worldUP, delta.X * rotation_speed * dt);

            float angle =  delta.Y * rotation_speed * dt;

            if (angle + pitchAngle > MathHelper.DegreesToRadians(89))
            {
                angle = MathHelper.DegreesToRadians(89) - pitchAngle;
         
            } else if (angle + pitchAngle < MathHelper.DegreesToRadians(-89)) {
                angle = MathHelper.DegreesToRadians(-89) - pitchAngle;
            }

            pitchAngle += angle;
            Matrix4 pitch = Matrix4.CreateFromAxisAngle(Vector3.Cross(cameraDirection, worldUP) , angle);
                
            cameraDirection = new Vector3(pitch * yaw  * new Vector4(cameraDirection, 0.0f));

            transform.setForward(cameraDirection);

          

            prevMouseCoords = MouseCoords;


            Vector3 dir = new Vector3(0, 0, 0);
            
            if (ks.IsKeyDown(Keys.W))
            {
                dir.X += 1;
            }

            if (ks.IsKeyDown(Keys.S))
            {
                dir.X -= 1;
            }

            if (ks.IsKeyDown(Keys.D))
            {
                dir.Y -= 1;
            }

            if (ks.IsKeyDown(Keys.A))
            {
                dir.Y += 1;
            }

            if (ks.IsKeyDown(Keys.Q))
            {
                dir.Z += 1;
            }
            if (ks.IsKeyDown(Keys.E))
            {
                dir.Z -= 1;
            }
            
            if (dir.Length != 0)
                dir.Normalize();
            dir *= metersPerSecond * dt;


            if (ms.IsButtonDown(MouseButton.Middle))
            {
                transform.moveForward(dir.X);
            }
            else
            {
                Vector3 forward = transform.getForward().Xyz;
                Vector3 v = new Vector3(forward.X, 0, forward.Z);
                v.Normalize();
                v *= dir.X;
                transform.translate(v);

            }

            transform.moveRight(dir.Y);
            transform.moveUp(dir.Z);




            if (ks.IsKeyDown(Keys.Space)) {
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
