namespace CrossEngine
{
    public struct WaitForSeconds(double delay)
    {
        public DateTime time = DateTime.UtcNow + TimeSpan.FromSeconds(delay);
    }
}
