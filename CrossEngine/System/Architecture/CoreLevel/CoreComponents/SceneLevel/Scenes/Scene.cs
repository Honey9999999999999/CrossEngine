namespace CrossEngine.System
{
    public sealed class Scene
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public GameObject RootNode { get; init; }

        internal Scene()
        {
            Name = "Default Scene";
            RootNode = new("RootNode", null);
        }

        public TComponent[] GetAllComponents<TComponent>(Transform[] parent) where TComponent : Component, new()
        {
            TComponent[] components = [];

            foreach (var transform in parent)
            {
                if(transform.TryGetComponent(out TComponent component))
                {
                    components = [.. components, component];
                }

                TComponent[] childComponents = GetAllComponents<TComponent>(transform.GetChilds());

                foreach (var component1 in childComponents)
                {
                    components = [..components, component1];
                }
            }

            return components;
        }
    }
}
