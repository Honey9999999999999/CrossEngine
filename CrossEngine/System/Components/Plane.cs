using CrossEngine.Render;
using CrossEngine.System;

using System.Numerics;

namespace CrossEngine.Objects
{
    internal class Plane : Component, IRayCastable
    {
        public Ray.Hit Cast(Ray ray)
        {
            // Vector3 Intersect(Vector3 planeP, Vector3 planeN, Vector3 rayP, Vector3 rayD)
            var d = Vector3.Dot(Transform.Position, ray.Direction);
            var t = d + Vector3.Dot(ray.Origin, -ray.Direction);
            Vector3 point = ray.Origin + t * ray.Direction;
            return new() { Point = point };

            //Vector3 Intersect(Vector3 planeP, Vector3 planeN, Vector3 rayP, Vector3 rayD)
            //{
            //    var d = Vector3.Dot(planeP, -planeN);
            //    var t = -(d + Vector3.Dot(rayP, planeN)) / Vector3.Dot(rayD, planeN);
            //    return rayP + t * rayD;
            //}
        }

        public Bounds Bounds { get; }
    }
}
