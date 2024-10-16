using CrossEngine.System;
using CrossEngine.System.Kernel;
using SharpHook.Native;
using SharpHook;
using System.Runtime.InteropServices;
using System.Text;

namespace CrossEngine
{
    public class Input : CoreComponent<Input>, IInput
    {
        public static bool anyKeyDown => Console.KeyAvailable;
        private ConsoleKey key;



        private static int hookCode;

        public override void Initialize()
        {
            base.Initialize();

            Core.OnPreUpdate += CheckKey;
            Core.OnLateUpdate += ResetKey;


            var global_hook = new TaskPoolGlobalHook();
            List<KeyCode> keys = [];

            global_hook.KeyPressed += (sender, event_args) =>
            {
                var key = event_args.Data.KeyCode;
                if (!keys.Contains(key)) keys.Add(key);
            };

            global_hook.KeyReleased += (sender, event_args) =>
            {
                var key = event_args.Data.KeyCode;
                if (!keys.Remove(key))
                    Console.WriteLine(key.ToString());
            };

            global_hook.RunAsync();
        }

        public static bool GetKeyDown(ConsoleKey key)
        {
            if (instance.key == key)
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
