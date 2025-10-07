using Cryptex.Utils;

namespace Cryptex.Commands;

public class HelpCommand
{
    public static bool ShowHelp()
    {
        ConsoleEx.PrintInfo("=== Cryptex Commands ===");
        Console.WriteLine("  setup           - Setup your user account (first time)");
        Console.WriteLine("  my-info         - Show your public key and info");
        Console.WriteLine("  edit-profile    - Change your username");
        Console.WriteLine("  regenerate-keys - Generate new RSA keys");
        Console.WriteLine("  reset-account   - Delete all data and start fresh");
        Console.WriteLine("  create-session  - Create a new chat session");
        Console.WriteLine("  set-friend-key  - Set friend's public key in a session");
        Console.WriteLine("  encrypt         - Encrypt a message for your friend");
        Console.WriteLine("  decrypt         - Decrypt a message from your friend");
        Console.WriteLine("  list-sessions   - List your chat sessions");
        Console.WriteLine("  change-storage  - Change storage location");
        Console.WriteLine("  show-config     - Show current configuration");
        Console.WriteLine("  fix-storage     - Fix storage path issues");
        Console.WriteLine("  help            - Show this help");
        return true;
    }
}