namespace CrossEngine
{
    public readonly struct WaitForSeconds(double delay) : ICoroutineDelay
    {
        public readonly bool Ready => DateTime.UtcNow >= End;
        private readonly DateTime End = DateTime.UtcNow + TimeSpan.FromSeconds(delay);
    }
}
