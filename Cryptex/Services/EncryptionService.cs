using System.Security.Cryptography;
using System.Text;
using Cryptex.Utils;

namespace Cryptex.Services;

public static class EncryptionService
{
    public static void EncryptMessage(string sessionName, string plainText)
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

        string publicKeyPath = Path.Combine(sessionPath, "other_public.pem");
        if (!File.Exists(publicKeyPath))
        {
            ConsoleEx.PrintError("Friend's public key not found. Use 'set-friend-key' first.");
            return;
        }

        try
        {
            string publicKeyContent = File.ReadAllText(publicKeyPath);
            using var rsa = RSA.Create();
            rsa.ImportFromPem(publicKeyContent.ToCharArray());

            byte[] dataToEncrypt = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedData = rsa.Encrypt(dataToEncrypt, RSAEncryptionPadding.OaepSHA256);

            string encryptedBase64 = Convert.ToBase64String(encryptedData);
            SaveEncryptedMessage(sessionPath, encryptedBase64);

            ConsoleEx.PrintSuccess("Message encrypted successfully!");
            ConsoleEx.PrintInfo($"Encrypted: {encryptedBase64}");
        }
        catch (Exception ex)
        {
            ConsoleEx.PrintError($"Encryption failed: {ex.Message}");
        }
    }

    public static void DecryptMessage(string sessionName, string encryptedMessageBase64)
    {
        var config = ConfigService.LoadConfig();

        if (string.IsNullOrEmpty(config.CurrentUser))
        {
            ConsoleEx.PrintError("No user account found. Run 'setup' first.");
            return;
        }

        string privateKeyPath = Path.Combine(config.StoragePath, "user", config.CurrentUser, "private.pem");
        if (!File.Exists(privateKeyPath))
        {
            ConsoleEx.PrintError("Private key not found.");
            return;
        }

        try
        {
            string privateKeyContent = File.ReadAllText(privateKeyPath);
            using var rsa = RSA.Create();
            rsa.ImportFromPem(privateKeyContent.ToCharArray());

            byte[] encryptedData = Convert.FromBase64String(encryptedMessageBase64);
            byte[] decryptedData = rsa.Decrypt(encryptedData, RSAEncryptionPadding.OaepSHA256);

            string decryptedText = Encoding.UTF8.GetString(decryptedData);

            ConsoleEx.PrintSuccess("Message decrypted successfully!");
            ConsoleEx.PrintInfo($"Decrypted: {decryptedText}");
        }
        catch (Exception ex)
        {
            ConsoleEx.PrintError($"Decryption failed: {ex.Message}");
        }
    }

    private static void SaveEncryptedMessage(string sessionPath, string encryptedBase64)
    {
        string fileName = $"message_{DateTime.Now:yyyyMMdd_HHmmss}.enc";
        string filePath = Path.Combine(sessionPath, fileName);
        File.WriteAllText(filePath, encryptedBase64);
        ConsoleEx.PrintInfo($"Saved to: {filePath}");
    }

    private static string GetSessionPath(string username, string sessionName)
    {
        var config = ConfigService.LoadConfig();
        return Path.Combine(config.StoragePath, "user", username, "sessions", sessionName);
    }
}