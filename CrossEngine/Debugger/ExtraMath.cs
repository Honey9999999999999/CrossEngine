using System.Numerics;

namespace CrossEngine.Debugger
{
    public static class ExtraMath
    {
        public static bool Quadratic(float a, float b, float c, out float x, out float y)
        {
            float D = b * b - 4 * a * c;
            a = 0.5f / a;

            switch (D)
            {
                case > 0:
                    x = (MathF.Sqrt(D) - b) * a;
                    y = (-MathF.Sqrt(D) - b) * a;
                    if (x > y) (x, y) = (y, x); // swap
                    break;
                case 0:
                    x = y = -b * a;
                    break;
                default:
                case < 0:
                    x = y = float.NaN;
                    return false;
            }

            return true;
        }

        private const float rad_to_deg = 180 / MathF.PI;
        private const float deg_to_rad = MathF.PI / 180;
        public static Vector3 RadToDeg(this Vector3 vector) => vector * rad_to_deg;
        public static Vector3 DegToRad(this Vector3 vector) => vector * deg_to_rad;
    }
}
