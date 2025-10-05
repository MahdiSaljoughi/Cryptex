using System.Security.Cryptography;
using Cryptex.Utils;

namespace Cryptex.Services;

public static class KeyService
{
    public static void GenerateKeys(string folderPath, int keySize = 2048)
    {
        Directory.CreateDirectory(folderPath);

        using var rsa = RSA.Create(keySize);

        var privateKey = ExportPrivateKeyPem(rsa);
        var publicKey = ExportPublicKeyPem(rsa);

        File.WriteAllText(Path.Combine(folderPath, "private.pem"), privateKey);
        File.WriteAllText(Path.Combine(folderPath, "public.pem"), publicKey);
    }

    private static string ExportPrivateKeyPem(RSA rsa)
    {
        var bytes = rsa.ExportPkcs8PrivateKey();
        return "-----BEGIN PRIVATE KEY-----\n" +
               Convert.ToBase64String(bytes, Base64FormattingOptions.InsertLineBreaks) +
               "\n-----END PRIVATE KEY-----";
    }

    private static string ExportPublicKeyPem(RSA rsa)
    {
        var bytes = rsa.ExportSubjectPublicKeyInfo();
        return "-----BEGIN PUBLIC KEY-----\n" +
               Convert.ToBase64String(bytes, Base64FormattingOptions.InsertLineBreaks) +
               "\n-----END PUBLIC KEY-----";
    }

    public static void SetOtherPublicKey(string username, string sessionName, string keyOrPath)
    {
        string sessionPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".cryptex",
            "users",
            username,
            "sessions",
            sessionName
        );

        if (!Directory.Exists(sessionPath))
        {
            ConsoleEx.PrintColored($"Session '{sessionName}' for user '{username}' not found! Please create it first.",
                ConsoleColor.Red);
            return;
        }

        string keyFilePath = Path.Combine(sessionPath, "other_public.pem");

        string keyContent;
        if (File.Exists(keyOrPath))
        {
            keyContent = File.ReadAllText(keyOrPath).Trim();
        }
        else
        {
            keyContent = keyOrPath.Trim();
        }

        if (string.IsNullOrWhiteSpace(keyContent) || !keyContent.Contains("BEGIN PUBLIC KEY"))
        {
            ConsoleEx.PrintColored("Invalid public key format. Must be a valid PEM encoded public key.",
                ConsoleColor.Yellow);
            return;
        }

        File.WriteAllText(keyFilePath, keyContent);

        ConsoleEx.PrintColored($"Other public key set for session '{sessionName}'", ConsoleColor.Green);
    }
}