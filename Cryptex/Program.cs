using Cryptex.Services;
using Cryptex.Utils;

namespace Cryptex;

class Program
{
    private static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            ShowWelcomeMessage();
            return;
        }

        var command = args[0].ToLower();

        switch (command)
        {
            case "help":
                ShowHelp();
                break;

            case "create-user":
                if (args.Length < 2)
                {
                    ConsoleEx.PrintColored("Usage: create-user <username>", ConsoleColor.Yellow);
                    break;
                }

                CreateUser(args[1]);
                break;

            case "create-session":
                if (args.Length < 3)
                {
                    ConsoleEx.PrintColored("Usage: create-session <username> <session-name>", ConsoleColor.Yellow);
                    break;
                }

                SessionService.CreateSession(args[1], args[2]);
                break;

            case "encrypt":
                if (args.Length < 4)
                {
                    ConsoleEx.PrintColored("Usage: encrypt <username> <session-name> <message>", ConsoleColor.Yellow);
                    break;
                }

                EncryptionService.EncryptMessage(args[1], args[2], args[3]);
                break;


            case "decrypt":
                Console.WriteLine(">> [Decrypt] Command selected.");
                break;

            case "set-public-key":
                if (args.Length < 4)
                {
                    ConsoleEx.PrintColored("Usage: set-public-key <username> <session-name> <key-or-path>",
                        ConsoleColor.Yellow);
                    break;
                }

                KeyService.SetOtherPublicKey(args[1], args[2], args[3]);
                break;


            case "set-storage-path":
                Console.WriteLine(">> [set-storage-path] Command selected.");
                break;

            case "list-sessions":
                Console.WriteLine(">> [list-sessions] Command selected.");
                break;

            default:
                ConsoleEx.PrintColored("Unknown command. Use ` help ` for list of commands.", ConsoleColor.Yellow);
                break;
        }
    }

    private static void ShowWelcomeMessage()
    {
        ConsoleEx.PrintColored("Welcome to Cryptex CLI - Secure RSA Messenger", ConsoleColor.Cyan);
        ConsoleEx.PrintColored("Use ` help ` for list of commands.", ConsoleColor.Yellow);
    }

    private static void ShowHelp()
    {
        ConsoleEx.PrintColored("Cryptex Help", ConsoleColor.Yellow);
        Console.WriteLine("Available commands:");
        Console.WriteLine("  create-user       Create a new user and generate RSA keys");
        Console.WriteLine("  create-session    Create a new chat session");
        Console.WriteLine("  encrypt           Encrypt a message in a session");
        Console.WriteLine("  decrypt           Decrypt a message in a session");
        Console.WriteLine("  set-public-key    Set public key of another user in a session");
        Console.WriteLine("  set-storage-path  Change default storage path");
        Console.WriteLine("  list-sessions     List all available sessions");
    }

    private static void CreateUser(string username)
    {
        string basePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".cryptex",
            "users",
            username
        );

        if (Directory.Exists(basePath))
        {
            ConsoleEx.PrintColored($"User '{username}' already exists!", ConsoleColor.Red);
            return;
        }

        KeyService.GenerateKeys(basePath);
        ConsoleEx.PrintColored($"User '{username}' created successfully at {basePath}", ConsoleColor.Green);
    }
}