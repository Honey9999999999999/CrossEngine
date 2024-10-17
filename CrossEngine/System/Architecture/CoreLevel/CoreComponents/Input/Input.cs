using CrossEngine.System;
using CrossEngine.System.Kernel;
using SharpHook.Native;
using SharpHook;

namespace CrossEngine
{
    public class Input : CoreComponent<Input>
    {
        public static bool anyKeyDown => instance._keys.Count > 0;

        private TaskPoolGlobalHook global_hook = new TaskPoolGlobalHook();
        private List<KeyCode> _keys = [];
        private List<KeyCode> _oldKeys = [];

        public override void Initialize()
        {
            base.Initialize();

            Core.OnLateUpdate += () => _oldKeys = new List<KeyCode>(_keys);

            global_hook.KeyPressed += (sender, event_args) =>
            {
                var key = event_args.Data.KeyCode;
                if (!_keys.Contains(key)) _keys.Add(key);
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
            if (instance._keys.Contains(key))
            {
                return true;
            }

            return false;
        }

        public static bool GetKeyUp(KeyCode key)
        {
            if (instance._oldKeys.Contains(key) && !instance._keys.Contains(key))
            {
                return true;
            }

            return false;
        }

        public static bool GetKeyDown(KeyCode key)
        {
            if (!instance._oldKeys.Contains(key) && instance._keys.Contains(key))
            {
                return true;
            }

            return false;
        }
    }
}
