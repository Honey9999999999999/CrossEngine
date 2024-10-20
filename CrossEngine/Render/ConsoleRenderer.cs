using CrossEngine.Objects;

using System;
using System.Numerics;

using static CrossEngine.Render.ConsoleOutput;

namespace CrossEngine.Render
{

    public class Buffer
    {
        //private readonly [] char_buffer;
        private readonly float[] depth_buffer;
    }

    public class ConsoleRenderer
    {
        private readonly char[] gradient = [' ', '.', '`', ';', 'I', 'S', 'O', '%', '&', '@'];
        //private readonly char[] gradient = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '@'];

        //const float MaxDrawDistance = 9999;
        //const float minShadow = 10;

        private readonly CharInfo[] char_buffer;
        private readonly float[] depth_buffer;

        private readonly ConsoleScreen screen;
        private readonly Camera camera;

        public ConsoleRenderer(ConsoleScreen screen, ref Camera camera)
        {
            this.camera = camera;
            this.screen = screen;

            char_buffer = new CharInfo[screen.Width * screen.Height];
            depth_buffer = new float[screen.Width * screen.Height];

            camera.RayLength = screen.AspectRatio / 2;
        }

        public CharInfo[] Render(List<IRayCastable> gameObjects)//, List<ILightSource> light)
        {
            CharInfo sky = new()
            {
                Char = new('@'),
                Attributes = (short)ConsoleColor.DarkBlue// << 4
            };
            Array.Fill(char_buffer, sky);
            Array.Fill(depth_buffer, float.PositiveInfinity);

            // The FUN part
            Parallel.ForEach(gameObjects, obj =>
            //foreach (IRayCastable obj in gameObjects)
            {
                //Matrix4x4 projection = Matrix4x4.CreatePerspectiveFieldOfView(MathF.PI / 3f, screen.AspectRatio, 0.001f, 1000f),
                //          rotation = camera.Transform.RotationMatrix,
                //          translation = Matrix4x4.CreateTranslation(obj.Bounds.Center - camera.Transform.Position);
                Ray ray = new(camera.Transform.Position, camera.Transform.Forward);

                var d = Vector3.Dot(camera.Transform.Position + camera.Transform.Position * 0.01f, ray.Direction);
                var t = d + Vector3.Dot(ray.Origin, -ray.Direction);
                Vector3 point = ray.Origin + t * ray.Direction;

                Vector3 D = Vector3.Normalize(point - camera.Transform.Position) + Vector3.One;
                //int indx = D.X * screen.Width + D.Y * screen.Height;

                //char_buffer[indx].Char.UnicodeChar = ' ';
                //char_buffer[indx].Attributes = (int)ConsoleColor.Black << 4 | (int)ConsoleColor.White;
                //Console.WriteLine(point - camera.Transform.Position);

                for (int i = 0; i < screen.Width; i++)
                {
                    for (int j = 0; j < screen.Height; j++)
                    {
                        //Parallel.For(0, screen.Width * screen.Height, i =>
                        //{
                        //int j = i / screen.Width; i %= screen.Width;

                        int index = j * screen.Width + i;
                        float u = i * 2.0f / screen.Width - 1,
                                v = j * 2.0f / screen.Height - 1;
                        Vector2 uv = new(u * screen.AspectRatio / screen.SymbolAspectRatio, v);

                        ray.Direction = Vector3.Transform(new Vector3(uv, camera.RayLength), camera.Transform.RotationMatrix);

                        Ray.Hit hit = obj.Cast(ray);

                        if (i == screen.Width / 2 && j == screen.Height / 2)
                        {
                            // symbol = 'X';
                            //SetPixel(i, j, ConsoleColor.White);
                            //Console.SetCursorPosition(i, j);
                            //Console.Write('X');
                            //Console.SetCursorPosition(0, screen.Height - 2);
                            //Console.Write(hit.Distance);
                            char_buffer[index].Char.UnicodeChar = 'X';
                            char_buffer[index].Attributes = (int)ConsoleColor.Black << 4 | (int)ConsoleColor.White;
                            continue;
                        }

                        if (hit.Distance >= depth_buffer[index]) continue;

                        float light = hit.Object != null ? Vector3.Dot(hit.Normal, -Vector3.UnitZ) / 2 + 0.5f : 1;
                        //light = hit.Object == null ? 1 : hit.Distance / 10;
                        char symbol = gradient[(int)(light * (gradient.Length - 1))];

                        depth_buffer[index] = hit.Distance;
                        char_buffer[index].Char.UnicodeChar = symbol;
                        char_buffer[index].Attributes = (short)ConsoleColor.Red;

                        //SetPixel(i, j, hit.Object != null ? ConsoleColor.Red : ConsoleColor.DarkBlue);
                        //});
                    }
                }
            });
            return char_buffer;
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

        //private void SetPixel(int x, int y, ConsoleColor color)
        //{
        //    char_buffer[screen.Width * y + x].Char.UnicodeChar = symbol;
        //    char_buffer[screen.Width * y + x].Attributes = (short)color;
        //}
    }
}
