using OpenTK.Mathematics;

namespace Shard
{
    class Transform3D
    {
        private Matrix4 modelMatrix = Matrix4.Identity;
        
        private Quaternion rot = Quaternion.Identity;

        private Vector4 forward = new Vector4(0, 0, -1, 0);
        private Vector4 right = new Vector4(1, 0, 0, 0);
        private Vector4 up = new Vector4(0, 1, 0, 0);

        public float X { get => getPos().X; set => setPos(value, getPos().Y, getPos().Z); }
        public float Y { get => getPos().Y; set => setPos(getPos().X, value, getPos().Z); }
        public float Z { get => getPos().Z; set => setPos(getPos().X, getPos().Y, value); }

        public Transform3D(GameObject parent) { 
        
        }

        public Transform3D()
        {

        }

        public void setForward(Vector4 forward) {
            this.forward = forward.Normalized();
            right = new  Vector4(Vector3.Cross(up.Xyz, forward.Xyz), 0);
        }
        public void setUP(Vector4 up)
        {
            this.up = up.Normalized();
            right = new Vector4(Vector3.Cross(up.Xyz, forward.Xyz), 0);
        }

        public Matrix4 getModelMatrix()
        {
            return modelMatrix;
        }

        public Vector4 getForward() {
            return (rot * forward).Normalized();
        }

        public Vector4 getUp()
        {
            return (rot * up).Normalized();
        }

        public Vector4 getRight()
        {
            return (rot * right).Normalized();
        }

        // scale
        public void setScale(float x, float y, float z) {
            modelMatrix = modelMatrix.ClearScale() * Matrix4.CreateScale(x,y,z);
        }

        public void scale(float x, float y, float z) { 
            modelMatrix *= Matrix4.CreateScale(x, y, z);
        }

        public Vector3 getScale() {
            return modelMatrix.ExtractScale();
        }


        // translate 
        public void translate(float x, float y, float z) {
            modelMatrix *= Matrix4.CreateTranslation(x,y,z);
        }

        public void setPos(float x, float y, float z)
        {
            modelMatrix = modelMatrix.ClearTranslation() * Matrix4.CreateTranslation(x, y, z);
        }

        public Vector3 getPos()
        {
            return modelMatrix.ExtractTranslation();
        }

        public void moveForward(float dist) {
            Vector4 move = getForward() * dist;
            translate(move.X,move.Y,move.Z);
        }

        public void moveUp(float dist)
        {
            Vector4 move = getUp() * dist;
            translate(move.X, move.Y, move.Z);
        }

        public void moveRight(float dist)
        {
            Vector4 move = getRight() * dist;
            translate(move.X, move.Y, move.Z);
        }
        // rot
        public void rotate(float x, float y, float z) {
            rot = Quaternion.Multiply(rot, Quaternion.FromEulerAngles(x, y, z));
            rot.Normalize();
            modelMatrix = modelMatrix.ClearRotation() * Matrix4.CreateFromQuaternion(rot);
        }

        public void rotate(Vector3 axis, float deg) {
            rot = Quaternion.Multiply(rot, Quaternion.FromAxisAngle(axis, deg));
            rot.Normalize();
            modelMatrix = modelMatrix.ClearRotation() * Matrix4.CreateFromQuaternion(rot);
        }

        public void setRotation(float x, float y, float z)
        {
            rot = Quaternion.FromEulerAngles(x, y, z);
            rot.Normalize();
            modelMatrix = modelMatrix.ClearRotation() * Matrix4.CreateFromQuaternion(rot);
        }

        public void setRotation(Vector3 axis, float deg)
        {
            rot = Quaternion.FromAxisAngle(axis, deg);
            rot.Normalize();
            modelMatrix = modelMatrix.ClearRotation() * Matrix4.CreateFromQuaternion(rot);
        }

        public Vector3 getRotation() {
            return modelMatrix.ExtractRotation().ToEulerAngles();
        }


        public Vector3 toRad(float x, float y, float z) {
            return new Vector3(MathHelper.DegreesToRadians(x), MathHelper.DegreesToRadians(y), MathHelper.DegreesToRadians(z));
        }

    }
}
