using System.Security.Cryptography;
using Cryptex.Models;
using Cryptex.Utils;

namespace Cryptex.Services;

public static class KeyService
{
    public static void GenerateKeys(string username)
    {
        var config = ConfigService.LoadConfig();
        string userPath = Path.Combine(config.StoragePath, "user", username);
        Directory.CreateDirectory(userPath);

        using var rsa = RSA.Create(2048);

        string privateKey = ExportPrivateKeyPem(rsa);
        string publicKey = ExportPublicKeyPem(rsa);

        File.WriteAllText(Path.Combine(userPath, "private.pem"), privateKey);
        File.WriteAllText(Path.Combine(userPath, "public.pem"), publicKey);

        ConsoleEx.PrintSuccess($"RSA keys generated for user: {username}");
    }

    public static void SetFriendPublicKey(string sessionName, string keyOrPath)
    {
        var config = ConfigService.LoadConfig();

        if (string.IsNullOrEmpty(config.CurrentUser))
        {
            ConsoleEx.PrintError("No user account found. Run 'setup' first.");
            return;
        }

        string sessionPath = GetSessionPath(config.CurrentUser, sessionName);
        if (!Directory.Exists(sessionPath))
        {
            ConsoleEx.PrintError($"Session '{sessionName}' not found.");
            return;
        }

        string keyContent = GetKeyContent(keyOrPath);
        if (string.IsNullOrEmpty(keyContent))
        {
            ConsoleEx.PrintError("Invalid public key format.");
            return;
        }

        string keyFilePath = Path.Combine(sessionPath, "other_public.pem");
        File.WriteAllText(keyFilePath, keyContent);

        ConsoleEx.PrintSuccess($"Friend's public key set for session: {sessionName}");
    }

    private static string GetKeyContent(string keyOrPath)
    {
        if (File.Exists(keyOrPath))
            return File.ReadAllText(keyOrPath).Trim();

        string keyContent = keyOrPath.Trim();
        return keyContent.Contains("BEGIN PUBLIC KEY") ? keyContent : null;
    }

    private static string ExportPrivateKeyPem(RSA rsa)
    {
        byte[] bytes = rsa.ExportPkcs8PrivateKey();
        return "-----BEGIN PRIVATE KEY-----\n" +
               Convert.ToBase64String(bytes, Base64FormattingOptions.InsertLineBreaks) +
               "\n-----END PRIVATE KEY-----";
    }

    private static string ExportPublicKeyPem(RSA rsa)
    {
        byte[] bytes = rsa.ExportSubjectPublicKeyInfo();
        return "-----BEGIN PUBLIC KEY-----\n" +
               Convert.ToBase64String(bytes, Base64FormattingOptions.InsertLineBreaks) +
               "\n-----END PUBLIC KEY-----";
    }

    private static string GetSessionPath(string username, string sessionName)
    {
        var config = ConfigService.LoadConfig();
        return Path.Combine(config.StoragePath, "user", username, "sessions", sessionName);
    }
}