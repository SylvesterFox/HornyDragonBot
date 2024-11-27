namespace HornyDragonBot.Settings {
    public static class EnvSerializer {
        public static BotConfig Deserialize(string filepath) {
            var envVariables = new BotConfig();

            if (!File.Exists(filepath)) {
                throw new FileNotFoundException($"File {filepath} not found.");
            }

            string[] lines = File.ReadAllLines(filepath);

            foreach (var line in lines) {
                if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("#")) 
                {
                    string[] parts = line.Split("=");

                    if (parts.Length == 2) {
                        string key = parts[0].Trim();
                        string value = parts[1].Trim();

                        foreach (var property in typeof(BotConfig).GetProperties()) {
                            if (property.Name.Equals(key, StringComparison.OrdinalIgnoreCase)) {
                                object typeValue = Convert.ChangeType(value, property.PropertyType);

                                property.SetValue(envVariables, typeValue, null);
                                break;
                            }
                        }
                    } else {
                        throw new FormatException($"Invalid format in line: {line}");
                    }
                }
            }

            return envVariables;
        }
    }


}

