using Newtonsoft.Json;
using PropertyChanged;

namespace DayZTediratorToolz.Models
{
    [AddINotifyPropertyChangedInterface]
    public class AppConfigurationModel
    {
        [AddINotifyPropertyChangedInterface]
        public class AppConfiguration
        {
            [JsonProperty("ServerIP")]
            public string ServerIP { get; set; }

            [JsonProperty("ServerPort")]
            public string ServerPort { get; set; }

            [JsonProperty("QueryUri")]
            public string QueryUri { get; set; }

            [JsonProperty("GameExecutablePath")]
            public string GameExecutablePath { get; set; }

            [JsonProperty("ModPagePath")] public string ModPagePath { get; set; }
        }

        [AddINotifyPropertyChangedInterface]
        public class Root
        {
            [JsonProperty("AppConfiguration")]
            public AppConfiguration AppConfiguration { get; set; }
        }
    }
}