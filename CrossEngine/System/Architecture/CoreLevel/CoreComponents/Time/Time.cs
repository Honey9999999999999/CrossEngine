using CrossEngine.System.Kernel;

namespace CrossEngine
{
    public class Time : CoreComponent<Time>
    {
        public static float DeltaTime { get; private set; }

        private DateTime last = DateTime.UtcNow;

        public override void Initialize()
        {
            base.Initialize();

            Core.OnPreUpdate += CalculateDeltaTime;
        }

        private void CalculateDeltaTime()
        {
            DateTime now = DateTime.UtcNow;
            DeltaTime = (float)(now - last).TotalSeconds;
            last = now;
        }
    }
}
