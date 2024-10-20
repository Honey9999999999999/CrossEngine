using CrossEngine.System;

namespace CrossEngine
{
    internal class TestScript2 : CrossBehaviour
    {
        private float DeltaTime;
        private DateTime _oldtime;

        public override bool Enabled { get => base.Enabled; set => base.Enabled = value; }

        public override void Awake()
        {
            base.Awake();
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }

        public override void OnEnable()
        {
            base.OnEnable();
        }

        public override void Start()
        {
            base.Start();
        }

        public override string? ToString()
        {
            return base.ToString();
        }

        public override void Update()
        {
            base.Update();

            //Console.WriteLine();
            //foreach (var item in SceneManager.GetActiveScene().GetRootObjects())
            //{
            //    Console.WriteLine($"RootObject : {item.GameObject.Name}");

            //    Component[] components = item.GetComponents();
            //    for (int i = 0; i < components.Length; i++)
            //    {
            //        Console.CursorLeft = 4;
            //        Console.WriteLine($"{(i == components.Length - 1 ? "└" : "├")}Object component : {components[i].GetType().Name}");
            //    }
            //}

            //DateTime timeTick = DateTime.UtcNow;
            //DeltaTime = (float)(timeTick - _oldtime).TotalSeconds;
            //_oldtime = timeTick;

            //Console.WriteLine(1 / DeltaTime);
        }
    }
}
