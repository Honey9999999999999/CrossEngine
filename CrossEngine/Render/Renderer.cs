using System.Numerics;

using static CrossEngine.Render.ConsoleOutput;

namespace CrossEngine.Render
{
    public class Renderer
    {
        private char symbol;
        private readonly char[] gradient = ['`', '.', ';', 'I', 'S', 'O', '%', '&', '@'];

        //const float MaxDrawDistance = 9999;
        //const float minShadow = 10;

        private readonly CharInfo[] buffer;

        private readonly ConsoleScreen screen;
        private readonly Camera camera;

        public Renderer(ConsoleScreen screen, ref Camera camera)
        {
            this.camera = camera;
            this.screen = screen;

            buffer = new CharInfo[screen.Width * screen.Height];
            camera.RayLength = screen.AspectRatio / 2;
        }

        public CharInfo[] Render(List<IRayCastable> gameObjects)//, List<ILightSource> light)
        {
            for (int i = 0; i < screen.Width; i++)
            {
                for (int j = 0; j < screen.Height; j++)
                {
                    //Parallel.For(0, screen.Width * screen.Height, i =>
                    //{
                    //int j = i / screen.Width; i %= screen.Width;

                    float u = i * 2.0f / screen.Width - 1,
                          v = j * 2.0f / screen.Height - 1;
                    Vector2 uv = new(u * screen.AspectRatio / screen.SymbolAspectRatio, v);

                    Ray ray = new(
                        camera.Transform.Position,
                        Vector3.Transform(new Vector3(uv, camera.RayLength), camera.Transform.RotationMatrix)
                    );

                    Ray.Hit hit = Ray.Hit.Miss;
                    foreach (IRayCastable obj in gameObjects)
                        hit = hit.Closest(obj.Cast(ray));

                    if (i == screen.Width / 2 && j == screen.Height / 2)
                        _ = "STOP HERE";

                    float light = Vector3.Dot(hit.Normal, Vector3.UnitZ) / 2 + 0.5f;
                    symbol = gradient[(int)(light * (gradient.Length - 1))];

                    SetPixel(i, j, hit.Object != null ? ConsoleColor.Red : ConsoleColor.DarkBlue);
                    //});
                }
            }
            return buffer;
        }


        //public CharInfo[] DrawImage(ConsoleScreen screen, List<GameObject> gameObjects, List<Light3D> lights3D)
        //{
        //    for (int i = 0; i < width; i++)
        //    {
        //        for (int j = 0; j < height; j++)
        //        {
        //            Vector2 UV = new(((float)i / width * 2.0f - 1.0f) * screen.sympolAspect * screen.aspect, (float)j / height * 2.0f - 1.0f);
        //            Vector3 rayDirection = Vector3.Normalize(Ray.GetRayDirection(new Vector3(UV, camera.rayLength), camera.transform.rotation));

        //            Ray ray = new(camera.transform.position, rayDirection);

        //            ray.TryGetNearInterSection(gameObjects, out float distanceToInterSection);

        //            MeshObject drawbleObject = ray.GetNearObject();
        //            drawbleObject.SetNormalSurface(ray, distanceToInterSection);

        //            float distanceToObstacle = MaxDrawDistance;
        //            List<Light3D> li = new();

        //            if (distanceToInterSection < MaxDrawDistance)
        //            {
        //                ray = new Ray(ray.startPos + ray.direction * (distanceToInterSection - 0.001f), ray.direction);
        //                distanceToObstacle = GetNearObstacle(ray, lights3D, gameObjects);
        //            }

        //            SetBrightnessLvl(drawbleObject, lights3D, distanceToObstacle);
        //            SetPixel(i, j, drawbleObject.material.color);
        //        }
        //    }
        //    return buffer;
        //}

        //private float GetNearObstacle(Ray ray, List<Light3D> lights3D, List<MeshObject> gameObjects)
        //{
        //    float distanceToObstacle = MaxDrawDistance;

        //    foreach (Light3D light in lights3D)
        //    {
        //        ray = light.GetReverseRayLight(ray.startPos);
        //        bool isShadow = true;

        //        if (isShadow && ray.TryGetNearInterSection(gameObjects, out float _distanceToObstacle))
        //            if (ray.GetNearInterSection() < MaxDrawDistance)
        //                distanceToObstacle = distanceToObstacle > _distanceToObstacle ? _distanceToObstacle : distanceToObstacle;
        //            else
        //            {
        //                isShadow = false;
        //                distanceToObstacle = MaxDrawDistance;
        //            }
        //    }

        //    return distanceToObstacle;
        //}

        //private void SetBrightnessLvl(GameObject meshObject, List<Light3D> lights, float distanceToObstacle)
        //{
        //    float lightLevel = meshObject.GetSurfaceIlluminationLevel(lights) * gradient.Length;

        //    if (distanceToObstacle < MaxDrawDistance)
        //        lightLevel -= gradient.Length * minShadow / distanceToObstacle;

        //    brightness = gradient[Math.Clamp((int)Math.Ceiling(lightLevel), 0, gradient.Length - 1)];
        //}

        private void SetPixel(int x, int y, ConsoleColor color)
        {
            buffer[screen.Width * y + x].Char.UnicodeChar = symbol;
            buffer[screen.Width * y + x].Attributes = (short)color;
        }
    }
}
