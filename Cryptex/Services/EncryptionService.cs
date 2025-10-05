using System.Security.Cryptography;
using System.Text;
using Cryptex.Utils;

namespace Cryptex.Services;

public class EncryptionService
{
    public static void EncryptMessage(string username, string sessionName, string plainText)
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
            ConsoleEx.PrintColored($"Session '{sessionName}' for user '{username}' not found!", ConsoleColor.Red);
            return;
        }

        string publicKeyPath = Path.Combine(sessionPath, "other_public.pem");
        if (!File.Exists(publicKeyPath))
        {
            ConsoleEx.PrintColored("Other user's public key not found! Use 'set-public-key' first.",
                ConsoleColor.Yellow);
            return;
        }

        string publicKeyContent = File.ReadAllText(publicKeyPath);
        if (string.IsNullOrWhiteSpace(publicKeyContent))
        {
            ConsoleEx.PrintColored("Other user's public key is empty!", ConsoleColor.Yellow);
            return;
        }

        try
        {
            using var rsa = RSA.Create();
            rsa.ImportFromPem(publicKeyContent.ToCharArray());

            byte[] dataToEncrypt = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedData = rsa.Encrypt(dataToEncrypt, RSAEncryptionPadding.Pkcs1);

            string encryptedBase64 = Convert.ToBase64String(encryptedData);

            string encryptedFilePath = Path.Combine(sessionPath, $"message_{DateTime.Now:yyyyMMdd_HHmmss}.enc");
            File.WriteAllText(encryptedFilePath, encryptedBase64);

            ConsoleEx.PrintColored("Message encrypted successfully!", ConsoleColor.Green);
            ConsoleEx.PrintColored($"Encrypted text: {encryptedBase64}", ConsoleColor.Cyan);
            ConsoleEx.PrintColored($"Saved to: {encryptedFilePath}", ConsoleColor.Magenta);
        }
        catch (Exception ex)
        {
            ConsoleEx.PrintColored($"Encryption failed: {ex.Message}", ConsoleColor.Red);
        }
    }
}