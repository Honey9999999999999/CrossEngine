using System.Reflection;

namespace CrossEngine
{
    public static partial class Debug
    {
        /// <summary> Recursively prints <typeparamref name="T"/> fields </summary>
        public static TreeNode<ObjectInfo<object>> TreeInfo<T>(T obj, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance, int depth = int.MaxValue)
        {
            ObjectInfo<object> info = new(obj);
            FieldInfo[] fields = info.Type.GetFields(flags);

            if (fields.Length == 0 || depth <= 0 || obj is null) return new(info);

            TreeNode<ObjectInfo<object>> tree = new(info);
            foreach (var f in fields)
            {
                var node = TreeInfo(f.GetValue(obj), flags, depth - 1);
                node.Value.Name = f.Name;
                tree.AddNode(node);
            }

            return tree;
        }

        public class ObjectInfo<T>
        {
            public string Name;
            public Type Type;
            public T Value;

            public ObjectInfo(T value)
            {
                Value = value;
                Type = value?.GetType() ?? typeof(T);
            }

            public override string ToString() => $"{Type.Name} {Name}: \"{Value}\"";
        }

        public class TreeNode<T>(T name)
        {
            public T Value = name;
            public List<TreeNode<T>> Childs = [];

            public void Add(T value) => Childs.Add(new(value));
            public void AddNode(TreeNode<T> value) => Childs.Add(value);
            public void Remove(T value) => Childs.Remove(new(value));
            public void RemoveNode(TreeNode<T> value) => Childs.Remove(value);

            public override string ToString()
            {
                if (Childs.Count == 0) return $"{Value}";
                Stack<char> indent = new(' ' + new string('│', Childs.Count - 1)),
                            tree = new("└" + new string('├', Childs.Count - 1));
                var childs = Childs.Select(node => $" {tree.Pop()}─ {node}".Replace("\n", "\n " + indent.Pop()));
                return $"{Value}\n{string.Join($"\n", childs)}";
            }
        }
    }
}