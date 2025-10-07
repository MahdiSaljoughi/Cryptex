using Cryptex.Services;
using Cryptex.Utils;

namespace Cryptex.Commands;

public static class CryptoCommands
{
    public static bool Encrypt(string[] args)
    {
        if (args.Length < 3)
        {
            ConsoleEx.PrintWarning("Usage: encrypt <session-name> <message>");
            return false;
        }

        EncryptionService.EncryptMessage(args[1], args[2]);
        return true;
    }

    public static bool Decrypt(string[] args)
    {
        if (args.Length < 3)
        {
            ConsoleEx.PrintWarning("Usage: decrypt <session-name> <encrypted-message-base64>");
            return false;
        }

        EncryptionService.DecryptMessage(args[1], args[2]);
        return true;
    }
}