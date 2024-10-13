using System.Numerics;

namespace CrossEngine.Render
{
    public struct Ray(Vector3 origin, Vector3 direction)
    {
        public Vector3 Origin = origin, Direction = Vector3.Normalize(direction);

        public readonly Vector3 this[float distance] => Origin + Direction * distance;
        public readonly Vector3 PointAt(float distance) => Origin + Direction * distance;

        public readonly Hit Cast(IRayCastable other) => other.Cast(this);
        public static Hit Cast(Ray ray, IRayCastable other) => other.Cast(ray);

        public struct Hit
        {
            public Vector3 Point;
            public Vector3 Normal;
            public Vector2 Texture;
            public object? Object;
            public float Distance;

            public static readonly Hit Miss = new() { Distance = float.NaN };

            public readonly Hit Closest(Hit other) =>
                other.Distance < 0 ? this :
                other.Distance < this.Distance ? other :
                this.Distance < 0 ? other : this;
        }
    }

    public interface IRayCastable
    {
        public Ray.Hit Cast(Ray ray);
    }
}
