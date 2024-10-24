using CrossEngine.System;
using CrossEngine.System.Kernel;
using SharpHook;
using SharpHook.Native;

namespace CrossEngine
{
    public class Input : CoreComponent<Input>
    {
        internal static event Action? OnAnyKeyDown;

        public static bool anyKeyDown => Instance._keys.Count > 0;

        private TaskPoolGlobalHook global_hook = new TaskPoolGlobalHook();
        private List<KeyCode> _keys = [];
        private List<KeyCode> _oldKeys = [];

        private static Input Instance => Engine.GetCoreComponent<Input>();

        public override void Initialize()
        {
            base.Initialize();

            Core.OnLateUpdate += () => _oldKeys = new List<KeyCode>(_keys);

            global_hook.KeyPressed += (sender, event_args) =>
            {
                var key = event_args.Data.KeyCode;
                if (!_keys.Contains(key)) _keys.Add(key);

                OnAnyKeyDown?.Invoke();
            };

            global_hook.KeyReleased += (sender, event_args) =>
            {
                var key = event_args.Data.KeyCode;
                if (!_keys.Remove(key))
                    Console.WriteLine(key.ToString());
            };

            global_hook.RunAsync();
        }

        public static bool GetKey(KeyCode key)
        {
            if (Instance._keys.Contains(key))
            {
                return true;
            }

            return false;
        }

        public static bool GetKeyUp(KeyCode key)
        {
            if (Instance._oldKeys.Contains(key) && !Instance._keys.Contains(key))
            {
                return true;
            }

            return false;
        }

        public static bool GetKeyDown(KeyCode key)
        {
            if (!Instance._oldKeys.Contains(key) && Instance._keys.Contains(key))
            {
                return true;
            }

            return false;
        }
    }
}
