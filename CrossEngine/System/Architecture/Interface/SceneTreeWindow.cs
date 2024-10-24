using CrossEngine.System.Interface;

namespace CrossEngine.System.Architecture.Interface
{
    internal class SceneTreeWindow : ConsoleWindow
    {
        private readonly CharInfo[] _openState =
            [
                new CharInfo('[', ConsoleColor.White),
                new CharInfo('-', ConsoleColor.White),
                new CharInfo(']', ConsoleColor.White)
            ];
        private readonly CharInfo[] _closeState =
            [
                new CharInfo('[', ConsoleColor.White),
                new CharInfo('+', ConsoleColor.White),
                new CharInfo(']', ConsoleColor.White)
            ];

        private SceneTree _tree;

        public SceneTreeWindow() : base()
        {
            SceneTree.OnTreeUpdated += UpdateInfo;
            _tree = Engine.GetCoreComponent<SceneTree>();
        }

        protected override CharInfo[] BuildCharArray()
        {
            CharInfo[] buffer = BuildBuffer();

            if(buffer.Length < WriteWidth * WriteHeight)
            {
                int oldLength = buffer.Length;
                Array.Resize(ref buffer, WriteWidth * WriteHeight);

                for (int i = oldLength; i < buffer.Length; i++)
                {
                    buffer[i] = new CharInfo(' ', ConsoleColor.White);
                }
            }

            return buffer;
        }

        private CharInfo[] BuildBuffer()
        {
            CharInfo[] buffer = [];
            Branch[] visibleBranches = _tree.VisibleBranches;

            for (int i = 0; i < WriteHeight && i < visibleBranches.Length; i++)
            {
                buffer = ConcateBuffers(buffer, BuildString(visibleBranches[i]));
            }

            return buffer;
        }

        private static CharInfo[] ConcateBuffers(CharInfo[] a, CharInfo[] b)
        {
            int oldLenght = a.Length;
            Array.Resize(ref a, a.Length + b.Length);
            Array.Copy(b, 0, a, oldLenght, b.Length);

            return a;
        }

        private CharInfo[] BuildString(Branch branch)
        {
            CharInfo[] str = new CharInfo[WriteWidth];

            int x = 0;
            for (int i = 0; i < branch.Level && i < WriteWidth; i++)
            {
                str[x++] = new CharInfo(' ', ConsoleColor.White);
            }

            if (branch.IsParent)
            {
                CharInfo[] state = branch.IsOpen ? _openState : _closeState;

                for (int i = 0; i < state.Length && x + i < WriteWidth; i++)
                {
                    str[x + i] = state[i];
                }
            }
            x += 3;

            for (int i = x; i < WriteSize.X; i++)
            {
                int currentIndex = i - x;
                str[i] = branch.GameObject.Name.Length > currentIndex
                       ? new(branch.GameObject.Name[currentIndex], ConsoleColor.White)
                       : new CharInfo(' ', ConsoleColor.White);
            }

            return str;
        }

        public override void Update()
        {
            
        }
    }
}
