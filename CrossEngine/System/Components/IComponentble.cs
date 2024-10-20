using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossEngine.System.Components
{
    internal interface IComponentble
    {
        public void AddComponent<TComponent>() where TComponent : Component, new();
        public TComponent GetComponent<TComponent>() where TComponent : Component, new();
        public bool TryGetComponent<TComponent>(out TComponent? crossBehaviour) where TComponent : Component, new();
        public Component[] GetComponents();
        public T[] GetComponents<T>() where T : Component;
    }
}
