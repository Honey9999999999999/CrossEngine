using System.Numerics;

namespace CrossEngine
{
    public class Transform
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation
        {
            get => _rotation;
            set
            {
                _rotation = new Vector3(value.X % MathF.Tau,
                                        value.Y % MathF.Tau,
                                        value.Z % MathF.Tau);

                Matrix4x4 x = Matrix4x4.CreateRotationX(Rotation.X);
                Matrix4x4 y = Matrix4x4.CreateRotationY(Rotation.Y);
                Matrix4x4 z = Matrix4x4.CreateRotationZ(Rotation.Z);

                RotationMatrix = x * y * z;
            }
        }
        public Matrix4x4 RotationMatrix { get; private set; }
        public Vector3 Scale { get; set; }

        private Vector3 _rotation;

        public Transform() : this(Vector3.Zero, Vector3.Zero, Vector3.One) { }
        public Transform(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;
        }
    }
}
