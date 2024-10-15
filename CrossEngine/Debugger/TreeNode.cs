using System.Collections;
using System.Reflection;
using System.Text;

namespace CrossEngine
{
    public static partial class Debug
    {
        /* Table of Ascii Extended characters:
         *   +---+   *   ┌-┬-┐   *     /\     *
         *   | \ |   *   ├-┼-┤   *    //\\    *
         *   +---+   *   └-┴-┘   *    ----    */

        /* Tree structure example:
         *   Element: '0'                     *
         *   ├- Element: '1'                  *
         *   |  ├- Element: '1.1'             *
         *   |  └- Element: '1.2'             *
         *   └- Element: '2'                  *
         *      ├- Element: '2.1'             *
         *      └- Element: '2.2'             */

        // not implemented and never will be, but fun idea
        /* Waterfall tree:
         *   Element: '0' -------------------------┬-----------------------------------┐
         *   └ Element: '1' ------┐                Element: '2' ------┐                Element: '3' ------┐
         *     └ Element: '1.1'   Element: '1.2'   └ Element: '2.1'   Element: '2.2'   └ Element: '3.1'   Element: '3.2'
         */

        public struct Tree
        {
            public Node Root;

            public class Config
            {
                public int MaxDepth = int.MaxValue;
                public BindingFlags BindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static;
                public MemberTypes MemberTypes = MemberTypes.All;
                public bool FilterByAssembly = true;

                public static readonly Config Default = new() { },
                    PublicFields = new() { BindingFlags = BindingFlags.Public | BindingFlags.Instance, MemberTypes = MemberTypes.Field };
            }

            public static Tree FromObject(object obj) => FromObject(obj, Config.Default);
            public static Tree FromObject(object obj, Config config)
            {
                Tree tree = new();
                Type[] AssemblyFilter = obj.GetType().Assembly.GetTypes();
                Stack<(int depth, Node node, object? value)> nodes = new();
                nodes.Push((0, tree.Root = new($"{obj.GetType().Name}: \"{obj}\""), obj));

                // HashSet to store objects that have already been processed
                HashSet<object?> references = [];

                while (nodes.Count > 0)
                {
                    // Pop a node from the stack
                    var (depth, node, value) = nodes.Pop();

                    // If the object has already been processed or depth limit is reached, skip processing
                    if (depth >= config.MaxDepth) continue;
                    if (!references.Add(value)) continue;

                    // Get the type of the object and its members
                    var type = value?.GetType();
                    var members = type?.GetMembers(config.BindingFlags);
                    // Filter the members based on the MemberTypes specified in the config
                    members = members?.Where(m => (m.MemberType & config.MemberTypes) != 0).ToArray();

                    // If the object is an array, process each element of the array
                    if (value is Array array)
                    {
                        node.Childs.Add(new($"{array.Length.GetType()} Length = \"{array.Length}\""));
                        nodes.Push((depth + 1, node.Childs[^1], null));
                        for (int i = 0; i < array.Length; i++)
                        {
                            // Add a new child node to the current node and push it into the stack for processing
                            node.Childs.Add(new($"[{i}] {array.GetValue(i)?.GetType()} = \"{array.GetValue(i)}\""));
                            nodes.Push((depth + 1, node.Childs[^1], array.GetValue(i)));
                        }
                    }

                    // If the object is an array, process each element of the array
                    if (value is IEnumerable enumerable)
                    {
                        // node.Childs.Add(new($"{objects.Length.GetType()} Length = \"{array.Length}\""));
                        // nodes.Push((depth + 1, node.Childs[^1], null));
                        int i = 0;
                        foreach (var item in enumerable)
                        {
                            // Add a new child node to the current node and push it into the stack for processing
                            node.Childs.Add(new($"[{i++}] {item?.GetType()} = \"{item}\""));
                            nodes.Push((depth + 1, node.Childs[^1], item));
                        }
                    }

                    // If the type or members are null, skip processing
                    if (type == null || members == null) continue;
                    if (!AssemblyFilter.Contains(type)) continue;

                    foreach (var member in members)
                    {
                        // Get the value of the member if it's a field
                        var memberValue = (member as FieldInfo)?.GetValue(value);

                        string memberInfo = MemberToString(member, memberValue);

                        // Add a new child node to the current node and push it into the stack for processing
                        node.Childs.Add(new(memberInfo));
                        nodes.Push((depth + 1, node.Childs[^1], memberValue));
                    }
                }

                return tree;
            }

            private static string AttributesToString(MemberInfo member) => (member switch
            {
                ConstructorInfo info => $"{info.Attributes:F}",
                PropertyInfo info => $"{info.Attributes:F}",
                MethodInfo info => $"{info.Attributes:F}",
                FieldInfo info => $"{info.Attributes:F}",
                EventInfo info => $"{info.Attributes:F}",
                TypeInfo info => $"{info.Attributes:F}",
                _ => string.Empty
            }).Replace(",", string.Empty);

            private static string ParametersToString(MemberInfo member) => string.Join(", ", (member switch
            {
                ConstructorInfo info => info.GetParameters(),
                PropertyInfo info => info.GetIndexParameters(),
                MethodInfo info => info.GetParameters(),
                _ => null
            })?.Select(p => $"{p.ParameterType.Name} {p.Name}") ?? []);

            private static string MemberToString(MemberInfo member, object? memberValue) => $"{AttributesToString(member)} {member switch
            {
                // System.String(char c, int count)
                ConstructorInfo info => $"{info.DeclaringType}({ParametersToString(info)})",

                // Int32 Length = "32"
                FieldInfo info => $"{info.FieldType.Name} {info.Name} = \"{memberValue}\"",

                // Event<System.Action> OnInput
                EventInfo info => $"Event<{info.EventHandlerType?.Name}> {info.Name}",

                // Int32 Sum(Int32 a, Int32 b)
                MethodInfo info => $"{info.ReturnType.Name} {info.Name}({ParametersToString(info)})",

                // Int32 this[Int32 Index] { get; set; }
                PropertyInfo info => $"{info.PropertyType.Name} {info.Name}[{ParametersToString(info)}] {{ " +
                info switch
                {
                    { CanRead: true, CanWrite: true } => "get; set;",   // read and write
                    { CanRead: true, CanWrite: false } => "get;",       // read only
                    { CanRead: false, CanWrite: true } => "set;",       // write only
                    _ => string.Empty                                   // no read, no write
                } + $" }}".Replace("[] ", string.Empty),

                // Class Node
                TypeInfo info => info switch
                {
                    { IsValueType: true } => "Struct ",
                    { IsClass: true } => "Class ",
                    _ => string.Empty
                } + info.Name,

                _ => $"{member}",
            }}";

            public override readonly string ToString()
            {
                // Init variables
                StringBuilder output = new();
                Stack<(int Depth, Node Node, string Ident)> nodes = new();
                nodes.Push((0, Root, string.Empty));

                // Walk all nodes
                while (nodes.Count > 0)
                {
                    // Pop current
                    var (depth, node, ident) = nodes.Pop();

                    // Print to output
                    output.AppendLine(ident + node.ToString());

                    // Replace characters
                    ident = ident.Replace('├', '|').Replace('└', ' ');

                    // Append all Childs
                    for (int i = 1; i <= node.Childs.Count; i++)
                        nodes.Push((depth + 1, node.Childs[^i], ident + (i > 1 ? "├ " : "└ ")));
                }

                // return output
                return output.ToString();
            }

            public class Node(object? value)
            {
                public object? Value = value;
                public List<Node> Childs = [];

                public override string ToString() => $"{Value}";
            }
        }
    }
}