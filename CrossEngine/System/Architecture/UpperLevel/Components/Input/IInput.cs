namespace CrossEngine
{
    internal interface IInput
    {
        public static abstract bool anyKeyDown { get; }
        public static abstract bool GetKeyDown(ConsoleKey key);
    }
}
