using System.Numerics;

using static CrossEngine.Render.ConsoleOutput;

namespace CrossEngine.Render
{
    public class Render
    {
        private char brightness;
        private readonly char[] gradient = ['`', '.', ';', 'I', 'S', 'O', '%', '&', '@'];

        const float MaxDrawDistance = 9999;
        const float minShadow = 10;

        private readonly CharInfo[] buffer;

        private readonly Camera camera;
        private readonly int width;
        private readonly int height;

        public Render(ConsoleScreen screen, ref Camera camera)
        {
            buffer = new CharInfo[screen.Width * screen.Height];
            width = screen.Width;
            height = screen.Height;
            this.camera = camera;
            camera.RayLength = screen.AspectRatio / 2;
        }

        public CharInfo[] RenderImage(ConsoleScreen screen, List<IRayCastable> gameObjects)
        {
            brightness = '@';
            //int i = width / 2,
            //    j = height / 2;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    float u = i * 2.0f / width - 1,
                          v = j * 2.0f / height - 1;
                    Vector2 uv = new(u * screen.AspectRatio / screen.SymbolAspectRatio, v);

                    Ray ray = new(
                        camera.Transform.Position,
                        Vector3.Transform(new Vector3(uv, camera.RayLength), camera.Transform.RotationMatrix)
                    );

                    Ray.Hit hit = Ray.Hit.Miss;
                    foreach (IRayCastable obj in gameObjects)
                        hit = hit.Closest(obj.Cast(ray));

                    SetPixel(i, j, hit.Object != null ? ConsoleColor.Red : ConsoleColor.DarkBlue);
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
            buffer[width * y + x].Char.UnicodeChar = brightness;
            buffer[width * y + x].Attributes = (short)color;
        }
    }
}
