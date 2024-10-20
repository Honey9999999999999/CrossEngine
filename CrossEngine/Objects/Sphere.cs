using CrossEngine.Render;
using CrossEngine.System;
using System.Numerics;

namespace CrossEngine.Objects
{
    public class Sphere : Component, IRayCastable
    {
        public Ray.Hit Cast(Ray ray)
        {
            ray.Origin /= Transform.Scale;
            ray.Direction *= Transform.Scale;
            ray.Direction = Vector3.Normalize(ray.Direction);

            Vector3 offset = ray.Origin - Transform.Position;
            float Z = Vector3.Dot(offset, ray.Direction);
            float D = Vector3.Dot(offset, offset) - 1;
            float diff = Z * Z - D;

            if (diff < 0) return Ray.Hit.Miss;
            diff = MathF.Sqrt(diff);
            float distance = -Z - diff;
            if (distance < 0) return Ray.Hit.Miss;

            Vector3 point = ray[distance];

            return new Ray.Hit
            {
                Distance = distance,
                Point = point,
                Normal = Vector3.Normalize(point - Transform.Position),
                Object = this
            };
        }

        public Bounds Bounds => new(Transform.Position, Transform.Scale);
    }
}
