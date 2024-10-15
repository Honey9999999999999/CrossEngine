using CrossEngine.System;

namespace CrossEngine
{
    public class Input : CoreComponent<Input>, IInput
    {
        public static bool anyKeyDown => Console.KeyAvailable;
        private ConsoleKey key;

        public override void Initialize()
        {
            base.Initialize();

            Engine.GetCore().OnPreUpdate += CheckKey;
            Engine.GetCore().OnLateUpdate += ResetKey;
        }

        public static bool GetKeyDown(ConsoleKey key)
        {
            if (_instance.key == key)
            {
                return true;
            }

            return false;
        }

        private void CheckKey()
        {
            if (Console.KeyAvailable)
            {
                key = Console.ReadKey().Key;
            }
        }
        private void ResetKey() => key = ConsoleKey.None;
    }
}
