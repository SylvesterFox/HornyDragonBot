namespace HornyDragonBot.Settings {
    public class BotSettingInit {
        public BotConfig LoadedConfig = new BotConfig();

        public const string fileEnv = ".env";
        public const string BotName = "HorryDragonBot";

        public readonly string BotDataDirectory;

        private BotSettingInit() {
            BotDataDirectory = Directory.GetCurrentDirectory();
        }

        public bool LoadConfiguration() {
            string configFilePath = Path.Combine(BotDataDirectory, fileEnv);

            try {
                Directory.CreateDirectory(BotDataDirectory);
                if(!File.Exists(configFilePath)) return false;
            } catch (Exception ex) {
                return false;
            }

            LoadedConfig = EnvSerializer.Deserialize(configFilePath);
            return LoadedConfig != null;
        }


        private static BotSettingInit? _PrivateInstance;
        
        public static BotSettingInit Instance {
            get {
                return _PrivateInstance ??= new BotSettingInit();
            }
        }

    }

}


