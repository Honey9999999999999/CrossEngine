using System.Collections;
using System.Xml.Linq;

namespace CrossEngine
{
    internal class TestScript2 : CrossBehaviour
    {
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

            StartCoroutine(asd());

            IEnumerator asd()
            {
                Console.WriteLine(GameObject.Name);
                Console.WriteLine(GameObject.Name);
                Console.WriteLine(GameObject.Name);
                Console.WriteLine(GameObject.Name);
                Console.WriteLine(GameObject.Name);

                yield return null;
            }
        }

        public override string? ToString()
        {
            return base.ToString();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
