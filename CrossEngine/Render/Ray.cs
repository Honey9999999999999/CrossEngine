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

            public static readonly Hit Miss = new() { Distance = float.PositiveInfinity };

            public readonly Hit Closest(Hit other)
            {
                if (other.Distance == float.PositiveInfinity) return this;
                if (this.Distance == float.PositiveInfinity) return other;

                if (other.Distance <= 0) return this;
                if (this.Distance <= 0) return other;

                if (other.Distance < this.Distance) return other;

                return this;
            }
        }
    }

    public interface IRayCastable
    {
        public Ray.Hit Cast(Ray ray);
        public Bounds Bounds { get; }
    }

    public struct Bounds(Vector3 center, Vector3 size)
    {
        public Vector3 Center { readonly get; private set; } = center;
        public Vector3 Size { readonly get; private set; } = size;

        public readonly Vector3 Min => Center - Size * 0.5f;
        public readonly Vector3 Max => Center + Size * 0.5f;

        public readonly Bounds Translate(Vector3 offset) => new(Center + offset, Size);
        public readonly Bounds Extend(Vector3 extend) => new(Center + extend / Size, Size + extend / Size);
        public readonly Bounds Scale(Vector3 scale) => new(Center, Size + scale);

        public static Bounds Translate(Bounds bound, Vector3 offset) => bound.Translate(offset);
        public static Bounds Extend(Bounds bound, Vector3 extend) => bound.Extend(extend);
        public static Bounds Scale(Bounds bound, Vector3 scale) => bound.Scale(scale);
    }
}
