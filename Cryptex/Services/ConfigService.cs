using System.Text.Json;
using Cryptex.Models;
using Cryptex.Utils;

namespace Cryptex.Services;

public static class ConfigService
{
    private static readonly string DefaultConfigPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        "Cryptex",
        "config.json"
    );

    public static Config LoadConfig()
    {
        try
        {
            if (File.Exists(DefaultConfigPath))
            {
                string json = File.ReadAllText(DefaultConfigPath);
                return JsonSerializer.Deserialize<Config>(json) ?? new Config();
            }
        }
        catch (Exception ex)
        {
            ConsoleEx.PrintError($"Error loading config: {ex.Message}");
        }

        return new Config();
    }

    public static void SaveConfig(Config config)
    {
        try
        {
            string configDir = Path.GetDirectoryName(DefaultConfigPath);
            if (!Directory.Exists(configDir))
                Directory.CreateDirectory(configDir);

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(config, options);
            File.WriteAllText(DefaultConfigPath, json);
        }
        catch (Exception ex)
        {
            ConsoleEx.PrintError($"Error saving config: {ex.Message}");
        }
    }

    public static void SetStoragePath(string newPath)
    {
        try
        {
            string fullPath = Path.GetFullPath(newPath);

            if (newPath.StartsWith("~"))
            {
                string homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                fullPath = Path.Combine(homePath, newPath.Substring(1).TrimStart('/'));
            }

            if (!Directory.Exists(fullPath))
            {
                ConsoleEx.PrintWarning($"Path does not exist: {fullPath}");
                ConsoleEx.PrintInfo("Creating directory...");

                try
                {
                    Directory.CreateDirectory(fullPath);
                }
                catch (UnauthorizedAccessException)
                {
                    ConsoleEx.PrintError($"No permission to create directory: {fullPath}");
                    ConsoleEx.PrintInfo("Try a different location like your home folder.");
                    return;
                }
            }

            try
            {
                string testFile = Path.Combine(fullPath, ".write_test");
                File.WriteAllText(testFile, "test");
                File.Delete(testFile);
            }
            catch (UnauthorizedAccessException)
            {
                ConsoleEx.PrintError($"No write permission for: {fullPath}");
                ConsoleEx.PrintInfo("Try a location in your home folder.");
                return;
            }

            var config = LoadConfig();
            config.StoragePath = fullPath;
            SaveConfig(config);

            ConsoleEx.PrintSuccess($"Storage path updated: {fullPath}");
        }
        catch (Exception ex)
        {
            ConsoleEx.PrintError($"Failed to set storage path: {ex.Message}");
            ConsoleEx.PrintInfo("Try using: change-storage ~/Documents/cryptex");
        }
    }

    public static void ShowCurrentConfig()
    {
        var config = LoadConfig();
        ConsoleEx.PrintInfo("=== Current Configuration ===");
        Console.WriteLine($"Storage Path: {config.StoragePath}");
        Console.WriteLine($"Current User: {config.CurrentUser ?? "None"}");
    }
}