using Cryptex.Services;
using Cryptex.Utils;

namespace Cryptex.Commands;

public static class ConfigCommands
{
    public static bool ChangeStorage(string[] args)
    {
        if (args.Length < 2)
        {
            ConsoleEx.PrintWarning("Usage: change-storage <new-path>");
            return false;
        }

        ConfigService.SetStoragePath(args[1]);
        return true;
    }

    public static bool ShowConfig()
    {
        ConfigService.ShowCurrentConfig();
        return true;
    }
    
    public static bool FixStorage()
    {
        try
        {
            string defaultPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Cryptex"
            );

            var config = ConfigService.LoadConfig();
        
            ConsoleEx.PrintInfo("Fixing storage path...");
            ConsoleEx.PrintInfo($"Current: {config.StoragePath}");
            ConsoleEx.PrintInfo($"Changing to: {defaultPath}");
            
            Directory.CreateDirectory(defaultPath);

            config.StoragePath = defaultPath;
            ConfigService.SaveConfig(config);

            ConsoleEx.PrintSuccess($"Storage fixed: {defaultPath}");
            return true;
        }
        catch (Exception ex)
        {
            ConsoleEx.PrintError($"Failed to fix storage: {ex.Message}");
            return false;
        }
    }
}