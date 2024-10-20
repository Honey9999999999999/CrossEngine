using CrossEngine.System;
using CrossEngine.System.Components;
using System.Numerics;

namespace CrossEngine
{
    public class Transform : Component
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
        private Transform[] _childs;



        public Transform() : this(Vector3.Zero, Vector3.Zero, Vector3.One) { }
        public Transform(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;

            _childs = [];
        }



        public Transform[] GetChilds() => _childs;
        public void AddChild(Transform transform)
        {
            _childs = this != transform 
                ? [.. _childs, transform]
                : throw new CrossException("GameObject cannot contain itself");
        }
    }
}
