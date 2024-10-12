using System.Numerics;
using CrossEngine.Render;

namespace CrossEngine.Objects
{
    public class Sphere : GameObject, IRayCastable
    {
        public Ray.Hit Cast(Ray ray)
        {
            ray.Direction *= Transform.Scale;
            Vector3 offset = ray.Origin - Transform.Position;
            float Z = Vector3.Dot(offset, ray.Direction);
            float D = Vector3.Dot(offset, offset) - 1;
            float diff = Z * Z - D;

            if (diff < 0) return Ray.Hit.Miss;
            diff = MathF.Sqrt(diff);
            float distance = -Z - diff;

            Vector3 point = ray[distance];

            return new Ray.Hit
            {
                Distance = distance,
                Point = point,
                Normal = Vector3.Normalize(point - Transform.Position),
                Object = this
            };
        }
    }
}
