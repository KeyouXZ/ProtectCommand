using System.Text.Json;
using Newtonsoft.Json;
using TShockAPI;

namespace ProtectCommand
{
    public class Config
    {

        [JsonProperty("Enable")]
        public bool Enable { get; set; } = true;

        [JsonProperty("Not in the Region Message")]
        public string NotInRegionMessage { get; set; } = "You are not in the correct region to use this command. Please use this command in {region}.";

        [JsonProperty("Protected")]
        public Dictionary<string, string> ProtectedCommands { get; set; } = new Dictionary<string, string>();
        public static Config Read()
        {
            string configPath = Path.Combine(TShock.SavePath, "ProtectCommand.json");

            try
            {
                Config config = new Config().DefaultConfig();

                if (!File.Exists(configPath))
                {
                    File.WriteAllText(configPath, JsonConvert.SerializeObject(config, Formatting.Indented));
                }
                config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configPath)) ?? new Config();

                return config;
            }

            catch (Exception ex)
            {
                TShock.Log.ConsoleError(ex.ToString());
                return new Config();
            }
        }
        private Config DefaultConfig()
        {
            var defaultConfig = new Config();

            defaultConfig.ProtectedCommands["cone"] = "Region1";
            defaultConfig.ProtectedCommands["ctwo"] = "Region2";

            return defaultConfig;
        }
    }

}