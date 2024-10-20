using CrossEngine.System.Kernel;

namespace CrossEngine
{
    public class Time : CoreComponent<Time>
    {
        public static float DeltaTime { get; private set; }

        private DateTime _oldtime = DateTime.UtcNow;

        public override void Initialize()
        {
            base.Initialize();

            Core.OnPreUpdate += CalculateDeltaTime;
        }

        private void CalculateDeltaTime()
        {
            DateTime timeTick = DateTime.UtcNow;
            DeltaTime = (float)(timeTick - _oldtime).TotalSeconds;
            _oldtime = timeTick;
        }
    }
}
