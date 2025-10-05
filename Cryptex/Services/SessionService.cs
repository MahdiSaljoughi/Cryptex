using Cryptex.Utils;

namespace Cryptex.Services;

public static class SessionService
{
    public static void CreateSession(string username, string sessionName)
    {
        string userPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".cryptex",
            "users",
            username
        );

        if (!Directory.Exists(userPath))
        {
            ConsoleEx.PrintColored($"User '{username}' does not exist!", ConsoleColor.Red);
            return;
        }

        string sessionsPath = Path.Combine(userPath, "sessions", sessionName);

        if (Directory.Exists(sessionsPath))
        {
            ConsoleEx.PrintColored($"Session '{sessionName}' already exists!", ConsoleColor.Yellow);
            return;
        }

        Directory.CreateDirectory(Path.Combine(sessionsPath, "messages"));
        
        string myPublicKey = Path.Combine(userPath, "public.pem");
        if (File.Exists(myPublicKey))
        {
            File.Copy(myPublicKey, Path.Combine(sessionsPath, "my_public.pem"));
        }
        
        File.WriteAllText(Path.Combine(sessionsPath, "other_public.pem"), "");

        ConsoleEx.PrintColored($"Session '{sessionName}' created for user '{username}'", ConsoleColor.Green);
    }
}