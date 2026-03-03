using Shard;

namespace GameTest
{
    class Asteroid : GameObject, CollisionHandler, InputListener
    {
        int torqueCounter = 0;
        public void handleInput(InputEvent inp, string eventType)
        {

            if (eventType == "MouseDown" && inp.Button == 2)
            {
                if (MyBody.checkCollisions(new Shard.Vector(inp.X, inp.Y)) != null)
                {
                    torqueCounter += 10;
                }
            }


        }

        public override void initialize()
        {
            this.Transform.SpritePath = "asteroid.png";

            setPhysicsEnabled();

            MyBody.MaxTorque = 100;
            MyBody.Mass = 1;
            MyBody.AngularDrag = 0.1f;
            MyBody.MaxForce = 100;
            MyBody.StopOnCollision = false;
            MyBody.ImpartForce = true;
            MyBody.ReflectOnCollision = true;


            MyBody.addCircleCollider(32, 32, 30);
  //          MyBody.addRectCollider();
            Bootstrap.getInput().addListener(this);

            addTag("Asteroid");

        }


        public override void physicsUpdate()
        {
            for (int i = 0; i < torqueCounter; i++)
            {
                MyBody.addTorque(0.1f);
            }

            if (torqueCounter > 0)
            {
                torqueCounter -= 1;
            }
            //            MyBody.addForce(this.Transform.Forward, 0.1f);
        }

        public override void update()
        {
            Bootstrap.getDisplay().addToDraw(this);
        }

        public void onCollisionEnter(PhysicsBody x)
        {
            if (x.Parent.checkTag("Bullet") == true)
            {
                ToBeDestroyed = true;
                Debug.getInstance().log("Boom");
            }

        }

        public void onCollisionExit(PhysicsBody x)
        {
        }

        public void onCollisionStay(PhysicsBody x)
        {
        }

        public override string ToString()
        {
            return "Asteroid: [" + Transform.X + ", " + Transform.Y + ", " + Transform.Wid + ", " + Transform.Ht + "]";
        }
    }
}
