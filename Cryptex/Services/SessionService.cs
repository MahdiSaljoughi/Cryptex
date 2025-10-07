using Cryptex.Utils;

namespace Cryptex.Services;

public static class SessionService
{
    public static void CreateSession(string sessionName)
    {
        var config = ConfigService.LoadConfig();

        if (string.IsNullOrEmpty(config.CurrentUser))
        {
            ConsoleEx.PrintError("No user account found. Run 'setup' first.");
            return;
        }

        string sessionPath = GetSessionPath(config.CurrentUser, sessionName);

        if (Directory.Exists(sessionPath))
        {
            ConsoleEx.PrintWarning($"Session '{sessionName}' already exists.");
            return;
        }

        Directory.CreateDirectory(sessionPath);
        CopyUserPublicKey(config.CurrentUser, sessionPath);

        ConsoleEx.PrintSuccess($"Session '{sessionName}' created!");
        ConsoleEx.PrintInfo("Next: Share your public key and set friend's key using 'set-friend-key'");
    }

    public static void ListSessions()
    {
        var config = ConfigService.LoadConfig();

        if (string.IsNullOrEmpty(config.CurrentUser))
        {
            ConsoleEx.PrintError("No user account found. Run 'setup' first.");
            return;
        }

        string sessionsPath = Path.Combine(config.StoragePath, "users", config.CurrentUser, "sessions");

        if (!Directory.Exists(sessionsPath))
        {
            ConsoleEx.PrintInfo("No sessions found. Create one with 'create-session'.");
            return;
        }

        var sessions = Directory.GetDirectories(sessionsPath);
        ConsoleEx.PrintInfo("Your Chat Sessions:");

        foreach (var session in sessions)
        {
            string sessionName = Path.GetFileName(session);
            string status = HasFriendKey(session) ? "✅ Ready" : "⏳ Waiting for friend's key";
            Console.WriteLine($"  - {sessionName} - {status}");
        }
    }

    private static void CopyUserPublicKey(string username, string sessionPath)
    {
        var config = ConfigService.LoadConfig();
        string userPublicKeyPath = Path.Combine(config.StoragePath, "users", username, "public.pem");
        string sessionPublicKeyPath = Path.Combine(sessionPath, "my_public.pem");

        if (File.Exists(userPublicKeyPath))
            File.Copy(userPublicKeyPath, sessionPublicKeyPath, true);
    }

    private static bool HasFriendKey(string sessionPath)
    {
        string otherKeyPath = Path.Combine(sessionPath, "other_public.pem");
        return File.Exists(otherKeyPath) && new FileInfo(otherKeyPath).Length > 0;
    }

    private static string GetSessionPath(string username, string sessionName)
    {
        var config = ConfigService.LoadConfig();
        return Path.Combine(config.StoragePath, "users", username, "sessions", sessionName);
    }
}