namespace CrossEngine.System
{
    internal class CrossException : Exception
    {
        public CrossException(string? message) : base(message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
    }
}
