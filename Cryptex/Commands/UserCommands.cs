using Cryptex.Services;
using Cryptex.Utils;

namespace Cryptex.Commands;

public static class UserCommands
{
    public static bool Setup()
    {
        ConsoleEx.PrintInfo("=== Setup Your Account ===");
        Console.Write("Enter username: ");
        string username = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(username))
        {
            ConsoleEx.PrintError("Username cannot be empty.");
            return false;
        }

        var config = ConfigService.LoadConfig();

        if (!string.IsNullOrEmpty(config.CurrentUser))
        {
            ConsoleEx.PrintWarning($"You already have an account: {config.CurrentUser}");
            Console.Write("Regenerate keys? (y/n): ");
            if (Console.ReadLine()?.ToLower() != "y")
                return true;
        }

        KeyService.GenerateKeys(username);
        config.CurrentUser = username;
        ConfigService.SaveConfig(config);

        ConsoleEx.PrintSuccess($"Account '{username}' created!");
        ConsoleEx.PrintInfo("Run 'my-info' to see your public key.");
        return true;
    }

    public static bool ShowMyInfo()
    {
        var config = ConfigService.LoadConfig();

        if (string.IsNullOrEmpty(config.CurrentUser))
        {
            ConsoleEx.PrintError("No account found. Run 'setup' first.");
            return false;
        }

        string publicKeyPath = Path.Combine(config.StoragePath, "user", config.CurrentUser, "public.pem");

        if (!File.Exists(publicKeyPath))
        {
            ConsoleEx.PrintError("Public key not found. Run 'setup' again.");
            return false;
        }

        ConsoleEx.PrintInfo("=== Your Information ===");
        Console.WriteLine($"Username: {config.CurrentUser}");
        Console.WriteLine($"Storage: {config.StoragePath}");
        Console.WriteLine($"Public Key:\n{File.ReadAllText(publicKeyPath)}");
        ConsoleEx.PrintInfo("Share this public key with your friends!");
        return true;
    }

    public static bool EditProfile()
    {
        var config = ConfigService.LoadConfig();

        if (string.IsNullOrEmpty(config.CurrentUser))
        {
            ConsoleEx.PrintError("No account found. Run 'setup' first.");
            return false;
        }

        ConsoleEx.PrintInfo("=== Edit Your Profile ===");
        Console.WriteLine($"Current username: {config.CurrentUser}");
        Console.Write("New username (press Enter to keep current): ");
        string newUsername = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(newUsername) || newUsername == config.CurrentUser)
        {
            ConsoleEx.PrintInfo("Username unchanged.");
            return true;
        }

        string oldUserPath = Path.Combine(config.StoragePath, "users", config.CurrentUser);
        string newUserPath = Path.Combine(config.StoragePath, "users", newUsername);

        if (Directory.Exists(newUserPath))
        {
            ConsoleEx.PrintError($"Username '{newUsername}' already exists!");
            return false;
        }

        if (Directory.Exists(oldUserPath))
        {
            Directory.Move(oldUserPath, newUserPath);
            ConsoleEx.PrintSuccess($"Username changed from '{config.CurrentUser}' to '{newUsername}'");
        }

        config.CurrentUser = newUsername;
        ConfigService.SaveConfig(config);
        return true;
    }

    public static bool RegenerateKeys()
    {
        var config = ConfigService.LoadConfig();

        if (string.IsNullOrEmpty(config.CurrentUser))
        {
            ConsoleEx.PrintError("No account found. Run 'setup' first.");
            return false;
        }

        ConsoleEx.PrintWarning("⚠️  WARNING: This will regenerate your RSA keys!");
        ConsoleEx.PrintWarning("All existing encrypted messages may become undecryptable!");
        Console.Write("Continue? (y/n): ");

        var response = Console.ReadLine()?.ToLower();
        if (response != "y" && response != "yes")
        {
            ConsoleEx.PrintInfo("Operation cancelled.");
            return true;
        }

        KeyService.GenerateKeys(config.CurrentUser);
        ConsoleEx.PrintSuccess("New RSA keys generated successfully!");
        ConsoleEx.PrintInfo("Share your NEW public key with your friends!");
        return true;
    }

    public static bool ResetAccount()
    {
        var config = ConfigService.LoadConfig();

        if (string.IsNullOrEmpty(config.CurrentUser))
        {
            ConsoleEx.PrintError("No account found. Run 'setup' first.");
            return false;
        }

        ConsoleEx.PrintError("⚠️  ⚠️  ⚠️  WARNING: This will DELETE ALL YOUR DATA!");
        ConsoleEx.PrintError("This includes: RSA keys, sessions, and all messages!");
        Console.Write("Are you sure? (type 'DELETE' to confirm): ");

        var response = Console.ReadLine()?.Trim();
        if (response != "DELETE")
        {
            ConsoleEx.PrintInfo("Operation cancelled.");
            return true;
        }

        string userPath = Path.Combine(config.StoragePath, "user", config.CurrentUser);
        if (Directory.Exists(userPath))
        {
            Directory.Delete(userPath, true);
            ConsoleEx.PrintSuccess("All user data deleted!");
        }

        config.CurrentUser = string.Empty;
        ConfigService.SaveConfig(config);

        ConsoleEx.PrintSuccess("Account reset successfully!");
        ConsoleEx.PrintInfo("You can run 'setup' to create a new account.");
        return true;
    }
}