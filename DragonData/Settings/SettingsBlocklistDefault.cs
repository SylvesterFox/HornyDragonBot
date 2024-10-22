using System.Text.Json;

namespace DragonData.Settings
{
    public class DefaultList
    {
        public List<string> Blocklist { get; set; }
    }

    public static class SettingsBlocklistDefault
    {
        private static DefaultList _defaultList = new DefaultList { 
            Blocklist = new List<string>() { "gore", "scat", "watersports", "loli", "shota", "my_little_pony", "young", "fart", "intersex", "humanoid" } 
        };

        private static string folder = "HorryDragon";
        private static string file = "blocklistDefault.json";
        private static string folderComine = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), folder);

        public static string InitBlocklist()
        {
            string jsonString = JsonSerializer.Serialize(_defaultList, new JsonSerializerOptions { WriteIndented = true });
            
            bool folderExists = Directory.Exists(folderComine);

            if (!folderExists)
            {
                Directory.CreateDirectory(folderComine);
            }

            var PathJson = Path.Join(folderComine, file);
            if (!File.Exists(PathJson))
            {
                File.WriteAllText(PathJson, jsonString);
            }
            return File.ReadAllText(PathJson);
        }

        public static List<string> GetJsonBlocklist()
        {
            string jsonString = InitBlocklist();

            var defaultList = JsonSerializer.Deserialize<DefaultList>(jsonString);
            if (defaultList != null)
            {
                return defaultList.Blocklist;
            }

            throw new JsonException("Json file not found");
        }



    }
}
