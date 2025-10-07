using Cryptex.Services;
using Cryptex.Utils;

namespace Cryptex.Commands;

public static class SessionCommands
{
    public static bool CreateSession(string[] args)
    {
        if (args.Length < 2)
        {
            ConsoleEx.PrintWarning("Usage: create-session <session-name>");
            return false;
        }

        SessionService.CreateSession(args[1]);
        return true;
    }

    public static bool SetFriendKey(string[] args)
    {
        if (args.Length < 3)
        {
            ConsoleEx.PrintWarning("Usage: set-friend-key <session-name> <friend-public-key-or-path>");
            return false;
        }

        KeyService.SetFriendPublicKey(args[1], args[2]);
        return true;
    }

    public static bool ListSessions()
    {
        SessionService.ListSessions();
        return true;
    }
}