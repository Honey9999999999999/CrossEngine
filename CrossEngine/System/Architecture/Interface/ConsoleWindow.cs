using CrossEngine.Render;
using CrossEngine.System.FSM;
using System.Numerics;

namespace CrossEngine.System.Interface
{
    internal abstract class ConsoleWindow : IState
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public int Width => (int)Size.X;
        public int Height => (int)Size.Y;
        public Vector2 WriteSize { get; }
        public int WriteWidth => (int)WriteSize.X;
        public int WriteHeight => (int)WriteSize.Y;

        protected SmallRect _fullRect;
        protected SmallRect _writeRect;

        private char[] _activeWindow = ['╔', '╗', '╝', '╚', '║', '═'];
        //private char[] _unactiveWindow = ['┌', '┐', '┘', '└', '│', '─'];
        private char[] _unactiveWindow = ['Г', '\\', '/', 'L', '|', '-'];

        private Dictionary<string, char> _unactiveFrame;
        private Dictionary<string, char> _activeFrame;


        public ConsoleWindow() : this(new Vector2(), new Vector2(25, 60)) { }
        public ConsoleWindow(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;

            WriteSize = new Vector2(size.X - 2, size.Y - 2);

            _fullRect = new SmallRect()
            {
                Left = (short)Position.X,
                Top = (short)Position.Y,
                Right = (short)(Position.X + Size.X),
                Bottom = (short)(Position.Y + Size.Y)
            };
            _writeRect = new SmallRect()
            {
                Left = (short)(Position.X + 1),
                Top = (short)(Position.Y + 1),
                Right = (short)(Position.X + Size.X - 1),
                Bottom = (short)(Position.Y + Size.Y - 1)
            };

            _unactiveFrame = [];
            _activeFrame = [];

            for (int i = (int)Position.X; i < Position.X + Size.X; i++)
            {
                if (i == Position.X)
                {
                    _unactiveFrame[$"{i} {Position.Y}"] = _unactiveWindow[0];
                    _activeFrame[$"{i} {Position.Y}"] = _activeWindow[0];
                    _unactiveFrame[$"{i} {Position.Y + Size.Y - 1}"] = _unactiveWindow[3];
                    _activeFrame[$"{i} {Position.Y + Size.Y - 1}"] = _activeWindow[3]; continue;
                }

                if (i == Position.X + Size.X - 1)
                {
                    _unactiveFrame[$"{i} {Position.Y}"] = _unactiveWindow[1];
                    _activeFrame[$"{i} {Position.Y}"] = _activeWindow[1];
                    _unactiveFrame[$"{i} {Position.Y + Size.Y - 1}"] = _unactiveWindow[2];
                    _activeFrame[$"{i} {Position.Y + Size.Y - 1}"] = _activeWindow[2]; continue;
                }

                _unactiveFrame[$"{i} {Position.Y}"] = _unactiveWindow[5];
                _activeFrame[$"{i} {Position.Y}"] = _activeWindow[5];
                _unactiveFrame[$"{i} {Position.Y + Size.Y - 1}"] = _unactiveWindow[5];
                _activeFrame[$"{i} {Position.Y + Size.Y - 1}"] = _activeWindow[5];
            }

            for (int i = (int)(Position.Y + 1); i < Position.Y + Size.Y - 1; i++)
            {
                _unactiveFrame[$"{Position.X} {i}"] = _unactiveWindow[4];
                _activeFrame[$"{Position.X} {i}"] = _activeWindow[4];
                _unactiveFrame[$"{Position.X + Size.X - 1} {i}"] = _unactiveWindow[4];
                _activeFrame[$"{Position.X + Size.X - 1} {i}"] = _activeWindow[4];
            }
        }
        

        public void Enter()
        {
            UpdateWithBounds(true);
        }

        public void Exit()
        {
            UpdateWithBounds();
        }

        public virtual void Update()
        {
            if (Input.GetKeyDown(SharpHook.Native.KeyCode.VcTab))
            {

            }
        }

        public void UpdateInfo()
        {
            ConsoleOutput.Write(BuildCharArray(), Size - new Vector2(2, 2), Vector2.Zero, ref _writeRect);
        }

        protected abstract CharInfo[] BuildCharArray();

        public void UpdateWithBounds(bool isActive = false)
        {
            CharInfo[] buffer = BuildCharArray();
            CharInfo[] windowBuffer = new CharInfo[(int)(Size.Y * Size.X)];

            int bufferIndex = 0;
            int windowIndex = 0;

            for (int y = (int)Position.Y; y < Position.Y + Size.Y; y++)
            {
                for (int x = (int)Position.X; x < Position.X + Size.X; x++)
                {
                    if (_unactiveFrame.ContainsKey($"{x} {y}"))
                    {
                        windowBuffer[windowIndex++] = isActive
                        ? new CharInfo(_activeFrame[$"{x} {y}"], ConsoleColor.White)
                        : new CharInfo(_unactiveFrame[$"{x} {y}"], ConsoleColor.White);
                    }
                    else
                    {
                        windowBuffer[windowIndex++] = buffer.Length > bufferIndex
                            ? buffer[bufferIndex++]
                            : new CharInfo(' ', ConsoleColor.White);
                    }
                }
            }

            ConsoleOutput.Write(windowBuffer, Size, Vector2.Zero, ref _fullRect);
        }

        public bool isBounds(int x, int y) => ((x == Position.X || x == Position.X + Size.X - 1) && (y == Position.Y || y == Position.Y + Size.Y - 1))
                                           || ((x == Position.X || x == Position.X + Size.X - 1)  ^ (y == Position.Y || y == Position.Y + Size.Y - 1));
    }
}
