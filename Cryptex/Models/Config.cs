using System.Text.Json.Serialization;

namespace Cryptex.Models;
    public class Config
    {
        [JsonPropertyName("storagePath")]
        public string StoragePath { get; set; } = GetDefaultStoragePath();

        [JsonPropertyName("currentUser")]
        public string CurrentUser { get; set; } = string.Empty;

        private static string GetDefaultStoragePath()
        {
            string homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return Path.Combine(homePath, "Cryptex");
        }
    }
