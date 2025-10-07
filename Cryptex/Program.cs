using Cryptex.Commands;
using Cryptex.Utils;

namespace Cryptex;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            ShowWelcome();
            return;
        }

        try
        {
            ExecuteCommand(args);
        }
        catch (Exception ex)
        {
            ConsoleEx.PrintError($"Unexpected error: {ex.Message}");
        }
    }

    private static void ExecuteCommand(string[] args)
    {
        string command = args[0].ToLower();

        var result = command switch
        {
            "help" => HelpCommand.ShowHelp(),
            "setup" => UserCommands.Setup(),
            "my-info" => UserCommands.ShowMyInfo(),
            "edit-profile" => UserCommands.EditProfile(),
            "regenerate-keys" => UserCommands.RegenerateKeys(),
            "reset-account" => UserCommands.ResetAccount(),
            "create-session" => SessionCommands.CreateSession(args),
            "set-friend-key" => SessionCommands.SetFriendKey(args),
            "encrypt" => CryptoCommands.Encrypt(args),
            "decrypt" => CryptoCommands.Decrypt(args),
            "list-sessions" => SessionCommands.ListSessions(),
            "change-storage" => ConfigCommands.ChangeStorage(args),
            "show-config" => ConfigCommands.ShowConfig(),
            "fix-storage" => ConfigCommands.FixStorage(),
            _ => HandleUnknownCommand(command)
        };

        if (!result)
        {
            Environment.Exit(1);
        }
    }

    private static bool HandleUnknownCommand(string command)
    {
        ConsoleEx.PrintError($"Unknown command: {command}");
        HelpCommand.ShowHelp();
        return false;
    }

    private static void ShowWelcome()
    {
        ConsoleEx.PrintInfo("=== Cryptex CLI - Secure Messenger ===");
        Console.WriteLine("To get started: cryptex setup");
        Console.WriteLine("For help: cryptex help");
    }
}