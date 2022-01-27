using Shard;

namespace MissileCommand
{
    class ArsenalSprite : GameObject
    {

        public override void initialize()
        {


            this.Transform.X = 200.0f;
            this.Transform.Y = 100.0f;
            this.Transform.SpritePath = "missile.png";

        }

        public override void update()
        {
            Bootstrap.getDisplay().addToDraw(this);
        }

    }
}
