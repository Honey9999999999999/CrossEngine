namespace CrossEngine.Render
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
    }
}
