using CrossEngine.System.Core;

namespace CrossEngine.System
{
    internal interface IEngine
    {
        public static abstract void StartCore();
        public static abstract ICore GetCore();
        public static abstract void RunPlayMode();
        public static abstract void StopPlayMode();
    }
}
