namespace CrossEngine.System
{
    public sealed class CrossMessager
    {
        public static void PrintWarningMessage(string message)
        {
            PrintMessage(ConsoleColor.Yellow, message);
        }

        public static void PrintCriticalMessage(string message)
        {
            PrintMessage(ConsoleColor.Red, message);
        }

        private static void PrintMessage(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"\n{message}\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
