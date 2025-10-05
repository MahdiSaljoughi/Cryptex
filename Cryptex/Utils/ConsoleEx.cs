namespace Cryptex.Utils;
public static class ConsoleEx
{
    private static bool IsWindows => OperatingSystem.IsWindows();

    public static void PrintColored(string message, ConsoleColor color)
    {
        if (IsWindows)
        {
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = prev;
        }
        else
        {
            string ansiColor = color switch
            {
                ConsoleColor.Red => "\u001b[31;1m",
                ConsoleColor.Green => "\u001b[32;1m",
                ConsoleColor.Yellow => "\u001b[33;1m",
                ConsoleColor.Blue => "\u001b[34;1m",
                ConsoleColor.Cyan => "\u001b[36;1m",
                _ => "\u001b[37;1m"
            };
            Console.WriteLine($"{ansiColor}{message}\u001b[0m");
        }
    }
}