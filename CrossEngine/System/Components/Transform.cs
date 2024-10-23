using CrossEngine.System;
using System.Numerics;

namespace CrossEngine
{
    public class Transform : Component
    {
        public Vector3 Position
        {
            get => _position;
            set
            {
                Vector3 difference = _position - value;
                _position = value;

                foreach (var transform in _childs)
                {
                    transform.Position -= difference;
                }
            }
        }
        private Vector3 _position;


        public Vector3 Rotation
        {
            get => _rotation;
            set
            {
                _rotation = new Vector3(value.X % MathF.Tau,
                                        value.Y % MathF.Tau,
                                        value.Z % MathF.Tau);

                Matrix4x4 x = Matrix4x4.CreateRotationX(Rotation.X),
                          y = Matrix4x4.CreateRotationY(Rotation.Y),
                          z = Matrix4x4.CreateRotationZ(Rotation.Z);

                RotationMatrix = x * y * z;
            }
        }
        private Vector3 _rotation;

        public Matrix4x4 RotationMatrix { get; private set; }
        public Vector3 Scale { get; set; }
        public Vector3 Forward => Vector3.TransformNormal(Vector3.UnitZ, RotationMatrix);

        public Transform Parent
        {
            get => _parent;
            set
            {
                _parent = this != value
                ? value
                : throw new CrossException("GameObject cannot be parent itself");
                _parent.AddChild(this);
            }
        }
        public int ChildCount => _childs.Length;

        private Transform _parent;
        private Transform[] _childs;



        public Transform() : this(Vector3.Zero, Vector3.Zero, Vector3.One) { }
        public Transform(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            _childs = [];
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }



        public Transform[] GetChilds() => _childs;

        public void AddChild(Transform transform)
        {
            _childs = this != transform
                ? [.. _childs, transform]
                : throw new CrossException("GameObject cannot contain itself");
            transform._parent = Transform;
        }
    }
}
